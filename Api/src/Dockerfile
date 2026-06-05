# ==========================================
# STAGE 1: BUILD THE APPLICATION
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Docker is looking inside 'src/'. Copy the csproj file directly.
COPY BelgianFootballFantasy.Web/BelgianFootballFantasy.Web.csproj ./BelgianFootballFantasy.Web/
RUN dotnet restore "BelgianFootballFantasy.Web/BelgianFootballFantasy.Web.csproj"

# Copy all other source files from the current folder context
COPY BelgianFootballFantasy.Web/ ./BelgianFootballFantasy.Web/

# Step inside the project folder to publish
WORKDIR /app/BelgianFootballFantasy.Web
RUN dotnet publish -c Release -o /app/out

# ==========================================
# STAGE 2: RUN THE APPLICATION
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Copy the binaries over to the slim runtime image
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BelgianFootballFantasy.Web.dll"]
