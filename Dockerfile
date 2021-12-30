# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0.100-preview.7-bullseye-slim-amd64 AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs npm ffmpeg

WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY YoutubeConverter/*.csproj ./YoutubeConverter/
RUN dotnet restore -r linux-musl-x64

# copy everything else and build app
COPY YoutubeConverter/. ./YoutubeConverter/
WORKDIR /source/YoutubeConverter

RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained false --no-restore

RUN file="$(which ffmpeg)" && echo $file
RUN echo $(which ffmpeg)
RUN echo $(ls)

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0.0-preview.7-alpine3.13-amd64
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /usr/bin/ffmpeg /usr/bin/ffmpeg
RUN chmod -R 777 /app
RUN chmod -R 777 ./
RUN chmod -R 777 /usr/bin/ffmpeg
RUN apk add ffmpeg

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8


# See: https://github.com/dotnet/announcements/issues/20
# Uncomment to enable globalization APIs (or delete)
# ENV \
#     DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
#     LC_ALL=en_US.UTF-8 \
#     LANG=en_US.UTF-8
# RUN apk add --no-cache icu-libs

ENTRYPOINT ["./YoutubeConverter"]