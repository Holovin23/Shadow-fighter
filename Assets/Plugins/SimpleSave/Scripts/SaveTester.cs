using System;
using UnityEngine;
using System.Collections.Generic;
using SimpleSave.SerializableTypes;
using UnityEngine.UI;

namespace SimpleSave
{
   public class SaveTester : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Transform _savedTransform;

    private void Awake()
    {
        _saveButton.onClick.AddListener(SaveAll);
        _loadButton.onClick.AddListener(LoadAll);
    }

    private void OnDestroy()
    {
        _saveButton.onClick.RemoveListener(SaveAll);
        _loadButton.onClick.RemoveListener(LoadAll);
    }

    private void SaveAll()
    {
        Debug.Log("Saving data...");

        SimpleSaver.Save("vec2", new Vector2(1.1f, 2.2f));
        SimpleSaver.Save("vec2int", new Vector2Int(3, 4));
        SimpleSaver.Save("vec3int", new Vector3Int(5, 6, 7));
        SimpleSaver.Save("vec3", new Vector3(1.1f, 2.2f, 3.3f));
        SimpleSaver.Save("vec4", new Vector4(1, 2, 3, 4));
        SimpleSaver.Save("color", Color.cyan);
        SimpleSaver.Save("color32", new Color32(10, 20, 30, 255));
        SimpleSaver.Save("quat", Quaternion.Euler(10, 20, 30));
        SimpleSaver.Save("rect", new Rect(1, 2, 3, 4));
        SimpleSaver.Save("rectInt", new RectInt(1, 2, 3, 4));
        SimpleSaver.Save("bounds", new Bounds(Vector3.one, Vector3.one * 2));
        SimpleSaver.Save("boundsInt", new BoundsInt(Vector3Int.one, Vector3Int.one * 2));
        SimpleSaver.Save("matrix", Matrix4x4.TRS(Vector3.one, Quaternion.Euler(10, 20, 30), Vector3.one));
        SimpleSaver.Save("date", DateTime.Now);
        SimpleSaver.Save("timespan", TimeSpan.FromMinutes(123));
        SimpleSaver.Save("guid", Guid.NewGuid());
        SimpleSaver.Save("hash128", UnityEngine.Hash128.Parse("1234567890abcdef1234567890abcdef"));

        // Transform
        SimpleSaver.Save("transform", _savedTransform);

        // List<int>
        var myList = new List<int> { 5, 10, 15 };
        SimpleSaver.Save("intList", myList);

        // Dictionary<string, float>
        var stats = new Dictionary<string, float>() {
            { "speed", 2.5f },
            { "jump", 1.7f }
        };
        SimpleSaver.Save("stats", stats);

        // Queue<int>
        var queue = new Queue<int>(new[] { 1, 2, 3 });
        SimpleSaver.Save("queue", queue);

        // HashSet<string>
        var set = new HashSet<string>(new[] { "a", "b", "c" });
        SimpleSaver.Save("set", set);

        Debug.Log("✅ all saved.");
        
        
        List<string>  stringList = new List<string>(){"one,two,three,four,five,six","seven,eight,nine"};
        ;
        Dictionary<int,string> dict = new Dictionary<int,string>(){};
        dict.Add(1,"one");
        dict.Add(2,"two");
        var cutsomClass = new CustomSaveClass(1f,"two",stringList,dict);
        SimpleSaver.Save("cutsomClass", cutsomClass);
        
    }
    
