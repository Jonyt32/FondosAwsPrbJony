FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY ["BackendFondos/BackendFondos.csproj", "BackendFondos/"]
COPY ["BackendFondos.Tests/BackendFondos.Tests.csproj", "BackendFondos.Tests/"]
COPY ["LambdaDebug/LambdaDebug.csproj", "LambdaDebug/"]
RUN dotnet restore "BackendFondos/BackendFondos.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/BackendFondos"
RUN dotnet build "BackendFondos.csproj" -c Release -o /app/build -a $TARGETARCH

FROM build AS publish
ARG TARGETARCH
RUN dotnet publish "BackendFondos.csproj" -c Release -o /app/publish -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendFondos.dll"]
