﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystemAPI.Models;
using RecruitmentSystemAPI.Repositories;
using RecruitmentSystemAPI.ViewModels;

namespace RecruitmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LabourerJobsController : ControllerBase
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly LabourerJobsRepo _labourerJobsRepo;

        public LabourerJobsController(UserManager<SystemUser> userManager, LabourerJobsRepo labourerJobsRepo)
        {
            _userManager = userManager;
            _labourerJobsRepo = labourerJobsRepo;
        }

        // GET: api/LabourerJobs
        [HttpGet]
        public ActionResult<IEnumerable<LabourerJobVM>> GetLabourerJobs(int count = 20, int page = 1, int? jobId = null, int? labourerId = null, int? companyId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            int totalRows;
            var result = _labourerJobsRepo.GetLabourerJobsByUserRole(User, count, page, jobId, labourerId, companyId, out totalRows, fromDate, toDate);
            return Ok(new { result, totalRows });
        }

        // GET: api/LabourerJobs/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("LabourerJobReport")]
        public ActionResult<IEnumerable<LabourerJobReportVM>> GetLabourerJobReport(int count = 20, int page = 1, int? labourerId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            int totalRows;
            var result = _labourerJobsRepo.GetLabourerJobReport(User, count, page, labourerId, out totalRows, fromDate, toDate);
            return Ok(new { result, totalRows });
        }

        [HttpGet]
        [Route("LabourerJobReport/{labourerId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<LabourerJobDetailedReportVM>> GetLabourerJobDetailedReport(int labourerId, DateTime fromDate, DateTime toDate, int count = 20, int page = 1)
        {
            try
            {
                var result = _labourerJobsRepo.GetLabourerJobDetailedReport(labourerId, fromDate, toDate, count, page);
                return Ok(new { result = result.Item2, totalRows = result.Item1 });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }


        // POST: api/LabourerJobs
        [HttpPost]
        [Authorize(Roles = "Labourer")] //TODO: consider to remove the function, bc there is no option to add labourer's job from client
        public ActionResult<LabourerJobVM> PostLabourerJob([FromBody]LabourerJobVM labourerJobVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    var result = _labourerJobsRepo.AddLabourerJob(labourerJobVM, userId);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return StatusCode(500, new { message = e.Message });
                }
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("UpdateJobRating")]
        [Authorize(Roles = "Labourer")]
        public ActionResult UpdateJobRating(int idToGrade, int rating)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _labourerJobsRepo.UpdateJobRating(idToGrade, rating, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }


        [HttpPut("{labourerJobId}")]
        [Authorize(Roles = "Company, Admin")]
        public ActionResult UpdateLabourerJob(int labourerJobId, int? qualityRating, int? safetyRating)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _labourerJobsRepo.UpdateLabourerJob(userId, labourerJobId, User.IsInRole("Admin"), qualityRating, safetyRating);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("GetInvoices")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<InvoiceVM>> GetInvoices(DateTime fromDate, DateTime toDate, int? companyId = null, int count = 20, int page = 1)
        {
            try
            {
                var result = _labourerJobsRepo.GetInvoices(fromDate, toDate, companyId, count, page);
                return Ok(new { result = result.Item2, totalRows = result.Item1 });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
        [HttpGet]
        [Route("GetInvoices/{companyId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<InvoiceDetailsVM>> GetCompanyInvoiceDetails(int companyId, DateTime fromDate, DateTime toDate, int count = 20, int page = 1)
        {
            try
            {
                var result = _labourerJobsRepo.GetCompanyInvoiceDetails(companyId,fromDate, toDate, count, page);
                return Ok(new { result = result.Item2, totalRows = result.Item1 });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
