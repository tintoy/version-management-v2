FROM microsoft/aspnet:1.0.0-rc1-final-coreclr

RUN apt-get update && apt-get install -y libsqlite3-dev

ADD ./src/VersionManagement /app
WORKDIR /app

ENV DNX_BUILD_VERSION "1.0.0-alpha1-2"
RUN ["dnu", "restore"]

EXPOSE 5000
ENTRYPOINT ["dnx", "web"]
