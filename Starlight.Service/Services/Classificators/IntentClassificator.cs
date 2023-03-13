using Starlight.Domain.Models;
using Starlight.Service.EntityExtractors;
using System.Text.Json;

namespace Starlight.Service.Services.Classificators
{
    public class IntentClassificator
    {
        private static readonly string datasetPath = Path.Combine(Environment.CurrentDirectory, "Dataset");
        private static List<string> _intentList;
        private List<BinaryClassificator> _binaryClassificators;

        public IntentClassificator(string datasetpath = null, bool debug = false)
        {
            _binaryClassificators = new List<BinaryClassificator>();

            if (debug)
                Console.WriteLine("=============== Starlight Build ===============\n");

            foreach (var intentName in GetIntentList())
                _binaryClassificators.Add(new BinaryClassificator(intentName, datasetpath, false, debug));
        }

        public string Cognize(string query, bool debug = false)
        {
            Utterance utterance = new Utterance();
            utterance.Query = query;

            for (int i = 0; i < _intentList.Count; i++)
                utterance.Intents.Add(_binaryClassificators[i].Classify(query, debug));

            if (utterance.TopScoringIntent.Score < 0.8)
            {
                utterance.Intents.Add(new Intent("none", (float)0.8));
            }

            EntityExtractorController.Fetch(utterance);

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(utterance, options);
        }

        private static List<string> GetIntentList()
        {
            DirectoryInfo d = new DirectoryInfo(datasetPath);
            FileInfo[] files = d.GetFiles("*.txt");
            _intentList = new List<string>();
            foreach (FileInfo file in files)
            {
                _intentList.Add(file.Name.Replace(".txt", string.Empty));
            }
            return _intentList;
        }
    }
}