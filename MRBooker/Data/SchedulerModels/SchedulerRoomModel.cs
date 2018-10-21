using System.Runtime.Serialization;

namespace MRBooker.Data.SchedulerModels
{
    [DataContract]
    public class SchedulerRoomModel
    {
        [DataMember(Name = "key")]
        public long Id { get; set; }

        [DataMember(Name = "label")]
        public string Name { get; set; }
    }
}
