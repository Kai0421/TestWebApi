/**
* 
* @author : Kai Tam    
*  
*/

using Company.Service.Interfaces;
using Company.Service.Models;
using Company.Service.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace TestWebApi.Test.Services
{
    [TestFixture]
    public class CompanyRetrievalServiceTests
    {
        [Test]
        public void Ctor_WithInvalidArgument_ThrowArgumentNullException()
        {
            //Arrange /Act /Assert
            Assert.Throws<ArgumentNullException>(() => new CompanyRetrievalService(null));
        }

        [Test]
        public void GetAllCompany_NoRecords_ShouldReturnEmptyList()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            readerMock.SetupSequence(_ => _.Read()).Returns(false);

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetAllCompany();

            //Assert
            Assert.AreEqual(0, companyRecords.Count);
        }

        [Test]
        public void GetAllCompany_ShouldReturnAllRecord()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            var expectedCompanyRecords = new List<CompanyRecord>
            {
                new CompanyRecord
                {
                    CompanyName = "Abc",
                    Exchange = "Abc -Exchange",
                    Ticker = "Abc -Ticker",
                    Isin = "Abc -Isin",
                    Website = "Abc -Website",
                }
            };

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true)
                .Returns(false);

            readerMock.Setup(reader => reader.GetOrdinal("CompanyName")).Returns(0);
            readerMock.Setup(reader => reader.GetOrdinal("Exchange")).Returns(1);
            readerMock.Setup(reader => reader.GetOrdinal("Ticker")).Returns(2);
            readerMock.Setup(reader => reader.GetOrdinal("Isin")).Returns(3);
            readerMock.Setup(reader => reader.GetOrdinal("Website")).Returns(4);


            readerMock.Setup(reader => reader.GetString(0)).Returns("Abc");
            readerMock.Setup(reader => reader.GetString(1)).Returns("Abc -Exchange");
            readerMock.Setup(reader => reader.GetString(2)).Returns("Abc -Ticker");
            readerMock.Setup(reader => reader.GetString(3)).Returns("Abc -Isin");
            readerMock.Setup(reader => reader.GetString(4)).Returns("Abc -Website");

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetAllCompany();

            //Assert
            Assert.AreEqual(expectedCompanyRecords.Count, companyRecords.Count);
            Assert.AreEqual(expectedCompanyRecords[0].CompanyName, companyRecords[0].CompanyName);
            Assert.AreEqual(expectedCompanyRecords[0].Exchange, companyRecords[0].Exchange);
            Assert.AreEqual(expectedCompanyRecords[0].Ticker, companyRecords[0].Ticker);
            Assert.AreEqual(expectedCompanyRecords[0].Isin, companyRecords[0].Isin);
            Assert.AreEqual(expectedCompanyRecords[0].Website, companyRecords[0].Website);
        }

        [Test]
        public void GetCompanyRecordByIsin_WithNoRecord_ReturnNull()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            readerMock.SetupSequence(_ => _.Read()).Returns(false);

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetCompanyRecordByIsin("AN125763124");

            //Assert
            Assert.IsNull(companyRecords);
        }

        [Test]
        public void GetCompanyRecordByIsin_WithRecord_ReturnCompanyRecord()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            var expectedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Abc -Isin",
                Website = "Abc -Website",
            };

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true)
                .Returns(false);

            readerMock.Setup(reader => reader.GetOrdinal("CompanyName")).Returns(0);
            readerMock.Setup(reader => reader.GetOrdinal("Exchange")).Returns(1);
            readerMock.Setup(reader => reader.GetOrdinal("Ticker")).Returns(2);
            readerMock.Setup(reader => reader.GetOrdinal("Isin")).Returns(3);
            readerMock.Setup(reader => reader.GetOrdinal("Website")).Returns(4);


            readerMock.Setup(reader => reader.GetString(0)).Returns(expectedCompanyRecord.CompanyName);
            readerMock.Setup(reader => reader.GetString(1)).Returns(expectedCompanyRecord.Exchange);
            readerMock.Setup(reader => reader.GetString(2)).Returns(expectedCompanyRecord.Ticker);
            readerMock.Setup(reader => reader.GetString(3)).Returns(expectedCompanyRecord.Isin);
            readerMock.Setup(reader => reader.GetString(4)).Returns(expectedCompanyRecord.Website);

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetCompanyRecordByIsin("AU456781923");

            //Assert
            Assert.IsNotNull(companyRecords);
            Assert.AreEqual(expectedCompanyRecord.CompanyName, companyRecords.CompanyName);
            Assert.AreEqual(expectedCompanyRecord.Exchange, companyRecords.Exchange);
            Assert.AreEqual(expectedCompanyRecord.Ticker, companyRecords.Ticker);
            Assert.AreEqual(expectedCompanyRecord.Isin, companyRecords.Isin);
            Assert.AreEqual(expectedCompanyRecord.Website, companyRecords.Website);
        }

        [Test]
        public void GetCompanyRecordById_WithNoRecord_ReturnNull()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            readerMock.SetupSequence(_ => _.Read()).Returns(false);

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetCompanyRecordById(1);

            //Assert
            Assert.IsNull(companyRecords);
        }

        [Test]
        public void GetCompanyRecordById_WithRecord_ReturnCompanyRecord()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            var expectedCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Abc -Isin",
                Website = "Abc -Website",
            };

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true)
                .Returns(false);

            readerMock.Setup(reader => reader.GetOrdinal("CompanyName")).Returns(0);
            readerMock.Setup(reader => reader.GetOrdinal("Exchange")).Returns(1);
            readerMock.Setup(reader => reader.GetOrdinal("Ticker")).Returns(2);
            readerMock.Setup(reader => reader.GetOrdinal("Isin")).Returns(3);
            readerMock.Setup(reader => reader.GetOrdinal("Website")).Returns(4);


            readerMock.Setup(reader => reader.GetString(0)).Returns(expectedCompanyRecord.CompanyName);
            readerMock.Setup(reader => reader.GetString(1)).Returns(expectedCompanyRecord.Exchange);
            readerMock.Setup(reader => reader.GetString(2)).Returns(expectedCompanyRecord.Ticker);
            readerMock.Setup(reader => reader.GetString(3)).Returns(expectedCompanyRecord.Isin);
            readerMock.Setup(reader => reader.GetString(4)).Returns(expectedCompanyRecord.Website);

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteReader()).Returns(readerMock.Object);

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            var companyRecords = dbAccessor.GetCompanyRecordById(1);

            //Assert
            Assert.IsNotNull(companyRecords);
            Assert.AreEqual(expectedCompanyRecord.CompanyName, companyRecords.CompanyName);
            Assert.AreEqual(expectedCompanyRecord.Exchange, companyRecords.Exchange);
            Assert.AreEqual(expectedCompanyRecord.Ticker, companyRecords.Ticker);
            Assert.AreEqual(expectedCompanyRecord.Isin, companyRecords.Isin);
            Assert.AreEqual(expectedCompanyRecord.Website, companyRecords.Website);
        }

        [Test]
        public void UpdateCompanyRecord_WithRecord_CallSqlExecution()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            var updateCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Abc -Isin",
                Website = "Abc -Website",
            };

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteNonQuery());

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            dbAccessor.UpdateCompanyRecord(updateCompanyRecord);

            //Assert
            dbFactoryMock.Verify(d => d.GetSqlConnection(),Times.Once);
            sqlConnectionMock.Verify(d => d.CreateCommand(),Times.Once);
            commandMock.Verify(d => d.ExecuteNonQuery(),Times.Once);
        }

        [Test]
        public void InsertCompanyRecord_WithRecord_CallSqlExecution()
        {
            //Arrange
            var dbFactoryMock = new Mock<IDatabaseFactory>();
            var sqlConnectionMock = new Mock<IDbConnection>();
            var commandMock = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            var updateCompanyRecord = new CompanyRecord
            {
                CompanyName = "Abc",
                Exchange = "Abc -Exchange",
                Ticker = "Abc -Ticker",
                Isin = "Ab3247293",
                Website = "www.abc.net",
            };

            dbFactoryMock.Setup(d => d.GetSqlConnection()).Returns(sqlConnectionMock.Object);
            sqlConnectionMock.Setup(s => s.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(c => c.ExecuteNonQuery());

            var dbAccessor = new CompanyRetrievalService(dbFactoryMock.Object);

            //Act
            dbAccessor.InsertCompanyRecord(updateCompanyRecord);

            //Assert
            dbFactoryMock.Verify(d => d.GetSqlConnection(), Times.Once);
            sqlConnectionMock.Verify(d => d.CreateCommand(), Times.Once);
            commandMock.Verify(d => d.ExecuteNonQuery(), Times.Once);
        }

    }
}
