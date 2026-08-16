
<div align="center">
    <image src="logo.png" style="width:256px;" />
    <br/>
    <span style="font-size:16px;font-weight:bold;">A lightweight, modern, and open-source ADO.NET data provider for MariaDB, built for .NET applications.</span>
</div>

-----

> **Disclaimer**: This is an independent, unofficial .NET provider for MariaDB. It is a thin ADO.NET wrapper and type-mapping layer built on top of [MySql.Data](https://www.nuget.org/packages/mysql.data) and is not affiliated with or endorsed by MariaDB plc or the MariaDB Foundation.

The project aims to provide a dedicated MariaDB connector based on the standard `System.Data.Common` abstractions, while exposing MariaDB-specific data types, behaviors, and capabilities where applicable. All objects will be prefixed by `MariaDb` so it is more standardized in .NET.

It implements the `Async` equivalent of the [MySql.Data](https://www.nuget.org/packages/mysql.data) that is dedicated for MariaDB. It also covers the full implementation of Bulk operations using the `MySql.Data`'s `MySqlBulkLoader` class.

> **Status:** Early development. The API and implementation are subject to change.

## Why is this exists?
As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated MariaDB objects are required within its extension library, [RepoDb.MariaDb](https://www.nuget.org/packages/RepoDb.MariaDb), to avoid class collisions with [RepoDb.MySql](https://www.nuget.org/packages/RepoDb.MySql) and [RepoDb.MySqlConnector](https://www.nuget.org/packages/RepoDb.MySqlConnector). The same applies to its Bulk Operations extension, [RepoDb.MariaDb.BulkOperations](https://www.nuget.org/packages/RepoDb.MariaDb.BulkOperations).

This library will serve as the **official MariaDB connector for RepoDB** and will be used internally by the [RepoDB project](https://github.com/mikependon/RepoDB).

## Goals of the library

MariaDbConnector aims to:

* Provide a dedicated ADO.NET data provider for MariaDB.
* Follow the standard `System.Data.Common` provider architecture.
* Support modern synchronous and asynchronous .NET APIs.
* Provide MariaDB-specific data type mappings and behaviors.
* Remain lightweight and suitable for use by ORMs and other data-access libraries.
* Support high-performance MariaDB operations, including bulk operations, in future releases.
* Remain usable independently of any ORM.

## Core ADO.NET Objects

MariaDbConnector is built around the standard abstractions provided by `System.Data.Common`.

The following provider-specific objects form the core of the connector:

| MariaDbConnector                 | ADO.NET Base Class          | Purpose                                     |
| -------------------------------- | --------------------------- | ------------------------------------------- |
| `MariaDbConnection`              | `DbConnection`              | Establishes and manages MariaDB connections |
| `MariaDbCommand`                 | `DbCommand`                 | Executes SQL commands                       |
| `MariaDbDataReader`              | `DbDataReader`              | Reads query results                         |
| `MariaDbParameter`               | `DbParameter`               | Represents command parameters               |
| `MariaDbParameterCollection`     | `DbParameterCollection`     | Manages command parameters                  |
| `MariaDbTransaction`             | `DbTransaction`             | Manages database transactions               |
| `MariaDbException`               | `DbException`               | Represents MariaDB errors                   |
| `MariaDbConnectionStringBuilder` | `DbConnectionStringBuilder` | Builds and parses connection strings        |
| `MariaDbProviderFactory`         | `DbProviderFactory`         | Creates provider-specific ADO.NET objects   |

The architecture follows the standard ADO.NET provider model:

```text
System.Data.Common
│
├── DbConnection
│     └── MariaDbConnection
│
├── DbCommand
│     └── MariaDbCommand
│
├── DbDataReader
│     └── MariaDbDataReader
│
├── DbParameter
│     └── MariaDbParameter
│
├── DbParameterCollection
│     └── MariaDbParameterCollection
│
├── DbTransaction
│     └── MariaDbTransaction
│
├── DbException
│     └── MariaDbException
│
├── DbConnectionStringBuilder
│     └── MariaDbConnectionStringBuilder
│
└── DbProviderFactory
      └── MariaDbProviderFactory
```

## Basic Usage

MariaDbConnector is intended to provide the familiar ADO.NET programming model.

```csharp
using MariaDbConnector;

var connectionString =
    "Server=localhost;" +
    "Port=3306;" +
    "Database=TestDb;" +
    "User ID=root;" +
    "Password=password;";

await using var connection =
    new MariaDbConnection(connectionString);

await connection.OpenAsync();

await using var command = connection.CreateCommand();

command.CommandText = """
    SELECT Id, Name, Email
    FROM Customer
    WHERE Id = @Id;
    """;

command.Parameters.AddWithValue("@Id", 100);

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    var id = reader.GetInt32(0);
    var name = reader.GetString(1);
    var email = reader.GetString(2);

    Console.WriteLine($"{id}: {name} ({email})");
}
```

## MariaDbConnection

`MariaDbConnection` extends `DbConnection` and represents a connection to a MariaDB server.

```csharp
await using var connection =
    new MariaDbConnection(connectionString);

await connection.OpenAsync();

Console.WriteLine(connection.ServerVersion);
Console.WriteLine(connection.Database);
Console.WriteLine(connection.State);
```

Its responsibilities include:

* Connection establishment and termination
* Connection state management
* MariaDB session management
* Command creation
* Transaction creation
* Connection string handling
* Synchronous and asynchronous operations

## MariaDbCommand

`MariaDbCommand` extends `DbCommand` and represents a SQL statement executed against MariaDB.

```csharp
await using var command = new MariaDbCommand(
    "SELECT * FROM Customer WHERE Id = @Id",
    connection);

command.Parameters.AddWithValue("@Id", 100);

await using var reader =
    await command.ExecuteReaderAsync();
```

The implementation is intended to support:

* `ExecuteNonQuery`
* `ExecuteScalar`
* `ExecuteReader`
* Async equivalents
* Parameterized SQL
* Prepared statements
* Command timeout
* Cancellation
* Multiple result sets

## MariaDbParameter

`MariaDbParameter` extends `DbParameter` and represents a parameter associated with a `MariaDbCommand`.

```csharp
var parameter = new MariaDbParameter
{
    ParameterName = "@Id",
    Value = 100
};

command.Parameters.Add(parameter);
```

A MariaDB-specific type system is also planned:

```csharp
var parameter = new MariaDbParameter
{
    ParameterName = "@Id",
    MariaDbType = MariaDbType.Int,
    Value = 100
};
```

## MariaDB Data Types

MariaDbConnector aims to provide a `MariaDbType` enumeration in addition to the standard ADO.NET `DbType`.

The current set of types includes:

```csharp
public enum MariaDbType
{
    TinyInt,
    SmallInt,
    MediumInt,
    Int,
    BigInt,

    Decimal,
    Float,
    Double,
    Bit,

    Char,
    VarChar,
    TinyText,
    Text,
    MediumText,
    LongText,
    Enum,
    Set,

    Binary,
    VarBinary,
    TinyBlob,
    Blob,
    MediumBlob,
    LongBlob,

    Date,
    Time,
    DateTime,
    Timestamp,
    Year,

    Json,

    Geometry,
    Point,
    LineString,
    Polygon,
    MultiPoint,
    MultiLineString,
    MultiPolygon,
    GeometryCollection
}
```

The connector will provide mappings between:

```text
MariaDbType
     ↕
DbType
     ↕
.NET CLR Type
     ↕
MariaDB Server Type
```

`MariaDbTypeConverter` provides the current leg of that mapping, converting between `MariaDbType` and the underlying `MySql.Data.MySqlClient.MySqlDbType`:

```csharp
var mariaDbType = MariaDbTypeConverter.ToMariaDbType(MySqlDbType.VarChar);
var mySqlDbType = MariaDbTypeConverter.ToMySqlDbType(MariaDbType.BigInt);
```

## Transactions

`MariaDbTransaction` extends `DbTransaction` and provides standard ADO.NET transaction semantics.

```csharp
await using var transaction =
    await connection.BeginTransactionAsync();

try
{
    await using var command = connection.CreateCommand();

    command.Transaction = transaction;

    command.CommandText =
        "UPDATE Customer SET Name = @Name WHERE Id = @Id";

    command.Parameters.AddWithValue("@Name", "John Doe");
    command.Parameters.AddWithValue("@Id", 100);

    await command.ExecuteNonQueryAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

## Connection String Builder

`MariaDbConnectionStringBuilder` extends `DbConnectionStringBuilder` and provides a strongly typed way of creating MariaDB connection strings.

```csharp
var builder = new MariaDbConnectionStringBuilder
{
    Server = "localhost",
    Port = 3306,
    Database = "TestDb",
    UserId = "root",
    Password = "password"
};

await using var connection =
    new MariaDbConnection(builder.ConnectionString);

await connection.OpenAsync();
```

## Provider Factory

`MariaDbProviderFactory` extends `DbProviderFactory` and enables provider-independent ADO.NET applications and libraries to create MariaDbConnector objects.

```csharp
var factory = MariaDbProviderFactory.Instance;

using var connection = factory.CreateConnection();

connection.ConnectionString = connectionString;
connection.Open();
```

## Additional ADO.NET Support

Future releases may provide additional traditional ADO.NET components:

```text
MariaDbDataAdapter
    └── DbDataAdapter

MariaDbCommandBuilder
    └── DbCommandBuilder
```

These components will provide support for `DataTable`, `DataSet`, and other traditional ADO.NET workflows.

The initial development effort will prioritize the core connection, command, parameter, transaction, and data-reader infrastructure.

## Bulk Operations

MariaDbConnector provides bulk-loading support under the `MariaDbConnector.Bulk` namespace, built on top of `LOAD DATA LOCAL INFILE` via `MySql.Data`'s `MySqlBulkLoader`.

| MariaDbConnector.Bulk                       | Purpose                                                                             |
| -------------------------------------------- | ------------------------------------------------------------------------------------ |
| `MariaDbBulkCopy`                            | Efficiently bulk-loads a `DbDataReader`/`IDataReader`, `DataTable`, or `DataRow[]` into a MariaDB table |
| `MariaDbBulkColumnMapping`                   | Defines the mapping between a source column and a destination column                |
| `MariaDbBulkCopyColumnMappingCollection`     | The collection of `MariaDbBulkColumnMapping` objects exposed by `MariaDbBulkCopy.ColumnMappings` |
| `MariaDbBulkLoader`                          | A strongly typed wrapper around `LOAD DATA LOCAL INFILE`, for loading directly from a file or stream |
| `MariaDbBulkLoaderConflictOption`            | Controls how `MariaDbBulkLoader` behaves when a key conflict arises during a load   |
| `MariaDbBulkLoaderPriority`                  | Controls the priority (`None`, `Low`, `Concurrent`) of a `MariaDbBulkLoader` operation |

### MariaDbBulkCopy

`MariaDbBulkCopy` writes its source rows to a temporary file and loads them with `MariaDbBulkLoader`:

```csharp
await using var connection =
    new MariaDbConnection(connectionString);

await connection.OpenAsync();

using var bulkCopy = new MariaDbBulkCopy(connection)
{
    DestinationTableName = "Customer",
    BatchSize = 10_000
};

bulkCopy.ColumnMappings.Add("Id", "Id");
bulkCopy.ColumnMappings.Add("Name", "Name");
bulkCopy.ColumnMappings.Add("Email", "Email");

await bulkCopy.WriteToServerAsync(customersDataTable);

Console.WriteLine(bulkCopy.RowsCopied);
```

`WriteToServer`/`WriteToServerAsync` are overloaded to accept an `IDataReader`, a `DbDataReader`, a `DataTable` (optionally filtered by `DataRowState`), or a `DataRow[]`.

### MariaDbBulkLoader

`MariaDbBulkLoader` can also be used directly for `LOAD DATA LOCAL INFILE`-style loading from a file or stream, without going through `MariaDbBulkCopy`:

```csharp
var bulkLoader = new MariaDbBulkLoader(connection)
{
    TableName = "Customer",
    FileName = "customers.csv",
    FieldTerminator = ",",
    LineTerminator = "\n",
    Local = true
};

bulkLoader.Columns.Add("Id");
bulkLoader.Columns.Add("Name");
bulkLoader.Columns.Add("Email");

var rowsLoaded = await bulkLoader.LoadAsync();
```

## Architecture

MariaDbConnector is more than a set of ADO.NET wrapper classes. The public ADO.NET API will sit on top of the MariaDB communication and protocol infrastructure.

```text
Application / ORM
       │
       ▼
MariaDbConnection
       │
       ▼
MariaDbCommand
       │
       ▼
MariaDB Session
       │
       ├── Authentication
       ├── TLS
       ├── Prepared Statements
       ├── Parameter Encoding
       ├── Result Set Parsing
       ├── Type Encoding/Decoding
       └── Cancellation
               │
               ▼
        MariaDB Protocol
               │
               ▼
              TCP
               │
               ▼
        MariaDB Server
```

## Roadmap

The initial development will focus on the essential ADO.NET provider infrastructure:

1. `MariaDbConnection`
2. `MariaDbCommand`
3. `MariaDbParameter`
4. `MariaDbParameterCollection`
5. `MariaDbDataReader`
6. `MariaDbTransaction`
7. `MariaDbException`
8. `MariaDbConnectionStringBuilder`
9. `MariaDbProviderFactory`
10. `MariaDbType` and `MariaDbTypeConverter`
11. `MariaDbBulkCopy`, `MariaDbBulkColumnMapping`, `MariaDbBulkCopyColumnMappingCollection`, and `MariaDbBulkLoader`

Subsequent development may include:

* Connection pooling
* Prepared statements
* TLS/SSL
* Authentication mechanisms
* Cancellation
* Multiple result sets
* Advanced server metadata
* `MariaDbDataAdapter`
* `MariaDbCommandBuilder`
* Native (non `LOAD DATA`-based) bulk execution protocol
* Performance optimizations

## ORM and Library Integration

Although MariaDbConnector can be used directly through ADO.NET, it is designed to work naturally with libraries that operate against the standard `System.Data.Common` abstractions.

For example:

```text
RepoDB
Dapper
Custom Data Access Layers
ADO.NET Applications
Other DbConnection-based Libraries
          │
          ▼
   MariaDbConnector
          │
          ▼
      MariaDB Server
```

The connector itself should remain independent of any ORM.

## Contributing

MariaDbConnector is in its early stages, and contributions are welcome.

Areas where contributions will be particularly valuable include:

* MariaDB wire protocol implementation
* Authentication
* Type mappings
* Prepared statements
* Connection pooling
* TLS/SSL
* Async I/O
* MariaDB version compatibility
* Performance benchmarking
* Bulk operations
* Integration and compatibility testing

When contributing, please keep the implementation aligned with the standard ADO.NET architecture and avoid unnecessary abstractions that could negatively affect performance.

## License

MariaDbConnector is an independent open-source project. MariaDB is a trademark of its respective owner. This project is not affiliated with, sponsored by, or endorsed by MariaDB plc or the MariaDB Foundation.

[MIT License](LICENSE) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
