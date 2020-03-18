using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Models
{
    public class IssueModel
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }


        public string IssueName { get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }

        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

    }
}
