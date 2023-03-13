namespace Starlight.Domain.Models
{
    public class Intent
    {
        public string Name { get; set; }
        public float Score { get; set; }

        public Intent()
        { }

        public Intent(string Name, float Score)
        {
            this.Name = Name;
            this.Score = Score;
        }
    }
}