﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LabourersController : ControllerBase
    {
        private readonly RecruitmentSystemContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly LabourerRepo _labourerRepo;

        public LabourersController(RecruitmentSystemContext context, UserManager<SystemUser> userManager, LabourerRepo labourerRepo)
        {
            _context = context;
            _userManager = userManager;
            _labourerRepo = labourerRepo;
        }

        // GET: api/Labourers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<LabourerVM>> GetLabourers()
        {
            var result = _labourerRepo.GetLabourers();
            return Ok(result);
        }

        // GET: api/Labourers/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Labourer")]
        public ActionResult<LabourerVM> GetLabourer(int id)
        {
            var result = _labourerRepo.GetLabourerById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // PUT: api/Labourers/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Labourer")]
        public async Task<IActionResult> PutLabourer(int id, LabourerVM labourerVM)
        {
            if (id != labourerVM.Id)
            {
                return BadRequest();
            }
            try
            {
                await _labourerRepo.UpdateLabourer(labourerVM);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        // POST: api/Labourers
        [HttpPost]
        [Authorize(Roles = "Labourer")]
        public async Task<ActionResult<CompanyVM>> PostLabourer(LabourerVM labourerVM)
        {

            try
            {
                var userId = _userManager.GetUserId(User);

                if (_labourerRepo.GetUserLabourerId(userId).HasValue)
                {
                    return BadRequest(new { message = "Labourer already exist" });
                }

                var result = await _labourerRepo.AddLabourer(labourerVM, userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }
    }
}
