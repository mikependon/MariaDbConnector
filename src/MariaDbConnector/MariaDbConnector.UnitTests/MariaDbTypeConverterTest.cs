using MySql.Data.MySqlClient;

namespace MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbTypeConverterTest
    {
        [TestMethod]
        public void TestMariaDbTypeConverterToMySqlDbTypeForBigInt()
        {
            // Setup
            var input = MariaDbType.BigInt;

            // Act
            var output = MariaDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Int64, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForVarChar()
        {
            // Setup
            var input = MySqlDbType.VarChar;

            // Act
            var output = MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.AreEqual(MariaDbType.VarChar, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForGuid()
        {
            // Setup
            var input = MySqlDbType.Guid;

            // Act
            var output = MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.AreEqual(MariaDbType.Binary, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMySqlDbTypeForPoint()
        {
            // Setup
            var input = MariaDbType.Point;

            // Act
            var output = MariaDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Geometry, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForVectorThrowsNotSupportedException()
        {
            // Setup
            var input = MySqlDbType.Vector;

            // Act
            void Act() => MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }
    }
}
