using BookShop.Server.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Transactions;

namespace DataTests
{
    public class TestBase
    {
        private string DbConnection = "Server=localhost;Database=BookShopTest;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected ApplicationDbContext dbContext;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer( DbConnection )
                .Options;

            dbContext = new ApplicationDbContext( options );     
        }


        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}