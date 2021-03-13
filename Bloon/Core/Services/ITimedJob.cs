namespace Bloon.Core.Services
{
    using System.Threading.Tasks;

    public interface ITimedJob
    {
        ulong Emoji { get; }

        int Interval { get; }

        Task Execute();
    }
}
