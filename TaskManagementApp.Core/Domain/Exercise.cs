using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Domain
{
    public class Exercise
    {
        public int UserId { get; set; }
        public string Exercise_Name { get; set; }

        public int Weight { get; set; }

        public int Reps { get; set; }

        public int Exercise_Type { get; set; }

        public int Status_Id { get; set; }

        public string UserNotes { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}
