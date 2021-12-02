#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FunWithin/FunWithin.csproj", "FunWithin/"]
RUN dotnet restore "FunWithin/FunWithin.csproj"
COPY . .
WORKDIR "/src/FunWithin"
RUN dotnet build "FunWithin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FunWithin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "FunWithin.dll"]