/**
* 
* @author : Kai Tam    
*  
*/

using Company.Service.Extensions;
using Company.Service.Interfaces;
using Company.Service.Models;
using System.Collections.Generic;
using System.Data;

namespace Company.Service.Services
{
    public class CompanyRetrievalService : ICompanyRetrievalService
    {
        private readonly IDatabaseFactory _databaseFactory;

        public CompanyRetrievalService(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory.ThrowIfArgumentIsNull(nameof(databaseFactory));
        }

        public IList<CompanyRecord> GetAllCompany()
        {
            var companyRecords = new List<CompanyRecord>();

            using (var sqlConnection = _databaseFactory.GetSqlConnection())
            {
                using var command = sqlConnection.CreateCommand();
                command.CommandText = "EXECUTE sp_GetAllCompany";
                sqlConnection.Open();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    string companyName = reader.GetString(reader.GetOrdinal("CompanyName"));
                    var exchange = reader.GetString(reader.GetOrdinal("Exchange"));
                    var ticker = reader.GetString(reader.GetOrdinal("Ticker"));
                    var isin = reader.GetString(reader.GetOrdinal("Isin"));
                    var website= reader.GetString(reader.GetOrdinal("Website"));

                    companyRecords.Add(new CompanyRecord
                    {
                        Id = id,
                        CompanyName = companyName,
                        Exchange = exchange,
                        Ticker = ticker,
                        Isin = isin,
                        Website = website
                    });                    
                }
                sqlConnection.Close();
            }

            return companyRecords;
        }

        public CompanyRecord GetCompanyRecordByIsin(string isinInput)
        {
            string command = $"EXECUTE sp_GetCompanyWithIsin {isinInput}";
            var companyRecords = GetCompanyRecordFromScript(command);

            return companyRecords;
        }

        public CompanyRecord GetCompanyRecordById(int id)
        {
            string command = $"EXECUTE sp_GetCompanyWithId {id}";
            var companyRecords = GetCompanyRecordFromScript(command);

            return companyRecords;
        }

        private CompanyRecord GetCompanyRecordFromScript(string commandString)
        {
            CompanyRecord companyRecord = null;
            using (var sqlConnection = _databaseFactory.GetSqlConnection())
            {
                using var command = sqlConnection.CreateCommand();
                command.CommandText = commandString;
                sqlConnection.Open();

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    string companyName = reader.GetString(reader.GetOrdinal("CompanyName"));
                    var exchange = reader.GetString(reader.GetOrdinal("Exchange"));
                    var ticker = reader.GetString(reader.GetOrdinal("Ticker"));
                    var isin = reader.GetString(reader.GetOrdinal("Isin"));
                    var website = reader.GetString(reader.GetOrdinal("Website"));

                    companyRecord = new CompanyRecord
                    {
                        Id = id,
                        CompanyName = companyName,
                        Exchange = exchange,
                        Ticker = ticker,
                        Isin = isin,
                        Website = website
                    };
                }
                sqlConnection.Close();
            }

            return companyRecord;
        }

        public void UpdateCompanyRecord(CompanyRecord companyRecord)
        {
            var sqlCommandUpdate = $"EXECUTE sp_UpdateCompanyRecord {GetCompanyRecordSpParams(companyRecord)}";
            UpdateOrInsertToCompanyRecord(sqlCommandUpdate);
        }

        public void InsertCompanyRecord(CompanyRecord companyRecord)
        {
            var sqlCommandUpdate = $"EXECUTE sp_InsertNewCompany  {GetCompanyRecordSpParams(companyRecord)}";
            UpdateOrInsertToCompanyRecord(sqlCommandUpdate);
        }

        private string GetCompanyRecordSpParams(CompanyRecord companyRecord)
        {
            return $"@CompanyName = \'{companyRecord.CompanyName}\', @Exchange = \'{companyRecord.Exchange}\', @Ticker = \'{companyRecord.Ticker}\', @Isin = \'{companyRecord.Isin}\', @Website = \'{companyRecord.Website}\'";
        }

        private void UpdateOrInsertToCompanyRecord(string sqlCommand)
        {
            using (var sqlConnection = _databaseFactory.GetSqlConnection())
            {
                using var command = sqlConnection.CreateCommand();
                command.CommandText = sqlCommand;
                sqlConnection.Open();

                command.ExecuteNonQuery();

                sqlConnection.Close();
            }
        }
    }
}
