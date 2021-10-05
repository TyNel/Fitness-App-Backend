using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Requests
{
    public class ExerciseAddRequest
    {
        public int UserId { get; set; }
        public string Exercise_Name { get; set; }

        public int Weight { get; set; }

        public int Reps { get; set; }

        public int Exercise_Type { get; set; }

        public int Status_Id { get; set; }

        public string UserNotes { get; set; }

        public string DateAdded { get; set; }

        public string DateModified { get; set; }

    }
}
