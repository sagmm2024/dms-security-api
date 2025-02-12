FROM ghcr.io/architecture-it/net-sdk:8.0 as publish
WORKDIR /app
COPY . .
WORKDIR "/app/src/Api"
RUN dotnet publish "SecurityApi.csproj" -c Release -o /app/publish

FROM ghcr.io/architecture-it/net:8.0
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SecurityApi.dll"]