FROM microsoft/aspnet:1.0.0-rc1-final-coreclr

RUN apt-get update && apt-get install -y libsqlite3-dev

ADD ./src/VersionManagement /app
WORKDIR /app
RUN ["dnu", "restore"]

EXPOSE 5000
ENTRYPOINT ["dnx", "web"]
