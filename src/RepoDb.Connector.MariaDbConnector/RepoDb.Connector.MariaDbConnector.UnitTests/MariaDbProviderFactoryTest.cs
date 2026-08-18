namespace RepoDb.Connector.MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbProviderFactoryTest
    {
        [TestMethod]
        public void TestMariaDbProviderFactoryInstanceForNotNull()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateConnectionForReturnsMariaDbConnection()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<MariaDbConnection>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateCommandForReturnsMariaDbCommand()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<MariaDbCommand>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateParameterForReturnsMariaDbParameter()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<MariaDbParameter>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateConnectionStringBuilderForReturnsMariaDbConnectionStringBuilder()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<MariaDbConnectionStringBuilder>(output);
        }
    }
}
