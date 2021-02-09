using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel 
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Job name is required.")]
        public string Name { get; set; }

        public List<SelectListItem> Employers { get; set; }
        public List<Skill> Skills { get; set; }

        public AddJobViewModel(List<Employer> employers, List<Skill> selectedSkills)
        {
            Skills = new List<Skill>();
            foreach (var skill in selectedSkills)
            {
                Skills.Add(skill);
            }

            Employers = new List<SelectListItem>();
            foreach (var employer in employers)
            {
                Employers.Add(
                    new SelectListItem
                    {
                        Value = employer.Id.ToString(),
                        Text = employer.Name
                    });
            }
        }

        public AddJobViewModel() { }
    }
}
