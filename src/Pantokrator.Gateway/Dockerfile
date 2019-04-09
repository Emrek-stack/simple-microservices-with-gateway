FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /workdir

COPY ./Pantokrator.Gateway.csproj ./src/Pantokrator.Gateway/
RUN dotnet restore ./src/Pantokrator.Gateway/Pantokrator.Gateway.csproj

COPY ./ ./src/Pantokrator.Gateway
RUN dotnet publish ./src/Pantokrator.Gateway/Pantokrator.Gateway.csproj -c Release -o /publish

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /publish
COPY --from=build-env /publish /publish

ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Pantokrator.Order.Api.dll"]