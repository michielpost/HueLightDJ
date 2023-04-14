FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 59919
EXPOSE 44339
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HueLightDJ.BlazorWeb/Server/HueLightDJ.BlazorWeb.Server.csproj", "HueLightDJ.BlazorWeb/Server/"]
COPY ["HueLightDJ.Effects/HueLightDJ.Effects.csproj", "HueLightDJ.Effects/"]
RUN dotnet restore "HueLightDJ.BlazorWeb/Server/HueLightDJ.BlazorWeb.Server.csproj"
COPY . .
WORKDIR "/src/HueLightDJ.BlazorWeb/Server"
RUN dotnet build "HueLightDJ.BlazorWeb.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HueLightDJ.BlazorWeb.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HueLightDJ.BlazorWeb.Server.dll"]
