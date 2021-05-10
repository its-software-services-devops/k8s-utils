#/bin/bash

VERSION=v0.0.0

sudo docker run \
-it gcr.io/its-artifact-commons/k8s-utils:${VERSION} \
init
