#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ElectionBlockchain.Model/ElectionBlockchain.Model.csproj", "ElectionBlockchain.Model/"]
COPY ["ElectionBlockchain.Services/ElectionBlockchain.Services.csproj", "ElectionBlockchain.Services/"]
COPY ["ElectionBlockchain.DAL/ElectionBlockchain.DAL.csproj", "ElectionBlockchain.DAL/"]
COPY ["ElectionBlockchain.API/ElectionBlockchain.API.csproj", "ElectionBlockchain.API/"]
RUN dotnet restore "ElectionBlockchain.API/ElectionBlockchain.API.csproj"

COPY ["ElectionBlockchain.API/ElectionBlockchain.API.csproj", "ElectionBlockchain.API/"]
RUN dotnet restore "ElectionBlockchain.API/ElectionBlockchain.API.csproj"
COPY . .
WORKDIR "/src/ElectionBlockchain.API"
RUN dotnet build "ElectionBlockchain.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElectionBlockchain.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElectionBlockchain.API.dll"]

ENV DB_IP="127.0.0.1"