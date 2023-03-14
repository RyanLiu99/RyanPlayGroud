
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Medrio.CspReport.sln"

WORKDIR "/src/Medrio.CspReport"
RUN dotnet build "Medrio.CspReport.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Medrio.CspReport.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medrio.CspReport.dll"]