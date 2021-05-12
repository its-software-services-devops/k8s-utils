#!/bin/bash

dotnet build

dotnet k8s-utils/bin/Debug/net5.0/k8s-utils.dll $*