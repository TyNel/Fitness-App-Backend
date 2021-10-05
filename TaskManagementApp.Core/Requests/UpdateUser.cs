using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Requests
{
    public class UpdateUser : UserAddRequest
    {
        [Required]
        public int userId { get; set; }
    }
}
