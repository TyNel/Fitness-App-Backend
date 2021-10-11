using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Requests
{
    public class ExerciseAddRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Exercise_Name { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Weight { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Reps { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Exercise_Type { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Status_Id { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string UserNotes { get; set; }
       
        public string DateAdded { get; set; }
       
        public string DateModified { get; set; }

    }
}
