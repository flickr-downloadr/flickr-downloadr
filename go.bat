@echo off
build\nant\nant-0.92\bin\NAnt.exe -buildfile:FlickrDownloadr.build %1 -D:project.build.type=Debug
pause