    # BASE: ASP.Net Core Base Image, listening on port 80
    FROM  mcr.microsoft.com/dotnet/aspnet:5.0 AS base
    WORKDIR /app
    EXPOSE 80
    EXPOSE 443
	
	##################################################################################
    # INITIALIZE: Create a base image with the dotnet SDK, in order to build the     #
	#             projects 															 #
	##################################################################################
    FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
	ARG version=1.0.0.42
    WORKDIR /src

	
	RUN echo "using version $version"
	
	##################################################################################
    # COPY: Copy All Workspace contents into Docker Container                        #
	##################################################################################
    COPY . .
	
	##################################################################################
	# RESTORE: Restore all nuget packages for Solution                               #
	##################################################################################
	RUN dotnet restore ./WebApiNet5.sln
	
	##################################################################################
	# BUILD: Compile API in Release Mode											 #
	##################################################################################
    RUN dotnet build ./WebApiNet5.sln /p:Version=$version --no-restore -c Release
	
	

	##################################################################################
	# PUBLISH: Package API into dotnet hosted web app								 #
	##################################################################################
	FROM build AS publish
    RUN dotnet publish ./WebApiNet5.sln  --no-build -c Release -o /app
	
	
	FROM base AS final
    WORKDIR /app
	
    COPY --from=publish /app .
	
    ENTRYPOINT ["dotnet", "WebApiNet5.dll"]