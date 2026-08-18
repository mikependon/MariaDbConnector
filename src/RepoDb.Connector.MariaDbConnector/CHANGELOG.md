# Changelog for RepoDb.Connector.MariaDbConnector

All notable changes to the connectors in this repository are documented in this file. Each connector lives in its own directory under [`src/`](src) and ships as its own NuGet package with its own release cadence — this file tracks that history in one place, grouped by connector.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and each connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1

Date: 2026-08-18

> **Disclaimer**: `RepoDb.Connector.MariaDbConnector` started life as a 1:1 file copy of [RepoDb.Connector.MariaDb](https://www.nuget.org/packages/RepoDb.Connector.MariaDb) — same classes, same folder layout, same tests — and was then refactored to run on [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) instead of Oracle's `MySql.Data`. The "Refactor Notes" at the bottom of the `0.0.1` entry below detail exactly what that refactor touched.

Initial draft release of the MariaDB connector built on [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) — the `MySqlConnector`-based counterpart to [RepoDb.Connector.MariaDb](https://www.nuget.org/packages/RepoDb.Connector.MariaDb), which is built on Oracle's `MySql.Data` instead. Introduces the core ADO.NET provider objects and the bulk-copy support described in the connector's own [README](src/RepoDb.Connector.MariaDbConnector/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.MariaDbConnector` namespace), each wrapping the equivalent [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) type behind the standard `System.Data.Common` provider model:

- `MariaDbConnection` — extends `DbConnection`. Establishes and manages a connection to a MariaDB server, creates `MariaDbCommand` and `MariaDbTransaction` instances, and reports connection state, server version, and data source. Ships with both synchronous (`Open`) and asynchronous (`OpenAsync`) connection establishment.
- `MariaDbCommand` — extends `DbCommand`. Executes SQL statements against a `MariaDbConnection`. Implements `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`, each with an `Async` equivalent (`ExecuteNonQueryAsync`, `ExecuteScalarAsync`, `ExecuteReaderAsync`) that delegates directly to the underlying `MySqlCommand`, plus parameter creation and command preparation.
- `MariaDbDataReader` — extends `DbDataReader`. Reads the forward-only result set produced by a `MariaDbCommand`, exposing typed column accessors (`GetInt32`, `GetString`, `GetDateTime`, etc.). `Read`, `NextResult`, and `IsDBNull` each have an async counterpart (`ReadAsync`, `NextResultAsync`, `IsDBNullAsync`).
- `MariaDbParameter` — extends `DbParameter`. Represents a single named or positional parameter attached to a `MariaDbCommand`.
- `MariaDbParameterCollection` — extends `DbParameterCollection`. The strongly typed collection of `MariaDbParameter` objects exposed by `MariaDbCommand.Parameters`.
- `MariaDbTransaction` — extends `DbTransaction`. Wraps a MariaDB transaction, providing `Commit` and `Rollback` semantics scoped to a `MariaDbConnection`.
- `MariaDbException` — extends `DbException`. Wraps the underlying `MySqlException` so consumers can catch a single, connector-specific exception type instead of depending on `MySqlConnector` directly.
- `MariaDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Provides strongly typed properties (`Server`, `Port`, `Database`, `UserId`, `Password`, ...) for building and parsing MariaDB connection strings.
- `MariaDbProviderFactory` — extends `DbProviderFactory`. Lets provider-independent code construct `RepoDb.Connector.MariaDbConnector` ADO.NET objects (`CreateConnection`, `CreateCommand`, `CreateParameter`, ...) without a direct reference to the concrete types.
- `MariaDbType` — an enumeration of MariaDB-specific column types spanning numeric, string, binary, date/time, JSON, and spatial (geometry) types.
- `MariaDbTypeConverter` — converts between `MariaDbType` and the underlying `MySqlConnector.MySqlDbType`, forming one leg of the `MariaDbType` ↔ `DbType` ↔ CLR type ↔ MariaDB server type mapping.

**Bulk operations** (`RepoDb.Connector.MariaDbConnector.Bulk` namespace), built directly on `MySqlConnector`'s `MySqlBulkCopy`:

- `MariaDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable` (optionally filtered by `DataRowState`), or `DataRow[]` into a MariaDB table, with both synchronous (`WriteToServer`) and asynchronous (`WriteToServerAsync`) overloads for every source type. Internally wraps a `MySqlBulkCopy`, resolving any name-based column mapping against the source schema and the destination table's `SHOW COLUMNS` output before delegating the write. Unlike the `MySql.Data`-based `RepoDb.Connector.MariaDb` package, there is no `BatchSize` property or `MariaDbBulkLoader`/`LOAD DATA LOCAL INFILE` path — `MySqlBulkCopy` has no batching concept to expose.
- `MariaDbBulkColumnMapping` — defines the mapping between a single source column (by name or ordinal) and a destination column (by name or ordinal).
- `MariaDbBulkCopyColumnMappingCollection` — the collection of `MariaDbBulkColumnMapping` entries exposed by `MariaDbBulkCopy.ColumnMappings`.

#### Refactor Notes

Refactored `RepoDb.Connector.MariaDbConnector` (the copy under `src/`) into a real, independent MySqlConnector-based sibling of `RepoDb.Connector.MariaDb`:

**Renaming (project + namespace, class names untouched)**
- Inner project/test folders, `.csproj`/`.slnx` files, `AssemblyName`/`Title`, and every `namespace`/`using` declaration renamed `RepoDb.Connector.MariaDb` → `RepoDb.Connector.MariaDbConnector` (and `.Bulk`/`.UnitTests`/`.IntegrationTests` variants). All `MariaDb*` class names (`MariaDbConnection`, `MariaDbCommand`, etc.) were left unchanged.
- Also renamed the integration-test database identifier (`RepoDb.Connector.MariaDb` → `RepoDb.Connector.MariaDbConnector`) so the two packages' integration tests don't collide against a shared MariaDB instance.
- Fixed a couple of copy artifacts found along the way: the `.IntegrationTests.csproj` was missing its `ProjectReference` entirely, and `PackageProjectUrl`/`RepositoryUrl` pointed at the wrong repo.

**Library swap**
- `PackageReference` swapped from `MySql.Data` 26.7.0 → `MySqlConnector` 2.6.1; every `using MySql.Data.MySqlClient;` → `using MySqlConnector;`. Verified via reflection against the real MySqlConnector assembly that every `MySqlDbType`/`MySqlException`/etc. member this code touches has a match — everything besides `MariaDbBulkCopy` needed nothing more than the namespace swap.

**Bulk copy: removed `MariaDbBulkLoader`, rebuilt on `MySqlBulkCopy`**
- Deleted `MariaDbBulkLoader.cs`, `MariaDbBulkLoaderConflictOption.cs`, `MariaDbBulkLoaderPriority.cs`, and their unit test — all pure remnants of the old `LOAD DATA LOCAL INFILE` implementation, with no other callers.
- Rewrote `MariaDbBulkCopy` to wrap `MySqlConnector.MySqlBulkCopy` directly instead of hand-writing/escaping a temp file. It still preserves the full 4-way column-mapping API (source name/ordinal × destination name/ordinal) by resolving names/ordinals against the source schema and a `SHOW COLUMNS` lookup before handing a `List<MySqlBulkCopyColumnMapping>` to the underlying `MySqlBulkCopy`.
- Dropped the `BatchSize` property — `MySqlBulkCopy` has no batching concept to back it with, so it was removed rather than kept as a property that would silently do nothing (its now-defunct unit test was removed too). This is a deliberate, documented difference from the `MySql.Data`-based sibling package.

**Verification**
- Full solution builds clean (0 warnings/errors).
- Fixed three unit tests whose assertions baked in `MySql.Data`-specific behavior that genuinely differs in MySqlConnector (verified each via a throwaway probe against the real library): default `CommandTimeout` is `0` not `30`; `ResetDbType()` actually re-infers from `Value` (→ `String`, not left at `Int32`); a cancelled `OpenAsync` throws `OperationCanceledException`, not `TaskCanceledException`.
- Ran everything against a live MariaDB container: 70/70 unit tests and 43/43 integration tests pass, including the full `WriteToServerTest` suite exercising the rebuilt bulk-copy path end-to-end.

**Docs**
- Updated the connector's own `README.md` and `CHANGELOG.md` (renamed throughout, `MySql.Data`→`MySqlConnector`, rewrote the Bulk Operations section, corrected the license attribution to MySqlConnector's MIT license, and added a cross-reference note to the `RepoDb.Connector.MariaDb` sibling).

**Not touched (out of scope for this refactor)**
- The root `README.md`'s "Supported Connectors" table and root `CHANGELOG.md` don't yet list this new package, and there's no CI workflow for it.
