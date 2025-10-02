# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5230
EXPOSE 7230

# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookStackMcpServer/BookStackMcpServer.csproj", "BookStackMcpServer/"]
RUN dotnet restore "BookStackMcpServer/BookStackMcpServer.csproj"
COPY . .
WORKDIR "/src/BookStackMcpServer"
RUN dotnet build "BookStackMcpServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookStackMcpServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for ASP.NET Core
#ENV ASPNETCORE_URLS=http://+:5230

ENTRYPOINT ["dotnet", "BookStackMcpServer.dll"]
