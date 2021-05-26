namespace Bloon.Core.Services
{
    using System.Threading.Tasks;

    public abstract class Feature
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public bool Enabled { get; private set; }

        public virtual bool Protected => false;

        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public virtual Task Enable()
        {
            this.Enabled = true;
            return Task.CompletedTask;
        }

        public virtual Task Disable()
        {
            if (!this.Protected)
            {
                this.Enabled = false;
            }

            return Task.CompletedTask;
        }

        public async Task Reload()
        {
            await this.Disable();
            await this.Enable();
        }
    }
}
