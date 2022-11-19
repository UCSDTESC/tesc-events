#!/bin/bash

eval $(aws s3 cp s3://tesc.events/envs/$ENV/$ms/envs - | sed 's/^/export /')
dotnet TescEvents.dll
