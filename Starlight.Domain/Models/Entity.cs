namespace Starlight.Domain.Models
{
    public class Entity
    {
        public string EntityText { get; set; }
        public string Type { get; set; }
        public byte? StartIndex { get; set; }
        public byte? EndIndex { get; set; }
        public DateTime? DateTime { get; set; }

        public Entity()
        {
            EntityText = string.Empty;
            Type = string.Empty;
        }

        public Entity(string entityText)
            : base()
        {
            Type = string.Empty;
            EntityText = entityText;
        }
    }
}