namespace Starlight.Domain.Models
{
    public class Utterance
    {
        private string _query;

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value.ToLower();
            }
        }

        public List<Intent> Intents { get; set; }

        public Intent TopScoringIntent
        {
            get
            {
                return GetTopScoringIntent();
            }
        }

        public Entity Entity { get; set; }

        public Utterance()
        {
            _query = string.Empty;
            Intents = new List<Intent>();
            Entity = new Entity();
        }

        private Intent GetTopScoringIntent()
        {
            var topScoringIntent = new Intent();
            float maxValue = 0.0f;

            foreach (var intent in Intents)
            {
                if (intent.Score > maxValue)
                {
                    topScoringIntent = intent;
                    maxValue = intent.Score;
                }
            }

            return topScoringIntent;
        }
    }
}