    private void LoadAll()
    {
        Debug.Log("Loading data...");

        Vector2 v2 = SimpleSaver.Load<Vector2>("vec2");
        Debug.Log($"Loaded Vector2: {v2}");

        Vector2Int v2i = SimpleSaver.Load<Vector2Int>("vec2int");
        Debug.Log($"Loaded Vector2Int: {v2i}");

        Vector3Int v3i = SimpleSaver.Load<Vector3Int>("vec3int");
        Debug.Log($"Loaded Vector3Int: {v3i}");

        Vector3 v3 = SimpleSaver.Load<Vector3>("vec3");
        Debug.Log($"Loaded Vector3: {v3}");

        Vector4 v4 = SimpleSaver.Load<Vector4>("vec4");
        Debug.Log($"Loaded Vector4: {v4}");

        Color color = SimpleSaver.Load<Color>("color");
        Debug.Log($"Loaded Color: {color}");

        Color32 color32 = SimpleSaver.Load<Color32>("color32");
        Debug.Log($"Loaded Color32: {color32}");

        Quaternion quat = SimpleSaver.Load<Quaternion>("quat");
        Debug.Log($"Loaded Quaternion: {quat.eulerAngles}");

        Rect rect = SimpleSaver.Load<Rect>("rect");
        Debug.Log($"Loaded Rect: {rect}");

        RectInt rectInt = SimpleSaver.Load<RectInt>("rectInt");
        Debug.Log($"Loaded RectInt: {rectInt}");

        Bounds bounds = SimpleSaver.Load<Bounds>("bounds");
        Debug.Log($"Loaded Bounds: center={bounds.center}, size={bounds.size}");

        BoundsInt boundsInt = SimpleSaver.Load<BoundsInt>("boundsInt");
        Debug.Log($"Loaded BoundsInt: pos={boundsInt.position}, size={boundsInt.size}");

        Matrix4x4 matrix = SimpleSaver.Load<Matrix4x4>("matrix");
        Debug.Log($"Loaded Matrix4x4: {matrix}");

        DateTime date = SimpleSaver.Load<DateTime>("date");
        Debug.Log($"Loaded DateTime: {date}");

        TimeSpan ts = SimpleSaver.Load<TimeSpan>("timespan");
        Debug.Log($"Loaded TimeSpan: {ts}");

        Guid guid = SimpleSaver.Load<Guid>("guid");
        Debug.Log($"Loaded Guid: {guid}");

        UnityEngine.Hash128 hash = SimpleSaver.Load<UnityEngine.Hash128>("hash128");
        Debug.Log($"Loaded Hash128: {hash}");

        // Transform
        SimpleSaver.LoadInto("transform", _savedTransform);
        Debug.Log("Transform reseted");
        

        var loadedList = SimpleSaver.Load<List<int>>("intList");
        if(loadedList!=null)
            Debug.Log("Loaded List: " + string.Join(",", loadedList));

        var loadedStats = SimpleSaver.Load<Dictionary<string, float>>("stats");
        if(loadedStats!=null)
        {
            foreach (var kv in loadedStats)
            {
                Debug.Log($"Stat: {kv.Key} = {kv.Value}");
            }
        }

        // Queue<int>
        var loadedQueue = SimpleSaver.Load<Queue<int>>("queue");
        if(loadedQueue!=null)
            Debug.Log("Loaded Queue: " + string.Join(",", loadedQueue));

        // HashSet<string>
        var loadedSet = SimpleSaver.Load<HashSet<string>>("set");
        if(loadedSet!=null)
            Debug.Log("Loaded HashSet: " + string.Join(",", loadedSet));

        var customClass = SimpleSaver.Load<CustomSaveClass>("cutsomClass");
        if (customClass != null)
        {
            Debug.Log("Loaded Custom Class: " + customClass);
            Debug.Log($"Loaded :" + customClass.floatValue);
            Debug.Log($"Loaded :" + customClass.stringValue);
            foreach (var stringValue in customClass.stringList)
            {
                Debug.Log($"Loaded from List :" + stringValue);
            }
            var dictinary = customClass.intDictionary.ToDictionary();
            foreach (var stringValue in dictinary)
            {
                Debug.Log($"Loaded from Dictionary :" + stringValue.Value);
            }
        }
        Debug.Log("✅ All loaded.");
    }
}

[Serializable]
public class CustomSaveClass
{
    public float floatValue;
    public string stringValue;
    public List<string> stringList;
    public SerializableDictionary<int, string> intDictionary;

    public CustomSaveClass() { }
    
    public CustomSaveClass(float floatValue, string stringValue, List<string> stringList,
        Dictionary<int, string> intDictionary)
    {
        this.floatValue = floatValue;
        this.stringValue = stringValue;
        this.stringList = stringList;
        this.intDictionary = new SerializableDictionary<int, string>(intDictionary);
    }
} 
}
