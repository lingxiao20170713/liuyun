using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public string serverIp = "";
    public int serverPort = 0;

    protected override void Awake()
    {
        base.Awake();
        Util.Mono = this;//方便协程等需要MonoBehaviour的使用
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    IEnumerator Start()
    {
        ControlerManager.Instance.Start();
        StageManager.Instance.Start();

        yield return Util.StartCoroutine(AssetBundleManager.WWWDataDownCheck());
        yield return Util.StartCoroutine(AssetBundleManager.updateFromServerByChecked());

        InitUnity();
        InitGame();
        InitOther();

        Client client = new Client();
        client.connectedCall = ConnectedCallBack;
        client.BeginConnect(serverIp, serverPort);


    }

    static void ConnectedCallBack(bool suc)
    {
        if (suc)
        {
            Debug.Log("连接服务器成功");
            StageManager.Instance.SetNextStage(StageEnum.LOGIN);
            //SceneMgr.Instance.LoadScene("Login", (int a) => { Debug.Log(a); });//加载场景控制类（备用可选）
        }
        else
        {
            Debug.LogError(string.Format("Socket无法成功连接！请确认IP和端口是否正确 StackTrace = {0}", StackTraceUtility.ExtractStackTrace()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(GameData.world.compress_network_data);
            Debug.Log(GameData.world.client_sys_title);
        }
        if (Client.Instance != null)
        {
            Client.Instance.Update();//消息
        }

        StageManager.Instance.Update();//加载场景
        ModalManager.Instance.Update();//模块中的update控制更新

        // 时间更新要放在最前面
        //Clock.Instance.Update();
        //Scheduler.ExecuteSchedule();

        //HttpSession.Instance.Update();

        //SceneMgr.Instance.Update();
        //InputMgr.Instance.Update();
        //GameStateMgr.Instance.Update();

        WindowManager.Instance.Update();
        CacheManager.CheckSave();
    }

    void OnApplicationPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    void OnApplicationQuit()
    {
        //Debug.Log("OnApplicationQuit");
        CacheManager.Save();
        WindowManager.Instance.OnExit();
    }

    void OnApplicationFocus(bool hasFocus)
    {

    }

    private void InitUnity()
    {
        Application.targetFrameRate = 30; // 帧率
        //Screen.orientation = ScreenOrientation.Portrait;
        //Screen.autorotateToLandscapeLeft = false;
        //Screen.autorotateToLandscapeRight = false;
        //Screen.autorotateToPortrait = false;
        //Screen.autorotateToPortraitUpsideDown = false;
        UnityEngine.Random.InitState(Environment.TickCount);
    }

    private void InitGame()
    {
        CacheManager.Load();//本地保存数据加载

        GameData.Init();//游戏数据加载

        AssetManager.Instance.InitResource(() =>
        {
            Debug.Log("资源加载完成");
            WindowManager.Instance.Register("MailBoxPanel");
            WindowManager.Instance.Register("RankPanel");
            WindowManager.Instance.Register("CertificationPanel");
            WindowManager.Instance.Register("DaySignUpPanel");
            WindowManager.Instance.Register("LoadingPanel");
            WindowManager.Instance.Register("Dialog");
            WindowManager.Instance.Register("MessageBox");
        });
    }

    private void InitOther()
    {
        Person per = new Person();
        per.age = 1;
        per.name = "xiangmu";

        using (Stream s = File.OpenWrite("test.dat"))
        {

            //序列化对象到文件
            ProtoBuf.Serializer.Serialize<Person>(s, per);
        }

        Person per2 = null;
        using (Stream s = File.OpenRead("test.dat"))
        {
            //从文件中读取并反序列化到对象
            per2 = ProtoBuf.Serializer.Deserialize<Person>(s);

            //打印
            print("name>" + per2.name + " age>" + per2.age);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}

//定义一个序列化与反序列化对象
[ProtoBuf.ProtoContract]
class Person
{
    [ProtoBuf.ProtoMember(1)]
    public string name;
    [ProtoBuf.ProtoMember(2)]
    public int age;
}