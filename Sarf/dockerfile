FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /opt/build

COPY Sarf/ Sarf/
WORKDIR Sarf
COPY StandardShared ../StandardShared
RUN apt update && apt install python3 -y
COPY shared_path_fix.py .

RUN python3 shared_path_fix.py Sarf.sln Sarf.csproj ../StandardShared/StandardShared/StandardShared.csproj
# CMD sleep 36500
RUN dotnet restore Sarf.sln

RUN mkdir /opt/app
RUN dotnet publish Sarf.sln -c Release -o /opt/app/

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /opt/app
COPY --from=build-env /opt/build/ .
WORKDIR /opt/app/Sarf/bin/Release/net6.0

EXPOSE 7002

ENTRYPOINT ["dotnet", "Sarf.dll"]
