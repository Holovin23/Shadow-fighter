using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSave
{
    public class SimpleSaverExamples : MonoBehaviour
    {
        [Serializable]
        public class PlayerData
        {
            public int level;
            public float health;
            public Vector3 position;
        }

        void Start()
        {
            // Save and load a simple value
            SimpleSaver.Save("score", 100);
            int score = SimpleSaver.Load<int>("score");
            Debug.Log($"Loaded score: {score}");

            // Save and load a custom class
            var player = new PlayerData { level = 5, health = 80.5f, position = new Vector3(1, 2, 3) };
            SimpleSaver.Save("player", player);
            PlayerData loadedPlayer = SimpleSaver.Load<PlayerData>("player");
            Debug.Log(
                $"Loaded player: level={loadedPlayer.level}, health={loadedPlayer.health}, pos={loadedPlayer.position}");

            // Save and load a list
            var list = new List<string> { "one", "two", "three" };
            SimpleSaver.Save("stringList", list);
            List<string> loadedList = SimpleSaver.Load<List<string>>("stringList");
            Debug.Log($"Loaded list: {string.Join(", ", loadedList)}");

            // Save and load a dictionary
            var dict = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
            SimpleSaver.Save("dict", dict);
            Dictionary<int, string> loadedDict = SimpleSaver.Load<Dictionary<int, string>>("dict");
            foreach (var kv in loadedDict)
                Debug.Log($"Loaded dict: {kv.Key} = {kv.Value}");

            // Save and load a Transform (position/rotation/scale)
            SimpleSaver.Save("playerTransform", transform);
            SimpleSaver.LoadInto("playerTransform", transform);

            // Async save/load
            SaveAndLoadAsync();
        }

        async void SaveAndLoadAsync()
        {
            await SimpleSaver.SaveAsync("asyncScore", 42);
            int asyncScore = await SimpleSaver.LoadAsync<int>("asyncScore");
            Debug.Log($"Loaded async score: {asyncScore}");
        }
    }
}