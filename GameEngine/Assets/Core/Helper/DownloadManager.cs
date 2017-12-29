using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadManager : MonoBehaviour
{
    private static DownloadManager _instance;
    public static DownloadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Download Manager").AddComponent<DownloadManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    Dictionary<string, WeakReference> _cache = new Dictionary<string, WeakReference>();
    List<DownloadItem> _loading = new List<DownloadItem>();
    const int MaxLoading = 5;
    List<DownloadItem> _tmpList = new List<DownloadItem>();
    int _totalLoaded = 0;
    List<DownloadItem> _waiting = new List<DownloadItem>();

    public DownloadItem GetDownload(string assetName, string assetType)
    {
        DownloadItem target = null;
        if (_cache.ContainsKey(assetName))
        {
            WeakReference reference = _cache[assetName];
            target = reference.Target as DownloadItem;
            if (target != null && !target.isDone && !_loading.Contains(target) && !_waiting.Contains(target))
                _waiting.Add(target);
            return target;
        }
        target = new DownloadItem(assetName, assetType);
        _waiting.Add(target);
        _cache[assetName] = new WeakReference(target);
        return target;
    }

    void Update()
    {
        DestroyBundles();
        _tmpList.Clear();
        for (int i = 0; i < _loading.Count; ++i)
        {
            if (_loading[i].IsLoadCompleted())
                _tmpList.Add(_loading[i]);
        }
        if (_tmpList.Count > 0)
        {
            for (int i = 0; i < _tmpList.Count; ++i)
            {
                DownloadItem item = _tmpList[i];
                _loading.Remove(item);
                item.isDone = true;
                ++_totalLoaded;
                Debug.Log(item);
                Debug.Log(item.www);
                Debug.Log(item.www.url);
                Debug.Log(item.www.error);
                item.ab = item.www.assetBundle;
                item.FireLoadCompleted();
            }
            _tmpList.Clear();
        }
        int count = MaxLoading - _loading.Count;
        if (count > 0 && _waiting.Count > 0)
        {
            if (count > _waiting.Count)
            {
                count = _waiting.Count;
            }
            for (int i = 0; i < count; i++)
            {
                DownloadItem item = _waiting[0];
                _waiting.Remove(item);
                _loading.Add(item);
                item.Start();
            }
        }
    }

    #region Bundle销毁
    List<AssetBundle> _bundlesToDestroy = new List<AssetBundle>();
    private static readonly object _lock = new object();

    public void AddToDestroy(AssetBundle bundle)
    {
        lock (_lock)
        {
            _bundlesToDestroy.Add(bundle);
        }
    }

    public void DestroyBundles()
    {
        lock (_lock)
        {
            if (_bundlesToDestroy.Count > 0)
            {
                for (int i = _bundlesToDestroy.Count - 1; i >= 0; --i)
                {
                    if (_bundlesToDestroy[i] != null)
                        _bundlesToDestroy[i].Unload(false);
                }
                _bundlesToDestroy.Clear();
            }
        }
    }
    #endregion
}

