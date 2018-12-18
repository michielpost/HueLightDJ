FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 59919
EXPOSE 44339
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["HueLightDJ.Web/HueLightDJ.Web.csproj", "HueLightDJ.Web/"]
COPY ["HueLightDJ.Effects/HueLightDJ.Effects.csproj", "HueLightDJ.Effects/"]
RUN dotnet restore "HueLightDJ.Web/HueLightDJ.Web.csproj"
COPY . .
WORKDIR "/src/HueLightDJ.Web"
RUN dotnet build "HueLightDJ.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HueLightDJ.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HueLightDJ.Web.dll"]
