using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MRBooker.Data.SchedulerModels
{
    public class SchedulerEventHolderModel
    {
        [DataMember (Name = "data")]
        public List<SchedulerEventModel> data { get; set; }
    }
}
