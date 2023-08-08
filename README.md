# Traditional Handicraft Gallery

## Getting Started

1. Double click the `.sln` file to open the project.
2. Create a new file called `appsettings.Development.json`.
3. Copy the content in `appsettings.template.json` to the newly created `appsettings.Development.json`.
4. Modify any necessary settings in the `appsettings.Development.json`.
5. Go to Package Manager Console (at the bottom). Run `update-database` command.
6. Should be good to go.

## To go to admin page

1. Head to `/admin`.
2. Login with the credential `admin` & `Admin@123`.

## User's credential

1. `user` & `User@123`

## To deploy to Elastic Beanstalk

1. Use t2.micro
2. After successfully launched the Elastic Beanstalk in Visual Studio, go to AWS Web Interface.
3. Navigate to the Elastic Beanstalk dashboard.
4. Select the environment you want to inspect.
5. In the sidebar, select "Configuration".
6. Click the Edit button at the section "Updates, monitoring, and logging".
7. Scroll down to add new Environment properties with the key `ASPNETCORE_ENVIRONMENT` and value `Production`.
8. This will set the application to Production mode and will not rely on the AWS Credentials in appsettings file.
