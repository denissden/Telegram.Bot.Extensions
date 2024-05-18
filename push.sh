#!/bin/bash

dotnet publish

dotnet nuget push Telegram.Bot.Extensions.Hosting/bin/Release/*.nupkg -k $APIKEY --source nuget.org --skip-duplicate
dotnet nuget push Telegram.Bot.Extensions.EntityFrameworkCore/bin/Release/*.nupkg -k $APIKEY --source nuget.org --skip-duplicate
dotnet nuget push Telegram.Bot.Extensions.Roles/bin/Release/*.nupkg -k $APIKEY --source nuget.org --skip-duplicate
