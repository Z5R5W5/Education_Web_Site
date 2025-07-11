﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Trainee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public int Level { get; set; }
        [ForeignKey(nameof(department))]
        public int? dept_id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public virtual Department department { get; set; }
        public virtual List<CourseResult> courseResult { get; set; }
    }
}
