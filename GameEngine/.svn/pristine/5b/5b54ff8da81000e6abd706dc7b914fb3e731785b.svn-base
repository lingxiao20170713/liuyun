using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour {

    public Text gameName;
    //public Image imgfg;
    public Text roomId;
    public Text[] playerinfos;//6
    public Text timeTxt;

    public object item_his = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InfoInit(Record data)
    {
        for (int i=0;i<playerinfos.Length;i++)
        {
            playerinfos[i].gameObject.SetActive(false);
        }
        item_his = null;

        switch ((Game_City)data.gameEnum)
        {
            case Game_City.NanChang:
                {
                    gameName.text = "麻将--南昌麻将";
                    OuterItem HisList = fastJSON.JSON.ToObject<OuterItem>(data.record);
                    item_his = HisList;
                    roomId.text = HisList.room_info.room_id.ToString();
                    DateTime dt = Util.StampToDateTime(HisList.room_info.game_end_time);
                    timeTxt.text = string.Format("{0:yyyy-MM-dd}", dt) + "\n" + string.Format("{0:HH:mm:ss}", dt);

                    for(int i=0;i<HisList.player_info.Count;i++)
                    {
                        playerinfos[i].gameObject.SetActive(true);
                        playerinfos[i].text = string.Format("{0}:{1}", HisList.player_info[i].name, HisList.player_info[i].score);
                        if (HisList.player_info[i].player_id == Util.getSelfID())
                        {
                            playerinfos[i].color = new Color(1f, 126 / 255f, 0f);
                        }
                    }
                }
                break;
            case Game_City.CowCow:
                {
                    gameName.text = "扑克--牛牛";
                    NiuNiuOuterItem HisList = fastJSON.JSON.ToObject<NiuNiuOuterItem>(data.record);
                    item_his = HisList;
                    roomId.text = HisList.deskInfo.deskId.ToString();
                    DateTime dt = Util.StampToDateTime(HisList.deskInfo.gameEndTime);
                    timeTxt.text = string.Format("{0:yyyy-MM-dd}", dt) + "\n" + string.Format("{0:HH:mm:ss}", dt);

                    for (int i = 0; i < HisList.playerInfoList.Count; i++)
                    {
                        playerinfos[i].gameObject.SetActive(true);
                        playerinfos[i].text = string.Format("{0}:{1}", HisList.playerInfoList[i].name, HisList.playerInfoList[i].score);
                        if (HisList.playerInfoList[i].playerId == Util.getSelfID())
                        {
                            playerinfos[i].color = new Color(1f, 126 / 255f, 0f);
                        }
                    }
                }
                break;
            case Game_City.FuJian:
                {
                    gameName.text = "扑克--十三水";
                    WaterOuterItem HisList = fastJSON.JSON.ToObject<WaterOuterItem>(data.record);
                    item_his = HisList;
                    roomId.text = HisList.deskInfo.deskId.ToString();
                    DateTime dt = Util.StampToDateTime(HisList.deskInfo.gameEndTime);
                    timeTxt.text = string.Format("{0:yyyy-MM-dd}", dt) + "\n" + string.Format("{0:HH:mm:ss}", dt);

                    for (int i = 0; i < HisList.playerInfoList.Count; i++)
                    {
                        playerinfos[i].gameObject.SetActive(true);
                        playerinfos[i].text = string.Format("{0}:{1}", HisList.playerInfoList[i].name, HisList.playerInfoList[i].score);
                        if (HisList.playerInfoList[i].playerId == Util.getSelfID())
                        {
                            playerinfos[i].color = new Color(1f, 126 / 255f, 0f);
                        }
                    }
                }
                break;
            case Game_City.Hhchz:
                {
                    gameName.text = "跑胡子--扯胡子";
                    PhzOuterItem HisList = fastJSON.JSON.ToObject<PhzOuterItem>(data.record);
                    item_his = HisList;
                    roomId.text = HisList.room_info.room_id.ToString();
                    DateTime dt = Util.StampToDateTime(HisList.room_info.game_end_time);
                    timeTxt.text = string.Format("{0:yyyy-MM-dd}", dt) + "\n" + string.Format("{0:HH:mm:ss}", dt);

                    for (int i = 0; i < HisList.player_info.Count; i++)
                    {
                        playerinfos[i].gameObject.SetActive(true);
                        playerinfos[i].text = string.Format("{0}:{1}", HisList.player_info[i].name, HisList.player_info[i].score);
                        if (HisList.player_info[i].player_id == Util.getSelfID())
                        {
                            playerinfos[i].color = new Color(1f, 126 / 255f, 0f);
                        }
                    }
                }
                break;
            case Game_City.Hhlhq:
                {
                    gameName.text = "跑胡子--六胡抢";
                }
                break;
            case Game_City.RedCenter:
                {
                    gameName.text = "麻将--红中麻将";
                    HZMJOuterItem HisList = fastJSON.JSON.ToObject<HZMJOuterItem>(data.record);
                    item_his = HisList;
                    roomId.text = HisList.room_info.room_id.ToString();
                    DateTime dt = Util.StampToDateTime(HisList.room_info.game_end_time);
                    timeTxt.text = string.Format("{0:yyyy-MM-dd}", dt) + "\n" + string.Format("{0:HH:mm:ss}", dt);

                    for (int i = 0; i < HisList.player_info.Count; i++)
                    {
                        playerinfos[i].gameObject.SetActive(true);
                        playerinfos[i].text = string.Format("{0}:{1}", HisList.player_info[i].name, HisList.player_info[i].score);
                        if (HisList.player_info[i].player_id == Util.getSelfID())
                        {
                            playerinfos[i].color = new Color(1f, 126 / 255f, 0f);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        GameHistory.Instance.InitSubList(item_his);
    }
}
