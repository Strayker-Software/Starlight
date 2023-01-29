namespace Starlight.MLCore
{
    internal interface IClassificator
    {
        Intent Classify(string query);
    }
}