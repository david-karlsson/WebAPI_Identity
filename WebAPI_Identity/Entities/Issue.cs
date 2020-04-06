using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Identity.Entities
{
    public class Issue
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string IssueName { get; set; }

   /*     [Required]
        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }
*/
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }

        public int CustomerId { get;set; }

        public virtual Customer Customer { get; set; }

    }
}
