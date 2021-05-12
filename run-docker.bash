#/bin/bash

VERSION=develop-64fdf01

sudo docker run \
-v $(pwd)/output:/wip/output \
-v ${HOME}/.kube/config:/root/.kube/config \
-it gcr.io/its-artifact-commons/k8s-utils:${VERSION} \
export -o /wip/output/cluster.txt
