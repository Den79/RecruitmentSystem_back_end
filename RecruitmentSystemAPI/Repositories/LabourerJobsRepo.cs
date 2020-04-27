﻿using Microsoft.EntityFrameworkCore;
using RecruitmentSystemAPI.Models;
using RecruitmentSystemAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentSystemAPI.Repositories
{
    public class LabourerJobsRepo : BaseRepo
    {
        public LabourerJobsRepo(RecruitmentSystemContext context) : base(context)
        {
        }

        public IQueryable<LabourerJobVM> GetLabourerJobsByUserId(string userId, int count, int page, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _context.LabourerJobs
                .Where(l => (!fromDate.HasValue || l.Date >= fromDate) && (!toDate.HasValue || l.Date < toDate))
                .Include(l => l.Skill).Include(l => l.Labourer).Where(l => l.Labourer.UserId == userId)
                .Include(l => l.Job).OrderByDescending(l => l.Date).Skip(count * (page - 1)).Take(count).Select(l => new LabourerJobVM
                {
                    Id = l.Id,
                    JobTitle = l.Job.Title,
                    SkillName = l.Skill.Name,
                    Date = l.Date,
                    SafetyRating = l.SafetyRating,
                    QualityRating = l.QualityRating,
                    JobRating = l.JobRating,
                    WageAmount = l.WageAmount,
                    CompanyAddress = l.Job.Company.Name,
                    CompanyName = l.Job.Address
                });
        }

        public LabourerJobVM AddLabourerJob(LabourerJobVM labourerJobVM, string userId)
        {
            var labourerSkill = _context.LabourerSkills.Where(ls => ls.SkillId == labourerJobVM.SkillId.Value).Include(ls => ls.Labourer).Where(ls => ls.Labourer.UserId == userId).FirstOrDefault();
            var labourerJob = new LabourerJob
            {
                JobId = labourerJobVM.JobId.Value,
                SkillId = labourerSkill.SkillId,
                Labourer = labourerSkill.Labourer,
                Date = labourerJobVM.Date,
                JobRating = labourerJobVM.JobRating,
                QualityRating = labourerJobVM.QualityRating,
                SafetyRating = labourerJobVM.SafetyRating,
                WageAmount = labourerJobVM.WageAmount
            };
            _context.LabourerJobs.Add(labourerJob);
            _context.SaveChanges();
            labourerJobVM.Id = labourerJob.Id;
            return labourerJobVM;
        }

        public void UpdateJobRating(int idToGrade, int rating, string userId)
        {
            var jobToRate = _context.LabourerJobs.Where(ls => ls.Id == idToGrade && ls.Labourer.UserId == userId).FirstOrDefault();

            if (jobToRate.JobRating ==null){

                //update rating in labourerjobs table
                jobToRate.JobRating = rating;
                _context.Update(jobToRate);
                _context.SaveChanges();

                //update rating in jobs table
                var jobId = jobToRate.JobId;
                var jobInTable = _context.Jobs.Where(j => j.Id == jobId).FirstOrDefault();
                var previousJobRate = jobInTable.Rating;
                var allJobWithSameId = _context.LabourerJobs.Where(ls => ls.JobId == jobId).Select(j => j.JobRating).Where(v => v != 0 && v != null).ToArray();
                var newJobRating = (float)allJobWithSameId.Average();
                jobInTable.Rating = newJobRating;
                _context.Update(jobInTable);
                _context.SaveChanges();

                //update rating in companies table
                var CompanyToRate = _context.Companies.Where(c => c.Id == jobInTable.CompanyId).FirstOrDefault();
                var allJobWithSameCompanyId = _context.Jobs.Where(j => j.CompanyId == jobInTable.CompanyId).Select(j => j.Rating).Where(v => v!= 0 && v != null).ToArray();
                var newCompanyRating = (float)allJobWithSameCompanyId.Average();
                CompanyToRate.Rating = newCompanyRating;
                _context.Update(CompanyToRate);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("You have already graded this job");
            }
        
        }
    }
}