#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Medrio.AuditLog.MessagePusher/Medrio.AuditLog.MessagePusher.csproj", "Medrio.AuditLog.MessagePusher/"]
RUN dotnet restore "Medrio.AuditLog.MessagePusher/Medrio.AuditLog.MessagePusher.csproj"
COPY . .
WORKDIR "/src/Medrio.AuditLog.MessagePusher"
RUN dotnet build "Medrio.AuditLog.MessagePusher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Medrio.AuditLog.MessagePusher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medrio.AuditLog.MessagePusher.dll"]