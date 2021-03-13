namespace Bloon.Core.Database
{
    using System;
    using Bloon.Features.PackageAccounts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;

    public partial class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options)
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
                + $"Database={Environment.GetEnvironmentVariable("DB_NAME_ACC")};"
                + $"SSL Mode=none";
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
            };

            modelBuilder.Entity<PackageAccount>()
                .Property(s => s.Type)
                .HasConversion(new EnumToStringConverter<Permission>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
