FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app

#COPY */*.csproj ./
#RUN dotnet restore

#COPY . ./
#RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5000
ENTRYPOINT ["dotnet", "WebAPI/WebAPI.dll"]