#!/bin/sh
HERE="$(dirname "$(readlink -f "${0}")")"
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
exec "$HERE"/usr/bin/dbfal $@
