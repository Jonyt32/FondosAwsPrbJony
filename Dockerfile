# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o /app/publish

# Lambda-compatible image
FROM public.ecr.aws/lambda/provided:al2
WORKDIR /var/task

# Copiar archivos publicados
COPY --from=build /app/publish .

# Entrypoint para Lambda
CMD ["dotnet", "BackendFondos.dll"]