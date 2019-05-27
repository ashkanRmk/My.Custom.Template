FROM mcr.microsoft.com/dotnet/core/sdk:2.2.105 as build-env

ENV APP_NAME="My.Custom.Template"

# Set the current directory for the build phase.
WORKDIR /app

# By default all ports are closed in the container. Here the web app port is opened.
# Other ports that need to be opened can be added here (only ports above 1024), separated by spaces.
EXPOSE 5000

# Copy the required project definition and dependency files into the Docker image.
# Don't copy the application files yet as they prevent `dotnet restore` from being cached by
# Docker's layer caching mechanism.
COPY ${APP_NAME}.sln .
COPY src/${APP_NAME}/${APP_NAME}.csproj src/${APP_NAME}/${APP_NAME}.csproj
COPY src/${APP_NAME}.DataLayer/${APP_NAME}.DataLayer.csproj src/${APP_NAME}.DataLayer/${APP_NAME}.DataLayer.csproj
COPY src/${APP_NAME}.ModelLayer/${APP_NAME}.ModelLayer.csproj src/${APP_NAME}.ModelLayer/${APP_NAME}.ModelLayer.csproj
COPY src/${APP_NAME}.ServiceLayer/${APP_NAME}.ServiceLayer.csproj src/${APP_NAME}.ServiceLayer/${APP_NAME}.ServiceLayer.csproj
COPY src/${APP_NAME}.Entities/${APP_NAME}.Entities.csproj src/${APP_NAME}.Entities/${APP_NAME}.Entities.csproj

RUN dotnet restore -s https://api.nuget.org/v3/index.json

# Copy the application files. Initially copy them to a temp directory so their
# permissions can be updated and then copy them to the target directory. This
# reduces the size of the Docker image.
COPY . /tmp/code
RUN chgrp -R 0 /tmp/code && \
    chmod -R g=u /tmp/code && \
    cp -a /tmp/code/. . && \
    rm -rf /tmp/code && \
    # Output the binary and assets to /out
    mkdir -p /out && \
    # 1) Build the application.
    dotnet publish src/${APP_NAME}/${APP_NAME}.csproj -c Release -o /out
    # 2) Run the tests and clean up if successful,
    #    Otherwise exit with non-zero code if anything fails.
    # cd /app/tests && \
    # dotnet restore -s https://api.nuget.org/v3/index.json -s ${NUGET_URL} && \
    # dotnet test && \
    # # 3) Clean up all the source files
    # rm -rf /app && \
    # rm -rf /root/.nuget && \
    # rm -rf /root/.npm

# WORKDIR /out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.3

COPY --from=build-env /out ./app


WORKDIR /app

# Specify the name of built application to run when the container starts.
ENTRYPOINT dotnet ${APP_NAME}.dll
RUN chmod -R ug+rwx /app
# Useful for debug
#ENTRYPOINT /bin/bash -c "trap : TERM INT; sleep infinity & wait"
