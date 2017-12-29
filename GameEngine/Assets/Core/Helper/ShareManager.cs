using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareManager : Singleton<ShareManager>
{
    //分享给微信好友或者微信群
    public void ShareWechatSession(string title, string desc, string urlStr)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            IOSBridgeManager.DMShareWechatSession(title, desc, urlStr);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Util.StartCoroutine(Util.DelayToInvokeDo(() =>
            {
                Debug.LogWarning("Andorid");
                using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.third.wx.WechatUtil"))
                {
                    //要传递过去的参数
                    object[] message = new object[3];
                    message[0] = title; // title
                    message[1] = desc; // desc
                    message[2] = urlStr; // urlStr
                    jc.CallStatic("ShareWechatSession", message);
                }
            }, 0.5f));
        }

    }

    //分享给微信朋友圈
    public void ShareWechatTimeLine(string title, string desc, string urlStr)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            IOSBridgeManager.DMShareWechatTimeline(title, desc, urlStr);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Util.StartCoroutine(Util.DelayToInvokeDo(() =>
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.third.wx.WechatUtil"))
                {
                    //要传递过去的参数
                    object[] message = new object[3];
                    message[0] = title; // title
                    message[1] = desc; // desc
                    message[2] = urlStr; // urlStr
                    jc.CallStatic("ShareWechatTimeLine", message);
                }
            }, 0.5f));
        }
    }

    //截屏分享给微信好友或者微信群
    public void ShareScreenshotToWechatSession()
    {
        string path = Application.persistentDataPath + "/ShareView.png";
        if (System.IO.File.Exists(path))
        {

            System.IO.File.Delete(path);
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            Util.StartCoroutine(iOSShareScreenshot(false));

        }
        else if (Application.platform == RuntimePlatform.Android)
        {

            Util.StartCoroutine(AndroidShareScreenshot("ShareScreenshotToWechatSession"));

        }

    }

    //截屏分享给微信朋友圈
    public void ShareScreenshotToWechatTimeLine()
    {
        string path = Application.persistentDataPath + "/ShareView.png";
        if (System.IO.File.Exists(path))
        {

            System.IO.File.Delete(path);
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            Util.StartCoroutine(iOSShareScreenshot(true));
        }
        else if (Application.platform == RuntimePlatform.Android)
        {

            Util.StartCoroutine(AndroidShareScreenshot("ShareScreenshotToWechatTimeLine"));

        }
    }

    //苹果分享协程
    private IEnumerator iOSShareScreenshot(bool shareToTimeLine)
    {

        Application.CaptureScreenshot("ShareView.png");
        Debug.Log("start capture");

        while (!System.IO.File.Exists(Application.persistentDataPath + "/ShareView.png"))
        {

            Debug.Log(System.IO.File.Exists(Application.persistentDataPath + "/ShareView.png"));

            yield return new WaitForSeconds(0.05f); ;
        }

        if (shareToTimeLine)
        {

            IOSBridgeManager.DMShareScreenshotToWechatTimeline();
        }
        else
        {

            IOSBridgeManager.DMShareScreenshotToWechatSession();
        }
    }

    //安卓分享协程
    private IEnumerator AndroidShareScreenshot(string shareType)
    {
        Application.CaptureScreenshot("ShareView.png");

        Debug.Log("start capture");

        while (!System.IO.File.Exists(Application.persistentDataPath + "/ShareView.png"))
        {

            Debug.Log(System.IO.File.Exists(Application.persistentDataPath + "/ShareView.png"));

            yield return new WaitForSeconds(0.05f); ;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.third.wx.WechatUtil"))
        {
            jc.CallStatic(shareType);
        }

    }
}
