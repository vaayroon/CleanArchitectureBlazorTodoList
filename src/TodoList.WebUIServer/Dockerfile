# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj and restore as distinct layers
COPY *.sln .
COPY TodoList.WebUIServer/*.csproj ./TodoList.WebUIServer/
COPY TodoList.Application/*.csproj ./TodoList.Application/
COPY TodoList.Domain/*.csproj ./TodoList.Domain/
COPY TodoList.Infrastructure/*.csproj ./TodoList.Infrastructure/
RUN dotnet restore TodoList.WebUIServer/TodoList.WebUIServer.csproj

# Copy everything else and build the application
COPY . .
WORKDIR /app/TodoList.WebUIServer
RUN dotnet publish -c Release -o out

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/TodoList.WebUIServer/out .
ENTRYPOINT ["dotnet", "TodoList.WebUIServer.dll"]