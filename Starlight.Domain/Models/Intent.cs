namespace Starlight.Domain.Models
{
    public class Intent
    {
        public string Name { get; set; }
        public float Score { get; set; }

        public Intent()
        {
            Name = string.Empty;
            Score = 0.0f;
        }

        public Intent(string name, float score)
        {
            Name = name;
            Score = score;
        }
    }
}