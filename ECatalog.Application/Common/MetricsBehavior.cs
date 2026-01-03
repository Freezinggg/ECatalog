using ECatalog.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Common
{
    public class MetricsBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMetricRecorder _metrics;

        public MetricsBehavior(IMetricRecorder metrics)
        {
            _metrics = metrics;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var requestName = typeof(TRequest).Name;
            var sw = Stopwatch.StartNew();

            _metrics.RequestStarted(requestName);

            try
            {
                var response = await next();
                if (response is not null)
                {
                    dynamic result = response;
                    if (!result.IsSuccess) _metrics.RequestInvalid(requestName);
                    else _metrics.RequestSucceeded(requestName);
                }

                return response;
            }
            catch
            {
                _metrics.RequestFailed(requestName);
                throw;
            }
            finally
            {
                sw.Stop();
                _metrics.RecordDuration(requestName, sw.ElapsedMilliseconds);
            }
        }
    }
}
