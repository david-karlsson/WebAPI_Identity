﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Identity.Entities;

namespace WebAPI_Identity.Entities
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext>options): base(options)

        {

        }

        public DbSet<User>Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet <Issue> Issue { get; set; }



    }
}
