/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：TxtConfig.cs
 * 文件功能描述：Txt格式配置表解析
 * 
 * 创建者：
 * 创建日期：2014/07/23
 * 
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Text;


public class TxtConfig
{
    private string _data;
    private Dictionary<string, string> _paramMap = new Dictionary<string, string>();

    public bool Analyze()
    {
        if (this._data == null)
        {
            return false;
        }
        this._paramMap.Clear();
        char[] separator = new char[] { '\r', '\n' };
        foreach (string line in this._data.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!line.StartsWith("#"))
            {
                char[] lineSeprator = new char[] { ' ', '=' };
                string[] elements = line.Split(lineSeprator, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length == 2)
                {
                    this._paramMap.Add(elements[0], elements[1]);
                }
                if (elements.Length > 2)
                {
                    string str = string.Empty;
                    for (int i = 1; i < elements.Length; i++)
                    {
                        if (i != (elements.Length - 1))
                        {
                            str = str + elements[i] + "=";
                        }
                        else
                        {
                            str = str + elements[i];
                        }
                    }
                    this._paramMap.Add(elements[0], str);
                }
            }
        }
        return true;
    }

    public void Dispose()
    {
        if (this._paramMap != null)
        {
            this._paramMap.Clear();
        }
        this._paramMap = null;
        this._data = null;
    }

    public int GetInt(string name, int defaultValue)
    {
        string str;
        if (this._paramMap.TryGetValue(name, out str))
        {
            return Convert.ToInt32(str);
        }
        return defaultValue;
    }

    public string GetString(string name, string defaultValue)
    {
        string str;
        if (this._paramMap.TryGetValue(name, out str))
        {
            return str;
        }
        return defaultValue;
    }

    public void Load(string texts)
    {
        if (texts != null)
        {
            this._data = texts;
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("\n");
        foreach (string str in this._paramMap.Keys)
        {
            builder.AppendFormat("{0} = {1}\n", str, this._paramMap[str]);
        }
        return builder.ToString();
    }
}


