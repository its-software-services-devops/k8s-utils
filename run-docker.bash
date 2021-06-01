#/bin/bash

VERSION=develop-a21fed6

sudo docker run \
-v $(pwd)/output:/wip/output \
-v ${HOME}/.kube/config:/root/.kube/config \
-it gcr.io/its-artifact-commons/k8s-utils:${VERSION} \
snapshot -o .
