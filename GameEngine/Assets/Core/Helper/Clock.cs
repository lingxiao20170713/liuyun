using System;
using System.Collections;
using UnityEngine;

public class Clock : Singleton<Clock>
{
    //private pbDateInfo m_serverDate = new pbDateInfo();
    //private long m_diffTime = 0; // 客户端与服务器时间的差值（毫秒）
    //private long m_serverTime = 0; // 服务器时间（毫秒）
    //private long m_deltaTime = 0; // 帧间隔时间（毫秒）
    //private long m_lastTime = 0; // 最后的客户端时间（毫秒）

    private long m_serverTime = 0; // 当前服务器时间
    private long m_lastTime = 0; // 上一帧服务器时间

    //public int gameStartInSecond;
    //public float timeSinceGameStart = 0f;

    public Clock()
    {
        //m_diffTime = 0;
        //m_serverTime = Utils.UnixNowTicks();
        //m_deltaTime = 0;
        //m_lastTime = m_serverTime;

        m_serverTime = 0;
        m_lastTime = 0;

        SyncServerTime();

        //gameStartInSecond = Utils.UnixNowTicksInSecond();
    }

    /// <summary>
    /// 获取服务器同步时间（秒）                                                                                                                                                                                        
    /// </summary>
    public long seconds
    {
        get { return m_serverTime / 1000; }
    }

    /// <summary>
    /// 获取服务器同步时间（毫秒）
    /// </summary>
    public long milliseconds
    {
        get { return m_serverTime; }
    }

    /// <summary>
    /// 获取服务器同步的帧间隔（秒）
    /// </summary>
    //public float deltaTime
    //{
    //    get { return (float)m_deltaTime / 1000.0f; }
    //}

    /// <summary>
    /// 获取服务器同步的帧间隔（毫秒）
    /// </summary>
    //public long deltaTimeMilliseconds
    //{
    //    get { return m_deltaTime; } 
    //}

    /// <summary>
    /// 获取服务器同步日期
    /// </summary>
    public DateTime dateTime
    {
        get { return (Util.UnixEpoch + TimeSpan.FromMilliseconds(milliseconds)).ToLocalTime(); }
    }

    public DateTime GetTime(int timestamp)
    {
        return (Util.UnixEpoch + TimeSpan.FromSeconds(timestamp)).ToLocalTime();
    }

    public void Update()
    {
        if (!IsTimeSynchronized())
        {
            //if (Time.frameCount % 0xff == 0)
            //    SyncServerTime();
            return;
        }
        else
        {

            m_lastTime = m_serverTime;
            m_serverTime += (long)(Time.deltaTime * 1000f);

            //long time = Utils.UnixNowTicks();

            //m_serverTime = time - m_diffTime;
            //m_deltaTime = time - m_lastTime;
            //m_lastTime = time;
        }
        //timeSinceGameStart += TimeMgr.deltaTime;
    }

    //public void SetGameTime(pbDateInfo di)
    //{
    //    long serverTime = (long)di.timestamp * 1000;

    //    m_diffTime = Utils.UnixNowTicks() - serverTime;
    //    m_serverTime = serverTime;
    //    m_serverDate = di;
    //}

    public void SetGameTime(int timestamp)
    {
        m_serverTime = (long)timestamp * 1000;
        Debug.Log(m_serverTime);
    }

    public bool IsTimeSynchronized()
    {
        return m_serverTime != 0;
    }

    public void SyncServerTime()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Util.StartCoroutine(StartSyncServerTime());
        }
    }

    IEnumerator StartSyncServerTime()
    {
        WWW www = new WWW(AssetBundleManager.server_url + "currentTime.txt");//new WWW("http://203.195.200.120/hulu/currentTime");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            string timestr = www.text;
            CodeTitans.JSon.IJSonObject timeData = new CodeTitans.JSon.JSonReader().ReadAsJSonObject(timestr);
            int timestamp = timeData["time"].Int32Value;
            SetGameTime(timestamp);
        }

        www.Dispose();
        www = null;
    }
}