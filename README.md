# RepoDb Adapters

This repository is the home of the official [RepoDB](https://github.com/mikependon/RepoDB) database adapters — dedicated, provider-specific ADO.NET implementations that RepoDB relies on for data access and bulk operations.

> **Status:** Early development. The API and implementation are subject to change.

## Why does this exist?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated ADO.NET objects are required for each provider to avoid class collisions and to expose provider-specific data types, behaviors, and capabilities where applicable.

Rather than bundling every provider into a single library, each database provider gets its own adapter project, with its own NuGet package, its own release cadence, and its own documentation, while still following a shared, consistent design across the ADO.NET `System.Data.Common` abstractions.

This repository will progressively host those adapters as they are built, starting with MariaDB.

## Supported Adapters

| Adapter | Database Provider | Source | NuGet | Build Status |
| ------- | ------------------ | ------ | ----- | ------------ |
| [RepoDb.Adapter.MariaDb](src/RepoDb.Adapter.MariaDb/README.md) | MariaDB | [`src/RepoDb.Adapter.MariaDb`](src/RepoDb.Adapter.MariaDb) | [![NuGet](https://img.shields.io/nuget/v/RepoDb.Adapter.MariaDb.svg)](https://www.nuget.org/packages/RepoDb.Adapter.MariaDb) | [![Build](https://img.shields.io/github/actions/workflow/status/mikependon/RepoDb.Adapter.MariaDb/build.yml?branch=main&label=build)](https://github.com/mikependon/RepoDb.Adapter.MariaDb/actions/workflows/build.yml) |

Each adapter lives in its own directory under [`src/`](src) and ships as its own NuGet package. See the adapter's own README for its goals, architecture, usage examples, and roadmap.

## Contributing

Contributions are welcome, whether that means improving an existing adapter or proposing support for a new database provider. Please open an issue to discuss significant changes before submitting a pull request.

## License

[MIT License](LICENSE) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
