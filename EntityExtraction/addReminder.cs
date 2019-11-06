﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Starlight.EntityExtraction {
    public class addReminder {

        public static void Fetch(Utterance u) {

            List<String> queryArray = new List<string>(u.Query.Split(" "));
            string[] entityTextArray = { };

            if (queryArray.Contains("remind")) {

                if (u.Query.Contains("remind me")) {
                    entityTextArray = Util.EntityUtil.GetEntityTextArray(queryArray, "me", 2);
                    u.Entity.EntityText = String.Join(" ", entityTextArray);
                }
                   
                else {
                    entityTextArray = Util.EntityUtil.GetEntityTextArray(queryArray, "remind", 2);
                    u.Entity.EntityText = String.Join(" ", entityTextArray);
                }
            }

            if (u.Query.Contains("add a reminder")) {
                entityTextArray = Util.EntityUtil.GetEntityTextArray(queryArray, "reminder", 2);
                u.Entity.EntityText = String.Join(" ", entityTextArray);
            }

            if (u.Query.Contains("i need")) {
                entityTextArray = Util.EntityUtil.GetEntityTextArray(queryArray, "need", 2);
                u.Entity.EntityText = String.Join(" ", entityTextArray);
            }

            if (u.Entity.EntityText != String.Empty && u.Entity.EntityText != null) {
                u.Entity.Type = "reminder";
                u.Entity.startIndex = (Byte) u.Query.IndexOf(entityTextArray[0]);
                u.Entity.endIndex = (Byte) (u.Entity.EntityText.Length - 1);
            }
        }

    }
}
