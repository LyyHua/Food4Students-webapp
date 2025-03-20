FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore as distinct layers. Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY Food4Student-webapp.sln Food4Student-webapp.sln
COPY src/RestaurantService/RestaurantService.csproj src/RestaurantService/RestaurantService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY src/RatingService/RatingService.csproj src/RatingService/RatingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

# Restore package deps
RUN dotnet restore Food4Student-webapp.sln

# Copy the app folders over
COPY src/NotificationService src/NotificationService
COPY src/Contracts src/Contracts
WORKDIR /app/src/NotificationService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "NotificationService.dll" ]