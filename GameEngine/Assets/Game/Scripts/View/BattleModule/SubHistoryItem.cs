using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubHistoryItem : MonoBehaviour {


    public Text round;
    public Text[] playerScores;//6
    public Button backplayBtn;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InfoInit(int r, object param,Game_City gameType,object outer,int self)
    {
        GameHistory.Instance.outer_info = outer;
        GameHistory.Instance.game_type = gameType;
        GameHistory.Instance.round_local = r + 1;

        round.text = string.Format("{0}", r + 1);

        //南昌麻将 红中麻将 牛牛...
        if(param.GetType().Equals(typeof(RoundRecordScoreInfo)))
        {
            RoundRecordScoreInfo item = (RoundRecordScoreInfo)param;
            int len = item.playerScoreList.Count;
            for (int i = 0; i < playerScores.Length; i++)
            {
                if (i < len)
                {
                    playerScores[i].text = string.Format("{0}", item.playerScoreList[i]);
                    if (item.playerScoreList[i] >= 0)
                    {
                        playerScores[i].color = new Color(209/255f, 89 / 255f, 0f);
                    }
                    else if (item.playerScoreList[i] < 0)
                    {
                        playerScores[i].color = Color.red;
                    }

                    if(i== self)
                    {
                        playerScores[i].color = new Color(1f, 126 / 255f, 0f); ;
                    }
                }
                else
                {
                    playerScores[i].text = "";
                }
            }

            UIEventListener.Get(backplayBtn.gameObject).onClick = obj => 
            {
                long id = item.playbackRecordID;
				Debug.Log("playbackRecordID:" + id);
                //发送协议通讯
                CMDAskHandle.Instance.GetRecordDetailAsk(id);
            };
        }
        //十三水
        if(param.GetType().Equals(typeof(RoundPlayRecordScore)))
        {
            RoundPlayRecordScore item = (RoundPlayRecordScore)param;
            int len = item.playerScores.Count;
            for (int i = 0; i < playerScores.Length; i++)
            {
                if (i < len)
                {
                    playerScores[i].text = string.Format("{0}", item.playerScores[i]);
                    if (item.playerScores[i] >= 0)
                    {
                        playerScores[i].color = new Color(209 / 255f, 89 / 255f, 0f);
                    }
                    else if (item.playerScores[i] < 0)
                    {
                        playerScores[i].color = Color.red;
                    }
                    if (i == self)
                    {
                        playerScores[i].color = new Color(1f, 126 / 255f, 0f); ;
                    }
                }
                else
                {
                    playerScores[i].text = "";
                }
            }
            UIEventListener.Get(backplayBtn.gameObject).onClick = obj => 
            {
                long id = item.playbackRecordID;
				Debug.Log("playbackRecordID:" + id);
                //发送协议通讯
                CMDAskHandle.Instance.GetRecordDetailAsk(id);
            };
        }
    }
}
