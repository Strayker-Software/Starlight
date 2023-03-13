using Starlight.Domain.Models;
using Starlight.Service.Services;
using System.Text.RegularExpressions;

namespace Starlight.Service.EntityExtractors
{
    public class AddReminder
    {
        public static void Fetch(Utterance u)
        {
            List<string> queryArray = new List<string>(u.Query.Split(" "));
            string[] entityTextArray = { };

            if (queryArray.Contains("remind"))
            {
                if (u.Query.Contains("remind me"))
                {
                    entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "me", 2);
                    u.Entity.EntityText = string.Join(" ", entityTextArray);
                }
                else
                {
                    entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "remind", 2);
                    u.Entity.EntityText = string.Join(" ", entityTextArray);
                }
            }

            if (u.Query.Contains("add a reminder"))
            {
                entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "reminder", 2);
                u.Entity.EntityText = string.Join(" ", entityTextArray);
            }

            if (u.Query.Contains("i need"))
            {
                entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "need", 2);
                u.Entity.EntityText = string.Join(" ", entityTextArray);
            }

            if (u.Entity.EntityText != string.Empty && u.Entity.EntityText != null)
            {
                u.Entity.Type = "reminder";
                EntityUtil.SetEntityIndexes(u, entityTextArray[0]);
            }

            DateTime? parsedDate;

            Match match = Regex.Match(u.Query, @"(?:(?:0?[0-9]|1[0-2]):[0-5][0-9] [ap]m|(?:[01][0-9]|2[0-3]):[0-5][0-9])", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                parsedDate = DateTimeUtil.SetDatetimeEntities(match.Value, match.Index, u);
                u.Entity.Type = "reminder";
                u.Entity.DateTime = parsedDate;
            }
            else
            {
                match = Regex.Match(u.Query, @"(?:(?:0?[0-9]|1[0-2]) [ap]m|(?:[01][0-9]|2[0-3]))", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    parsedDate = DateTimeUtil.SetDatetimeEntities(match.Value.Insert(2, ":00"), match.Index, u);
                    u.Entity.Type = "reminder";
                    u.Entity.DateTime = parsedDate;
                }
            }
        }
    }
}