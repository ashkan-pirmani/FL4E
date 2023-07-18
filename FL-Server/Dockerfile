#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FL-Server/FL-Server.csproj", "FL-Server/"]
RUN dotnet restore "FL-Server/FL-Server.csproj"
COPY . .
WORKDIR "/src/FL-Server"
RUN dotnet build "FL-Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FL-Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt-get update && \
    apt-get install -y apt-utils && \
    apt-get install -y python3 && \
    apt-get install dos2unix && \
    apt-get install -y python3-pip

RUN pip3 install pandas 

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FL-Server.dll"]