﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystemAPI.Models;
using RecruitmentSystemAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentSystemAPI.Repositories
{
    public class LabourerRepo : BaseRepo
    {
        private readonly UserManager<SystemUser> _userManager;
        public LabourerRepo(RecruitmentSystemContext context, UserManager<SystemUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public IQueryable<LabourerVM> GetLabourers(int count, int page, out int totalRows, bool? orderByTopRated)
        {
            var query = _context.Labourers.Include(l => l.User).AsQueryable();
            totalRows = query.Count();
            if (orderByTopRated.HasValue && orderByTopRated.Value)
            {
                query = query.OrderByDescending(l => l.QualityRating);
            }
            else
            {
                query = query.OrderByDescending(l => l.Id);
            }
            return query.Skip(count * (page - 1)).Take(count)
               .Select(l => new LabourerVM
            {
                Id = l.Id,
                FirstName = l.FirstName,
                LastName = l.LastName,
                PersonalId = l.PersonalId,
                Email = l.User.Email,
                City = l.City,
                Province = l.Province,
                Country = l.Country,
                Address = l.Address,
                Phone = l.Phone,
                IsActive = l.IsActive,
                Sunday = l.Availability.HasFlag(Weekdays.Sunday),
                Monday = l.Availability.HasFlag(Weekdays.Monday),
                Tuesday = l.Availability.HasFlag(Weekdays.Tuesday),
                Wednesday = l.Availability.HasFlag(Weekdays.Wednesday),
                Thursday = l.Availability.HasFlag(Weekdays.Thursday),
                Friday = l.Availability.HasFlag(Weekdays.Friday),
                Saturday = l.Availability.HasFlag(Weekdays.Saturday),
                Skills = _context.LabourerSkills.Where(ls => ls.LabourerId == l.Id)
                  .Select(ls => new BaseSkillsVM
                  {
                      Id = ls.Id,
                      Name = ls.Skill.Name
                  }).ToList(),
                SafetyRating = l.SafetyRating,
                QualityRating = l.QualityRating,
            });
        }


        public IQueryable<BaseLabourersVM> GetLabourersDDL(bool isAdmin, string userId, int? jobId)
        {
            if (!isAdmin)
            {
                var companyId = _context.CompanyUsers.FirstOrDefault(cu => cu.UserId == userId).CompanyId;
                return _context.Labourers.Where(l => _context.LabourerJobs.Include(j => j.Job).Where(j => (!jobId.HasValue || j.JobId == jobId) && j.Job.CompanyId == companyId).Any(j => j.LabourerId == l.Id)).Select(l => new BaseLabourersVM
                {
                    Id = l.Id,
                    FullName = l.FirstName + " " + l.LastName,
                    IsActive = l.IsActive
                });
            }
            else
            {
                return _context.Labourers.Select(l => new BaseLabourersVM
                {
                    Id = l.Id,
                    FullName = l.FirstName + " " + l.LastName,
                    IsActive = l.IsActive

                });
            }
        }

        
        public LabourerVM GetLabourerById(int id)
        {
            return _context.Labourers.Where(l => l.Id == id).Include(l => l.User).Select(l => new LabourerVM
            {
                Id = l.Id,
                FirstName = l.FirstName,
                LastName = l.LastName,
                PersonalId = l.PersonalId,
                Email = l.User.Email,
                City = l.City,
                Province = l.Province,
                Country = l.Country,
                Address = l.Address,
                Phone = l.Phone,
                IsActive = l.IsActive,
                Sunday = l.Availability.HasFlag(Weekdays.Sunday),
                Monday = l.Availability.HasFlag(Weekdays.Monday),
                Tuesday = l.Availability.HasFlag(Weekdays.Tuesday),
                Wednesday = l.Availability.HasFlag(Weekdays.Wednesday),
                Thursday = l.Availability.HasFlag(Weekdays.Thursday),
                Friday = l.Availability.HasFlag(Weekdays.Friday),
                Saturday = l.Availability.HasFlag(Weekdays.Saturday),
                SafetyRating = l.SafetyRating,
                QualityRating = l.QualityRating,
                Skills = _context.LabourerSkills.Where(ls => ls.LabourerId == l.Id)
                      .Select(ls => new BaseSkillsVM
                      {
                          Id = ls.SkillId,
                          Name = ls.Skill.Name
                      }).ToList()
            }).FirstOrDefault();
        }

        public async Task UpdateLabourer(LabourerVM labourerVM)
        {
            var labourer = _context.Labourers.Include(l => l.LabourerSkills).FirstOrDefault(l => l.Id == labourerVM.Id);
            if (labourer == null) throw new KeyNotFoundException();

            if (labourer != null)
            {
                labourer.Id = labourer.Id;
                labourer.FirstName = labourerVM.FirstName;
                labourer.LastName = labourerVM.LastName;
                labourer.PersonalId = labourerVM.PersonalId;
                labourer.City = labourerVM.City;
                labourer.Province = labourerVM.Province;
                labourer.Country = labourerVM.Country;
                labourer.Address = labourerVM.Address;
                labourer.Phone = labourerVM.Phone;
                labourer.IsActive = labourerVM.IsActive;
                labourer.Availability = ConvertWeekdaysToEnum(labourerVM);
            }

            if (labourer.LabourerSkills != null)
            {
                var skillsToDelete = labourer.LabourerSkills.Where(s => !labourerVM.Skills.Any(ls => ls.Id == s.SkillId)).ToList();
                if (skillsToDelete != null && skillsToDelete.Count > 0)
                {
                    _context.LabourerSkills.RemoveRange(skillsToDelete);
                }
            }

            var newSkills = labourerVM.Skills.Where(s => !labourer.LabourerSkills.Any(ls => ls.SkillId == s.Id)).ToList();
            if (newSkills != null && newSkills.Count > 0)
            {
                foreach (var skill in newSkills)
                {
                    var newSkill = new LabourerSkill
                    {
                        LabourerId = labourer.Id,
                        SkillId = skill.Id.Value
                    };
                    _context.Add(newSkill);
                }
            }

            await UpdateUserEmail(labourer.UserId, labourerVM.Email);

            _context.Update(labourer);
            _context.SaveChanges();
        }


        public async Task<LabourerVM> AddLabourer(LabourerVM labourerVM, string userId)
        {
            var labourer = new Labourer
            {
                UserId = userId,
                FirstName = labourerVM.FirstName,
                LastName = labourerVM.LastName,
                PersonalId = labourerVM.PersonalId,
                City = labourerVM.City,
                Province = labourerVM.Province,
                Country = labourerVM.Country,
                Address = labourerVM.Address,
                Phone = labourerVM.Phone,
                IsActive = labourerVM.IsActive,
                Availability = ConvertWeekdaysToEnum(labourerVM)
            };

            _context.Add(labourer);
            var labourerSkills = new List<LabourerSkill>();
            foreach (var skill in labourerVM.Skills)
            {
                var newSkill = new LabourerSkill
                {
                    Labourer = labourer,
                    SkillId = skill.Id.Value
                };
                labourerSkills.Add(newSkill);
                _context.LabourerSkills.Add(newSkill);
            }

            labourer.LabourerSkills = labourerSkills;
            await UpdateUserEmail(userId, labourerVM.Email);
            _context.SaveChanges();
            labourerVM.Id = labourer.Id;
            return labourerVM;
        }

        private async Task UpdateUserEmail(string userId, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.Email != email)
            {
                if (_context.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                {
                    throw new Exception($"Email {email} is already taken");
                }
                user.Email = email;
                user.UserName = email;
                await _userManager.UpdateAsync(user);
            }
        }

        public (int?, string) GetUserLabourerId(string userId)
        {
            var labourer = _context.Labourers.Where(l => l.UserId == userId).FirstOrDefault();
            return (labourer?.Id, labourer?.FirstName);
        }

        private Weekdays ConvertWeekdaysToEnum(LabourerVM labourerVM)
        {
            Weekdays weekdays = 0;
            if (labourerVM.Sunday)
                weekdays |= Weekdays.Sunday;
            if (labourerVM.Monday)
                weekdays |= Weekdays.Monday;
            if (labourerVM.Tuesday)
                weekdays |= Weekdays.Tuesday;
            if (labourerVM.Wednesday)
                weekdays |= Weekdays.Wednesday;
            if (labourerVM.Thursday)
                weekdays |= Weekdays.Thursday;
            if (labourerVM.Friday)
                weekdays |= Weekdays.Friday;
            if (labourerVM.Saturday)
                weekdays |= Weekdays.Saturday;
            return weekdays;
        }
    }
}