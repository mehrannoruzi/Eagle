﻿using Elk.Core;
using Eagle.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class AppDbContext : ElkDbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.OverrideDeleteBehavior();
            //builder.RegisterAllEntities<IEntity>(typeof(Person).Assembly);
        }
    }
}
