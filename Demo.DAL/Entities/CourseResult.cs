using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class CourseResult
    {
        public int Id { get; set; }
        public int Degree { get; set; }
        [ForeignKey(nameof(Course))]
        public int? crs_id { get; set; }
        [ForeignKey(nameof(Trainee))]
        public int? trainee_id { get; set; }
        public virtual Course Course { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}
