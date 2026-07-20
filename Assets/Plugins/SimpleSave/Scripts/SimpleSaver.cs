using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using SimpleSave.Settings;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SimpleSave.SerializableTypes;

namespace SimpleSave
{
    public static class SimpleSaver
    {
        /// <summary>
        /// Saves an object with the specified key using the current save format and settings.
        /// </summary>
        /// <typeparam name="T">Type of the object to save.</typeparam>
        /// <param name="key">Unique key for the saved data.</param>
        /// <param name="obj">Object to save.</param>
        /// <example>
        /// <![CDATA[
        /// SimpleSaver.Save("playerScore", 123);
        /// SimpleSaver.Save("playerData", myPlayerData);
        /// ]]>
        /// </example>
        public static void Save<T>(string key, T obj)
        {
            var settings = SimpleSaveSettings.Instance;
            switch (settings.saveFormat)
            {
                case SimpleSaveSettings.SaveFormat.Json:
                    SaveJson(key, obj, settings);
                    break;
                case SimpleSaveSettings.SaveFormat.PlayerPrefs:
                    SaveBinaryPlayerPrefs(key, obj, settings);
                    break;
                default:
                    SaveBinary(key, obj, settings);
                    break;
            }
        }

        /// <summary>
        /// Loads an object of type T by key using the current save format and settings.
        /// </summary>
        /// <typeparam name="T">Type of the object to load.</typeparam>
        /// <param name="key">Unique key for the saved data.</param>
        /// <returns>The loaded object, or default(T) if not found or on error.</returns>
        /// <example>
        /// <![CDATA[
        /// int score = SimpleSaver.Load<int>("playerScore");
        /// PlayerData data = SimpleSaver.Load<PlayerData>("playerData");
        /// ]]>
        /// </example>
        public static T Load<T>(string key)
        {
            var settings = SimpleSaveSettings.Instance;
            switch (settings.saveFormat)
            {
                case SimpleSaveSettings.SaveFormat.Json:
                    return LoadJson<T>(key, settings);
                case SimpleSaveSettings.SaveFormat.PlayerPrefs:
                    return LoadBinaryPlayerPrefs<T>(key, settings);
                default:
                    return LoadBinary<T>(key, settings);
            }
        }

