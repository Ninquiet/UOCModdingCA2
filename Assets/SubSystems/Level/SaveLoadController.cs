using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SubSystems.Level
{
    public class SaveLoadController
    {
        [System.Serializable]
        private class LevelDataWrapper
        {
            public List<List<int>> level;
        }

        private static string GetFilePath(int slotIndex)
        {
            return Path.Combine(Application.persistentDataPath, $"slot_{slotIndex}.json");
        }

        public static void SaveLevel(List<List<int>> currentLevel, int slotIndex)
        {
            LevelDataWrapper wrapper = new LevelDataWrapper { level = currentLevel };
            string json = JsonUtility.ToJson(wrapper, true);

            string path = GetFilePath(slotIndex);
            File.WriteAllText(path, json);

            Debug.Log($"Level saved to {path}");
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

            return wrapper.level ?? new List<List<int>>();
        }
    }
}