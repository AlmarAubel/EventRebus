FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EventRebus/EventRebus.csproj", "EventRebus/"]
RUN dotnet restore "EventRebus/EventRebus.csproj"
COPY . .
WORKDIR "/src/EventRebus"
RUN dotnet build "EventRebus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventRebus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventRebus.dll"]
