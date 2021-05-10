ARG version
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /install

RUN apt-get -y update
RUN apt-get -y install curl
RUN apt-get -y install unzip

RUN curl -LO https://get.helm.sh/helm-v3.5.2-linux-amd64.tar.gz
RUN tar -xvf helm-v3.5.2-linux-amd64.tar.gz
RUN cp linux-amd64/helm /usr/local/bin/
RUN helm version

RUN curl -LO https://dl.k8s.io/release/v1.21.0/bin/linux/amd64/kubectl
RUN cp kubectl /usr/local/bin/
RUN kubectl version

WORKDIR /source

# copy csproj and restore as distinct layers
COPY k8s-utils/* ./k8s-utils/
COPY k8s-utils.sln .

RUN ls -lrt
RUN dotnet restore k8s-utils/k8s-utils.csproj
RUN dotnet publish k8s-utils/k8s-utils.csproj -c release -o /app --no-restore -p:PackageVersion=${version}

##### final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0

RUN apt-get -y update
RUN apt-get -y install curl

COPY --from=build /usr/local/bin/helm /usr/local/bin/
COPY --from=build /usr/local/bin/kubectl /usr/local/bin/

RUN helm version
RUN kubectl version

WORKDIR /app
COPY --from=build /app .
RUN ls -lrt
RUN dotnet k8s-utils.dll info

RUN mkdir -p /wip/input
RUN mkdir -p /wip/output

ENV K8S_UTILS_SRC_DIR=/wip/input
ENV K8S_UTILS_OUT_DIR=/wip/output
ENV K8S_UTILS_TMP_DIR=/tmp

ENTRYPOINT ["dotnet", "k8s-utils.dll"]
