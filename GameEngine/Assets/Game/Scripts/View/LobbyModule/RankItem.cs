using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour {

    private Text rank;
    private Text nickName;
    private Text power;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InfoInit(RankData data)
    {
        rank = this.transform.Find("Rank").GetComponent<Text>();
        nickName = this.transform.Find("Name").GetComponent<Text>();
        power = this.transform.Find("Power").GetComponent<Text>();

        rank.text = string.Format("{0}", data.rank);
        nickName.text = string.Format("{0}", data.name);
        power.text = string.Format("{0}", data.power);
    }
}
