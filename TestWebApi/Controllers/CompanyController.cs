/**
 * 
 * @author : Kai Tam    
 *  
*/

using Company.Service.Extensions;
using Company.Service.Interfaces;
using Company.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private const string IsinRegex = @"(^[A-Za-z]{2}\S)(\w+\S$)";
        private const string AlphaNumericRegex = "^[A-Za-z0-9? ,_-]+$";

        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyRetrievalService _companyRetrievalService;

        public CompanyController(ILogger<CompanyController> logger, ICompanyRetrievalService companyRetrievalService)
        {
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
            _companyRetrievalService = companyRetrievalService.ThrowIfArgumentIsNull(nameof(companyRetrievalService));
        }

        [HttpGet, Route("getAllCompanyRecords")]
        public ActionResult<IList<CompanyRecord>> GetAllCompanyRecords()
        {
            var companyRecords = _companyRetrievalService.GetAllCompany();

            return Ok(companyRecords);
        }

        [HttpGet, Route("isin/{isin}")]
        public ActionResult<CompanyRecord> GetCompanyRecordWithIsin(string isin)
        {
            if (string.IsNullOrEmpty(isin))
            {
                return BadRequest();
            }

            if (IsValidString(isin, IsinRegex) == false)
            {
                return BadRequest();
            }

            var companyRecord = _companyRetrievalService.GetCompanyRecordByIsin(isin);

            if (companyRecord == null)
            {
                return NotFound();
            }

            return Ok(companyRecord);
        }

        [HttpGet, Route("id/{id}")]
        public ActionResult<CompanyRecord> GetCompanyRecordWithId(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var companyRecord = _companyRetrievalService.GetCompanyRecordById(id);

            if (companyRecord == null)
            {
                return NotFound();
            }

            return Ok(companyRecord);
        }

        [HttpPatch, Route("update")]
        public ActionResult UpdateCompanyRecord([FromBody] CompanyRecord companyRecordUpdate)
        {
            if (companyRecordUpdate == null || !IsValidCompanyRecord(companyRecordUpdate))
            {
                return BadRequest();
            }

            var companyRecord = _companyRetrievalService.GetCompanyRecordByIsin(companyRecordUpdate.Isin);

            if (companyRecord == null)
            {
                return BadRequest();
            }

            _companyRetrievalService.UpdateCompanyRecord(companyRecordUpdate);

            return Ok();
        }

        [HttpPost, Route("add")]
        public ActionResult AddNewCompanyRecord([FromBody] CompanyRecord newCompanyRecord)
        {
            if (newCompanyRecord == null || !IsValidCompanyRecord(newCompanyRecord))
            {
                return BadRequest();
            }

            var companyRecord = _companyRetrievalService.GetCompanyRecordByIsin(newCompanyRecord.Isin);

            if (companyRecord != null)
            {
                return Conflict();
            }

            _companyRetrievalService.InsertCompanyRecord(newCompanyRecord);

            return Ok();
        }

        /// <summary>
        /// Sanitization of company record to ensure valid values for each props.
        /// Would prefer refactor validation of company records into its own class, but I'll put the simple validation here for the simplicity sake.
        /// </summary>
        /// <param name="companyRecordUpdate"></param>
        /// <returns></returns>
        private bool IsValidCompanyRecord(CompanyRecord companyRecordUpdate)
        {
            if (!IsValidString(companyRecordUpdate.CompanyName, AlphaNumericRegex))
            {
                return false;
            }

            //Dont have insight on the string pattern requirement, hence just regular alphanumeric validation
            if (!IsValidString(companyRecordUpdate.Exchange, AlphaNumericRegex))
            {
                return false;
            }

            //Dont have insight on the string pattern requirement, hence just regular alphanumeric validation
            if (!IsValidString(companyRecordUpdate.Ticker, AlphaNumericRegex))
            {
                return false;
            }

            // Ensureing that the format for isin is the first 2 chars from isin are Letters and conform to the alphanumeric as the entire string
            if (!IsValidString(companyRecordUpdate.Isin, IsinRegex))
            {
                return false;
            }

            // Sanitization on website string to ensure its a well form weblink not just random or malicious string
            if (!Uri.IsWellFormedUriString(companyRecordUpdate.Website, UriKind.RelativeOrAbsolute))
            {
                return false;
            }

            return true;
        }

        private bool IsValidString(string value, string pattern)
        {
            var regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

    }
}
