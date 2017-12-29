using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;
using System.Diagnostics;
using UnityEngine.UI;

public class Asset
{
    public string name;
    public string type;
    public DownloadItem download;

    public delegate void OnLoadCompleted(DownloadItem item);
    public OnLoadCompleted onLoadCompleted;

    public void LoadCompleted(DownloadItem item)
    {
        if (onLoadCompleted != null)
            onLoadCompleted(item);
    }

    public Asset(string name, string type)
    {
        this.name = name;
        this.type = type;
    }

    public void Load()
    {
        download = DownloadManager.Instance.GetDownload(name, type);
        download.onLoadCompleted += LoadCompleted;
    }

    public bool IsLoaded()
    {
        return download != null && download.isDone;
    }
}

public class MainAsset
{
    public Asset asset;
    List<Asset> deps = new List<Asset>();

    public bool isLoading = false;

    public delegate void OnLoadCompleted(DownloadItem item);
    public event OnLoadCompleted onLoadCompletedEvent;

    public MainAsset(string name, string type)
    {
        asset = new Asset(name, type);
        List<string> depList = AssetManager.Instance.GetDependancies(name);
        if (depList != null)
        {
            for (int i = 0; i < depList.Count; ++i)
            {
                deps.Add(new Asset(depList[i], AssetManager.ResourceShared));
            }
        }
    }

    public void Load()
    {
        if (isLoading)
            return;

        asset.Load();
        if (asset.onLoadCompleted == null)
            asset.onLoadCompleted = LoadCompleted;

        for (int i = 0; i < deps.Count; ++i)
        {
            deps[i].Load();
            if (deps[i].onLoadCompleted == null)
                deps[i].onLoadCompleted = LoadCompleted;
        }
    }

    public bool IsLoaded()
    {
        if (!asset.IsLoaded())
            return false;
        for (int i = 0; i < deps.Count; ++i)
        {
            if (!deps[i].IsLoaded())
                return false;
        }
        return true;
    }

    public void LoadCompleted(DownloadItem item)
    {
        if (IsLoaded() && onLoadCompletedEvent != null)
        {
            onLoadCompletedEvent(asset.download);
        }
    }
}

public class AssetManager : Singleton<AssetManager>
{
    public const string ResourceShared = "shared";
    public const string ResourceCharacter = "character";
    public const string ResourceMaterial = "material";
    public const string ResourceMap = "map";
    public const string ResourceItem = "item";
    public const string ResourceEffect = "effect";
    public const string ResourceImage = "image";
    public const string ResourceUI = "ui";
    public const string ResourceSound = "sound";

    Dictionary<string, WWWResource> _resourceCfg = new Dictionary<string, WWWResource>();
    Dictionary<string, MainAsset> _assetCache = new Dictionary<string, MainAsset>();
    Dictionary<string, Object> _prefabCache = new Dictionary<string, Object>();

    public const string ResourceVersionPref = "ResourceVersionPref";
    int _resourceVersion = 1865; // 默认值为导出资源时的资源版本号

    public int ResourceVersion
    {
        get
        {
            int localResVersion = PlayerPrefs.GetInt(ResourceVersionPref);
            return Mathf.Max(localResVersion, _resourceVersion);
        }
        set
        {
            if (_resourceVersion != value)
            {
                _resourceVersion = value;
                if (PlayerPrefs.GetInt(ResourceVersionPref) < _resourceVersion)
                    PlayerPrefs.SetInt(ResourceVersionPref, _resourceVersion);
            }
        }
    }

    public void InitResource(Action onComplete)
    {
        Util.StartCoroutine(LoadResource(onComplete));
    }

    IEnumerator LoadResource(Action onComplete)
    {
        yield return null;
        if (onComplete != null) onComplete();
    }

    public void Clear()
    {
        _prefabCache.Clear();
    }

    public void ClearAll()
    {
        Clear();
    }

