namespace Bloon.Core.Database
{
    using System;
    using Bloon.Core.Services;
    using Bloon.Features.Helprace;
    using Bloon.Features.RedditGuard;
    using Bloon.Features.SteamNews;
    using Bloon.Features.Twitter;
    using Bloon.Features.Wiki;
    using Bloon.Features.Workshop;
    using Bloon.Features.Youtube;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class BloonContext : DbContext
    {
        public BloonContext(DbContextOptions<BloonContext> options)
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
                + $"Database={Environment.GetEnvironmentVariable("DB_NAME")};"
                + $"SSL Mode=none";
            }
        }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<FeatureStatus> FeatureStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new ()
            {
                Formatting = Formatting.None,
            };

            // Magic
            modelBuilder.Entity<SocialItem>()
                .HasDiscriminator<SocialType>("Type")
                .HasValue<HelpracePost>(SocialType.Helprace)
                .HasValue<RedditPost>(SocialType.Reddit)
                .HasValue<SteamNewsPost>(SocialType.SteamNews)
                .HasValue<Tweet>(SocialType.Twitter)
                .HasValue<WikiArticle>(SocialType.Wiki)
                .HasValue<SocialItemWorkshopMap>(SocialType.Workshop)
                .HasValue<YouTubeVideo>(SocialType.YouTube);

            modelBuilder.Entity<SocialItem>()
                .Property(s => s.Additional)
                .HasConversion(s => s.ToString(Formatting.None), s => JObject.Parse(s));

            modelBuilder.Entity<SocialItem>()
                .Property(s => s.Type)
                .HasConversion(new EnumToStringConverter<SocialType>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
