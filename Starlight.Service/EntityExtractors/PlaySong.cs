using Starlight.Domain.Models;
using Starlight.Service.Services;

namespace Starlight.Service.EntityExtractors
{
    public class PlaySong
    {
        public static void Fetch(Utterance u)
        {
            List<string> queryArray = new List<string>(u.Query.Split(" "));
            string[] entityTextArray = { };

            if (queryArray.Contains("play"))
            {
                if (u.Query.Contains("play the music"))
                {
                    entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "music", 1);
                    u.Entity.EntityText = string.Join(" ", entityTextArray);
                }
                else if (u.Query.Contains("play the song"))
                {
                    entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "song", 1);
                    u.Entity.EntityText = string.Join(" ", entityTextArray);
                }
                else
                {
                    entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "play", 1);
                    u.Entity.EntityText = string.Join(" ", entityTextArray);
                }
            }

            if (u.Query.Contains("listen to") && !u.Query.Contains("listen to music"))
            {
                entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "to", 1);
                u.Entity.EntityText = string.Join(" ", entityTextArray);
            }
            else if (u.Query.Contains("listen to the music"))
            {
                entityTextArray = EntityUtil.GetEntityTextArray(queryArray, "music", 1);
                u.Entity.EntityText = string.Join(" ", entityTextArray);
            }

            if (u.Entity.EntityText != string.Empty && u.Entity.EntityText != null)
            {
                u.Entity.Type = "music";
                EntityUtil.SetEntityIndexes(u, entityTextArray[0]);
            }
        }
    }
}