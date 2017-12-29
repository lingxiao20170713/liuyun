using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有持久化类型的基类
/// </summary>
public abstract class Data
{
    public abstract string Key();
}

/// <summary>
/// 本地数据保存类
/// </summary>
public class CacheManager
{
    public const string ACCOUNT = "account";
    public const string PASSWORD = "password";

    public const string kBigDelimiter = "|*|";
    public const string kSmallDelimiter = "|#|";
    public const string kFieldValueDelimiter = "|=|";
    public const string kPairDelimiter = "|,|";
    public const string kEletmentDelimiter = "|%|";
    private const string kGameDataKey = "gameengine2017xxxx";// todo: 设置为一个唯一的key
    private static Dictionary<string, string> data_cache_;
    private static bool need_save_ = false;
    private static StringBuilder buffer_ = new StringBuilder(1024);

    public static void CheckSave()
    {
        if (need_save_)
            Save();
    }
    public static bool GetBool(string key, bool defaultValue = false)
    {
        string v = null;
        if (data_cache_.TryGetValue(key, out v))
        {
            return bool.Parse(v);
        }
        return defaultValue;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        string v = null;
        if (data_cache_.TryGetValue(key, out v))
        {
            return float.Parse(v);
        }
        return defaultValue;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        string v = null;
        if (data_cache_.TryGetValue(key, out v))
        {
            return int.Parse(v);
        }
        return defaultValue;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        string v = null;
        if (data_cache_.TryGetValue(key, out v))
        {
            return v;
        }
        return defaultValue;
    }

    public static void SetBool(string key, bool v)
    {
        data_cache_[key] = v.ToString();
        need_save_ = true;
    }

    public static void SetFloat(string key, float v)
    {
        data_cache_[key] = v.ToString();
        need_save_ = true;
    }

    public static void SetInt(string key, int v)
    {
        data_cache_[key] = v.ToString();
        need_save_ = true;
    }

    public static void SetString(string key, string v)
    {
        data_cache_[key] = v;
        need_save_ = true;
    }

