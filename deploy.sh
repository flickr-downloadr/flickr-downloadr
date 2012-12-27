PATH=$HOME/bin:$ADDPATH:$PATH
SSH_ENV="$HOME/.ssh/environment"
ssh -T git@github.com

# start the ssh-agent
function start_agent {
    echo "Initializing new SSH agent..."
    # spawn ssh-agent
    ssh-agent | sed 's/^echo/#echo/' > "$SSH_ENV"
    echo succeeded
    chmod 600 "$SSH_ENV"
    . "$SSH_ENV" > /dev/null
    ssh-add
}

# test for identities
function test_identities {
    # test whether standard identities have been added to the agent already
    ssh-add -l | grep "The agent has no identities" > /dev/null
    if [ $? -eq 0 ]; then
        ssh-add
        # $SSH_AUTH_SOCK broken so we start a new proper agent
        if [ $? -eq 2 ];then
            start_agent
        fi
    fi
}

# check for running ssh-agent with proper $SSH_AGENT_PID
if [ -n "$SSH_AGENT_PID" ]; then
    ps -ef | grep "$SSH_AGENT_PID" | grep ssh-agent > /dev/null
    if [ $? -eq 0 ]; then
  test_identities
    fi
# if $SSH_AGENT_PID is not properly set, we might be able to load one from
# $SSH_ENV
else
    if [ -f "$SSH_ENV" ]; then
  . "$SSH_ENV" > /dev/null
    fi
    ps -ef | grep "$SSH_AGENT_PID" | grep ssh-agent > /dev/null
    if [ $? -eq 0 ]; then
        test_identities
    else
        start_agent
    fi
fi

ssh -T git@github.com
exit

cd source/bin/Release
REPO=git@github.com:flickr-downloadr/flickr-downloadr.git
MSG="application (v${BUILDNUMBER})"
mkdir tmp-gh-pages
cd tmp-gh-pages
git clone -b gh-pages $REPO
cd flickr-downloadr
git config push.default tracking
cp -r ../../Deploy/* ./downloads/latest
git add .
git commit -m "deploying $MSG" -s
git checkout master
cp ../../../../../../build.number .
cp ../../../../../CommonAssemblyInfo.cs ./source
#git push
cd ../..
rm -rf tmp-gh-pages
echo "deployed $MSG successfully"