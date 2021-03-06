FROM microsoft/dotnet:latest

ENV ASPNETCORE_ENVIRONMENT="Production"

RUN apt-get update
RUN wget -qO- https://deb.nodesource.com/setup_6.x | bash -
RUN apt-get install -y build-essential nodejs

COPY . /app
WORKDIR /app/src/Blockchain.Investments.Web

RUN ["dotnet", "restore"]
CMD dotnet publish -c Debug -o bin/output
CMD dotnet bin/output/Blockchain.Investments.Web.dll --server.urls http://0.0.0.0:$PORT
