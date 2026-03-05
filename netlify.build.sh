#!/usr/bin/env bash
set -e

# install latest .NET8.0 release
pushd /tmp
wget https://builds.dotnet.microsoft.com/dotnet/scripts/v1/dotnet-install.sh

# Make the script executable
chmod u+x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 8.0

# Return to project directory
popd

# Now we can publish (since we've installed .NET in this environment)
dotnet publish -c Release -o release