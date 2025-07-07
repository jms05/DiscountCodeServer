using System;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class SafeErrorHandler
    {
        private readonly Action<Exception> _fallbackLogger;

        public SafeErrorHandler(Action<Exception> fallbackLogger = null)
        {
            _fallbackLogger = fallbackLogger;
        }

        public async Task HandleAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                try
                {
                    _fallbackLogger?.Invoke(ex);
                }
                catch { /* swallow to avoid crash */ }
            }
        }
    }
}
