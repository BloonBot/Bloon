namespace Bloon.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Utils;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class JobManager : IDisposable
    {
        // 300,000 = 5 minutes. 60,000 = 1 minute. Useful for debugging Jobs
        private const int IntervalMs = 300000;

        private readonly ActivityManager activityManager;
        private readonly BloonLog bloonLog;
        private readonly IServiceScopeFactory factory;
        private readonly DiscordClient dClient;
        private readonly HashSet<ITimedJob> jobs;
        private Timer timer;

        public JobManager(IServiceScopeFactory factory, DiscordClient dClient, ActivityManager activityManager, BloonLog bloonLog)
        {
            this.factory = factory;
            this.dClient = dClient;
            this.activityManager = activityManager;
            this.bloonLog = bloonLog;
            this.jobs = new HashSet<ITimedJob>();
        }

        public void Start()
        {
            this.timer = new Timer(IntervalMs)
            {
                Enabled = true,
                AutoReset = true,
            };

            this.timer.Elapsed += this.TimerElapsed;
        }

        public void AddJob(ITimedJob job)
        {
            this.jobs.Add(job);
        }

        public void RemoveJob(ITimedJob job)
        {
            this.jobs.Remove(job);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.timer == null)
            {
                return;
            }

            if (disposing)
            {
                this.timer.Dispose();
            }
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            List<ITimedJob> timedJobs = new List<ITimedJob>();
            using IServiceScope scope = this.factory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            List<Job> dbJobs = db.Jobs.ToList();

            foreach (ITimedJob timedJob in this.jobs)
            {
                string jobName = timedJob.GetType().FullName;

                Job job = dbJobs.Where(j => j.Name == jobName).FirstOrDefault();

                if (job == null)
                {
                    job = new Job()
                    {
                        Name = jobName,
                        LastExecution = DateTime.UnixEpoch.ToUniversalTime(),
                    };

                    db.Jobs.Add(job);
                }

                if ((DateTime.UtcNow - job.LastExecution).TotalMinutes >= timedJob.Interval)
                {
                    timedJobs.Add(timedJob);
                    job.LastExecution = DateTime.UtcNow;
                }
            }

            await db.SaveChangesAsync().ConfigureAwait(false);

            if (timedJobs.Count > 0)
            {
                await this.activityManager.TrySetActivityAsync($"{timedJobs.Count} job(s)", ActivityType.Watching, true).ConfigureAwait(false);

                Task jobs = Task.WhenAll(timedJobs.Select(t => t.Execute()));

                try
                {
                    await jobs.ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    if (jobs.Exception != null)
                    {
                        for (int i = 0; i < jobs.Exception.InnerExceptions.Count; i++)
                        {
                            this.bloonLog.Error($"{jobs.Exception.InnerExceptions[i].ToString().Truncate(1500)}");
                        }
                    }
                    else
                    {
                        this.bloonLog.Error($"{ex.ToString().Truncate(1500)}");
                    }

                    Log.Error(jobs.Exception, "One or more jobs failed.");
                }

                //this.bloonLog.Information(LogConsole.Jobs, SBGEmojis.Bloon, $"Jobs finished:\n- {string.Join("\n- ", timedJobs.Select(j => $"{DiscordEmoji.FromGuildEmote(this.dClient, j.Emoji)} {j.GetType().Name}"))}");
            }
        }
    }
}
