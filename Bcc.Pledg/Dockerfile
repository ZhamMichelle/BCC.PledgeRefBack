FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Bcc.Pledg/Bcc.Pledg.csproj", "Bcc.Pledg/"]
RUN dotnet restore "Bcc.Pledg/Bcc.Pledg.csproj"
COPY . .
WORKDIR "/src/Bcc.Pledg"
RUN dotnet build "Bcc.Pledg.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bcc.Pledg.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Bcc.Pledg/Resources/", "Resources/"]
ENTRYPOINT ["dotnet", "Bcc.Pledg.dll"]