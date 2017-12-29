/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigControl.cs
 * 文件功能描述：配置数据表加载控制器
 * 
 * 创建者：
 * 创建日期：2014/07/22
 * 
 *******************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;


public static class ConfigControl
{
    public static void Load(Action onComplete)
    {
#if UNITY_EDITOR

        if (onComplete != null) onComplete();
        //Util.StartCoroutine(_LoadConfig(onComplete));
#else
        if (onComplete != null) onComplete();
        //Util.StartCoroutine(_LoadConfig(onComplete));
#endif
    }

    static IEnumerator _LoadConfig(Action onComplete)
    {
        ConfigAsset asset = null;
        ConfigData data = null;
        float progress = 0f;
        //string assetPath = string.Format("file:///{0}/ConfigMapObjectData.a", Util.AssetRoot);
        string assetPath = string.Format("{0}/ConfigMapObjectData.a", Util.AssetsPath);
        WWW www = new WWW(assetPath);
        for (; !www.isDone; )
        {
            progress = www.progress;
            //LoadingWindow.LoginPanel.Instance.SetProgressBar(0.7f + progress * 0.1f);
            yield return new WaitForEndOfFrame();
        }
        if (!string.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);
        else
        {
            asset = www.assetBundle.mainAsset as ConfigAsset;
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopswatch();
            //sw.Start();
            data = asset.Deserialize();
            //sw.Stop();
            //Debug.LogWarning("11111111111111111111:" + sw.ElapsedMilliseconds);
            if (ConfigManager.Instance.configs.ContainsKey(asset.name))
                ConfigManager.Instance.configs[asset.name] = data;
            else
                ConfigManager.Instance.configs.Add(asset.name, data);
            www.assetBundle.Unload(true);
        }
        www.Dispose();
        www = null;
        yield return new WaitForEndOfFrame();

        assetPath = string.Format("{0}/ConfigMapObjectDungeonData.a", Util.AssetsPath);
        www = new WWW(assetPath);
        for (; !www.isDone; )
        {
            progress = www.progress;
            //LoadingWindow.LoginPanel.Instance.SetProgressBar(0.8f + progress * 0.1f);
            yield return new WaitForEndOfFrame();
        }
        if (!string.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);
        else
        {
            asset = www.assetBundle.mainAsset as ConfigAsset;
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            data = asset.Deserialize();
            //sw.Stop();
            //Debug.LogWarning("22222222222222222222222222:" + sw.ElapsedMilliseconds);
            if (ConfigManager.Instance.configs.ContainsKey(asset.name))
                ConfigManager.Instance.configs[asset.name] = data;
            else
                ConfigManager.Instance.configs.Add(asset.name, data);
            www.assetBundle.Unload(true);
        }
        www.Dispose();
        www = null;
        yield return new WaitForEndOfFrame();

        //assetPath = string.Format("file:///{0}/Config.a", Util.AssetRoot);
        assetPath = string.Format("{0}/Config.a", Util.AssetsPath);
        www = new WWW(assetPath);
        for (; !www.isDone; )
        {
            progress = www.progress;
            //LoadingWindow.LoginPanel.Instance.SetProgressBar(0.9f + progress * 0.05f);
            yield return new WaitForEndOfFrame();
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(string.Format("assetPath:{0}", assetPath));
            Debug.LogError(www.error);
        }
        else
        {
            Object[] configs = www.assetBundle.LoadAllAssets();

            for (int i = 0; i < configs.Length; ++i)
            {
                Object config = configs[i];
                if (config is ConfigAsset)
                {
                    asset = config as ConfigAsset;
                    data = asset.Deserialize();
                    ConfigManager.Instance.configs.Add(asset.name, data);
                    //LoadingWindow.LoginPanel.Instance.SetProgressBar(0.95f + (i + 1f) / configs.Length * 0.05f);
                    yield return new WaitForEndOfFrame();
                }
            }

            www.assetBundle.Unload(true);
        }
        www.Dispose();
        www = null;

        //LoadingWindow.LoginPanel.Instance.SetProgressBar(1f);
        //LoadingWindow.LoginPanel.Instance.ShowTip("");
        //LoadingWindow.LoginPanel.Instance.ShowProgressBar(false);
        //LoadingWindow.Instance.ShowLogin(true);

        if (onComplete != null) onComplete();
    }

