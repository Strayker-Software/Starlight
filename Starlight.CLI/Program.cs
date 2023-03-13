using Starlight.Service.Services.Classificators;

namespace Starlight.CLI
{
    internal class Program
    {
        private static void Main()
        {
            IntentClassificator cc = new IntentClassificator(null, true);

            while (true)
            {
                Console.WriteLine("Enter an utterance:");
                Console.Write("> ");
                Console.WriteLine(cc.Cognize(Console.ReadLine(), true));
            }
        }
    }
}