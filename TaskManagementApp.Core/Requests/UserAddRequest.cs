using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Requests
{
    public class UserAddRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Password { get; set; }

    }
}
