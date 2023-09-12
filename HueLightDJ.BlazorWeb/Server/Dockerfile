#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HueLightDJ.BlazorWeb/Server/HueLightDJ.BlazorWeb.Server.csproj", "HueLightDJ.BlazorWeb/Server/"]
COPY ["HueLightDJ.Services/HueLightDJ.Services.csproj", "HueLightDJ.Services/"]
COPY ["HueLightDJ.Effects/HueLightDJ.Effects.csproj", "HueLightDJ.Effects/"]
COPY ["HueLightDJ.Services.Interfaces/HueLightDJ.Services.Interfaces.csproj", "HueLightDJ.Services.Interfaces/"]
COPY ["HueLightDJ.BlazorWeb/Client/HueLightDJ.BlazorWeb.Client.csproj", "HueLightDJ.BlazorWeb/Client/"]
COPY ["HueLightDJ.Blazor.Controls/HueLightDJ.Blazor.Controls.csproj", "HueLightDJ.Blazor.Controls/"]
RUN dotnet restore "HueLightDJ.BlazorWeb/Server/HueLightDJ.BlazorWeb.Server.csproj"
COPY . .
WORKDIR "/src/HueLightDJ.BlazorWeb/Server"
RUN dotnet build "HueLightDJ.BlazorWeb.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HueLightDJ.BlazorWeb.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HueLightDJ.BlazorWeb.Server.dll"]