namespace Bloon.Core.Services
{
    using System.Threading.Tasks;

    public interface ISocialService<T>
    {
        Task<T> GetLatestAsync(string argument = null);

        Task<bool> TryStoreNewAsync(T entry);
    }
}
