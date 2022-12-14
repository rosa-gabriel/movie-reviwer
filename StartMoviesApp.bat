@echo off
start cmd.exe /C "cd API && dotnet run --launch-profile GitHub"
cd client-app 
npm start

