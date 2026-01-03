using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface IMetricRecorder
    {
        void RequestStarted(string requestName);
        void RequestSucceeded(string requestName);
        void RequestInvalid(string requestName);
        void RequestFailed(string requestName);
        void RecordDuration(string requestName, long milliseconds);
    }
}