    public static void SetDatas<T>(string key, List<T> datas)
    {
        if (datas != null && datas.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datas.Count; ++i)
            {
                if (sb.Length > 0)
                    sb.Append(kEletmentDelimiter);
                StringBuilder sb2 = new StringBuilder();
                FieldInfo[] fields = datas[i].GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int j = 0; j < fields.Length; ++j)
                {
                    FieldInfo fi = fields[j];
                    if (sb2.Length > 0)
                        sb2.Append(kPairDelimiter);
                    sb2.Append(fi.Name);
                    sb2.Append(kFieldValueDelimiter);
                    sb2.Append(fi.GetValue(datas[i]).ToString());
                }
                sb.Append(sb2.ToString());
            }
            data_cache_[key] = sb.ToString();
            need_save_ = true;
        }
    }

    public static List<T> GetDatas<T>(string key)
    {
        List<T> data = new List<T>();
        string strs = null;
        if (data_cache_.TryGetValue(key, out strs))
        {
            string[] pair_delimiter = new string[] { kPairDelimiter };
            string[] field_value_delimiter = new string[] { kFieldValueDelimiter };
            string[] elements_str = strs.Split(new string[] { kEletmentDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            if (elements_str != null)
            {
                for (int i = 0; i < elements_str.Length; ++i)
                {
                    T inst = Activator.CreateInstance<T>();
                    string elem_str = elements_str[i];
                    FieldInfo[] fields = inst.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    string[] field_value_str = elem_str.Split(pair_delimiter, StringSplitOptions.RemoveEmptyEntries);
                    if (field_value_str.Length > 0)
                    {
                        for (int j = 0; j < field_value_str.Length; ++j)
                        {
                            string[] field_value = field_value_str[j].Split(field_value_delimiter, StringSplitOptions.RemoveEmptyEntries);
                            if (field_value.Length != 2)
                                Debug.LogError("Wrong GameData: " + field_value);
                            else
                            {
                                for (int k = 0; k < fields.Length; ++k)
                                {
                                    if (fields[k].Name == field_value[0])
                                    {
                                        Type fieldType = fields[k].FieldType;
                                        if (fieldType == typeof(int))
                                            fields[k].SetValue(inst, int.Parse(field_value[1]));
                                        else if (fieldType == typeof(float))
                                            fields[k].SetValue(inst, float.Parse(field_value[1]));
                                        else if (fieldType == typeof(string))
                                            fields[k].SetValue(inst, field_value[1]);
                                        else
                                            Debug.LogError("不支持的类型：" + fieldType.Name);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    data.Add(inst);
                }
            }
        }
        return data;
    }

    // [key|#|field|=|value|,|field|=|value|,|...]
    public static void SetData<T>(T data) where T : Data
    {
        if (data == null) return;

        StringBuilder sb = new StringBuilder();
        FieldInfo[] fields = data.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fi = fields[i];
            if (sb.Length > 0)
                sb.Append(kPairDelimiter);
            sb.Append(fi.Name);
            sb.Append(kFieldValueDelimiter);
            sb.Append(fi.GetValue(data).ToString());
        }

        data_cache_[data.Key()] = sb.ToString();
        //Debug.Log(data_cache_[data.Key()]);
        need_save_ = true;
    }

    public static T GetData<T>(string key) where T : Data
    {
        string str = null;
        T inst = Activator.CreateInstance<T>();
        if (data_cache_.TryGetValue(key, out str))
        {
            FieldInfo[] fields = inst.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            string[] pair_delimiter = new string[] { kPairDelimiter };
            string[] field_value_delimiter = new string[] { kFieldValueDelimiter };
            string[] field_value_str = str.Split(pair_delimiter, StringSplitOptions.RemoveEmptyEntries);
            if (field_value_str.Length > 0)
            {
                for (int i = 0; i < field_value_str.Length; ++i)
                {
                    string[] field_value = field_value_str[i].Split(field_value_delimiter, StringSplitOptions.RemoveEmptyEntries);
                    if (field_value.Length != 2)
                        Debug.LogError("Wrong GameData: " + field_value);
                    else
                    {
                        for (int j = 0; j < fields.Length; ++j)
                        {
                            if (fields[j].Name == field_value[0])
                            {
                                Type fieldType = fields[j].FieldType;
                                if (fieldType == typeof(int))
                                    fields[j].SetValue(inst, int.Parse(field_value[1]));
                                else if (fieldType == typeof(float))
                                    fields[j].SetValue(inst, float.Parse(field_value[1]));
                                else if (fieldType == typeof(string))
                                    fields[j].SetValue(inst, field_value[1]);
                                else
                                    Debug.LogError("不支持的类型：" + fieldType.Name);

                                break;
                            }
                        }
                    }
                }
            }
        }
        return inst;
    }

    public static void Load()
    {
        if (data_cache_ != null) data_cache_.Clear();
        else data_cache_ = new Dictionary<string, string>();

        string data_str = PlayerPrefs.GetString(kGameDataKey);
        if (string.IsNullOrEmpty(data_str)) return;
        string[] big_delimiter = new string[] { kBigDelimiter };
        string[] small_delimiter = new string[] { kSmallDelimiter };
        string[] data_dict = data_str.Split(big_delimiter, StringSplitOptions.RemoveEmptyEntries);
        foreach (string key_value_str in data_dict)
        {
            string[] key_value = key_value_str.Split(small_delimiter, StringSplitOptions.RemoveEmptyEntries);
            if (key_value.Length != 2)
                Debug.LogError("Wrong GameData: " + key_value_str);
            else
                data_cache_[key_value[0]] = key_value[1];
        }
    }

    public static void Save()
    {
        need_save_ = false;
        buffer_.Remove(0, buffer_.Length);
        foreach (KeyValuePair<string, string> pair in data_cache_)
        {
            if (pair.Key.Length != 0 && pair.Value.Length != 0)
            {
                if (buffer_.Length > 0)
                    buffer_.Append(kBigDelimiter);
                buffer_.Append(pair.Key);
                buffer_.Append(kSmallDelimiter);
                buffer_.Append(pair.Value);
            }
        }
        PlayerPrefs.SetString(kGameDataKey, buffer_.ToString()); // todo: 可以加密
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }
}
