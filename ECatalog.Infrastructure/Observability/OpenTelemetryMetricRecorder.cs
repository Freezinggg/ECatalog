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

        // Attempts Counter (every handler hit)
        public static readonly Counter<long> CreateAttempt =
            Meter.CreateCounter<long>("catalog_create_attempt_total");

        public static readonly Counter<long> UpdateAttempt =
            Meter.CreateCounter<long>("catalog_update_attempt_total");

        public static readonly Counter<long> DeleteAttempt =
            Meter.CreateCounter<long>("catalog_delete_attempt_total");

        // Success Counter
        public static readonly Counter<long> CreateSuccess =
            Meter.CreateCounter<long>("catalog_create_success_total");

        public static readonly Counter<long> UpdateSuccess =
            Meter.CreateCounter<long>("catalog_update_success_total");

        public static readonly Counter<long> DeleteSuccess =
            Meter.CreateCounter<long>("catalog_delete_success_total");

        // Failure Counter
        public static readonly Counter<long> CreateFailure =
            Meter.CreateCounter<long>("catalog_create_failure_total");

        public static readonly Counter<long> UpdateFailure =
            Meter.CreateCounter<long>("catalog_update_failure_total");

        public static readonly Counter<long> DeleteFailure =
            Meter.CreateCounter<long>("catalog_delete_failure_total");

        
        //Will use this soon?
        public static readonly Histogram<long> HandlerDurationMs =
            Meter.CreateHistogram<long>("catalog_handler_duration_ms", "ms");

        
        public void CreateAttempted() => CreateAttempt.Add(1);
        public void CreateSucceeded() => CreateSuccess.Add(1);
        public void CreateFailed() => CreateFailure.Add(1);

        public void UpdateAttempted() => UpdateAttempt.Add(1);
        public void UpdateSucceeded() => UpdateSuccess.Add(1);
        public void UpdateFailed() => UpdateFailure.Add(1);

        public void DeleteAttempted() => DeleteAttempt.Add(1);
        public void DeleteSucceeded() => DeleteSuccess.Add(1);
        public void DeleteFailed() => DeleteFailure.Add(1);

        public void RecordDuration(long milliseconds) =>
            HandlerDurationMs.Record(milliseconds);
    }
}