        private static void SaveBinary<T>(string key, T obj, SimpleSaveSettings settings)
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(key);
                    WriteObject(obj, writer);
                    byte[] data = ms.ToArray();
                    if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                        data = Encrypt(data, settings.encryptionPassword);
                    File.WriteAllBytes(settings.GetPath(key), data);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SimpleSaver] Failed to save binary for key '{key}': {ex.Message}");
            }
        }

        private static T LoadBinary<T>(string key, SimpleSaveSettings settings)
        {
            string path = settings.GetPath(key);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SimpleSaver] File not found for key '{key}'");
                return default;
            }

            byte[] data = File.ReadAllBytes(path);
            if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                data = Decrypt(data, settings.encryptionPassword);
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms))
            {
                string storedKey = reader.ReadString();
                if (storedKey != key)
                {
                    Debug.LogWarning($"[SimpleSaver] Key mismatch: expected '{key}', got '{storedKey}'");
                    return default;
                }

                try
                {
                    return (T)ReadObject(typeof(T), reader);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[SimpleSaver] Failed to load object for key '{key}': {ex.Message}");
                    return default;
                }
            }
        }

        private static void SaveJson<T>(string key, T obj, SimpleSaveSettings settings)
        {
            try
            {
                string json;
                var objType = obj.GetType();
                if (objType == typeof(Transform))
                {
                    var st = SerializableTransform.FromTransform((Transform)(object)obj);
                    json = JsonUtility.ToJson(st);
                }
                else if (objType == typeof(DateTime))
                {
                    var sdt = new SerializableDateTime((DateTime)(object)obj);
                    json = JsonUtility.ToJson(sdt);
                }
                else if (objType == typeof(TimeSpan))
                {
                    var sts = new SerializableTimeSpan((TimeSpan)(object)obj);
                    json = JsonUtility.ToJson(sts);
                }
                else if (objType == typeof(Guid))
                {
                    var sg = new SerializableGuid((Guid)(object)obj);
                    json = JsonUtility.ToJson(sg);
                }
                else if (objType == typeof(Hash128))
                {
                    var sh = new SerializableHash128((Hash128)(object)obj);
                    json = JsonUtility.ToJson(sh);
                }
                else if (objType == typeof(Quaternion))
                {
                    var sq = new SerializableQuaternion((Quaternion)(object)obj);
                    json = JsonUtility.ToJson(sq);
                }
                else if (objType == typeof(Bounds))
                {
                    var sb = new SerializableBounds((Bounds)(object)obj);
                    json = JsonUtility.ToJson(sb);
                }
                else if (objType == typeof(BoundsInt))
                {
                    var sbi = new SerializableBoundsInt((BoundsInt)(object)obj);
                    json = JsonUtility.ToJson(sbi);
                }
                else if (objType == typeof(Vector2))
                {
                    var sv = new SerializableVector2((Vector2)(object)obj);
                    json = JsonUtility.ToJson(sv);
                }
                else if (objType == typeof(Vector2Int))
                {
                    var svi = new SerializableVector2Int((Vector2Int)(object)obj);
                    json = JsonUtility.ToJson(svi);
                }
                else if (objType == typeof(Vector3))
                {
                    var sv = new SerializableVector3((Vector3)(object)obj);
                    json = JsonUtility.ToJson(sv);
                }
                else if (objType == typeof(Vector3Int))
                {
                    var svi = new SerializableVector3Int((Vector3Int)(object)obj);
                    json = JsonUtility.ToJson(svi);
                }
                else if (objType == typeof(Vector4))
                {
                    var sv = new SerializableVector4((Vector4)(object)obj);
                    json = JsonUtility.ToJson(sv);
                }
                else if (objType == typeof(Color))
                {
                    var sc = new SerializableColor((Color)(object)obj);
                    json = JsonUtility.ToJson(sc);
                }
                else if (objType == typeof(Color32))
                {
                    var sc = new SerializableColor32((Color32)(object)obj);
                    json = JsonUtility.ToJson(sc);
                }
                else if (objType == typeof(Rect))
                {
                    var sr = new SerializableRect((Rect)(object)obj);
                    json = JsonUtility.ToJson(sr);
                }
                else if (objType == typeof(RectInt))
                {
                    var sri = new SerializableRectInt((RectInt)(object)obj);
                    json = JsonUtility.ToJson(sri);
                }
                else if (objType == typeof(Matrix4x4))
                {
                    var sm = new SerializableMatrix4x4((Matrix4x4)(object)obj);
                    json = JsonUtility.ToJson(sm);
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elemType = objType.GetGenericArguments()[0];
                    var wrapperType = typeof(ListWrapper<>).MakeGenericType(elemType);
                    var wrapper = Activator.CreateInstance(wrapperType);
                    wrapperType.GetField("list").SetValue(wrapper, obj);
                    json = JsonUtility.ToJson(wrapper);
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var keyType = objType.GetGenericArguments()[0];
                    var valueType = objType.GetGenericArguments()[1];
                    var wrapperType = typeof(DictionaryWrapper<,>).MakeGenericType(keyType, valueType);
                    var wrapper = Activator.CreateInstance(wrapperType);
                    var dict = (System.Collections.IDictionary)obj;
                    var keys = (System.Collections.IList)Activator.CreateInstance(
                        typeof(List<>).MakeGenericType(keyType));
                    var values =
                        (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));
                    foreach (System.Collections.DictionaryEntry entry in dict)
                    {
                        keys.Add(entry.Key);
                        values.Add(entry.Value);
                    }

                    wrapperType.GetField("keys").SetValue(wrapper, keys);
                    wrapperType.GetField("values").SetValue(wrapper, values);
                    json = JsonUtility.ToJson(wrapper);
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Queue<>))
                {
                    var elemType = objType.GetGenericArguments()[0];
                    var wrapperType = typeof(QueueWrapper<>).MakeGenericType(elemType);
                    var wrapper = Activator.CreateInstance(wrapperType, obj);
                    json = JsonUtility.ToJson(wrapper);
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    var elemType = objType.GetGenericArguments()[0];
                    var wrapperType = typeof(HashSetWrapper<>).MakeGenericType(elemType);
                    var wrapper = Activator.CreateInstance(wrapperType, obj);
                    json = JsonUtility.ToJson(wrapper);
                }
                else
                {
                    json = JsonUtility.ToJson(obj);
                }

                byte[] data = Encoding.UTF8.GetBytes(json);
                if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                    data = Encrypt(data, settings.encryptionPassword);
                File.WriteAllBytes(settings.GetPath(key), data);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SimpleSaver] Failed to save JSON for key '{key}': {ex.Message}");
            }
        }

        private static T LoadJson<T>(string key, SimpleSaveSettings settings)
        {
            string path = settings.GetPath(key);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SimpleSaver] JSON file not found for key '{key}'");
                return default;
            }

            byte[] data = File.ReadAllBytes(path);
            if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                data = Decrypt(data, settings.encryptionPassword);
            string json = Encoding.UTF8.GetString(data);
            try
            {
                if (typeof(T) == typeof(Transform))
                {
                    var st = JsonUtility.FromJson<SerializableTransform>(json);
                    // Transform cannot be created directly, return SerializableTransform
                    return (T)(object)st;
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    var sdt = JsonUtility.FromJson<SerializableDateTime>(json);
                    return (T)(object)sdt.ToDateTime();
                }
                else if (typeof(T) == typeof(TimeSpan))
                {
                    var sts = JsonUtility.FromJson<SerializableTimeSpan>(json);
                    return (T)(object)sts.ToTimeSpan();
                }
                else if (typeof(T) == typeof(Guid))
                {
                    var sg = JsonUtility.FromJson<SerializableGuid>(json);
                    return (T)(object)sg.ToGuid();
                }
                else if (typeof(T) == typeof(Hash128))
                {
                    var sh = JsonUtility.FromJson<SerializableHash128>(json);
                    return (T)(object)sh.ToHash128();
                }
                else if (typeof(T) == typeof(Quaternion))
                {
                    var sq = JsonUtility.FromJson<SerializableQuaternion>(json);
                    return (T)(object)sq.ToQuaternion();
                }
                else if (typeof(T) == typeof(Bounds))
                {
                    var sb = JsonUtility.FromJson<SerializableBounds>(json);
                    return (T)(object)sb.ToBounds();
                }
                else if (typeof(T) == typeof(BoundsInt))
                {
                    var sbi = JsonUtility.FromJson<SerializableBoundsInt>(json);
                    return (T)(object)sbi.ToBoundsInt();
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    var sv = JsonUtility.FromJson<SerializableVector2>(json);
                    return (T)(object)sv.ToVector2();
                }
                else if (typeof(T) == typeof(Vector2Int))
                {
                    var svi = JsonUtility.FromJson<SerializableVector2Int>(json);
                    return (T)(object)svi.ToVector2Int();
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    var sv = JsonUtility.FromJson<SerializableVector3>(json);
                    return (T)(object)sv.ToVector3();
                }
                else if (typeof(T) == typeof(Vector3Int))
                {
                    var svi = JsonUtility.FromJson<SerializableVector3Int>(json);
                    return (T)(object)svi.ToVector3Int();
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    var sv = JsonUtility.FromJson<SerializableVector4>(json);
                    return (T)(object)sv.ToVector4();
                }
                else if (typeof(T) == typeof(Color))
                {
                    var sc = JsonUtility.FromJson<SerializableColor>(json);
                    return (T)(object)sc.ToColor();
                }
                else if (typeof(T) == typeof(Color32))
                {
                    var sc = JsonUtility.FromJson<SerializableColor32>(json);
                    return (T)(object)sc.ToColor32();
                }
                else if (typeof(T) == typeof(Rect))
                {
                    var sr = JsonUtility.FromJson<SerializableRect>(json);
                    return (T)(object)sr.ToRect();
                }
                else if (typeof(T) == typeof(RectInt))
                {
                    var sri = JsonUtility.FromJson<SerializableRectInt>(json);
                    return (T)(object)sri.ToRectInt();
                }
                else if (typeof(T) == typeof(Matrix4x4))
                {
                    var sm = JsonUtility.FromJson<SerializableMatrix4x4>(json);
                    return (T)(object)sm.ToMatrix4x4();
                }
                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elemType = typeof(T).GetGenericArguments()[0];
                    var wrapperType = typeof(ListWrapper<>).MakeGenericType(elemType);
                    var wrapper = JsonUtility.FromJson(json, wrapperType);
                    return (T)wrapperType.GetField("list").GetValue(wrapper);
                }
                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var keyType = typeof(T).GetGenericArguments()[0];
                    var valueType = typeof(T).GetGenericArguments()[1];
                    var wrapperType = typeof(DictionaryWrapper<,>).MakeGenericType(keyType, valueType);
                    var wrapper = JsonUtility.FromJson(json, wrapperType);
                    var keys = (System.Collections.IList)wrapperType.GetField("keys").GetValue(wrapper);
                    var values = (System.Collections.IList)wrapperType.GetField("values").GetValue(wrapper);
                    var dict = (System.Collections.IDictionary)Activator.CreateInstance(
                        typeof(Dictionary<,>).MakeGenericType(keyType, valueType));
                    for (int i = 0; i < keys.Count; i++)
                        dict.Add(keys[i], values[i]);
                    return (T)dict;
                }
                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Queue<>))
                {
                    var elemType = typeof(T).GetGenericArguments()[0];
                    var wrapperType = typeof(QueueWrapper<>).MakeGenericType(elemType);
                    var wrapper = JsonUtility.FromJson(json, wrapperType);
                    return (T)wrapperType.GetMethod("ToQueue").Invoke(wrapper, null);
                }
                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    var elemType = typeof(T).GetGenericArguments()[0];
                    var wrapperType = typeof(HashSetWrapper<>).MakeGenericType(elemType);
                    var wrapper = JsonUtility.FromJson(json, wrapperType);
                    return (T)wrapperType.GetMethod("ToHashSet").Invoke(wrapper, null);
                }

                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SimpleSaver] Failed to load JSON for key '{key}': {ex.Message}");
                return default;
            }
        }

        private static void SaveBinaryPlayerPrefs<T>(string key, T obj, SimpleSaveSettings settings)
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(key);
                    WriteObject(obj, writer);
                    byte[] data = ms.ToArray();
                    if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                        data = Encrypt(data, settings.encryptionPassword);
                    string base64 = Convert.ToBase64String(data);
                    PlayerPrefs.SetString(key, base64);
                    PlayerPrefs.Save();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SimpleSaver] Failed to save PlayerPrefs for key '{key}': {ex.Message}");
            }
        }

        private static T LoadBinaryPlayerPrefs<T>(string key, SimpleSaveSettings settings)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"[SimpleSaver] PlayerPrefs key not found: '{key}'");
                return default;
            }

            string base64 = PlayerPrefs.GetString(key);
            byte[] data = Convert.FromBase64String(base64);
            if (settings.useEncryption && !string.IsNullOrEmpty(settings.encryptionPassword))
                data = Decrypt(data, settings.encryptionPassword);
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms))
            {
                string storedKey = reader.ReadString();
                if (storedKey != key)
                {
                    Debug.LogWarning($"[SimpleSaver] Key mismatch in PlayerPrefs: expected '{key}', got '{storedKey}'");
                    return default;
                }

                try
                {
                    return (T)ReadObject(typeof(T), reader);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[SimpleSaver] Failed to load object from PlayerPrefs for key '{key}': {ex.Message}");
                    return default;
                }
            }
        }

        // --- LEGACY SERIALIZER ---
        private static void WriteObject(object obj, BinaryWriter writer)
        {
            if (obj == null)
            {
                writer.Write((byte)0); // null marker
                return;
            }

            Type type = obj.GetType();

            // Structures — do not write null-marker
            if (type == typeof(int))
            {
                writer.Write((int)obj);
                return;
            }

            if (type == typeof(float))
            {
                writer.Write((float)obj);
                return;
            }

            if (type == typeof(bool))
            {
                writer.Write((bool)obj);
                return;
            }

            if (type == typeof(string))
            {
                writer.Write((string)obj);
                return;
            }

            if (type == typeof(Vector2))
            {
                var v = (Vector2)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                return;
            }

            if (type == typeof(Vector2Int))
            {
                var v = (Vector2Int)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                return;
            }

            if (type == typeof(Vector3))
            {
                var v = (Vector3)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);
                return;
            }

            if (type == typeof(Vector3Int))
            {
                var v = (Vector3Int)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);
                return;
            }

            if (type == typeof(Vector4))
            {
                var v = (Vector4)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);
                writer.Write(v.w);
                return;
            }

            if (type == typeof(Color))
            {
                var c = (Color)obj;
                writer.Write(c.r);
                writer.Write(c.g);
                writer.Write(c.b);
                writer.Write(c.a);
                return;
            }

            if (type == typeof(Color32))
            {
                var c = (Color32)obj;
                writer.Write(c.r);
                writer.Write(c.g);
                writer.Write(c.b);
                writer.Write(c.a);
                return;
            }

            if (type == typeof(Quaternion))
            {
                var q = (Quaternion)obj;
                writer.Write(q.x);
                writer.Write(q.y);
                writer.Write(q.z);
                writer.Write(q.w);
                return;
            }

            if (type == typeof(Rect))
            {
                var r = (Rect)obj;
                writer.Write(r.x);
                writer.Write(r.y);
                writer.Write(r.width);
                writer.Write(r.height);
                return;
            }

            if (type == typeof(RectInt))
            {
                var r = (RectInt)obj;
                writer.Write(r.x);
                writer.Write(r.y);
                writer.Write(r.width);
                writer.Write(r.height);
                return;
            }

            if (type == typeof(Bounds))
            {
                var b = (Bounds)obj;
                writer.Write(b.center.x);
                writer.Write(b.center.y);
                writer.Write(b.center.z);
                writer.Write(b.size.x);
                writer.Write(b.size.y);
                writer.Write(b.size.z);
                return;
            }

            if (type == typeof(BoundsInt))
            {
                var b = (BoundsInt)obj;
                writer.Write(b.position.x);
                writer.Write(b.position.y);
                writer.Write(b.position.z);
                writer.Write(b.size.x);
                writer.Write(b.size.y);
                writer.Write(b.size.z);
                return;
            }

            if (type == typeof(Matrix4x4))
            {
                var m = (Matrix4x4)obj;
                writer.Write(m.m00);
                writer.Write(m.m01);
                writer.Write(m.m02);
                writer.Write(m.m03);
                writer.Write(m.m10);
                writer.Write(m.m11);
                writer.Write(m.m12);
                writer.Write(m.m13);
                writer.Write(m.m20);
                writer.Write(m.m21);
                writer.Write(m.m22);
                writer.Write(m.m23);
                writer.Write(m.m30);
                writer.Write(m.m31);
                writer.Write(m.m32);
                writer.Write(m.m33);
                return;
            }

            if (type == typeof(DateTime))
            {
                writer.Write(((DateTime)obj).Ticks);
                return;
            }

            if (type == typeof(TimeSpan))
            {
                writer.Write(((TimeSpan)obj).Ticks);
                return;
            }

            if (type == typeof(Guid))
            {
                writer.Write(obj.ToString());
                return;
            }

            if (type == typeof(Hash128))
            {
                writer.Write(obj.ToString());
                return;
            }

            // For classes, write null-marker
            writer.Write((byte)1); // not null marker for class

            if (type == typeof(SerializableTransform))
            {
                var st = (SerializableTransform)obj;
                WriteObject(st.position, writer);
                WriteObject(st.rotation, writer);
                WriteObject(st.scale, writer);
                return;
            }

            if (type == typeof(Transform))
            {
                var st = SerializableTransform.FromTransform((Transform)obj);
                WriteObject(st.position, writer);
                WriteObject(st.rotation, writer);
                WriteObject(st.scale, writer);
                return;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (System.Collections.IList)obj;
                writer.Write(list.Count);
                foreach (var item in list)
                    WriteObject(item, writer);
                return;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dict = (System.Collections.IDictionary)obj;
                writer.Write(dict.Count);
                foreach (System.Collections.DictionaryEntry kvp in dict)
                {
                    WriteObject(kvp.Key, writer);
                    WriteObject(kvp.Value, writer);
                }

                return;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Queue<>))
            {
                var queue = (System.Collections.IEnumerable)obj;
                var list = new List<object>();
                foreach (var item in queue)
                    list.Add(item);
                writer.Write(list.Count);
                foreach (var item in list)
                    WriteObject(item, writer);
                return;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Stack<>))
            {
                var stack = (System.Collections.IEnumerable)obj;
                var list = new List<object>();
                foreach (var item in stack)
                    list.Add(item);
                writer.Write(list.Count);
                foreach (var item in list)
                    WriteObject(item, writer);
                return;
            }

            if (type.IsArray && type.GetArrayRank() == 1)
            {
                var arr = (System.Array)obj;
                writer.Write(arr.Length);
                var elemType = type.GetElementType();
                for (int i = 0; i < arr.Length; i++)
                    WriteObject(arr.GetValue(i), writer);
                return;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                var set = (System.Collections.IEnumerable)obj;
                var list = new List<object>();
                foreach (var item in set)
                    list.Add(item);
                writer.Write(list.Count);
                foreach (var item in list)
                    WriteObject(item, writer);
                return;
            }

            // Custom classes
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.IsNotSerialized) continue;
                if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)) continue;
                if (typeof(Delegate).IsAssignableFrom(field.FieldType)) continue;
                object value = field.GetValue(obj);
                WriteObject(value, writer);
            }
        }

        private static object ReadObject(Type type, BinaryReader reader)
        {
            if (type == typeof(int)) return reader.ReadInt32();
            if (type == typeof(float)) return reader.ReadSingle();
            if (type == typeof(bool)) return reader.ReadBoolean();
            if (type == typeof(string)) return reader.ReadString();
            if (type == typeof(Vector2))
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                return new Vector2(x, y);
            }

            if (type == typeof(Vector2Int))
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                return new Vector2Int(x, y);
            }

            if (type == typeof(Vector3))
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                return new Vector3(x, y, z);
            }

            if (type == typeof(Vector3Int))
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                int z = reader.ReadInt32();
                return new Vector3Int(x, y, z);
            }

            if (type == typeof(Vector4))
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                float w = reader.ReadSingle();
                return new Vector4(x, y, z, w);
            }

            if (type == typeof(Color))
            {
                float r = reader.ReadSingle();
                float g = reader.ReadSingle();
                float b = reader.ReadSingle();
                float a = reader.ReadSingle();
                return new Color(r, g, b, a);
            }

            if (type == typeof(Color32))
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = reader.ReadByte();
                return new Color32(r, g, b, a);
            }

            if (type == typeof(Quaternion))
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                float w = reader.ReadSingle();
                return new Quaternion(x, y, z, w);
            }

            if (type == typeof(Rect))
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float w = reader.ReadSingle();
                float h = reader.ReadSingle();
                return new Rect(x, y, w, h);
            }

            if (type == typeof(RectInt))
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                int w = reader.ReadInt32();
                int h = reader.ReadInt32();
                return new RectInt(x, y, w, h);
            }

            if (type == typeof(Bounds))
            {
                float cx = reader.ReadSingle();
                float cy = reader.ReadSingle();
                float cz = reader.ReadSingle();
                float sx = reader.ReadSingle();
                float sy = reader.ReadSingle();
                float sz = reader.ReadSingle();
                return new Bounds(new Vector3(cx, cy, cz), new Vector3(sx, sy, sz));
            }

            if (type == typeof(BoundsInt))
            {
                int px = reader.ReadInt32();
                int py = reader.ReadInt32();
                int pz = reader.ReadInt32();
                int sx = reader.ReadInt32();
                int sy = reader.ReadInt32();
                int sz = reader.ReadInt32();
                return new BoundsInt(new Vector3Int(px, py, pz), new Vector3Int(sx, sy, sz));
            }

            if (type == typeof(Matrix4x4))
            {
                var m = new Matrix4x4();
                m.m00 = reader.ReadSingle();
                m.m01 = reader.ReadSingle();
                m.m02 = reader.ReadSingle();
                m.m03 = reader.ReadSingle();
                m.m10 = reader.ReadSingle();
                m.m11 = reader.ReadSingle();
                m.m12 = reader.ReadSingle();
                m.m13 = reader.ReadSingle();
                m.m20 = reader.ReadSingle();
                m.m21 = reader.ReadSingle();
                m.m22 = reader.ReadSingle();
                m.m23 = reader.ReadSingle();
                m.m30 = reader.ReadSingle();
                m.m31 = reader.ReadSingle();
                m.m32 = reader.ReadSingle();
                m.m33 = reader.ReadSingle();
                return m;
            }

            if (type == typeof(DateTime))
            {
                long ticks = reader.ReadInt64();
                return new DateTime(ticks);
            }

            if (type == typeof(TimeSpan))
            {
                long ticks = reader.ReadInt64();
                return new TimeSpan(ticks);
            }

            if (type == typeof(Guid))
            {
                return new Guid(reader.ReadString());
            }

            if (type == typeof(Hash128))
            {
                return Hash128.Parse(reader.ReadString());
            }

            byte nullMarker = reader.ReadByte();
            if (nullMarker == 0) return null;

            if (type == typeof(SerializableTransform))
            {
                var st = new SerializableTransform();
                st.position = (Vector3)ReadObject(typeof(Vector3), reader);
                st.rotation = (Vector3)ReadObject(typeof(Vector3), reader);
                st.scale = (Vector3)ReadObject(typeof(Vector3), reader);
                return st;
            }

            if (type == typeof(Transform))
            {
                var st = (SerializableTransform)ReadObject(typeof(SerializableTransform), reader);
                return st;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                int count = reader.ReadInt32();
                var list = (System.Collections.IList)Activator.CreateInstance(type);
                Type itemType = type.GetGenericArguments()[0];
                for (int i = 0; i < count; i++)
                    list.Add(ReadObject(itemType, reader));
                return list;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                int count = reader.ReadInt32();
                var dict = (System.Collections.IDictionary)Activator.CreateInstance(type);
                Type keyType = type.GetGenericArguments()[0];
                Type valType = type.GetGenericArguments()[1];
                for (int i = 0; i < count; i++)
                {
                    var key = ReadObject(keyType, reader);
                    var val = ReadObject(valType, reader);
                    dict.Add(key, val);
                }

                return dict;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Queue<>))
            {
                int count = reader.ReadInt32();
                Type itemType = type.GetGenericArguments()[0];
                var list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                for (int i = 0; i < count; i++)
                    list.Add(ReadObject(itemType, reader));
                var queueObj = Activator.CreateInstance(type, list);
                return queueObj;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Stack<>))
            {
                int count = reader.ReadInt32();
                Type itemType = type.GetGenericArguments()[0];
                var list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                for (int i = 0; i < count; i++)
                    list.Add(ReadObject(itemType, reader));
                var stackObj = Activator.CreateInstance(type, list);
                return stackObj;
            }

            if (type.IsArray && type.GetArrayRank() == 1)
            {
                int count = reader.ReadInt32();
                Type elemType = type.GetElementType();
                var arr = Array.CreateInstance(elemType, count);
                for (int i = 0; i < count; i++)
                    arr.SetValue(ReadObject(elemType, reader), i);
                return arr;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                int count = reader.ReadInt32();
                Type itemType = type.GetGenericArguments()[0];
                var hashSet = Activator.CreateInstance(type);
                var addMethod = type.GetMethod("Add");
                for (int i = 0; i < count; i++)
                {
                    var item = ReadObject(itemType, reader);
                    addMethod.Invoke(hashSet, new object[] { item });
                }

                return hashSet;
            }

            // Custom classes
            object instance = Activator.CreateInstance(type);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.IsNotSerialized) continue;
                if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)) continue;
                if (typeof(Delegate).IsAssignableFrom(field.FieldType)) continue;
                object val = ReadObject(field.FieldType, reader);
                field.SetValue(instance, val);
            }

            return instance;
        }

        private static byte[] Encrypt(byte[] data, string password)
        {
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SimpleSaveSalt123"));
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                    return ms.ToArray();
                }
            }
        }

        private static byte[] Decrypt(byte[] data, string password)
        {
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SimpleSaveSalt123"));
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                    return ms.ToArray();
                }
            }
        }

        [System.Serializable]
        private class ListWrapper<T>
        {
            public List<T> list;
        }

        [System.Serializable]
        private class DictionaryWrapper<TKey, TValue>
        {
            public List<TKey> keys;
            public List<TValue> values;
        }

        [System.Serializable]
        private class QueueWrapper<T>
        {
            public List<T> items;

            public QueueWrapper()
            {
                items = new List<T>();
            }

            public QueueWrapper(Queue<T> queue)
            {
                items = new List<T>(queue);
            }

            public Queue<T> ToQueue() => new Queue<T>(items);
        }

        [System.Serializable]
        private class HashSetWrapper<T>
        {
            public List<T> items;

            public HashSetWrapper()
            {
                items = new List<T>();
            }

            public HashSetWrapper(HashSet<T> set)
            {
                items = new List<T>(set);
            }

            public HashSet<T> ToHashSet() => new HashSet<T>(items);
        }

        /// <summary>
        /// Loads saved transform data and applies it to the specified Transform.
        /// </summary>
        /// <param name="key">Unique key for the saved data.</param>
        /// <param name="target">Target Transform to apply loaded data to.</param>
        /// <example>
        /// <![CDATA[
        /// SimpleSaver.LoadInto("playerTransform", player.transform);
        /// ]]>
        /// </example>
        public static void LoadInto(string key, Transform target)
        {
            var st = Load<SerializableTransform>(key);
            if (st != null)
                st.ApplyTo(target);
        }

        /// <summary>
        /// Asynchronously saves an object with the specified key using the current save format and settings.
        /// </summary>
        /// <typeparam name="T">Type of the object to save.</typeparam>
        /// <param name="key">Unique key for the saved data.</param>
        /// <param name="obj">Object to save.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        /// <example>
        /// <![CDATA[
        /// await SimpleSaver.SaveAsync("playerScore", 123);
        /// ]]>
        /// </example>
        public static async Task SaveAsync<T>(string key, T obj)
        {
#if UNITY_WEBGL
    Save(key, obj);
#else
            await Task.Run(() => Save(key, obj));
#endif
        }

        /// <summary>
        /// Asynchronously loads an object of type T by key using the current save format and settings.
        /// </summary>
        /// <typeparam name="T">Type of the object to load.</typeparam>
        /// <param name="key">Unique key for the saved data.</param>
        /// <returns>The loaded object, or default(T) if not found or on error.</returns>
        /// <example>
        /// <![CDATA[
        /// int score = await SimpleSaver.LoadAsync<int>("playerScore");
        /// ]]>
        /// </example>
        public static async Task<T> LoadAsync<T>(string key)
        {
#if UNITY_WEBGL
    return Load<T>(key, fallback);
#else
            return await Task.Run(() => Load<T>(key));
#endif
        }
    }
}