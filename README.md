# sprig

## Server

** Requirements:
- dotnet8.0

** Building
- `dotnet build`

** Testing
- `dotnet test`

## Client

**Requirements: (Version used during development)**
- node (v20.11.0)
- npm (10.4.0)
- typescript (5.3.3)
    - Can be installed with `npm install -g typescript`

**How to build to production from source:**
1. Go into the client directory: `cd client`
1. Compile the project: `npm run build`
1. Start an HTTP server: `npm run http`
1. Open a web browser and visit: [localhost:8080](http://localhost:8080/)
1. Enjoy!

**How to build during development:**
1. Open two terminals
1. Navigate both of them into the client directory: `cd client`
1. In terminal "A": Compile the project in "watch" mode: `npm run watch`
1. In terminal "B": Start an HTTP server: `npm run http`
1. Open a web browser and visit: [localhost:8080](http://localhost:8080/)
1. Edit the typescript files in `client/src/*.ts` which will automatically compile
