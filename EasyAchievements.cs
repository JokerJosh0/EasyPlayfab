namespace EasyPlayfab.Achievements
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Oculus.Platform;

    public static class EasyAchievements
    {
        /// <summary>
        /// Retrieves the ID for each Achievement unlocked by the user.
        /// </summary>
        public static List<string> UnlockedAchievements { get { return Get(); } }

        private static List<string> Get()
        {
            if (Core.IsInitialized())
            {
                List<string> Ids = new List<string>();
                Achievements.GetAllProgress().OnComplete(message => { Ids.Add(message.GetAchievementUpdate().Name); });
                return Ids;
            }
            return new List<string>();
        }

        /// <summary>
        /// Unlock an Achievement
        /// </summary>
        public static void Achieve(string Identifier)
        {
            if (Core.IsInitialized() && !string.IsNullOrEmpty(Identifier))
            {
                Achievements.Unlock(Identifier);
            }
        }

        /// <summary>
        /// Add a count to the achievement
        /// </summary>
        public static void AddCount(string Identifier, int count)
        {
            if (Core.IsInitialized() && !string.IsNullOrEmpty(Identifier))
            {
                Achievements.AddCount(Identifier, (ulong)count);
            }
        }

        /// <summary>
        /// Add a field to the achievement
        /// </summary>
        public static void AddFields(string Identifier, string fields)
        {
            if (Core.IsInitialized() && !string.IsNullOrEmpty(Identifier) && !string.IsNullOrEmpty(fields))
            {
                Achievements.AddFields(Identifier, fields);
            }
        }
    }

}