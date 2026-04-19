#!/bin/sh
dotnet restore -r linux-x64
dotnet publish -c Release -r linux-x64
HERE="$(dirname "$(readlink -f "${0}")")"
mkdir -p "$HERE"/bin/AppDir/usr/bin
cp "$HERE"/bin/Release/net10.0/linux-x64/publish/* "$HERE"/bin/AppDir/usr/bin/
cp "$HERE"/dbfal.desktop "$HERE"/bin/AppDir/
cp "$HERE"/Assets/1.png "$HERE"/bin/AppDir/
cp "$HERE"/AppRun.sh "$HERE"/bin/AppDir/AppRun