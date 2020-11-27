# create the build instance 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ./ ./

# restore solution
RUN dotnet restore AecPackageDeployer.sln

WORKDIR /src/AecPackageDeployer

# build project
RUN dotnet build AecPackageDeployer.csproj -c Release

# publish project
WORKDIR /src/AecPackageDeployer
RUN dotnet publish AecPackageDeployer.csproj -c Release -o /app/published

# create the runtime instance 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime 

# add globalization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# installs required packages
RUN apk add libgdiplus --no-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted
RUN apk add libc-dev --no-cache

WORKDIR /app
RUN mkdir usb
RUN mkdir carga
RUN mkdir descarga
RUN mkdir log

COPY --from=build /app/published .

ENTRYPOINT ["dotnet", "AecPackageDeployer.dll"]
