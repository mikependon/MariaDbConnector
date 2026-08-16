using System.Data;

namespace MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbCommandTest
    {
        [TestMethod]
        public void TestMariaDbCommandCommandTextForConstructorWithCommandText()
        {
            // Setup
            using var command = new MariaDbCommand("SELECT 1");

            // Act
            var output = command.CommandText;

            // Assert
            Assert.AreEqual("SELECT 1", output);
        }

        [TestMethod]
        public void TestMariaDbCommandCommandTimeoutForDefaultValue()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.CommandTimeout;

            // Assert
            Assert.AreEqual(30, output);
        }

        [TestMethod]
        public void TestMariaDbCommandCommandTypeForDefaultValue()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.CommandType;

            // Assert
            Assert.AreEqual(CommandType.Text, output);
        }

        [TestMethod]
        public void TestMariaDbCommandParametersForNewCommand()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.Parameters;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestMariaDbCommandDesignTimeVisibleForGetSet()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            command.DesignTimeVisible = false;

            // Assert
            Assert.IsFalse(command.DesignTimeVisible);
        }

        [TestMethod]
        public void TestMariaDbCommandUpdatedRowSourceForGetSet()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            command.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            // Assert
            Assert.AreEqual(UpdateRowSource.FirstReturnedRecord, command.UpdatedRowSource);
        }
    }
}
