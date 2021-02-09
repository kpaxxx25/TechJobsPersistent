using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();
            AddJobViewModel addJobViewModel = new AddJobViewModel(employers, skills);
            return View(addJobViewModel);
        }

        [HttpPost, Route("/AddJob")]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, String[] selectedSkills)
        {
            int jobId = addJobViewModel.JobId;


            if (ModelState.IsValid)
            {
                Employer theEmployer = context.Employers.Find(addJobViewModel.EmployerId);
                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,

                    Employer = theEmployer
                };
                context.Jobs.Add(newJob);
                context.SaveChanges();

                List<JobSkill> existingJobs = context.JobSkills
                    .Where(js => js.JobId == jobId)
                    .ToList();

                if (existingJobs.Count == 0)
                {
                    for (int i = 0; i < selectedSkills.Length; i++)
                    {
                        JobSkill newJobSkill = new JobSkill
                        {
                            JobId = newJob.Id,
                            SkillId = int.Parse(selectedSkills[i])
                        };
                        context.JobSkills.Add(newJobSkill);
                    }
                }

                context.SaveChanges();
                return Redirect("/List");
            }

            return View("AddJob", addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
