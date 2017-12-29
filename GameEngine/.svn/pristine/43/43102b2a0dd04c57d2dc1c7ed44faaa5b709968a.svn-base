using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : BaseView
{
    private GameObject roomTrans;
    private Text roomTitle;
    private Button battleBtn;
    private Button rankBtn;
    private Button backBtn;
    private Button certificationBtn;
    private Button daySignUpBtn;
    private Button receiveBtn;
    private Button pauseBtn;
    private Button messageBoxBtn;

    private RawImage icon;
    private Text name;
    private Text id;
    private Text gold;
    private Text score;

    private AudioClip lobbyMusic;

    public LobbyView(string assertPath = "") : base(assertPath) { }

    public override int GetModuleType()
    {
        return (int)ModuleType.LOBBY_MAIN;
    }

    public override void Init()
    {
        base.Init();
        roomTrans = GameObject.Find("Canvas/WndLobby");
        roomTitle = roomTrans.transform.Find("RoomTitle").GetComponent<Text>();
        battleBtn = roomTrans.transform.Find("BattleBtn").GetComponent<Button>();
        rankBtn = roomTrans.transform.Find("RankBtn").GetComponent<Button>();
        backBtn = roomTrans.transform.Find("BackBtn").GetComponent<Button>();
        certificationBtn = roomTrans.transform.Find("CertificationBtn").GetComponent<Button>();
        daySignUpBtn = roomTrans.transform.Find("DaySignUpBtn").GetComponent<Button>();
        receiveBtn = roomTrans.transform.Find("ReceiveBtn").GetComponent<Button>();
        pauseBtn = roomTrans.transform.Find("PauseBtn").GetComponent<Button>();
        messageBoxBtn = roomTrans.transform.Find("MessageBoxBtn").GetComponent<Button>();

        icon = roomTrans.transform.Find("PlayerInfos/Icon").GetComponent<RawImage>();
        name = roomTrans.transform.Find("PlayerInfos/Info/Name").GetComponent<Text>();
        id = roomTrans.transform.Find("PlayerInfos/Info/Id/value").GetComponent<Text>();
        gold = roomTrans.transform.Find("PlayerInfos/Info/Gold/value").GetComponent<Text>();
        score = roomTrans.transform.Find("PlayerInfos/Info/Score/value").GetComponent<Text>();

        UIEventListener.Get(battleBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(rankBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(backBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(certificationBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(daySignUpBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(receiveBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(pauseBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(messageBoxBtn.gameObject).onClick = OnClick;

        lobbyMusic = Resources.Load<AudioClip>("Audios/lobbyMusic");

        PlayerInfoInit();

        Dialog.Instance.Open();

    }

    IEnumerator ResourcesLoadFromServer()
    {
        yield return Util.StartCoroutine(AssetBundleManager.WWWDataDownCheck());
        yield return Util.StartCoroutine(AssetBundleManager.updateFromServerByChecked());
        yield return new WaitForSeconds(1);
    }

    void PlayerInfoInit()
    {
        Player player = PlayerManager.Instance.player;
        Util.StartCoroutine(Util.loadImage(icon, player.icon));
        name.text = string.Format("{0}", player.nickName);
        id.text = string.Format("{0}", player.Id);
        gold.text = string.Format("{0}", player.gold);
        score.text = string.Format("{0}", player.score);
    }

    void Doback(object[] a)
    {
        string str = "参数列表：";
        for (int i = 0; i < a.Length; i++)
        {
            str += a[i];
        }
        Debug.Log(str);
    }

    void OnClick(GameObject go)
    {
        if (go == battleBtn.gameObject)
        {
            Debug.Log("进入战斗");
            StageManager.Instance.SetNextStage(StageEnum.BATTLE);
        }
        if(go == rankBtn.gameObject)
        {
            Debug.Log("排行榜:");
            Debug.Log(RankPanel.Instance);
            RankPanel.Instance.Open();
        }
        if(go == certificationBtn.gameObject)
        {
            Debug.Log("实名认证");
            Debug.Log(CertificationPanel.Instance);
            CertificationPanel.Instance.Open();
        }
        if(go == daySignUpBtn.gameObject)
        {
            Debug.Log("签到");
            DaySignUpPanel.Instance.Open();
        }
        if(go == receiveBtn.gameObject)
        {
            Debug.Log("获取");
            Dialog.Instance.Open();
            Dialog.Instance.reviveDlg.Open();
        }
        if(go == pauseBtn.gameObject)
        {
            Debug.Log("暂停");
            Dialog.Instance.Open();
            Dialog.Instance.pauseDlg.Open();
        }
        if(go == messageBoxBtn.gameObject)
        {
            Debug.Log("消息弹窗");
            int co = UnityEngine.Random.Range(0, 7);
            switch(co)
            {
                case 0:
                    MessageBox.Open("提示", "消息提示弹窗组件",null,null,null, MessageBox.Mode.OkCancel);
                    break;
                case 1:
                    MessageBox.Open("提示", "消息提示弹窗组件",null,null,null, MessageBox.Mode.Ok, okCallback => { Debug.Log("点击"); });
                    break;
                case 2:
                    MessageBox.Open("提示", "消息提示弹窗组件", null, null, null, MessageBox.Mode.OkCancel, okCallback => { Debug.Log("确定"); }, null, cancelCallback => { Debug.Log("取消"); },null);
                    break;
                case 3:
                    MessageBox.Open("提示", "消息提示弹窗组件",null,null,null, MessageBox.Mode.OkCancelMiddle, okCallback => { Debug.Log("确定"); }, null, middleCallback => { Debug.Log("忽略"); },null,cancelCallback=> { Debug.Log("取消"); },null);
                    break;
                case 4:
                    MessageBox.Open("提示", "消息提示弹窗组件", "购买", "充值", "取消", MessageBox.Mode.OkCancelMiddle, (object[] args) => { Debug.Log(args[0]); }, new string[] { "a","bbb","cc"}, arr => { Debug.Log("充值"); }, null, arr => { Debug.Log("取消"); },null, arr => { Debug.Log("关闭"); },null);
                    break;
                case 5:
                    MessageBox.Open("提示", "消息提示弹窗组件",null,null,null,Doback, new string[] { "a","bbb","cc"}, 2f);
                    break;
                case 6:
                    MessageBox.Open("提示", "消息提示弹窗组件",null,null,null,null,null,3f,MessageBox.Mode.OkCancel);
                    break;
                default:
                    break;
            }
        }
        if(go == backBtn.gameObject)
        {
            Debug.Log("返回登录");
            StageManager.Instance.SetNextStage(StageEnum.LOGIN);
            //LoadingPanel.Instance.Open();
        }
    }

    public override void AddEvent()
    {
        base.AddEvent();
        GamePublisher.Instance.RegisterListener(EventType.lobby_music_open,this);
    }

    public override void RemoveEvent()
    {
        base.RemoveEvent();
        GamePublisher.Instance.RemoveListener(EventType.lobby_music_open, this);
    }

    public override void ProcessEvent(GameEvent evt)
    {
        base.ProcessEvent(evt);
        switch(evt.Type)
        {
            case EventType.lobby_music_open:
                {
                    Debug.Log("播放大厅场景音乐");
                    AudioSource audio = Camera.main.transform.GetComponent<AudioSource>();
                    audio.clip = lobbyMusic;
                    audio.loop = true;
                    audio.Play();
                }
                break;
            default:
                break;
        }
    }

    public override GameObject ViewObject()
    {
        return roomTrans;
    }
    public override void Show()
    {
        base.Show();
    }

    public override void Show(object pData)
    {
        base.Show(pData);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Update()
    {
        base.Update();
    }
}
