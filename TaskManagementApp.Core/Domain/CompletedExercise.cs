using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Domain
{
    public class CompletedExercise : BaseExercise
    {
        public int Id { get; set; }

        public string Status_Name { get; set; }

        public string Type { get; set; }
    }
}
