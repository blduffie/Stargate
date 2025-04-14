# Introduction

Stargate is a .NET application for tracking astronaut duties, ranks, and service records. It leverages .NET 8, SQLite for local data storage, and optionally Angular for a front-end client. Below are the steps to get set up and running.

## Getting Started

Follow these steps to install and run Stargate on your own machine. Review [Getting Started with C# in VS Code](https://code.visualstudio.com/docs/csharp/get-started)

## Software Dependencies

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET CLI/SDK](https://dotnet.microsoft.com/en-us/download)
  - At least version 8 (8.0.5). You can verify installation by running: `dotnet --version`
  - For macOS:
  - Set the following two environment variables in your shell profile: `export DOTNET_ROOT=$HOME/.dotnet` `export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools`
  - Here are more setup steps for [macs](https://learn.microsoft.com/en-us/dotnet/core/install/macos#install-net-with-a-script)
- [SQLite](https://www.sqlite.org/download.html) (optional if you only want in-memory EF usage, but recommended)
  - For macOS users: `brew install sqlite`
- [Node.js](https://nodejs.org/en/download/)
  - [npm](https://docs.npmjs.com/about-npm) is included with Node
- [Angular CLI](https://angular.io/docs)
  - `npm install -g @angular/cli`
- [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [C# Devkit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
  - You are required to sign in to a Visual Studio subscription to use C# Dev Kit. Check out the Signing in to C# Dev Kit documentation to learn more [here](https://code.visualstudio.com/docs/csharp/signing-in)
- [.NET Install Tool](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-runtime)

## Installation Process

1. **Clone the Stargate repository**:

2. **Restore .NET dependencies**:
   - Navigate to the `Stargate/src/api` folder and run the following

   ```
   dotnet build
   dotnet restore
   ```

3. **Apply EF migrations** (creates or updates the local `starbase.db` file):

   ```
   dotnet ef database update
   ```

4. **Run the API**:

   ```
   dotnet run --launch-profile https
   ```

   By default, it listens on `http://localhost:5204` and `https://localhost:7204`. You can access Swagger at `http://localhost:5204/swagger`.

## Trust the Development SSL Certificates (Optional)

- Run `dotnet dev-certs https --trust` and follow prompts.
- **(Optional) Angular Development Certificate**
- If you create a self-signed certificate for Angular, install it in the Trusted Root Certification Authorities store. However, if you're just making plain HTTP requests from Angular to the .NET API's HTTPS endpoint, you generally only need the .NET dev-cert

## Build and Test the Angular App (If Applicable)

1. Open the Angular project folder (e.g., `web-client`) in VS Code.
2. Install dependencies:

   ```
   npm install
   ng build
   ```

3. Serve locally:

   ```
   ng serve -o
   ```

   This opens a browser at `http://localhost:4200`.
4. Run tests:

   ```
   ng test
   ```

   Press Ctrl + C to stop the tests.

## Angular App Organization

The Angular app uses multiple modules to organize code:

- **AppModule**
  The entry-point of the application. It loads the root `AppComponent`, manages top-level routing, and imports the `SharedModule`.
- **SharedModule**
  Contains components, directives, and models shared across the application. Also re-exports common Angular Material modules or other third-party modules.
- **Feature Modules**
  Each domain-specific feature has its own feature module located in `app/modules/<feature>`.
  - Each feature module is ideally self-contained and can be lazy-loaded.
  - Shared or common logic goes into the `SharedModule`.

# License

Distributed under the [MIT License](https://opensource.org/licenses/MIT). See the LICENSE file for more details.
