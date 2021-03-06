namespace Bloon.Core.Database
{
    using System;
    using Bloon.Analytics;
    using Bloon.Analytics.Users;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;

    public partial class AnalyticsContext : DbContext
    {
        public AnalyticsContext(DbContextOptions<AnalyticsContext> options)
            : base(options)
        {
        }

        public static string ConnectionString => $"Host={Environment.GetEnvironmentVariable("DB_HOST")};"
            + $"Port={Environment.GetEnvironmentVariable("DB_PORT")};"
            + $"Username={Environment.GetEnvironmentVariable("DB_USER")};"
            + $"Password={Environment.GetEnvironmentVariable("DB_PASS")};"
            + $"Database={Environment.GetEnvironmentVariable("DB_NAME_ANALYTICS")};"
            + $"SSL Mode=none";

        public DbSet<Commands> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
            };

            modelBuilder.Entity<UserEvent>()
            .Property(s => s.Event)
            .HasConversion(new EnumToStringConverter<Event>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
