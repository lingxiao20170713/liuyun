/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigBase.cs
 * 文件功能描述：配置数据表基类
 * 
 * 创建者：
 * 创建日期：2014/07/16
 * 
 *******************************************************************/
using System;
using System.Reflection;
using Debug = System.Diagnostics.Debug;


public class ConfigBase : IComparable
{
    public int NId;
    public string SName;


    public virtual int CompareTo(object obj)
    {
        try
        {
            ConfigBase other = (ConfigBase)obj;
            if (this.NId > other.NId)
            {
                return 1;
            }
            else if (this.NId < other.NId)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("比较异常", ex.InnerException);
        }
    }

    public object copy()
    {
        return MemberwiseClone();
    }

    public object copyDeep()
    {
        return MemberwiseClone();
    }

    public void LoadConfig(ConfigData data, int id)
    {
        Debug.Assert(data != null);
        ConfigSheetLine content = data.GetData(id);
        LoadConfigByLine(data, content);
    }

    public void LoadConfig(ConfigData data, ConfigSheetLine[] lines)
    {
        Debug.Assert(data != null && lines != null && lines.Length > 0);            
        ConfigSheetLine content = lines[0];
        LoadConfigByLine(data, content);          
    }
    public void LoadConfig(ConfigData data, string[] fieldNames, object[] values)
    {
        if (data != null)
        {
            ConfigSheetLine[] linesByFieldNameValue = data.GetLinesByFieldNameValue(fieldNames, values);
            if (linesByFieldNameValue != null && linesByFieldNameValue.Length > 0)
            {
                ConfigSheetLine content = linesByFieldNameValue[0];
                LoadConfigByLine(data, content);
            }
        }
    }

    public void LoadConfigByLine(ConfigData data, ConfigSheetLine content)
    {
        Debug.Assert(data != null && content != null);

        FieldInfo[] infos = GetType().GetFields();
        for (int i = 0; i < infos.Length; ++i)
        {
            FieldInfo info = infos[i];
            int fieldIndex = data.GetFieldByName(info.Name);
            if (fieldIndex == -1) continue;

            object fieldValue = content.GetData(fieldIndex);
            int fieldType = data.GetDataType(fieldIndex);
            switch (fieldType)
            {
                case ConfigDataType.UNKNOWN:
                case ConfigDataType.INT:
                case ConfigDataType.BOOL:
                case ConfigDataType.FLOAT:
                case ConfigDataType.LONG:
                case ConfigDataType.DOUBLE:
                case ConfigDataType.STRING:
                case ConfigDataType.ENUM:
                case ConfigDataType.JSON:
                    info.SetValue(this, fieldValue);
                    break;

                case ConfigDataType.ARRAY:
                    Type dataType = info.FieldType;
                    Type elementType = dataType.GetElementType();
                    if (dataType.IsArray && (elementType == typeof(int) || elementType.IsEnum || elementType == typeof(bool) || elementType == typeof(float)) || elementType == typeof(long) || elementType == typeof(double) || elementType == typeof(string))
                        DecodeArrayValue(info, fieldValue, elementType);
                    else
                        DecodeSpecialValue(info, fieldValue);
                    break;

                case ConfigDataType.CUSTOM:
                    DecodeSpecialValue(info, fieldValue);
                    break;

                case ConfigDataType.DATETIME:
                    if (!string.IsNullOrEmpty((string)fieldValue))
                        info.SetValue(this, Convert.ToDateTime(fieldValue));
                    break;
            }
        }
    }

    public override string ToString()
    {
        string str = string.Empty;
        FieldInfo[] infos = base.GetType().GetFields();
        for (int i = 0; i < infos.Length; ++i)
        {
            str += infos[i].Name;
            str += "=";
            str += infos[i].GetValue(this);
            str += "\n";
        }
        return str;
    }

    // 解析自定义类型字段
    protected virtual void DecodeSpecialValue(FieldInfo fi, object value)
    {
        throw new Exception("需要子类实现特定数据的解析!");
    }

    // 解析数组字段
    private void DecodeArrayValue(FieldInfo fi, object value, Type arrayType)
    {
        string[] strArray = (string[]) value;
        if (arrayType.IsEnum || (arrayType == typeof(int)))
        {
            int[] intArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                intArray[i] = int.Parse(strArray[i]);
            }
            fi.SetValue(this, intArray);
        }
        else if (arrayType == typeof(bool))
        {
            bool[] flagArray = new bool[strArray.Length];
            for (int j = 0; j < strArray.Length; j++)
            {
                flagArray[j] = bool.Parse(strArray[j]);
            }
            fi.SetValue(this, flagArray);
        }
        else if (arrayType == typeof(float))
        {
            float[] floatArray = new float[strArray.Length];
            for (int k = 0; k < strArray.Length; k++)
            {
                floatArray[k] = float.Parse(strArray[k]);
            }
            fi.SetValue(this, floatArray);
        }
        else if (arrayType == typeof(long))
        {
            long[] longArray = new long[strArray.Length];
            for (int m = 0; m < strArray.Length; m++)
            {
                longArray[m] = long.Parse(strArray[m]);
            }
            fi.SetValue(this, longArray);
        }
        else if (arrayType == typeof(double))
        {
            double[] doubleArray = new double[strArray.Length];
            for (int n = 0; n < strArray.Length; n++)
            {
                doubleArray[n] = double.Parse(strArray[n]);
            }
            fi.SetValue(this, doubleArray);
        }
        else if (arrayType == typeof(string))
        {
            fi.SetValue(this, strArray);
        }
    }      
}


