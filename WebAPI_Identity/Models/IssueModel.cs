using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Identity.Models
{
    public class IssueModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }

        public virtual CustomerModel Customer { get; set; }
    }
}
