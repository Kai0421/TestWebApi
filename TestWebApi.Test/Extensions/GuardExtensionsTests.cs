/**
* 
* @author : Kai Tam    
*  
*/

using Company.Service.Extensions;
using NUnit.Framework;
using System;

namespace TestWebApi.Test.Extensions
{
    [TestFixture]
    public class GuardExtensionsTests
    {
        [Test]
        public void ThrowIfArgumentIsNullOrEmpty_WithNulldString_ThrowArgumentNullException()
        {
            //Arrange
            string testNull = null;

            //Act /Assert
            Assert.Throws<ArgumentNullException>(() => testNull.ThrowIfArgumentIsNullOrEmpty(nameof(testNull)));
        }

        [Test]
        public void ThrowIfArgumentIsNullOrEmpty_WithEmptyString_ThrowArgumentException()
        {
            //Arrange
            string testNull = String.Empty;

            //Act /Assert
            Assert.Throws<ArgumentException>(() => testNull.ThrowIfArgumentIsNullOrEmpty(nameof(testNull)));
        }

        [Test]
        public void ThrowIfArgumentIsNullOrEmpty_ValidString_DoesntThrowException()
        {
            //Arrange
            var validString = "TestString";

            //Act /Assert
            Assert.DoesNotThrow(() => validString.ThrowIfArgumentIsNullOrEmpty(nameof(validString)));
        }

        [Test]
        public void ThrowIfArgumentIsNull_IsNull_ThrowsArgumentNullException()
        {
            object nullObject = null;
            Assert.Throws<ArgumentNullException>(() => nullObject.ThrowIfArgumentIsNull(nameof(nullObject)));
        }
    }
}
