#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ExpertStore.Payment/ExpertStore.Payment.csproj", "src/ExpertStore.Payment/"]
COPY ["src/ExpertStore.SeedWork/ExpertStore.SeedWork.csproj", "src/ExpertStore.SeedWork/"]
RUN dotnet restore "src/ExpertStore.Payment/ExpertStore.Payment.csproj"
COPY . .
WORKDIR "/src/src/ExpertStore.Payment"
RUN dotnet build "ExpertStore.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpertStore.Payment.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpertStore.Payment.dll"]