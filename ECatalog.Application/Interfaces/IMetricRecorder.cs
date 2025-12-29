using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface IMetricRecorder
    {
        void ItemCreated();
        void ItemUpdated();
        void ItemDeleted();
        void OperationFailed();
    }
}
