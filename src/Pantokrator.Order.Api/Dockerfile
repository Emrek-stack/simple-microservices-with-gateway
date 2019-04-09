FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /workdir

COPY ./src/Pantokrator.Order.Api/Pantokrator.Order.Api.csproj ./src/Pantokrator.Order.Api/
RUN dotnet restore ./src/Pantokrator.Order.Api/Pantokrator.Order.Api.csproj

COPY ./src/Pantokrator.Order.Api ./src/Pantokrator.Order.Api
RUN dotnet publish ./src/Pantokrator.Order.Api/Pantokrator.Order.Api.csproj -c Release -o /publish

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /publish
COPY --from=build-env /publish /publish

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "Pantokrator.Order.Api.dll"]