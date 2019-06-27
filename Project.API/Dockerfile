FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MongoDbQuickstart/MongoDbQuickstart.csproj", "MongoDbQuickstart/"]
RUN dotnet restore "MongoDbQuickstart/MongoDbQuickstart.csproj"
COPY . .
WORKDIR "/src/MongoDbQuickstart"
RUN dotnet build "MongoDbQuickstart.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MongoDbQuickstart.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MongoDbQuickstart.dll"]