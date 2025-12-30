using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface IMetricRecorder
    {
        void CreateAttempted();
        void CreateSucceeded();
        void CreateFailed();

        void UpdateAttempted();
        void UpdateSucceeded();
        void UpdateFailed();

        void DeleteAttempted();
        void DeleteSucceeded();
        void DeleteFailed();
    }
}
