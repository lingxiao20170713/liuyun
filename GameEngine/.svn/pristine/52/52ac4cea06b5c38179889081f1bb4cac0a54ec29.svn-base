using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : SingletonWindow<LoadingPanel>
{
    Image loadingImage;
    Text progress;

    private int progress_value = -1;
    public LoadingPanel() : base(LayerType.OverLayer) { }

    public override string Bundle
    {
        get
        {
            return "LoadingPanel";
        }
    }
    public int _progressValue
    {
        get { return progress_value; }
        set { progress_value = value; }
    }
    protected override void OnStart()
    {
        base.OnStart();

        loadingImage = Find("Image").GetComponent<Image>();
        progress = Find("Progress").GetComponent<Text>();
        progress.gameObject.SetActive(progress_value >= 0);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (loadingImage == null) return;
        loadingImage.transform.Rotate(Vector3.forward*Time.deltaTime*360);
        progress.text = _progressValue.ToString();
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
