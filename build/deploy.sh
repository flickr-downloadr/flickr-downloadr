PATH=$HOME/bin:$ADDPATH:$PATH

if [[ $APPVEYOR_REPO_COMMIT_MESSAGE != *\[deploy\]* ]]
then
  echo 'There is nothing to deploy here. Moving on!';
  exit
fi

git config --global user.email "contact.us@flickrdownloadr.com"
git config --global user.name "The CI Bot"

cd ../source/bin/Release
REPO="https://github.com/flickr-downloadr/flickr-downloadr.github.io.git"
SOURCEREPO="https://github.com/flickr-downloadr/flickr-downloadr.git"
VERSION="v${BUILDNUMBER}"
MSG="application ($VERSION)"

# make the temp directory
cd ../../../..
mkdir tmp-deploy-dir
cd tmp-deploy-dir

#clone repo in the tmp dir
echo 'Cloning github.io repo'
git clone -b master $REPO
cd flickr-downloadr.github.io
git checkout -b old-wpf-app
git config credential.helper "store --file=.git/fd-credentials"
echo "https://${GH_TOKEN}:@github.com" > .git/fd-credentials
git config push.default tracking

#remove all files except index.html in downloads/latest
echo 'Deleting the previous version artifacts'
mv downloads/latest/index.html .
cd downloads/latest/
git rm --cached **/.gitattributes
git rm -r .
git add -f **/.gitattributes
cd ../..
mv index.html downloads/latest

#add published files to gh-pages; commit; push
echo 'Creating the correct changeset from built artifacts'
cp -r ../../flickr-downloadr/source/bin/Release/Deploy/* ./downloads/latest
# cp ../../flickr-downloadr/build/build.number .
git add -f --ignore-removal .
git commit -m "deploying $MSG" -s
git push -f --set-upstream origin old-wpf-app

#checkout master to add the modified build.number and CommonAssemblyInfo; commit; push
echo 'Check out master branch and commit the changed Assembly Info and build.number'
cd ..
git clone -b master $SOURCEREPO
cd flickr-downloadr
git config credential.helper "store --file=.git/fd-credentials"
echo "https://${GH_TOKEN}:@github.com" > .git/fd-credentials
cp ../../flickr-downloadr/build/build.number ./build
cp ../../flickr-downloadr/source/CommonAssemblyInfo.cs ./source
git commit -a -m "deploying $MSG [ci skip]" -s
git tag -a $VERSION -m "tagging version $VERSION"
git push --tags origin master

# done
echo "deployed $MSG successfully"
exit
