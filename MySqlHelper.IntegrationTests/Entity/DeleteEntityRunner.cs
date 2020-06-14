using System;
using System.Collections.Generic;
using System.Text;
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using NUnit.Framework;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class DeleteEntityRunner
    {
        private EntityFactory entityFactory;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationSettings();
            entityFactory = new EntityFactory(config.ConnectionString);
        }


        [Test]
        public void DeleteRegisterByModel()
        {
            Assert.Fail();

        }
    }
}
