/**
* 
* @author : Kai Tam    
*  
*/

using System.Collections.Generic;
using Company.Service.Models;

namespace Company.Service.Interfaces
{
    public interface ICompanyRetrievalService
    {
        /// <summary>
        /// Get All Company Records
        /// </summary>
        /// <returns></returns>
        IList<CompanyRecord> GetAllCompany();

        /// <summary>
        /// Get Company Record by Isin
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        CompanyRecord GetCompanyRecordByIsin(string isin);

        /// <summary>
        /// Get Company Record By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyRecord GetCompanyRecordById(int id);

        /// <summary>
        /// Update Company Existing Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyName"></param>
        void UpdateCompanyRecord(CompanyRecord companyRecord);

        /// <summary>
        /// Insert Company Record
        /// </summary>
        /// <param name="companyRecord"></param>
        void InsertCompanyRecord( CompanyRecord companyRecord);
    }
}
