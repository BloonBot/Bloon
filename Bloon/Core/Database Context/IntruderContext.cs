namespace Bloon.Core.Database
{
    using System;
    using Bloon.Features.IntruderBackend.Agents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;

    public partial class IntruderContext : DbContext
    {
        public IntruderContext(DbContextOptions<IntruderContext> options)
            : base(options)
        {
        }

        public static string ConnectionString => $"Host={Environment.GetEnvironmentVariable("DB_HOST")};"
            + $"Port={Environment.GetEnvironmentVariable("DB_PORT")};"
            + $"Username={Environment.GetEnvironmentVariable("DB_USER")};"
            + $"Password={Environment.GetEnvironmentVariable("DB_PASS")};"
            + $"Database={Environment.GetEnvironmentVariable("DB_NAME_INTRUDER")};"
            + $"SSL Mode=none";

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

            modelBuilder.Entity<IntruderDBAgent>()
                .Property(s => s.Role)
                 .HasConversion(new EnumToStringConverter<IntruderDBAgentRole>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
