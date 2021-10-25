/**
 * 
 * @author : Kai Tam
 * 
 */

using Company.Service.Interfaces;
using Company.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestWebApi.Controllers;

namespace TestWebApi.Test
{
    public class CompanyControllerTests
    {

        [Test]
        [TestCaseSource("GetInvalidArguments")]
        public void Ctor_WithInvalidArgument_ThrowArgumentNullException(string expectedparamName, ILogger<CompanyController> logger, ICompanyRetrievalService companyRetrievalService)
        {
            //Arrange /Act /Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new CompanyController(logger, companyRetrievalService));
            Assert.AreEqual(expectedparamName, exception.ParamName);
        }

        private static IEnumerable<TestCaseData> GetInvalidArguments()
        {
            yield return new TestCaseData("logger", null, new Mock<ICompanyRetrievalService>().Object);
            yield return new TestCaseData("companyRetrievalService", new Mock<ILogger<CompanyController>>().Object, null);
        }

        [Test]
        public void GetAllCompanyRecords_WithNoCompanyRecords_ReturnEmptyList()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();
            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            companyRetrievalService.Setup(c => c.GetAllCompany()).Returns(new List<CompanyRecord>());

            //Act
            var response = companyController.GetAllCompanyRecords().Result;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(response);

            var responseCompanyRecords = ((OkObjectResult)response).Value as IList<CompanyRecord>;
            Assert.AreEqual(0, responseCompanyRecords.Count);
            companyRetrievalService.Verify(c => c.GetAllCompany(), Times.Once);
        }

        [Test]
        public void GetAllCompanyRecords_WithNoCompanyRecords_ReturnAllCompany()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyRecords = new List<CompanyRecord>
            {
                new CompanyRecord
                {
                    CompanyName = "Company Abc",
                    Exchange = "Exchnage Abc",
                    Ticker = "Ticker Abc",
                    Isin = "UA1234567890",
                    Website = "abc.dfc"
                }
            };

            companyRetrievalService.Setup(c => c.GetAllCompany()).Returns(companyRecords);

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetAllCompanyRecords().Result;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(response);

            var responseCompanyRecords = ((OkObjectResult)response).Value as IList<CompanyRecord>; 
            Assert.AreEqual(companyRecords.Count, responseCompanyRecords.Count);
            Assert.AreEqual(companyRecords[0].CompanyName, responseCompanyRecords[0].CompanyName);
            Assert.AreEqual(companyRecords[0].Exchange, responseCompanyRecords[0].Exchange);
            Assert.AreEqual(companyRecords[0].Ticker, responseCompanyRecords[0].Ticker);
            Assert.AreEqual(companyRecords[0].Isin, responseCompanyRecords[0].Isin);
            Assert.AreEqual(companyRecords[0].Website, responseCompanyRecords[0].Website);

