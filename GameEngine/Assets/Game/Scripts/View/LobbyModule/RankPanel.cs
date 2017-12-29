using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : SingletonWindow<RankPanel>
{
    private GameObject parent;
    private RankItem rankItem;
    private Button closeBtn;

    public RankPanel() : base(LayerType.Layer_0) { }

    public override string Bundle
    {
        get { return "RankPanel"; }
    }

    protected override void OnStart()
    {
        base.OnStart();

        parent = Find("Scroll View/Viewport/Content").gameObject;
        rankItem = Find("RankItem").GetComponent<RankItem>();
        closeBtn = Find("CloseBtn").GetComponent<Button>();

        UIEventListener.Get(closeBtn.gameObject).onClick = OnClick;

        InfomationInit(20);
    }
    void OnClick(GameObject go)
    {
        if (go == closeBtn.gameObject)
        {
            Close();
        }
    }

    List<RankItem> rankItemList = new List<RankItem>();

    public void InfomationInit(int count)
    {
        ItemObjectReset();

        for (int i = 0; i < count; i++)
        {
            RankData dt = new RankData();
            dt.rank = i + 1;
            dt.name = Random.Range(100, 1000).ToString() + "Nick" + Random.Range(1, 100);
            dt.power = Random.Range(10000, 1000000);
            RankItem ri = Object.Instantiate(rankItem, parent.transform);
            ri.gameObject.SetActive(true);
            ri.transform.localScale = Vector3.one;
            ri.transform.localRotation = Quaternion.identity;
            ri.InfoInit(dt);
            rankItemList.Add(ri);
        }
        Debug.Log(rankItemList.Count);
    }

    void ItemObjectReset()
    {
        Debug.Log("生成对象数目：" + rankItemList.Count);
        for (int i = 0; i < rankItemList.Count; i++)
        {
            RankItem temp = rankItemList[i];
            Object.Destroy(temp.gameObject);
        }
        rankItemList.Clear();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnActive(bool active)
    {
        base.OnActive(active);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}

public class RankData
{
    public int rank;
    public string name;
    public int power;
}