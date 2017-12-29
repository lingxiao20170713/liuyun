/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigManager.cs
 * 文件功能描述：配置数据管理器
 * 
 * 创建者：
 * 创建日期：2014/07/21
 * 
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using Debug = System.Diagnostics.Debug;


public class ConfigManager : Singleton<ConfigManager>
{
    //    private Dictionary<string, Dictionary<int, Array>> _allConfigs;
    //    public Dictionary<string, string> _commonValue;
    public Dictionary<string, ConfigData> configs = new Dictionary<string, ConfigData>();

    public void Clear()
    {
        //        if (_allConfigs != null)
        //            _allConfigs.Clear();
        //        if (_commonValue != null) 
        //            _commonValue.Clear();
        configs.Clear();
    }

    public T[] GetAllConfig<T>() where T : ConfigBase, new()
    {
        string url = GetUrl(typeof(T));
        if (url != null)
        {
            //            if (this._allConfigs == null)
            //            {
            //                this._allConfigs = new Dictionary<string, Dictionary<int, Array>>();
            //            }
            //            if (!this._allConfigs.ContainsKey(url))
            //            {
            //                this._allConfigs.Add(url, new Dictionary<int, Array>());
            //            }
            if (!this.configs.ContainsKey(url))
            {
                return null;
            }
            ConfigData data = this.configs[url];
            if (data != null)
            {
                //                Dictionary<int, Array> config = this._allConfigs[url];
                T[] lines = new T[data.data.Length];
                //                Dictionary<int, List<ConfigBase>> cfgBasesDict = new Dictionary<int, List<ConfigBase>>();
                for (int i = 0; i < data.data.Length; i++)
                {
                    T item = Activator.CreateInstance<T>();
                    item.LoadConfigByLine(data, data.data[i]);
                    lines[i] = item;
                    //                    if (!config.ContainsKey(item.NId))
                    //                    {
                    //                        if (!cfgBasesDict.ContainsKey(item.NId))
                    //                        {
                    //                            cfgBasesDict.Add(item.NId, new List<ConfigBase>());
                    //                        }
                    //                        cfgBasesDict[item.NId].Add(item);
                    //                    }
                }
                //                foreach (KeyValuePair<int, List<ConfigBase>> pair in cfgBasesDict)
                //                {
                //                    if (pair.Value.Count > 0)
                //                    {
                //                        List<T> cfgBaseList = new List<T>();
                //                        foreach (ConfigBase cfgBase in pair.Value)
                //                        {
                //                            cfgBaseList.Add(cfgBase as T);
                //                        }
                //                        config.Add(pair.Key, cfgBaseList.ToArray());
                //                    }
                //                    else
                //                    {
                //                        config.Add(pair.Key, null);
                //                    }
                //                }
                return lines;
            }
        }
        return null;
    }

    //    public string GetCommmonValue(string key)
    //    {
    //        if (this._commonValue == null)
    //        {
    //            ConfigData data;
    //            if (this.configs.ContainsKey("Common"))
    //            {
    //                data = this.configs["Common"];
    //                this._commonValue = new Dictionary<string, string>();
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //            foreach (ConfigSheetLine line in data.data)
    //            {
    //                string str = (string)line.GetData(1);
    //                string str2 = (string)line.GetData(2);
    //                this._commonValue.Add(str, str2);
    //            }
    //        }
    //        if (this._commonValue.ContainsKey(key))
    //        {
    //            return this._commonValue[key];
    //        }
    //        return null;
    //    }

    //    public T GetConfig<T>(int id) where T : ConfigBase, new()
    //    {
    //        T[] configArray = GetConfigArray<T>(id);
    //        if ((configArray != null) && (configArray.Length > 0))
    //        {
    //            return configArray[0];
    //        }
    //        return null;
    //    }

    //    public T[] GetConfigArray<T>(int id) where T : ConfigBase, new()
    //    {
    //        string url = GetUrl(typeof(T));
    //        if (url == null) return null;
    //
    //        if (this._allConfigs == null)
    //        {
    //            this._allConfigs = new Dictionary<string, Dictionary<int, Array>>();
    //        }
    //        if (!this._allConfigs.ContainsKey(url))
    //        {
    //            this._allConfigs.Add(url, new Dictionary<int, Array>());
    //        }
    //        Dictionary<int, Array> config = this._allConfigs[url];
    //        if (config.ContainsKey(id))
    //        {
    //            return (config[id] as T[]);
    //        }
    //        ConfigData data = this.configs[url];
    //        if (data != null)
    //        {
    //            string[] fieldNames = new string[] { "id" };
    //            object[] values = new object[] { id };
    //            ConfigSheetLine[] linesByFieldNameValue = data.GetLinesByFieldNameValue(fieldNames, values);
    //            if (linesByFieldNameValue != null)
    //            {
    //                T[] lines = new T[linesByFieldNameValue.Length];
    //                for (int i = 0; i < linesByFieldNameValue.Length; i++)
    //                {
    //                    lines[i] = Activator.CreateInstance<T>();
    //                    lines[i].LoadConfigByLine(data, linesByFieldNameValue[i]);
    //                }
    //                config.Add(id, lines);
    //                return lines;
    //            }
    //            config.Add(id, null);
    //        }
    //
    //        return null;
    //    }

    //    public T[] GetConfigBySearch<T>(string[] fields, object[] values) where T : ConfigBase, new()
    //    {
    //        Debug.Assert(fields != null && values != null && fields.Length == values.Length);
    //
    //        FieldInfo[] fieldArray = new FieldInfo[fields.Length];
    //        for (int i = 0; i < fields.Length; i++)
    //        {
    //            fieldArray[i] = typeof(T).GetField(fields[i]);
    //            if (fieldArray[i] == null)
    //            {
    //                return null;
    //            }
    //        }
    //        List<T> list = new List<T>();
    //        foreach (T local in this.GetAllConfig<T>())
    //        {
    //            bool flag = true;
    //            for (int j = 0; j < fieldArray.Length; j++)
    //            {
    //                if (!fieldArray[j].GetValue(local).ToString().Equals(values[j].ToString()))
    //                {
    //                    flag = false;
    //                    break;
    //                }
    //            }
    //            if (flag)
    //            {
    //                list.Add(local);
    //            }
    //        }
    //        if (list.Count > 0)
    //        {
    //            return list.ToArray();
    //        }
    //
    //        return null;
    //    }

    private string GetUrl(Type type)
    {
        return type.Name.Substring(type.Name.IndexOf("Config"));
    }
}
