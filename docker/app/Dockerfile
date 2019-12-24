FROM mcr.microsoft.com/dotnet/core/runtime:2.2

RUN dotnet --info
RUN pwd
COPY App/bin/Release/netcoreapp2.2/publish /usr/local/supporthub
RUN ls /usr/local/supporthub
WORKDIR /usr/local/supporthub

EXPOSE 8000

ENTRYPOINT ["dotnet", "App.dll"]
