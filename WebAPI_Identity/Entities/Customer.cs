﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Identity.Entities
{
    public class Customer
    {

        [Required]

        public int Id { get; set; }

        [Required]

        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }


        [Required]

        public string Email { get; set; }

        //public ICollection<Issue> Issue{ get; set; }

}
}
