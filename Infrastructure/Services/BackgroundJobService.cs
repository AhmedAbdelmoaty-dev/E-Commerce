using Application.Contracts.Services;
using Hangfire;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        public void Enqueue<T>(Expression<Action<T>> methodToCall)
        {
            BackgroundJob.Enqueue<T>(methodToCall);
        }
    }
}
