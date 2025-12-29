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


        public static readonly Counter<long> CreateCounter =
            Meter.CreateCounter<long>("catalog_create_total");

        public static readonly Counter<long> UpdateCounter =
            Meter.CreateCounter<long>("catalog_update_total");

        public static readonly Counter<long> DeleteCounter =
            Meter.CreateCounter<long>("catalog_delete_total");

        public static readonly Counter<long> FailureCounter =
            Meter.CreateCounter<long>("catalog_failure_total");

        public void ItemCreated()
        {
            CreateCounter.Add(1);
        }

        public void ItemDeleted()
        {
            DeleteCounter.Add(1);
        }

        public void ItemUpdated()
        {
            UpdateCounter.Add(1);
        }

        public void OperationFailed()
        {
            FailureCounter.Add(1);
        }
    }
}
