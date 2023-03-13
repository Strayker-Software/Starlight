using Starlight.Domain.Models;

namespace Starlight.Service.Services
{
    public class EntityUtil
    {
        public static string GetEntityText(List<string> textList, string referenceString, int offset)
        {
            return string.Join(" ", textList.GetRange(textList.IndexOf(referenceString) + offset, textList.Count - textList.IndexOf(referenceString) - offset).ToArray());
        }

        public static string[] GetEntityTextArray(List<string> textList, string referenceString, int offset)
        {
            return textList.GetRange(textList.IndexOf(referenceString) + offset, textList.Count - textList.IndexOf(referenceString) - offset).ToArray();
        }

        public static void SetEntityIndexes(Utterance u, string entity)
        {
            u.Entity.StartIndex = (byte)u.Query.IndexOf(entity);
            u.Entity.EndIndex = (byte)(u.Query.Count() - 1);
        }
    }
}