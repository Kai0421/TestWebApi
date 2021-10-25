/**
* 
* @author : Kai Tam    
*  
*/

using Company.Service.Factories;
using NUnit.Framework;
using System;

namespace TestWebApi.Test.Factories
{
    [TestFixture]
    public class DatabaseFactoryTests
    {
        [Test]
        public void Ctor_WithNullDbConnectionString_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DatabaseFactory(null));
        }

        [Test]
        public void Ctor_WithEmptyDbConnectionString_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new DatabaseFactory(string.Empty));
        }
    }
}
