namespace SkyGlobal.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public TimeOnly EventTime { get; set; }
        public DateOnly EventDate { get; set; }
        public string? EventThumbnail { get; set; }
        public int? EventCategorieId { get; set; }
        public virtual EventCategorie? EventCategorie { get; set; }
    }
}