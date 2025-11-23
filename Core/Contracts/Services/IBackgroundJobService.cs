using System.Linq.Expressions;

namespace Application.Contracts.Services
{
    public interface IBackgroundJobService
    {
        void Enqueue<T>(Expression<Action<T>> methodToCall);
    }
}
