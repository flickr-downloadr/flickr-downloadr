cd source/bin/Release
PATH=$HOME/bin:$ADDPATH:$PATH
REPO=https://flickr-downloadr-ci@github.com/flickr-downloadr/flickr-downloadr.git
MSG="application (v${BUILDNUMBER})"
mkdir tmp-gh-pages
cd tmp-gh-pages
git clone -b gh-pages $REPO
cd flickr-downloadr
git config push.default tracking
cp -r ../../Deploy/* ./downloads/latest
git add .
git commit -m "deploying $MSG" -s
git push --repo $REPO
cd ../..
rm -rf tmp-gh-pages
echo "deployed $MSG successfully"