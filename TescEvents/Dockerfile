# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Install dependencies
RUN yum install unzip

RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
unzip awscliv2.zip && \
./aws/install
    
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore
    
# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Production -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

COPY /startup/startup.sh /startup/startup.sh
RUN chmod 777 /startup/startup.sh

ENTRYPOINT ["/startup/startup.sh"]
