@echo off
IF dummy==dummy%2 (
build\nant\nant-0.92\bin\NAnt.exe -buildfile:FlickrDownloadr.build %1 -D:project.build.type=Debug
) ELSE (
build\nant\nant-0.92\bin\NAnt.exe -buildfile:FlickrDownloadr.build %1 -D:project.build.type=%2
)
date /t && time /t
pause