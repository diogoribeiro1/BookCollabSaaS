FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build

WORKDIR /src

COPY ["BookCollabSaaS.sln", "./"]
COPY ["BookCollabSaaS.Domain/BookCollabSaaS.Domain.csproj", "BookCollabSaaS.Domain/"]
COPY ["BookCollabSaaS.Application/BookCollabSaaS.Application.csproj", "BookCollabSaaS.Application/"]
COPY ["BookCollabSaaS.Infrastructure/BookCollabSaaS.Infrastructure.csproj", "BookCollabSaaS.Infrastructure/"]
COPY ["BookCollabSaaS.Presentation/BookCollabSaaS.Presentation.csproj", "BookCollabSaaS.Presentation/"]

RUN dotnet restore "BookCollabSaaS.Presentation/BookCollabSaaS.Presentation.csproj"

COPY . .
WORKDIR "/src/BookCollabSaaS.Presentation"
RUN dotnet publish "BookCollabSaaS.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8090

ENTRYPOINT ["dotnet", "BookCollabSaaS.Presentation.dll"]
