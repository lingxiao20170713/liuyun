using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardManager : Singleton<ClipBoardManager>
{
    public void AndroidJumpToWeChat()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.third.wx.WechatUtil"))
        {
            jc.CallStatic("OpenWechatApp");
        }

    }
    public void AndroidCopyToClipboard(string str)
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.clipboard.ClipboardUtil"))
        {
            object[] message = new object[1];
            message[0] = str; // txt
            jc.CallStatic("copyTextToClipboard", message);
        }
    }

    public void AndroidGetFromClipboard()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.damaiapp.ncmj.clipboard.ClipboardUtil"))
        {
            jc.CallStatic("getTextFromClipboard");
        }
    }
}
