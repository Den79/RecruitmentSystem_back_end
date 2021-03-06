﻿using Microsoft.EntityFrameworkCore;
using RecruitmentSystemAPI.Models;
using RecruitmentSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentSystemAPI.Helpers
{
    public class EmailHelper
    {
        private EmailSettings emailSettings;
        private RecruitmentSystemContext dbContext;
        private Job job;
        private List<LabourerJob> labourerJobs;
        private Company company;
        private List<Contact> labourersContactList;

        public EmailHelper(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void SendSchedulesByEmail(RecruitmentSystemContext dbContext, Job job, List<LabourerJob> labourerJobs)
        {
            this.dbContext = dbContext;
            this.job = job;
            this.labourerJobs = labourerJobs;
            this.company = dbContext.Companies.FirstOrDefault(c => c.Id == job.CompanyId);
            this.labourersContactList = GetLabourersContactList();
            BuildEmailToCompanies();
            BuildEmailToLabourers();
        }

        public void SendIncidenReportNotificationToAdmin(RecruitmentSystemContext dbContext, int reportId)
        {
            var adminRoleId = dbContext.Roles.Where(r => r.Name == "Admin").Select(r => r.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(adminRoleId))
            {
                var adminUser = dbContext.Users.FirstOrDefault(u => dbContext.UserRoles.Where(ur => ur.RoleId == adminRoleId).Select(ur => ur.UserId).Contains(u.Id));
                if (adminUser != null)
                {
                    var report = dbContext.IncidentReports.Where(r => r.Id == reportId).Include(r => r.Job).ThenInclude(j => j.Company).FirstOrDefault();
                    if (report != null)
                    {
                        var subject = "New incident report notofication";
                        var emailBodyText = $"New incident report has been submited by {report.Job.Company.Name}. The incident occurred on {report.Date.ToString("dddd, dd MMMM yyyy")} during the job: {report.Job.Title}.\n";
                        emailBodyText += $"Summary: {report.Summary}";
                        var emailBodyHtml = $"<p>New incident report has been submited by {report.Job.Company.Name}.</p><p>The incident occurred on {report.Date.ToString("dddd, dd MMMM yyyy")} during the job: {report.Job.Title}.</p><p>Summary: {report.Summary}</p>";
                        EmailSender emailSender = new EmailSender(emailSettings);
                        emailSender.SendMail(adminUser.Email, subject, emailBodyText, emailBodyHtml);
                    }
                }
            }
        }

        private void BuildEmailToCompanies()
        {
            string scheduleOfAssignedLabourers = "";
            string subject = "[RecruitmentSystem]: The schedule for your job was created.";
            Contact contact = new Contact();
            if (labourersContactList != null)
            {
                EmailSender emailSender = new EmailSender(emailSettings);
                foreach (var lj in labourerJobs)
                {
                    scheduleOfAssignedLabourers += $"<p> {lj.Date.ToString("dddd, dd MMMM yyyy")} ";
                    contact = labourersContactList.Find(lc => lc.id == lj.LabourerId);
                    if (contact != null)
                    {
                        scheduleOfAssignedLabourers += $"{contact.name} </p>";
                    }
                    else
                    {
                        scheduleOfAssignedLabourers += " N/A </p>";
                    }
                }
                string text = $"Dear {company.Name}. The schedule for your job {job.Title} was created.";
                string html = @"<p>Dear " + company.Name + ".</p><p> The schedule for your job " + job.Title + " was created.</p>" +
                              "<p>Start Date: " + job.StartDate.ToString("dddd, dd MMMM yyyy") +
                              "  End Date: " + job.EndDate.ToString("dddd, dd MMMM yyyy") + "</p>" +
                             "<p> Schedule:</p><hr />" + scheduleOfAssignedLabourers + "<p> Congratulations! </p>";
                emailSender.SendMail(company.Email, subject, text, html);
            }
        }

        private void BuildEmailToLabourers()
        {
            if (labourersContactList != null)
            {
                EmailSender emailSender = new EmailSender(emailSettings);
                string subject = "[RecruitmentSystem]: You have been assigned to a new job.";
                foreach (var laborer in labourersContactList)
                {

                    string text = $"Dear {laborer.name}. You have been assigned to a new job.";
                    string html = @"<p>Dear " + laborer.name + ".</p><p> You have been assigned to a new job: " + job.Title +
                                    ".</p><hr /><p> Details </p><hr /><p> Company: " + company.Name + "</p>" +
                                   "<p> Job description: " + job.Description + "</p><p> Location: " +
                                   job.Country + " " + job.Province + " " + job.City + " " + job.Address + "</p>" +
                                   "<hr /><p> Your schedule:</p><hr /><p>" + BuildLabourerSchedule(laborer.id) +
                                   "</p><hr /><p> Congratulations! </p>";

                    emailSender.SendMail(laborer.email, subject, text, html);
                }
            }
        }

        private string BuildLabourerSchedule(int laborerId)
        {
            if (labourerJobs != null)
            {
                string result = "";
                foreach (var labourerJob in labourerJobs)
                {
                    if (labourerJob.LabourerId == laborerId)
                    {
                        result += $"<p>{labourerJob.Date.ToString("dddd, dd MMMM yyyy")}<p>";
                    }
                }
                return result;
            }
            return "";
        }

        private List<Contact> GetLabourersContactList()
        {
            return dbContext.Labourers
                .Where(l => l.LabourerJobs.Any(lj => lj.LabourerId == l.Id))
                .Include(l => l.User)
                .Select(l => new Contact
                {
                    id = l.Id,
                    email = l.User.Email,
                    name = $"{ l.FirstName} {l.LastName}"
                }).ToList();
        }
    }
}