    private static void LoadConfigFromDir()
    {
        FileInfo[] files = new DirectoryInfo("../../common/csv").GetFiles();
        try
        {
            int handledCount = 0;
            for (int i = 0; i < files.Length; ++i)
            {
                FileInfo fi = files[i];
                if (fi.Name != "配置文件说明.csv")
                {
                    ConfigData data = CreateAsset(fi);
                    string fileName = fi.Name.Split(new char[] { '.' })[0];
                    if (!ConfigManager.Instance.configs.ContainsKey(fileName))
                        ConfigManager.Instance.configs.Add(fileName, data);
                    else
                        ConfigManager.Instance.configs[fileName] = data;
                    handledCount++;
                }
            }
            Debug.LogFormat("加载配置文件个数：{0}", handledCount);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private static ConfigData CreateAsset(FileInfo file)
    {
        if ((file.Name != "Template.csv") && (file.Extension == ".csv"))
        {
            Debug.LogFormat("导入文件: {0}......", file.Name);
            return ImportCsv(file);
        }
        //if (file.Extension == ".xml")
        //{
        //    Logger.Debug("导入文件: {0}......", file.Name);
        //    return ImportXml(file);
        //}
        return null;
    }

    private static ConfigData ImportCsv(FileInfo file)
    {
        StreamReader sr = null;
        FileStream stream = null;
        try
        {
            #region 以Unicode编码打开csv文件
            try
            {
                stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(stream, Encoding.Unicode);
            }
            catch (IOException exception)
            {
                throw new IOException("读取文件出错：" + file.FullName + "\r\n" + exception.StackTrace);
            }
            #endregion

            #region 解析列表名和数据类型，忽略描述

            // 列名
            string[] fieldNames = ReadLine(sr);
            List<string> fieldNameList = new List<string>();
            for (int i = 0; i < fieldNames.Length; ++i)
            {
                if (Util.IsNullOrWhiteSpace(fieldNames[i]))
                    continue;
                fieldNameList.Add(fieldNames[i]);
            }
            ConfigData config = new ConfigData
            {
                //FileType = 1,
                fieldNames = fieldNameList.ToArray()
            };
            int column = config.fieldNames.Length;

            // 列类型
            string[] types = ReadLine(sr);
            config.fieldTypes = new int[column];
            for (int i = 0; i < column; i++)
            {
                if (i < types.Length)
                {
                    switch (types[i])
                    {
                        case "int":
                            config.fieldTypes[i] = ConfigDataType.INT;
                            break;

                        case "bool":
                            config.fieldTypes[i] = ConfigDataType.BOOL;
                            break;

                        case "float":
                            config.fieldTypes[i] = ConfigDataType.FLOAT;
                            break;

                        case "long":
                            config.fieldTypes[i] = ConfigDataType.LONG;
                            break;

                        case "double":
                            config.fieldTypes[i] = ConfigDataType.DOUBLE;
                            break;

                        case "string":
                            config.fieldTypes[i] = ConfigDataType.STRING;
                            break;

                        case "enum":
                            config.fieldTypes[i] = ConfigDataType.ENUM;
                            break;

                        case "array":
                            config.fieldTypes[i] = ConfigDataType.ARRAY;
                            break;

                        case "json":
                            config.fieldTypes[i] = ConfigDataType.JSON;
                            break;

                        case "date":
                            config.fieldTypes[i] = ConfigDataType.DATETIME;
                            break;

                        case "custom":
                            config.fieldTypes[i] = ConfigDataType.CUSTOM;
                            break;

                        default:
                            Debug.LogError(file.Name + " - 未识别的数据类型格式：\"" + types[i] + "\" 在第\"" + i + 1 + "\"列");
                            config.fieldTypes[i] = ConfigDataType.UNKNOWN;
                            break;
                    }
                }
                else
                {
                    throw new Exception("列表错误，列数不对应。");
                }
            }

            // 列描述，可忽略
            ReadLine(sr);
            #endregion

            #region 解析所有行数据
            // 数据
            List<ConfigSheetLine> list = new List<ConfigSheetLine>();            
            int lineCount = 3;
            string[] sArray = null;
            while ((sArray = ReadLine(sr)) != null)
            {
                if (Util.IsNullOrWhiteSpace(sArray[0]))
                    continue;
                lineCount++;
                ConfigSheetLine item = new ConfigSheetLine
                {
                    line = new object[column]
                };
                for (int j = 0; j < column; j++)
                {
                    try
                    {
                        if (config.fieldTypes[j] == ConfigDataType.INT)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? 0 : int.Parse(sArray[j]);
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.BOOL)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? ((object)0) : ((object)bool.Parse(sArray[j]));
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.FLOAT)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? 0f : float.Parse(sArray[j]);
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.LONG)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? 0L : long.Parse(sArray[j]);
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.DOUBLE)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? 0.0 : double.Parse(sArray[j]);
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.ENUM)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? 0 : int.Parse(sArray[j]);
                        }
                        else if (config.fieldTypes[j] == ConfigDataType.ARRAY)
                        {
                            item.line[j] = !(sArray[j] != string.Empty) ? new string[0] : sArray[j].Split(new char[] { ';' });
                        }
                        else
                        {
                            item.line[j] = sArray[j];
                        }
                    }
                    catch (FormatException exception)
                    {
                        object[] exMsg = new object[] { file.Name, " 数据格式转换错误：第", lineCount, "行，第", j + 1, "列\r\n", exception.StackTrace };
                        throw new FormatException(string.Concat(exMsg));
                    }
                }
                list.Add(item);
            }
            #endregion

            // 检查是否有重复的id
            config.data = list.ToArray();
            if (!file.Name.Remove(file.Name.LastIndexOf(".csv", StringComparison.Ordinal)).Equals("配置文件说明"))
            {
                CheckRepeatedId(config);
            }
            return config;
        }
        finally
        {
            try
            {
                stream.Close();
                sr.Close();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 增加对Excel的"Unicode文本"格式中换行和双引号的解析支持
    /// </summary>
    /// <returns>The line.</returns>
    /// <param name="sr">Sr.</param>
    private static string[] ReadLine(StreamReader sr)
    {
        string str = sr.ReadLine();
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        bool flag = false;
        while (!flag && !string.IsNullOrEmpty(str))
        {
            if (flag && (str == string.Empty))
            {
                str = str + "\r\n";
            }
            else
            {
                if (IsFullLine(str))
                {
                    flag = true;
                    continue;
                }
                str = str + "\r\n" + string.Empty;
            }
        }
        char[] separator = new char[] { '\t' };
        string[] strArray = str.Split(separator);
        
        for (int i = 0; i < strArray.Length; i++)
        {
            string str2 = strArray[i];
            if (str2.StartsWith("\""))
            {
                str2 = str2.Substring(1, str2.Length - 2).Replace("\"\"", "\"");
            }
            strArray[i] = str2;
        }
        return strArray;
    }

    /// <summary>
    /// 判断一行是否还有后续行.
    /// </summary>
    /// <returns><c>true</c> if is full line the specified line; otherwise, <c>false</c>.</returns>
    /// <param name="line">Line.</param>
    private static bool IsFullLine(string line)
    {
        int index = line.LastIndexOf('\t') + 1;
        char[] chArray = line.ToCharArray();
        if ((index >= chArray.Length) || (chArray[index] != '"'))
        {
            return true;
        }
        for (int i = index + 1; i < chArray.Length; i++)
        {
            if (chArray[i] == '"')
            {
                if (((i + 1) >= chArray.Length) || (chArray[i + 1] != '"'))
                {
                    return true;
                }
                i++;
            }
        }
        return false;
    }

    private static void CheckRepeatedId(ConfigData data)
    {
        if (((data.data != null) && (data.data.Length > 1)) && (data.fieldNames.Length > 0))
        {
            int length = data.data.Length;
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (data.data[i].GetData(0).Equals(data.data[j].GetData(0)))
                    {
                        throw new Exception(string.Format("重复的Id项：第{0}行和第{1}行", i + 4, j + 4));
                    }
                }
            }
        }
    }
}