    public List<string> GetDependancies(string assetName)
    {
        if (_resourceCfg.ContainsKey(assetName))
            return _resourceCfg[assetName].Depends;
        return null;
    }

#region 异步接口
    public void LoadAsset<T>(string assetName, string assetType, Action<T> cb, bool cache) where T : Object
    {
        if (string.IsNullOrEmpty(assetName))
        {
            UnityEngine.Debug.LogError("LoadAsset<> failed - assetName is null!");
            return;
        }

#if COLLECTION_PERFORMANCE
        Stopwatch timer = new Stopwatch();
        timer.Start();
#endif
        if (_prefabCache.ContainsKey(assetName))
        {
            T asset = _prefabCache[assetName] as T;
            if (asset != null && cb != null)
                cb(asset);
        }
        else
        {
#if USE_E//使用在editor下的不打包文件
            T asset = null;
            switch (assetType)
            {
                case ResourceUI:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/ui/{0}.prefab", assetName)) as T;
                    break;
                case ResourceCharacter:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/character/{0}.prefab", assetName)) as T;
                    break;
                case ResourceMap:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/map/{0}.prefab", assetName)) as T;
                    break;
                case ResourceItem:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/item/{0}.prefab", assetName)) as T;
                    break;
                case ResourceEffect:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/effect/{0}.prefab", assetName)) as T;
                    break;
                case ResourceSound:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/sound/{0}.ogg", assetName)) as T;
                    break;
                case ResourceImage:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/Image/{0}.png", assetName)) as T;
                    break;

                case ResourceMaterial:
                    asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Game/__Load/material/{0}.mat", assetName)) as T;
                    break;
            }
            if (asset != null)
            {
                if (cache && !_prefabCache.ContainsKey(assetName))
                    _prefabCache.Add(assetName, asset);

                if (cb != null)
                    cb(asset);
            }
            else
                UnityEngine.Debug.LogErrorFormat("LoadAsset() - Failed: assetType is {0}, assetName is {1}", assetType, assetName);
#else
            MainAsset mainAsset = null;
            if (_assetCache.ContainsKey(assetName))
                mainAsset = _assetCache[assetName];
            else
            {
                mainAsset = new MainAsset(assetName, assetType);
                _assetCache.Add(assetName, mainAsset);
            }

            if (mainAsset.IsLoaded())
            {
                T asset = mainAsset.asset.download.ab.mainAsset as T;
                if (asset != null)
                {
                    if (cache && !_prefabCache.ContainsKey(assetName))
                        _prefabCache.Add(assetName, asset);

                    if (cb != null)
                        cb(asset);
                }
            }
            else
            {
                if (!mainAsset.isLoading)
                {
                    if (!mainAsset.IsLoaded())
                        mainAsset.Load();
                    mainAsset.isLoading = true;
                }

                mainAsset.onLoadCompletedEvent += (DownloadItem item) =>
                {
                    T asset = item.ab.mainAsset as T;
                    if (!Object.ReferenceEquals(asset, null))
                    {
                        if (cache && !_prefabCache.ContainsKey(assetName))
                            _prefabCache.Add(assetName, asset);

#if COLLECTION_PERFORMANCE
                        timer.Stop();
                        long Elapsed = timer.ElapsedMilliseconds;
                        timer.Reset();
                        timer.Start();
#endif
                        if (cb != null)
                            cb(asset);
#if COLLECTION_PERFORMANCE
                        timer.Stop();
                        CollectionPerformance.FileAppend(CollectionPerformance.TimesRecord, string.Format("{0}\t{1}\t{2}\n", assetName, Elapsed, timer.ElapsedMilliseconds));
#endif
                    }
                };
            }
#endif
        }
    }

    public void LoadUI(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceUI, cb, cache);
    }

    public void LoadCharacter(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceCharacter, cb, cache);
    }

    public void LoadPet(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceCharacter, cb, cache);
    }

    public void LoadMap(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceMap, cb, cache);
    }

    public void LoadItem(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceItem, cb, cache);
    }

    public void LoadEffect(string assetName, Action<GameObject> cb, bool cache = false)
    {
        LoadAsset<GameObject>(assetName, ResourceEffect, cb, cache);
    }
    public void LoadSound(string assetName, Action<AudioClip> cb)
    {
        LoadAsset<AudioClip>(assetName, ResourceSound, cb, true);
    }

    public void LoadImage(string assetName, Action<Image> cb, bool cache = false)
    {
        LoadAsset<Image>(assetName, ResourceImage, cb, true);
    }

    public void LoadMaterial(string assetName, Action<Material> cb, bool cache = false)
    {
        LoadAsset<Material>(assetName, ResourceMaterial, cb, true);
    }

    public void LoadCharTexture(string assetName, Action<Texture> cb, bool cache = false)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            UnityEngine.Debug.LogError("LoadAsset<> failed - assetName is null!");
            return;
        }

        if (_prefabCache.ContainsKey(assetName))
        {
            Texture asset = _prefabCache[assetName] as Texture;
            if (asset != null && cb != null)
                cb(asset);
        }
        else
        {
            Util.StartCoroutine(LoadCharTextureImpl(assetName, cb, cache));
        }
    }

    IEnumerator LoadCharTextureImpl(string assetName, Action<Texture> cb, bool cache = false)
    {
        string assetPath = string.Format("Texture/{0}", assetName);
        ResourceRequest request = Resources.LoadAsync<Texture>(assetPath);
        yield return request;

        Texture asset = request.asset as Texture;

        if (cache && !_prefabCache.ContainsKey(assetName))
            _prefabCache.Add(assetName, asset);

        if (cb != null)
            cb(asset);
    }
#endregion
}

public class DownloadItem : IDisposable
{
    public string assetName;
    public string assetType;
    public WWW www;
    public AssetBundle ab;
    public bool isDone;
    public string error;

    public delegate void LoadCompletedEventHandler(DownloadItem item);
    public event LoadCompletedEventHandler onLoadCompleted;

    public DownloadItem(string assetName, string assetType)
    {
        this.assetName = assetName;
        this.assetType = assetType;
        isDone = false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.www != null)
        {
            if (disposing)
            {
            }
            DownloadManager.Instance.AddToDestroy(ab);
            www.Dispose();
            www = null;
        }
    }

    ~DownloadItem()
    {
        Dispose(false);
    }

    public void FireLoadCompleted()
    {
        if (this.onLoadCompleted != null)
        {
            this.onLoadCompleted(this);
            this.onLoadCompleted = null;
        }
    }

    public bool IsLoadCompleted()
    {
        return (www == null) || (www.isDone);
    }

    public void Start()
    {
        if (this.www != null)
        {
            throw new System.InvalidOperationException();
        }
        string uri = "";
#if USE_S
        uri = string.Format("{0}/{1}/{2}.unity3d", Util.AssetsPath, assetType, assetName);
#elif UNITY_ANDROID ||UNITY_IPHONE
        uri = string.Format("{0}/{1}.unity3d", Util.AssetsPath, assetName);
#endif
        UnityEngine.Debug.Log("加载url:"+uri);
        www = new WWW(uri);
    }

}

public class WWWResource
{
    public string Name;
    public string Type;
    public int Size;
    public List<string> Depends = new List<string>();
}

public class WWWResourceCfg : ScriptableObject
{
    public int ResourceVersion;
    public List<WWWResource> Content = new List<WWWResource>();
}