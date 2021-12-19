#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY /NuGet.config .
COPY ["Invoice.Blazor.Server/Invoice.Blazor.Server.csproj", "Invoice.Blazor.Server/"]
COPY ["CommonLibrary/CommonLibrary.csproj", "CommonLibrary/"]
COPY ["Invoice.Module/Invoice.Module.csproj", "Invoice.Module/"]
COPY ["GUS.Module/GUS.Module.csproj", "GUS.Module/"]
COPY ["CarsDb.Module/CarsDb.Module.csproj", "CarsDb.Module/"]
COPY ["Waluty.Module/WalutyModule.csproj", "Waluty.Module/"]
COPY ["KodyPocztowe.Module/KodyPocztowe.Module.csproj", "KodyPocztowe.Module/"]
COPY ["Invoice.Module.Blazor/Invoice.Module.Blazor.csproj", "Invoice.Module.Blazor/"]
RUN dotnet restore "Invoice.Blazor.Server/Invoice.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Invoice.Blazor.Server"
RUN dotnet build "Invoice.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Invoice.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update
RUN apt-get install -y libxml2-dev;
RUN apt-get install -y libgdiplus
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Invoice.Blazor.Server.dll