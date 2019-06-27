using System.Threading.Tasks;

namespace Project.Infrastructure.Mongo
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
