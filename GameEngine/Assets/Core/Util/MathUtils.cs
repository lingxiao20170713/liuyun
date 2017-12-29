

using UnityEngine;
using System.Collections;

using System;


public class MathUtils
{
    /// Gets the distance using x and z coord
    public static float FlatDistance(Vector3 from, Vector3 to)
    {
        float xDif = from.x - to.x;
        float zDif = from.z - to.z;

        return (float)Math.Sqrt(xDif * xDif + zDif * zDif);
    }

    /// Converts degrees to radians
    public static float DegreesToRadians(float degrees)
    {
        return (float)(degrees * System.Math.PI) / 180f;
    }

    //向量字符串格式：[posX, posY, posZ] 
    public static Vector3 ToVector3(string value)
    {
        value = value.TrimStart(new char[] { '[', '{' });  //去掉"["或"{"
        value = value.TrimEnd  (new char[] { ']', '}' });    //去掉"]"或"}"

        string[] elems = value.Split(new char[] { ',', '，', '#', '|' });

        if (elems.Length != 3)
        {
            Debug.LogErrorFormat(">>>>Vector string {0} is invalid!", value);
            return Vector3.zero;
        }

        return new Vector3(MathUtils.ParseFloat(elems[0]), MathUtils.ParseFloat(elems[1]), MathUtils.ParseFloat(elems[2]));    
    }

    //旋转四元数字符串格式:[rotatex,rotatey, rotatez, rotatew] 
    public static Quaternion ToQuaternion(string value)
    {
        value = value.TrimStart(new char[] { '[', '{' });  //去掉"["或"{"
        value = value.TrimEnd  (new char[] { ']', '}' });    //去掉"]"或"}"

        string[] elems = value.Split(new char[] { ',', '，', '#', '|' });

        if (elems.Length != 4)
        {
            Debug.LogErrorFormat(">>>>Quaternion string {0} is invalid!", value);
            return Quaternion.identity;
        }

        return new Quaternion(MathUtils.ParseFloat(elems[0]), MathUtils.ParseFloat(elems[1]), MathUtils.ParseFloat(elems[2]), MathUtils.ParseFloat(elems[3]));
    }

    public static float ParseFloat(string value)
    {
        value = value.Replace("f","");  //去掉float数字后面可能带的f字符
        return float.Parse(value);
    }

    public static int ParseInt(string value)
    {
        return int.Parse(value);
    }

    public static long ParseLong(string value)
    {
        return long.Parse(value);
    }

    public static bool IsNumber(string strNumber)
    {
        float fNumber;
        return float.TryParse(strNumber, out fNumber);
    }

    public static bool TryParseFloat(string strValue, out float fValue)
    {
        if (MathUtils.IsNumber(strValue))
        {
            fValue = MathUtils.ParseFloat(strValue);
            return true;
        }
        else
        {
            fValue = 0.0f;
            return false;
        }
    }

    public static bool TryParseInt(string strValue, out int nValue)
    {
        if (MathUtils.IsNumber(strValue))
        {
            nValue = MathUtils.ParseInt(strValue);
            return true;
        }
        else
        {
            nValue = 0;
            return false;
        }
    }

    /// <summary>
    /// 从一系列参数中获取最小的那个，尽量不为0，如果都是0就返回0
    /// </summary>
    /// <param name="prams"></param>
    /// <returns></returns>
    public static int MinNoZero(params int[] prams)
    {
        int min = 0;
        int tmpParam;
        for (int counter = 0; counter < prams.Length; counter ++ )
        {
            tmpParam = prams[counter];
            if (tmpParam == 0) continue;
            if (min == 0) 
                min = tmpParam;
            else
                min = (min > tmpParam) ? tmpParam : min;
        }
        return min;
    }
}