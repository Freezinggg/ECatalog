using ECatalog.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Infrastructure.Observability
{
    public class OpenTelemetryMetricRecorder : IMetricRecorder
    {
        private static readonly Meter Meter = new("ECatalog.Metrics");

        private static readonly Counter<long> RequestStartedCounter =
            Meter.CreateCounter<long>("mediatr_request_started_total");

        private static readonly Counter<long> RequestSucceededCounter =
            Meter.CreateCounter<long>("mediatr_request_succeeded_total");

        private static readonly Counter<long> RequestInvalidCounter =
            Meter.CreateCounter<long>("mediatr_request_invalid_total");

        private static readonly Counter<long> RequestFailedCounter =
            Meter.CreateCounter<long>("mediatr_request_failed_total");

        private static readonly Histogram<long> RequestDurationMs =
            Meter.CreateHistogram<long>("mediatr_request_duration_ms", "ms");

        public void RequestStarted(string requestName)
        {
            RequestStartedCounter.Add(1,
                new KeyValuePair<string, object?>("request", requestName));
        }

        public void RequestSucceeded(string requestName)
        {
            RequestSucceededCounter.Add(1,
                new KeyValuePair<string, object?>("request", requestName));
        }

        public void RequestInvalid(string requestName)
        {
            RequestInvalidCounter.Add(1,
                new KeyValuePair<string, object?>("request", requestName));
        }

        public void RequestFailed(string requestName)
        {
            RequestFailedCounter.Add(1,
                new KeyValuePair<string, object?>("request", requestName));
        }

        public void RecordDuration(string requestName, long milliseconds)
        {
            RequestDurationMs.Record(milliseconds,
                new KeyValuePair<string, object?>("request", requestName));
        }
    }
}
