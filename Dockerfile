FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY . .
RUN dotnet build -c Release
CMD ["dotnet", "run"]
