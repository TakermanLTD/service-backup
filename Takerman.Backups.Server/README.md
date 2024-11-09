# Takerman.Backup

## Migrations
On Takerman.Backup.Server:
dotnet ef migrations add [name] --project ../Takerman.Backup.Data/Takerman.Backup.Data.csproj
dotnet ef database update --project ../Takerman.Backup.Data/Takerman.Backup.Data.csproj
dotnet ef migrations remove

## E2E tests
npx cypress open
cypress run --browser chrome
cypress run --specs path_to_file.cy.js

## Upgrade NPM packages
ncu --upgrade
npm install

## Concept
What do I have to backup:
- MS SQL Server databases
- MySQL databases
- Wordpress websites
- Configurations

How I want to use it:
- I want on schedule to keep the last 7 days, the last 3 months and the last 3 years
- I want manually to backup projects

How to do it:
- I need to create a list with projects
- I need to create a log with ran jobs
- I want for each project to have a specific list with small tasks to do
- I want  to create folders of packages for projects with date and to zip them
- I want then to keep the zips to a folder and sync the folder to google drive
- I want to execute the sync on schedule