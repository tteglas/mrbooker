using System.Runtime.Serialization;

namespace MRBooker.Data.SchedulerModels
{
    [DataContract]
    public class SchedulerEventModel
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
        
        [DataMember(Name = "start_date")]
        public string StartDate { get; set; }
        
        [DataMember(Name = "end_date")]
        public string EndDate { get; set; }

        [DataMember(Name = "roomId")]
        public long RoomId { get; set; }

        [DataMember(Name = "event_location")]
        public string Location { get; set; }

        public string UserId { get; set; }

        public string IpAddress { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }
    }
}
