# 1️⃣ Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia solo il file csproj e ripristina dipendenze
COPY *.csproj ./
RUN dotnet restore

# Copia tutto e builda in Release
COPY . ./
RUN dotnet publish -c Release -o out

# 2️⃣ Runtime stage leggero
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia l’output dal build stage
COPY --from=build /app/out ./

# Espone la porta 8080 (Render mapperà automaticamente la porta)
EXPOSE 8080

# Avvia l’app con il nome corretto
ENTRYPOINT ["dotnet", "ApiNetCoreAngularEnquiry.dll"]