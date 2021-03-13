namespace Bloon.Core.Database
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;
    using Bloon.Analytics.Users;
    using Bloon.Features.IntruderBackend.Agents;

    public partial class IntruderContext : DbContext
    {
        public IntruderContext(DbContextOptions<IntruderContext> options)
            : base(options)
        {
        }

        public static string ConnectionString
        {
            get
            {
                return $"Host={Environment.GetEnvironmentVariable("DB_HOST")};"
                + $"Port={Environment.GetEnvironmentVariable("DB_PORT")};"
                + $"Username={Environment.GetEnvironmentVariable("DB_USER")};"
                + $"Password={Environment.GetEnvironmentVariable("DB_PASS")};"
                + $"Database={Environment.GetEnvironmentVariable("DB_NAME_INTRUDER")};"
                + $"SSL Mode=none";
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
            };

            modelBuilder.Entity<Agent>()
                .Property(s => s.Role)
                .HasConversion(new EnumToStringConverter<Role>());

            modelBuilder.Entity<AgentsDB>()
                .Property(s => s.Role)
                .HasConversion(new EnumToStringConverter<Role>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
