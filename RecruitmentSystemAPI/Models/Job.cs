﻿using System;
using System.Collections.Generic;

namespace RecruitmentSystemAPI.Models
{
    public partial class Job
    {
        public Job()
        {
            JobSkills = new HashSet<JobSkill>();
            LabourerJobs = new HashSet<LabourerJob>();
            IncidentReports = new HashSet<IncidentReport>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public float Rating { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Weekdays Weekdays { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<LabourerJob> LabourerJobs { get; set; }
        public virtual ICollection<IncidentReport> IncidentReports { get; set; }
    }
}