PATH=$HOME/bin:$ADDPATH:$PATH
SSH_ENV="$HOME/.ssh/environment"

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

cd source/bin/Release
REPO=git@github.com:flickr-downloadr/flickr-downloadr.git
MSG="application (v${BUILDNUMBER})"

#clone repo in a tmp dir
mkdir tmp-gh-pages
cd tmp-gh-pages
git clone -b gh-pages $REPO
cd flickr-downloadr
git config push.default tracking

#remove all files except index.html in downloads/latest
mv downloads/latest/index.html .
cd downloads/latest/
git rm -r .
cd ../..
mv index.html downloads/latest

#add published files & build.number to gh-pages; commit; push
cp -r ../../Deploy/* ./downloads/latest
cp ../../../../../build.number .
git add .
git commit -m "deploying $MSG" -s
git push

#checkout master to add the modified build.number and CommonAssemblyInfo; commit; push
git checkout master
cp ../../../../../build.number .
cp ../../../../CommonAssemblyInfo.cs ./source
git commit -a -m "deploying $MSG" -s
git push

#remove the tmp dir
cd ../..
rm -rf tmp-gh-pages

#reset the modified build.number and CommonAssemblyInfo in the main (outer) repo
cd ../../..
git checkout -- build.number source/CommonAssemblyInfo.cs

# done
echo "deployed $MSG successfully"
exit