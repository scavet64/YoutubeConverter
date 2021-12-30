#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN sudo apt-get install -y build-essential
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["YoutubeConverter/YoutubeConverter.csproj", "YoutubeConverter/"]
RUN dotnet restore "YoutubeConverter/YoutubeConverter.csproj"
COPY . .
WORKDIR "/src/YoutubeConverter"
RUN dotnet build "YoutubeConverter.csproj" -c Release -o /app/build

FROM build AS publish

RUN sudo apt-get install -y build-essential
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

RUN dotnet publish "YoutubeConverter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YoutubeConverter.dll"]