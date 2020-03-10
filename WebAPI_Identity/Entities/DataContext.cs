﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Identity.Entities
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext>options): base(options)

        {

        }

        public DbSet<User>Users { get; set; }

    }
}
