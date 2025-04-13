using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SubSystems.Level
{
    public class SaveLoadController
    {
        [System.Serializable]
        public class LevelDataWrapper
        {
            public List<IntArrayWrapper> level;
        }
        
        [System.Serializable]
        public class IntArrayWrapper
        {
            public int[] row;

            public IntArrayWrapper(List<int> rowList)
            {
                row = rowList.ToArray();
            }
        }

        private static string GetFilePath(int slotIndex)
        {
            return Path.Combine(Application.persistentDataPath, $"slot_{slotIndex}.json");
        }

        public static void SaveLevel(List<List<int>> currentLevel, int slotIndex)
        {
            var levelWrapped = new List<IntArrayWrapper>();
            foreach (var row in currentLevel)
            {
                levelWrapped.Add(new IntArrayWrapper(row));
            }

            LevelDataWrapper wrapper = new LevelDataWrapper { level = levelWrapped };
            string json = JsonUtility.ToJson(wrapper, true);

            string path = GetFilePath(slotIndex);
            File.WriteAllText(path, json);
        }

        public static List<List<int>> LoadLevel(int slotIndex)
        {
            string path = GetFilePath(slotIndex);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"No level found at {path}, returning empty level.");
                return new List<List<int>>();
            }

            string json = File.ReadAllText(path);
            LevelDataWrapper wrapper = JsonUtility.FromJson<LevelDataWrapper>(json);

            if (wrapper == null || wrapper.level == null)
                return new List<List<int>>();

            var result = new List<List<int>>();
            foreach (var rowWrapper in wrapper.level)
            {
                result.Add(new List<int>(rowWrapper.row));
            }

            return result;
        }

        public static List<List<int>> ParseJsonToLevel(string json)
        {
            var wrapper = JsonUtility.FromJson<SaveLoadController.LevelDataWrapper>(json);
    
            if (wrapper?.level == null)
                return new List<List<int>>();

            var result = new List<List<int>>();
            foreach (var rowWrapper in wrapper.level)
            {
                result.Add(new List<int>(rowWrapper.row));
            }

            return result;
        }
    }
}