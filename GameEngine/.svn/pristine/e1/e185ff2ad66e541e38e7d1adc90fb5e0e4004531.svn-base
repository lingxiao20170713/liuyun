using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : Singleton<LocationManager>
{
    //每一次调用只会请求一次定位，不会连续返回
    public void StartLocation()
    {
        //定位
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            IOSBridgeManager.StartLocation();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {

            using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.location.LocationUtil"))
            {
                jc.CallStatic("startLocation");
            }
        }
    }


    // 安卓用于关闭定位服务
    public void StopLocation()
    {
        //定位
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            IOSBridgeManager.StopLocation();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {

            using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.location.LocationUtil"))
            {
                jc.CallStatic("destoryLocation");
            }
        }
    }
}
