using System.Collections.Generic;
using System.Runtime.InteropServices;
public class IOSBridgeManager
{
    /// <summary>
    /// APP启动的时候，更新一些权限信息
    /// </summary>
    [DllImport("__Internal")]
    public static extern void UpdatePermission();

    /// <summary>
    /// 跳转到微信
    /// </summary>
    /// <param name="scheme">Scheme.</param>
    [DllImport("__Internal")]
    public static extern void JumpToWechat(string scheme);

    /// <summary>
    /// 内购
    /// </summary>
    /// <param name="key"> 项目的key.</param>
    [DllImport("__Internal")]
    public static extern void DMIAPRequest(string key);

    /// <summary>
    /// 微信登录
    /// </summary>
    [DllImport("__Internal")]
    public static extern void DMWechatLogin();

    #region 声明iOS原生实现的方法
    /// <summary>
    /// 分享
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="desc">Desc.</param>
    /// <param name="urlStr">URL string.</param>
    [DllImport("__Internal")]
    public static extern void DMShareWechatSession(string title, string desc, string urlStr);

    [DllImport("__Internal")]
    public static extern void DMShareWechatTimeline(string title, string desc, string urlStr);

    [DllImport("__Internal")]
    public static extern void DMShareScreenshotToWechatSession();

    [DllImport("__Internal")]
    public static extern void DMShareScreenshotToWechatTimeline();

    /// <summary>
    /// iOS跳转APP Store做版本更新
    /// </summary>
    /// <param name="scheme">App页的网站.</param>
    [DllImport("__Internal")]
    public static extern void JumpToAPPStoreUpdate(string APPUrl);

    /// iOS原生弹窗，语音授权
    [DllImport("__Internal")]
    public static extern void iOSAudioPermissionAlertView();

    /// <summary>
    /// 苹果的ipv6适配
    /// </summary>
    /// <returns>The I pv6.</returns>
    /// <param name="host">Host.</param>
    /// <param name="port">Port.</param>
    [DllImport("__Internal")]
    public static extern string GetIPv6(string host, string port);

    /// <summary>
    /// 开始定位
    /// </summary>
    [DllImport("__Internal")]
    public static extern void StartLocation();

    /// <summary>
    /// 关闭定位Z
    /// </summary>
    [DllImport("__Internal")]
    public static extern void StopLocation();
    #endregion


    //以下为IOS粘贴板功能
    [DllImport("__Internal")]
    public static extern bool CopyTextToClipboard_Unity(string text);

    [DllImport("__Internal")]
    public static extern string GetTextFromClipboard_Unity();
}
