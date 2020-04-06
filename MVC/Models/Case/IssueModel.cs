using MVC.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.Case
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
