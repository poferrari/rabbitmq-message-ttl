FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build
WORKDIR /src
COPY . /src
RUN dotnet restore
COPY . .
WORKDIR "/src/Message.Consumer.Worker"
RUN dotnet build "Message.Consumer.Worker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Message.Consumer.Worker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Message.Consumer.Worker.dll"]