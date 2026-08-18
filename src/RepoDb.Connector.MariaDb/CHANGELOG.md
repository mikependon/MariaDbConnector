# Changelog for RepoDb.Connector.MariaDb

All notable changes to the connectors in this repository are documented in this file. Each connector lives in its own directory under [`src/`](src) and ships as its own NuGet package with its own release cadence — this file tracks that history in one place, grouped by connector.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and each connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1

Date: 2026-08-18

Initial draft release of the MariaDB connector. Introduces the core ADO.NET provider objects and the bulk-loading support described in the connector's own [README](src/RepoDb.Connector.MariaDb/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.MariaDb` namespace), each wrapping the equivalent [MySql.Data](https://www.nuget.org/packages/mysql.data) type behind the standard `System.Data.Common` provider model:

- `MariaDbConnection` — extends `DbConnection`. Establishes and manages a connection to a MariaDB server, creates `MariaDbCommand` and `MariaDbTransaction` instances, and reports connection state, server version, and data source. Ships with both synchronous (`Open`) and asynchronous (`OpenAsync`) connection establishment.
- `MariaDbCommand` — extends `DbCommand`. Executes SQL statements against a `MariaDbConnection`. Implements `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`, each with an `Async` equivalent (`ExecuteNonQueryAsync`, `ExecuteScalarAsync`, `ExecuteReaderAsync`) that delegates directly to the underlying `MySqlCommand`, plus parameter creation and command preparation.
- `MariaDbDataReader` — extends `DbDataReader`. Reads the forward-only result set produced by a `MariaDbCommand`, exposing typed column accessors (`GetInt32`, `GetString`, `GetDateTime`, etc.). `Read`, `NextResult`, and `IsDBNull` each have an async counterpart (`ReadAsync`, `NextResultAsync`, `IsDBNullAsync`).
- `MariaDbParameter` — extends `DbParameter`. Represents a single named or positional parameter attached to a `MariaDbCommand`.
- `MariaDbParameterCollection` — extends `DbParameterCollection`. The strongly typed collection of `MariaDbParameter` objects exposed by `MariaDbCommand.Parameters`.
- `MariaDbTransaction` — extends `DbTransaction`. Wraps a MariaDB transaction, providing `Commit` and `Rollback` semantics scoped to a `MariaDbConnection`.
- `MariaDbException` — extends `DbException`. Wraps the underlying `MySqlException` so consumers can catch a single, connector-specific exception type instead of depending on `MySql.Data` directly.
- `MariaDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Provides strongly typed properties (`Server`, `Port`, `Database`, `UserId`, `Password`, ...) for building and parsing MariaDB connection strings.
- `MariaDbProviderFactory` — extends `DbProviderFactory`. Lets provider-independent code construct `RepoDb.Connector.MariaDb` ADO.NET objects (`CreateConnection`, `CreateCommand`, `CreateParameter`, ...) without a direct reference to the concrete types.
- `MariaDbType` — an enumeration of MariaDB-specific column types spanning numeric, string, binary, date/time, JSON, and spatial (geometry) types.
- `MariaDbTypeConverter` — converts between `MariaDbType` and the underlying `MySql.Data.MySqlClient.MySqlDbType`, forming one leg of the `MariaDbType` ↔ `DbType` ↔ CLR type ↔ MariaDB server type mapping.

**Bulk operations** (`RepoDb.Connector.MariaDb.Bulk` namespace), built on top of `LOAD DATA LOCAL INFILE`:

- `MariaDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable` (optionally filtered by `DataRowState`), or `DataRow[]` into a MariaDB table, with both synchronous (`WriteToServer`) and asynchronous (`WriteToServerAsync`) overloads for every source type.
- `MariaDbBulkColumnMapping` — defines the mapping between a single source column (by name or ordinal) and a destination column (by name or ordinal).
- `MariaDbBulkCopyColumnMappingCollection` — the collection of `MariaDbBulkColumnMapping` entries exposed by `MariaDbBulkCopy.ColumnMappings`.
- `MariaDbBulkLoader` — a strongly typed wrapper around `MySqlBulkLoader`, for loading directly from a file or stream via `LOAD DATA LOCAL INFILE` without going through `MariaDbBulkCopy`, with `Load`/`LoadAsync` overloads.
- `MariaDbBulkLoaderConflictOption` — controls how `MariaDbBulkLoader` behaves when a key conflict arises during a load.
- `MariaDbBulkLoaderPriority` — controls the priority (`None`, `Low`, `Concurrent`) of a `MariaDbBulkLoader` operation.
