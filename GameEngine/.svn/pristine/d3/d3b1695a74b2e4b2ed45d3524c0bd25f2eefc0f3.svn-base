using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHistory : MonoBehaviour {

    public GameObject panel1;
    public GameObject panel2;

    public GameObject parent1;
    public HistoryItem item1;

    public GameObject parent2;
    public SubHistoryItem item2;

    public Text[] panel2playerNames;//6

    public Text nohistoryTip;

    public Button closeBtn;
    public Button backBtn;

    List<Record> recordList = new List<Record>();//保存信息


    public static GameHistory Instance = null;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        UIEventListener.Get(closeBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(backBtn.gameObject).onClick = OnClick;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick(GameObject go)
    {
        if (go == closeBtn.gameObject)
        {
            this.transform.gameObject.SetActive(false);
        }
        if (go == backBtn.gameObject)
        {
            panel1.gameObject.SetActive(true);
            panel2.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(true);
            backBtn.gameObject.SetActive(false);
        }
    }

    List<HistoryItem> itemObj = new List<HistoryItem>();
    public void InitList(List<Record> hisList = null)
    {
        for (int i = 0; i < itemObj.Count; i++)
        {
            Destroy(itemObj[i].gameObject);
        }
        itemObj.Clear();

        if (hisList.Count == 0)
        {
            panel1.gameObject.SetActive(false);
            panel2.gameObject.SetActive(false);
            backBtn.gameObject.SetActive(false);
            nohistoryTip.text = string.Format("暂时没有战绩记录");
            return;
        }
        recordList = hisList;

        panel1.gameObject.SetActive(true);
        panel2.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        nohistoryTip.text = "";

        for (int i = 0; i < hisList.Count; i++)
        {
            HistoryItem item = Instantiate(item1, parent1.transform).GetComponent<HistoryItem>();
            item.gameObject.transform.localPosition = Vector3.zero;
            item.gameObject.transform.localRotation = Quaternion.identity;
            item.gameObject.transform.localScale = Vector3.one;
            item.InfoInit(hisList[i]);
            itemObj.Add(item);
        }
    }

    List<SubHistoryItem> SubItemObj = new List<SubHistoryItem>();
    public void InitSubList(object param)
    {
        if (param == null)
        {
            Debug.Log("战绩数据不可用");
            return;
        }
        for (int i = 0; i < SubItemObj.Count; i++)
        {
            Destroy(SubItemObj[i].gameObject);
        }
        SubItemObj.Clear();

        for (int i = 0; i < panel2playerNames.Length; i++)
        {
            panel2playerNames[i].color = new Color(150/255f, 77 / 255f, 9 / 255f);
        }

        panel1.gameObject.SetActive(false);
        panel2.gameObject.SetActive(true);
        closeBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);

        if(param.GetType().Equals(typeof(OuterItem)))
        {
            Debug.Log("南昌");

            int selfline = 0;
            OuterItem his = (OuterItem)param;
            int len = his.player_info.Count;
            for (int i = 0; i < panel2playerNames.Length; i++)
            {
                if (i < len)
                {
                    panel2playerNames[i].text = string.Format("{0}", his.player_info[i].name);
                    if (his.player_info[i].player_id == Util.getSelfID())
                    {
                        selfline = i;
                        panel2playerNames[i].color = new Color(1f, 157 / 255f, 16 / 255f);
                    }
                }
                else
                {
                    panel2playerNames[i].text = "";
                }
            }

            for (int i = 0; i < his.round_play_record_scores.Count; i++)
            {
                RoundPlayRecordScore fo = his.round_play_record_scores[i];
                SubHistoryItem item = Instantiate(item2, parent2.transform).GetComponent<SubHistoryItem>();
                item.gameObject.transform.localPosition = Vector3.zero;
                item.gameObject.transform.localRotation = Quaternion.identity;
                item.gameObject.transform.localScale = Vector3.one;
                item.InfoInit(i, fo, Game_City.RedCenter, his, selfline);
                SubItemObj.Add(item);
            }
        }
        if(param.GetType().Equals(typeof(WaterOuterItem)))
        {
            Debug.Log("十三水");
            int selfline = 0;
            WaterOuterItem his = (WaterOuterItem)param;
            int len = his.playerInfoList.Count;
            for (int i = 0; i < panel2playerNames.Length; i++)
            {
                if (i < len)
                {
                    panel2playerNames[i].text = string.Format("{0}", his.playerInfoList[i].name);
                    if (his.playerInfoList[i].playerId == Util.getSelfID())
                    {
                        selfline = i;
                        panel2playerNames[i].color = new Color(1f, 157 / 255f, 16 / 255f);
                    }
                }
                else
                {
                    panel2playerNames[i].text = "";
                }
            }

            for (int i = 0; i < his.roundRecordScoreList.Count; i++)
            {
                RoundRecordScoreInfo fo = his.roundRecordScoreList[i];
                SubHistoryItem item = Instantiate(item2, parent2.transform).GetComponent<SubHistoryItem>();
                item.gameObject.transform.localPosition = Vector3.zero;
                item.gameObject.transform.localRotation = Quaternion.identity;
                item.gameObject.transform.localScale = Vector3.one;
                item.InfoInit(i, fo,Game_City.FuJian,his,selfline);
                SubItemObj.Add(item);
            }
        }
        if (param.GetType().Equals(typeof(HZMJOuterItem)))
        {
            Debug.Log("红中");

            int selfline = 0;
            HZMJOuterItem his = (HZMJOuterItem)param;
            int len = his.player_info.Count;
            for (int i = 0; i < panel2playerNames.Length; i++)
            {
                if (i < len)
                {
                    panel2playerNames[i].text = string.Format("{0}", his.player_info[i].name);
                    if (his.player_info[i].player_id == Util.getSelfID())
                    {
                        selfline = i;
                        panel2playerNames[i].color = new Color(1f, 157 / 255f, 16 / 255f); 
                    }
                }
                else
                {
                    panel2playerNames[i].text = "";
                }
            }

            for (int i = 0; i < his.round_play_record_scores.Count; i++)
            {
                RoundPlayRecordScore fo = his.round_play_record_scores[i];
                SubHistoryItem item = Instantiate(item2, parent2.transform).GetComponent<SubHistoryItem>();
                item.gameObject.transform.localPosition = Vector3.zero;
                item.gameObject.transform.localRotation = Quaternion.identity;
                item.gameObject.transform.localScale = Vector3.one;
                item.InfoInit(i, fo,Game_City.RedCenter,his,selfline);
                SubItemObj.Add(item);
            }
        }
        if (param.GetType().Equals(typeof(NiuNiuOuterItem)))
        {
            Debug.Log("牛牛");

            int selfline = 0;
            NiuNiuOuterItem his = (NiuNiuOuterItem)param;
            int len = his.playerInfoList.Count;
            for (int i = 0; i < panel2playerNames.Length; i++)
            {
                if (i < len)
                {
                    panel2playerNames[i].text = string.Format("{0}", his.playerInfoList[i].name);
                    if (his.playerInfoList[i].playerId == Util.getSelfID())
                    {
                        selfline = i;
                        panel2playerNames[i].color = new Color(1f, 157 / 255f, 16 / 255f);
                    }
                }
                else
                {
                    panel2playerNames[i].text = "";
                }
            }

            for (int i = 0; i < his.roundRecordScoreList.Count; i++)
            {
                RoundRecordScoreInfo fo = his.roundRecordScoreList[i];
                SubHistoryItem item = Instantiate(item2, parent2.transform).GetComponent<SubHistoryItem>();
                item.gameObject.transform.localPosition = Vector3.zero;
                item.gameObject.transform.localRotation = Quaternion.identity;
                item.gameObject.transform.localScale = Vector3.one;
                item.InfoInit(i, fo,Game_City.CowCow,his,selfline);
                SubItemObj.Add(item);
            }
        }
        if (param.GetType().Equals(typeof(PhzOuterItem)))
        {
            Debug.Log("跑胡子");

            int selfline = 0;
            PhzOuterItem his = (PhzOuterItem)param;
            int len = his.player_info.Count;
            for (int i = 0; i < panel2playerNames.Length; i++)
            {
                if (i < len)
                {
                    panel2playerNames[i].text = string.Format("{0}", his.player_info[i].name);
                    if (his.player_info[i].player_id == Util.getSelfID())
                    {
                        selfline = i;
                        panel2playerNames[i].color = new Color(1f, 157 / 255f, 16 / 255f);
                    }
                }
                else
                {
                    panel2playerNames[i].text = "";
                }
            }

            for (int i = 0; i < his.round_play_record_scores.Count; i++)
            {
                RoundPlayRecordScore fo = his.round_play_record_scores[i];
                SubHistoryItem item = Instantiate(item2, parent2.transform).GetComponent<SubHistoryItem>();
                item.gameObject.transform.localPosition = Vector3.zero;
                item.gameObject.transform.localRotation = Quaternion.identity;
                item.gameObject.transform.localScale = Vector3.one;
                item.InfoInit(i, fo, Game_City.RedCenter, his, selfline);
                SubItemObj.Add(item);
            }
        }
    }

    public object outer_info = null;
    public Game_City game_type = Game_City.None;
    public int round_local = 0;

    public void GetRecordDetailAck(MsgRecordPlay11111 msg)
    {
        //Debug.Log(msg.RecordDetail);
        //      DontDesScript.Instance.DisableClick(false);

        //      //消息返回执行
        //      if (game_type == Game_City.NanChang)
        //      {
        //          RecordPlay.isPlayVideo = true;
        //          DontDesScript.Instance.OnShow();

        //          if (DontDesScript.Instance.ROOT != null)
        //          {
        //              RecordPlay rd = DontDesScript.Instance.ROOT.AddMissingComponent<RecordPlay>();
        //              rd.baseInfo = (OuterItem)outer_info;//WndHistory.Instance.outInfos;
        //		rd.recordList = fastJSON.JSON.ToObject<RoundPlayRecordsBean>(msg.RecordDetail);
        //              rd.Index = round_local;
        //              Debug.Log(rd.Index);
        //          }
        //      }
        //      if (game_type == Game_City.RedCenter)
        //      {
        //          RecordPlay.isPlayVideo = true;
        //          DontDesScript.Instance.OnShow(2);
        //          if (DontDesScript.Instance.ROOT != null)
        //          {
        //              RecordPlay rd = DontDesScript.Instance.ROOT.AddMissingComponent<RecordPlay>();
        //              rd.HZbaseInfo = (HZMJOuterItem)outer_info;
        //		rd.recordList = fastJSON.JSON.ToObject<RoundPlayRecordsBean>(msg.RecordDetail);
        //              rd.Index = round_local;
        //              Debug.Log(rd.Index);
        //          }
        //      }
        //if (game_type == Game_City.CowCow) {
        //	RecordPlay.isPlayVideo = true;
        //	DontDesScript.Instance.OnShow (3);
        //	if (DontDesScript.Instance.ROOT != null) {
        //		StartCoroutine (DelayInitNiuNiuRecordPlayBackInfo (msg));
        //	}
        //}

        //if (game_type == Game_City.Hhchz)
        //{
        //    PaohuziRecordPlay.isPlayVideo = true;
        //    HhRoomLobbyView.Instance.Show();
        //    if (DontDesScript.Instance.ROOT != null)
        //    {
        //        PaohuziRecordPlay rd = DontDesScript.Instance.ROOT.AddMissingComponent<PaohuziRecordPlay>();
        //        rd.baseInfo = (PhzOuterItem)outer_info;
        //        rd.recordList = fastJSON.JSON.ToObject<HhRoundPlayRecordsBean>(msg.RecordDetail);
        //        rd.Index = round_local;
        //        Debug.Log(rd.Index);
        //    }
        //}

        //      outer_info = null;
        //      round_local = 0;
        //if (game_type != Game_City.CowCow) {
        //	game_type = Game_City.None;
        //	this.gameObject.SetActive (false);
        //}
    }

	//IEnumerator DelayInitNiuNiuRecordPlayBackInfo(MsgRecordPlay11111 msg) {
 //       while (CCowUIManager.Instance.haveInit == false)
 //       {
 //           yield return null;
 //       }
 //       CCowRecordView ccowRecordView = DontDesScript.Instance.ROOT.AddComponent<CCowRecordView>();
 //       if (msg.RecordDetail != null)
 //       {
 //           ccowRecordView.Init(fastJSON.JSON.ToObject<NiuNiuRecordPlayBackInfo>(msg.RecordDetail));
 //       }
 //       yield return new WaitForSeconds(1f);
 //       this.gameObject.SetActive(false);
 //   }
}
