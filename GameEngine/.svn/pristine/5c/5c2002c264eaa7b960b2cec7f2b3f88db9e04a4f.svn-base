using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaySignUpPanel : SingletonWindow<DaySignUpPanel>
{
    GameObject parent;
    GameObject date;

    Text theYearMonth;
    Text signValue;
    Button signBtn;
    Button closeBtn;

    List<Text> date_value_list = new List<Text>();
    public DaySignUpPanel() : base(LayerType.Layer_2) { }

    public override string Bundle
    {
        get
        {
            return "DaySignUpPanel";
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        parent = Find("SignTable/MonthlyCalendar").gameObject;
        date = Find("SignTable/Date").gameObject;

        theYearMonth = Find("SignTable/YearMonth").GetComponent<Text>();
        signValue = Find("SignValue/Value").GetComponent<Text>();
        signBtn = Find("SignBtn").GetComponent<Button>();
        closeBtn = Find("CloseBtn").GetComponent<Button>();

        this.Register(signBtn.gameObject).onClick = OnClick;
        this.Register(closeBtn.gameObject).onClick = OnClick;

        InfomationInit();
    }

    void OnClick(GameObject go)
    {
        if(go == signBtn.gameObject)
        {
            Debug.Log("签到");
        }
        if(go == closeBtn.gameObject)
        {
            Close();
        }
    }

    void InfomationInit()
    {
        if (date_value_list.Count != 0) return;

        DateTime day = DateTime.UtcNow;
        theYearMonth.text = string.Format("{0}年{1}月", day.Year, day.Month);
        DateTime cur_month = new DateTime(day.Year, day.Month, 1);
        int week1 = (int)cur_month.DayOfWeek;//获取这个月的第一天是星期几      
          
        int days = DateTime.DaysInMonth(day.Year, day.Month);
        int start = 0;
        for (int i=0;i<35;i++)
        {
            Text dat = GameObject.Instantiate(date, parent.transform).GetComponent<Text>();
            dat.gameObject.SetActive(true);
            dat.transform.localScale = Vector3.one;
            dat.text = i < week1 || start > days ? "" : string.Format("{0}", ++start);
            dat.transform.Find("Sign").gameObject.SetActive(start == day.Day);
            date_value_list.Add(dat);
        }

        signValue.text = string.Format("{0}",UnityEngine.Random.Range(0,days));
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
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
