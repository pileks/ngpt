# Next Generation Placement Tests

This is the public repository for the [NGPT Project](https://nextgenplacement.org/) and [its associated application](https://app.nextgenplacement.org/).

## Setup & configuration

### Requirements

NGPT is built using ASP.Net Core 3 and Angular.
It's built and tested running inside a Windows environment.

### NodeJS Version

Read the contents of `.nvmrc` to see which version of Node is required.

### Project setup

#### Application configuration

Inside `src/Ngpt.Web` you will find an `appsettings.json.template` file. Copy it and rename it into `appsettings.json`, and configure your settings.

#### Backend-to-Frontend exporter

NGPT automatically generates various services and models for the front-end based on the `Ngpt.Web` project.
Inside `src/Platform/Ngpt.Platform.BackendToFrontendExporter` you will find an `appsettings.json.template` file. Copy it and rename it into `appsettings.json`. Inside, set the `SolutionDirectory` property to the absolute path of your solution's directory, the `src` folder.

#### Installing `node_modules` for the front-end

Inside `src/Ngpt.Web/ClientApp`, run `npm i`.

#### Build once before running

NGPT might require you to build the entire solution once before running the project.

### Database

Inside the `database/backups` you will find `ngpt.bat`, which is a copy of the NGPT database with one default user.

#### Database migrator

NGPT uses a small app found inside `databse/mig-tool` to set up and migrate the database.

Inside that directory, you will find `DbMigrationTool.exe.config.template` which you should copy and rename to `DbMigrationTool.exe.config`.
Edit the `connectionStrings` section to point to your local database.
Edit the `appSettings` section:
- `DatabaseName` - the name of your database
- `PathToScriptsDirectory` - absolute path to `src/database/scripts`
- `DatabaseBackupsDirectoryPath` - absolute path to `src/database/backups`

Inside the project's root folder, there is a `mig.bat` file, which you can call via CMD or PowerShell to perform migrations. The most important commands are:
- `mig help` - get help for the tool
- `mig load` - load a backup from the configured backups directory
- `mig save` - save a backup to the configured backups directory
- `mig run` - run any new migrations

You can load the default `ngpt.bat` database by simply running `mig load ngpt` in the root directory, once the migrator has been configured.

You should add all new migrations to `src/database/scripts`. All scripts and their respective directories are ordered by number, and run in that exact order.

### Resources

NGPT saves user-uploaded resources to disk, the exact location is configured using the `UploadedResources.Directory` setting within `appsettings.json` of `Ngpt.Web`.

You can copy the `resources` directory to a suitable place in order to have access to all resources referenced in the `ngpt.bak` backup of the database.

### Running the app

Run the `Ngpt.Web` project inside the `Ngpt` solution in order to debug/test the app.

If you restored the `ngpt` database backup, you can log in using user `ngpt@ngpt.dev`, password `ngpt`.

## License

NGPT is licensed under the MIT license.