/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigSheetLine.cs
 * 文件功能描述：配置数据表的一行
 * 
 * 创建者：
 * 创建日期：2014/07/18
 * 
 *******************************************************************/

using System;
using Debug = System.Diagnostics.Debug;

[Serializable]
public class ConfigSheetLine
{
    public object[] line;

    public object GetData(int field)
    {
        Debug.Assert((field >= 0) && (field < this.line.Length));

        return this.line[field];
    }
}


