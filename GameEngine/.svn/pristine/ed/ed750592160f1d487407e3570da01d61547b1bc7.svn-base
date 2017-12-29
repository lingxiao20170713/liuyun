/********************************************************************
 * Copyright (C) 2011 网络科技有限公司
 * 版权所有
 * 
 * 文件名：FPS.cs
 * 文件功能描述：显示帧率信息
 *******************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    public float f_UpdateInterval = 0.5f;
    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;

    private void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;
        i_Frames = 0;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 300, 200, 200), "FPS:" + f_Fps.ToString("f2"));
    }
    private void Update()
    {
        ++i_Frames;
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;
            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}
