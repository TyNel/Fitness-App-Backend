using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Requests
{
    public class ExerciseUpdate : ExerciseAddRequest
    {
        [Required]
        public int Id { get; set; }
        
        public string Status_Name { get; set; }

        public string Type { get; set; }
    }
}