            companyRetrievalService.Verify(c => c.GetAllCompany(), Times.Once);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void GetCompanyRecordsWithIsin_WithNullAndEmptyIsin_ReturnBadRequest( string isin )
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithIsin(isin);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response.Result);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [TestCase(" ")]
        [TestCase("28364jsdbksd")]
        [TestCase("2A8364jsdbksd")]
        [TestCase("2a8364jsdbksd")]
        [TestCase("A88364jsdbksd")]
        [TestCase(" A88364jsdbksd")]
        [TestCase("AW8364 jsdbksd")]
        [TestCase("AW8364jsdbksd ")]
        [TestCase("A W8364jsdbksd ")]
        [TestCase("AW8364j\"£4567)(*&sdbksd")]
        public void GetCompanyRecordsWithIsin_WithInvalidFirstTwoCharacters_ReturnBadRequest( string isin )
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithIsin(isin);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response.Result);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetCompanyRecordsWithIsin_WithValidIsin_NoRecord_ReturnNotFound()
        {
            //Arrange
            var isin = "AB12638HA";
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(isin)).Returns<CompanyRecord>(null);

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithIsin(isin);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(response.Result);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(isin), Times.Once);
        }

        [Test]
        public void GetCompanyRecordsWithIsin_WithValidIsin_WithRecord_ReturnCompanyRecord()
        {
            //Arrange
            var isin = "AB12638HA";
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();
            var expectedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = isin,
                Website = "Abc -Website",
            };

            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(isin)).Returns(expectedCompanyRecord);

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithIsin(isin).Result;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(response);

            var companyRecords = ((OkObjectResult)response).Value as CompanyRecord;
            Assert.AreEqual(expectedCompanyRecord.CompanyName, companyRecords.CompanyName);
            Assert.AreEqual(expectedCompanyRecord.Exchange, companyRecords.Exchange);
            Assert.AreEqual(expectedCompanyRecord.Ticker, companyRecords.Ticker);
            Assert.AreEqual(expectedCompanyRecord.Isin, companyRecords.Isin);
            Assert.AreEqual(expectedCompanyRecord.Website, companyRecords.Website);

            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(isin), Times.Once);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetCompanyRecordsWithId_WithInvalidId_ReturnBadRequest(int id)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();
           

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithId(id).Result;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);

            companyRetrievalService.Verify(c => c.GetCompanyRecordById(id), Times.Never);
        }

        [Test]
        public void GetCompanyRecordsWithId_WithNotFoundId_ReturnNotFound()
        {
            //Arrange
            var id = 1;
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();
            var expectedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Abs -Isin",
                Website = "Abc -Website",
            };


            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithId(id).Result;

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(response);

            companyRetrievalService.Verify(c => c.GetCompanyRecordById(id), Times.Once);
        }

        [Test]
        public void GetCompanyRecordsWithId_WithValidId_WithRecord_ReturnCompanyRecord()
        {
            //Arrange
            var id = 1;
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();
            var expectedCompanyRecord = new CompanyRecord
            {
                Id = 1,
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Abc -Isin",
                Website = "Abc -Website",
            };

            companyRetrievalService.Setup(c => c.GetCompanyRecordById(id)).Returns(expectedCompanyRecord);

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.GetCompanyRecordWithId(id).Result;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(response);

            var companyRecords = ((OkObjectResult)response).Value as CompanyRecord;
            Assert.AreEqual(expectedCompanyRecord.Id, companyRecords.Id);
            Assert.AreEqual(expectedCompanyRecord.CompanyName, companyRecords.CompanyName);
            Assert.AreEqual(expectedCompanyRecord.Exchange, companyRecords.Exchange);
            Assert.AreEqual(expectedCompanyRecord.Ticker, companyRecords.Ticker);
            Assert.AreEqual(expectedCompanyRecord.Isin, companyRecords.Isin);
            Assert.AreEqual(expectedCompanyRecord.Website, companyRecords.Website);

            companyRetrievalService.Verify(c => c.GetCompanyRecordById(id), Times.Once);
        }

        [Test]
        [TestCase("28364jsdbksd")]
        [TestCase("2A8364jsdbksd")]
        [TestCase("2a8364jsdbksd")]
        [TestCase("A88364jsdbksd")]
        [TestCase(" A88364jsdbksd")]
        [TestCase("AW8364 jsdbksd")]
        [TestCase("AW8364jsdbksd ")]
        [TestCase("A W8364jsdbksd ")]
        [TestCase("AW8364j\"£4567)(*&sdbksd")]
        public void UpdateCompanyName_WithInvalidIsin_ReturnBadRequest(string isin)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = isin,
                Website = "Abc -Website",
            };

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.UpdateCompanyRecord(updatedCompanyRecord) ;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordById(It.IsAny<int>()), Times.Never);
            companyRetrievalService.Verify(c => c.UpdateCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }
        
        [Test]
        [TestCaseSource("InvalidCompanyDetails")]
        public void UpdateCompanyName_WithInvalidCompanyRecord_ReturnBadRequest(CompanyRecord companyRecord)
        {
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.UpdateCompanyRecord(companyRecord);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Never);
            companyRetrievalService.Verify(c => c.UpdateCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }

        private static IEnumerable<TestCaseData> InvalidCompanyDetails()
        {
            yield return new TestCaseData(new CompanyRecord { CompanyName = "£$%^", Exchange = "Abc", Ticker = "Abc", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "jdbcjsd$&%*^(", Exchange = "Abc", Ticker = "Abc", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "£$%^", Ticker = "Abc", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "jdbcjsd$&%*^(", Ticker = "Abc", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "£$%^", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "jdbcjsd$&%*^(", Isin = "BC34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "Abc", Isin = "3&(4235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "Abc", Isin = "BC 34235", Website = "www.abc.test" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "Abc", Isin = "BC34235", Website = "jsdks6$%^&*" });
            yield return new TestCaseData(new CompanyRecord { CompanyName = "Abc", Exchange = "Abc", Ticker = "Abc", Isin = "BC34235", Website = "jsdks.6$%^&*" });
        }

        [Test]
        public void UpdateCompanyName_WithIsinNotFound_ReturnBadRequest()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Ab5676823",
                Website = "www.abc.ie",
            };

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);
            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(It.IsAny<string>())).Returns<CompanyRecord>(null);

            //Act
            var response = companyController.UpdateCompanyRecord(updatedCompanyRecord);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Once);
            companyRetrievalService.Verify(c => c.UpdateCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }

        [Test]
        public void UpdateCompanyName_WithIsinExist_ReturnOk()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);
            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(It.IsAny<string>())).Returns(new CompanyRecord());

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Ab57623",
                Website = "www.abc.ie",
            };

            //Act
            var response = companyController.UpdateCompanyRecord(updatedCompanyRecord);

            //Assert
            Assert.IsInstanceOf<OkResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(updatedCompanyRecord.Isin), Times.Once);
            companyRetrievalService.Verify(c => c.UpdateCompanyRecord(updatedCompanyRecord), Times.Once);
        }

        [Test]
        [TestCase("28364jsdbksd")]
        [TestCase("2A8364jsdbksd")]
        [TestCase("2a8364jsdbksd")]
        [TestCase("A88364jsdbksd")]
        [TestCase(" A88364jsdbksd")]
        [TestCase("AW8364 jsdbksd")]
        [TestCase("AW8364jsdbksd ")]
        [TestCase("A W8364jsdbksd ")]
        [TestCase("AW8364j\"£4567)(*&sdbksd")]
        public void AddNewCompanyRecord_WithInvalidIsin_ReturnBadRequest(string isin)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = isin,
                Website = "Abc -Website",
            };

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.AddNewCompanyRecord(updatedCompanyRecord);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordById(It.IsAny<int>()), Times.Never);
            companyRetrievalService.Verify(c => c.InsertCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }

        [Test]
        [TestCaseSource("InvalidCompanyDetails")]
        public void AddNewCompanyRecord_WithInvalidCompanyRecord_ReturnBadRequest(CompanyRecord companyRecord)
        {
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);

            //Act
            var response = companyController.AddNewCompanyRecord(companyRecord);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Never);
            companyRetrievalService.Verify(c => c.InsertCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }


        [Test]
        public void AddNewCompanyRecord_WithIsinFound_ReturnConflict()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Ab5676823",
                Website = "www.abc.ie",
            };

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);
            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(It.IsAny<string>())).Returns(new CompanyRecord());

            //Act
            var response = companyController.AddNewCompanyRecord(updatedCompanyRecord);

            //Assert
            Assert.IsInstanceOf<ConflictResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(It.IsAny<string>()), Times.Once);
            companyRetrievalService.Verify(c => c.InsertCompanyRecord(It.IsAny<CompanyRecord>()), Times.Never);
        }

        [Test]
        public void AddNewCompanyRecord_WithIsinNotExist_ReturnOk()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<CompanyController>>();
            var companyRetrievalService = new Mock<ICompanyRetrievalService>();

            var companyController = new CompanyController(loggerMock.Object, companyRetrievalService.Object);
            companyRetrievalService.Setup(c => c.GetCompanyRecordByIsin(It.IsAny<string>())).Returns<CompanyRecord>(null);

            var updatedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Ab57623",
                Website = "www.abc.ie",
            };

            //Act
            var response = companyController.AddNewCompanyRecord(updatedCompanyRecord);

            //Assert
            Assert.IsInstanceOf<OkResult>(response);
            companyRetrievalService.Verify(c => c.GetCompanyRecordByIsin(updatedCompanyRecord.Isin), Times.Once);
            companyRetrievalService.Verify(c => c.InsertCompanyRecord(updatedCompanyRecord), Times.Once);
        }

    }
}