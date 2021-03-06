﻿using RecruitmentSystemAPI.Models;
using RecruitmentSystemAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentSystemAPI.Repositories
{
    public class SkillsRepo : BaseRepo
    {
        public SkillsRepo(RecruitmentSystemContext context) : base(context)
        {
        }

        public IQueryable<BaseSkillsVM> GetSkillsDDL()
        {
            return _context.Skills.Where(s => s.IsActive).Select(s => new BaseSkillsVM
            {
                Id = s.Id,
                Name = s.Name,
                IsActive = s.IsActive
            });
        }


        // GET all skills list
        public IQueryable<SkillsVM> GetSkills(int count, int page, out int totalRows)
        {
            var query = _context.Skills;
            totalRows = query.Count();

            return query
                .OrderByDescending(s => s.Name)
                .Skip(count * (page - 1))
                .Take(count)
                .Select(s => new SkillsVM
            {
                Id = s.Id,
                Name = s.Name,
                ChargeAmount = s.ChargeAmount,
                PayAmount = s.PayAmount,
                IsActive = s.IsActive
            });
        }

        // GET one skill by id
        public SkillsVM GetSkillById(int id)
        {
            return _context.Skills.Where(s => s.Id == id).Select(s => new SkillsVM
            {
                Id = s.Id,
                Name = s.Name,
                ChargeAmount = s.ChargeAmount,
                PayAmount = s.PayAmount,
                IsActive = s.IsActive
            }).FirstOrDefault();
        }

        public void UpdateSkill(SkillsVM skillVM)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == skillVM.Id);
            if (skill == null) throw new KeyNotFoundException();

            skill.Name = skillVM.Name;
            skill.ChargeAmount = skillVM.ChargeAmount;
            skill.PayAmount = skillVM.PayAmount;
            skill.IsActive = skillVM.IsActive;

            _context.Update(skill);
            _context.SaveChanges();
        }

        public SkillsVM AddSkill(SkillsVM skillsVM)
        {
            var skill = new Skill
            {
                Name = skillsVM.Name,
                ChargeAmount = skillsVM.ChargeAmount,
                PayAmount = skillsVM.PayAmount,
                IsActive = true
            };
            _context.Skills.Add(skill);
            _context.SaveChanges();
            skillsVM.Id = skill.Id;
            skillsVM.IsActive = true;
            return skillsVM;
        }

        public bool SkillAlreadyExists(string name, int? id = null)
        {
            return _context.Skills.Any(s => s.Name == name && (!id.HasValue || s.Id != id));
        }

        public bool SkillExists(int id)
        {
            return _context.Skills.Any(s => s.Id == id);
        }
    }
}