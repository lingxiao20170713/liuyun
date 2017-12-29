using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : BaseView
{
    private GameObject battleTrans;
    private Text battleName;
    private Image NPC;
    private Button reportBtn;
    private Button rulesBtn;
    private Button backBtn;

    public Actor people;
    public GameHistory gameHistoryPanel;

    private AudioClip battleMusic;

    public Transform[] gameOjbet_tran = new Transform[5];
    public GameObject ball;

    public BattleView(string assetpath = "") :base(assetpath) { }
    public override int GetModuleType()
    {
        return (int)ModuleType.BATTLE_MAIN;
    }
    public override void Init()
    {
        base.Init();

        battleTrans = GameObject.Find("Canvas/WndBattle");
        battleName = battleTrans.transform.Find("BattleName").GetComponent<Text>();
        NPC = battleTrans.transform.Find("NPC").GetComponent<Image>();
        reportBtn = battleTrans.transform.Find("ReportBtn").GetComponent<Button>();
        rulesBtn = battleTrans.transform.Find("RulesBtn").GetComponent<Button>();
        backBtn = battleTrans.transform.Find("BackBtn").GetComponent<Button>();

        people = battleTrans.transform.Find("People").GetComponent<Actor>();

        gameHistoryPanel = battleTrans.transform.Find("HistoryPanel").GetComponent<GameHistory>();

        UIEventListener.Get(reportBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(rulesBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(backBtn.gameObject).onClick = OnClick;

        battleMusic = Resources.Load<AudioClip>("Audios/battleMusic");
    }


    void OnClick(GameObject go)
    {
        if (go == reportBtn.gameObject)
        {
            Debug.Log("游戏战绩");
            CMDAskHandle.Instance.GetRecordAsk();
        }
        if (go == rulesBtn.gameObject)
        {
            Debug.Log("游戏规则");

            Debug.Log(MailBoxPanel.Instance);
            MailBoxPanel.Instance.Open();
        }
        if (go == backBtn.gameObject)
        {
            Debug.Log("返回");
            StageManager.Instance.SetNextStage(StageEnum.LOBBY);
        }
    }

    public void NPC_Ctrl(int dir)
    {
        if (dir == 0) NPC.gameObject.transform.Translate(Vector3.up);
        if (dir == 1) NPC.gameObject.transform.Translate(-Vector3.up);
        if (dir == 2) NPC.gameObject.transform.Translate(Vector3.left);
        if (dir == 3) NPC.gameObject.transform.Translate(-Vector3.left);
    }

    public override void Update()
    {
        base.Update();

    }
    public override void AddEvent()
    {
        base.AddEvent();
        GamePublisher.Instance.RegisterListener(EventType.battle_music_open, this);
    }

    public override void RemoveEvent()
    {
        base.RemoveEvent();
        GamePublisher.Instance.RemoveListener(EventType.battle_music_open, this);
    }

    public override void ProcessEvent(GameEvent evt)
    {
        base.ProcessEvent(evt);
        switch(evt.Type)
        {
            case EventType.battle_music_open:
                {
                    Debug.Log("播放战斗场景音乐");
                    AudioSource audio = Camera.main.transform.GetComponent<AudioSource>();
                    audio.clip = battleMusic;
                    audio.loop = true;
                    audio.Play();
                }
                break;
        }
    }

    public override GameObject ViewObject()
    {
        return battleTrans;
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
}
