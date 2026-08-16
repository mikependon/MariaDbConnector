# MariaDbConnector

```diff
- This is not the official .NET provider from MariaDB. It is just a thin wrapper and DB-types mapper from MySql.Data. Use with your own disclaimer.
```

**MariaDbConnector** is a lightweight, modern, and open-source **ADO.NET data provider for MariaDB**, built for .NET applications.

The project aims to provide a dedicated MariaDB connector based on the standard `System.Data.Common` abstractions, while exposing MariaDB-specific data types, behaviors, and capabilities where applicable.

> **Status:** Early development. The API and implementation are subject to change.

## Goals

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

Example types include:

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
    Text,
    MediumText,
    LongText,

    Binary,
    VarBinary,
    Blob,
    MediumBlob,
    LongBlob,

    Date,
    Time,
    DateTime,
    Timestamp,
    Year,

    Json,
    Enum,
    Set
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

High-performance bulk operations are an important future capability of MariaDbConnector.

Potential APIs include:

```text
MariaDbBulkCopy
MariaDbBulkCopyOptions
MariaDbBulkCopyColumnMapping
MariaDbBulkCopyColumnMappingCollection
MariaDbBulkLoader
```

The goal is to eventually provide an API such as:

```csharp
await using var connection =
    new MariaDbConnection(connectionString);

await connection.OpenAsync();

var bulkCopy = new MariaDbBulkCopy(connection)
{
    DestinationTableName = "Customer",
    BatchSize = 10_000
};

bulkCopy.ColumnMappings.Add("Id", "Id");
bulkCopy.ColumnMappings.Add("Name", "Name");
bulkCopy.ColumnMappings.Add("Email", "Email");

await bulkCopy.WriteToServerAsync(customers);
```

The underlying implementation may utilize MariaDB-specific high-performance mechanisms such as:

* `LOAD DATA INFILE`
* `LOAD DATA LOCAL INFILE`
* MariaDB bulk execution protocol
* Prepared statement batching

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

Subsequent development may include:

* MariaDB-specific type system
* Connection pooling
* Prepared statements
* TLS/SSL
* Authentication mechanisms
* Cancellation
* Multiple result sets
* Advanced server metadata
* `MariaDbDataAdapter`
* `MariaDbCommandBuilder`
* Native bulk operations
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

MariaDbConnector is an independent open-source project.

MariaDB is a trademark of its respective owner. This project is not affiliated with, sponsored by, or endorsed by MariaDB PLC or the MariaDB Foundation.
