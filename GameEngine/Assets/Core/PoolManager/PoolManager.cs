using UnityEngine;
using System.Collections.Generic;

public sealed class PoolManager:Singleton<PoolManager>,IzPoolManager
{
    private Dictionary<string, IzSpawnPool> _dicSpawnPool = new Dictionary<string, IzSpawnPool>();

    private IzPoolFactory _izPoolFactory = null;

    private GameObject _rootGameObject;

    public PoolManager()
    {
        Init();
    }

    private void Init()
    {
        this._rootGameObject = new GameObject("PoolManager");
        UnityEngine.Object.DontDestroyOnLoad(this._rootGameObject);
    }

    public Transform GetTransform<T>(string strPathName) 
    {
        string strPoolName = typeof(T).Name;
        IzSpawnPool izSpawnPool = null;
        if (!this._dicSpawnPool.TryGetValue(strPoolName, out izSpawnPool))
        {
            izSpawnPool = SpawnPool.CreateSpawnPool(strPoolName, this, this._rootGameObject);
            this._dicSpawnPool[strPoolName] = izSpawnPool;
        }
        Transform transform = izSpawnPool.GetTransform(strPathName);
        if (null == transform)
            return null;

        if (transform.gameObject.activeSelf == false)
            transform.gameObject.SetActive(true);
        return transform;
    }

    public void DisposeTransform(Transform transform,bool forceDestroy)
    {
        IzPoolObject izPoolObject = transform.GetComponent<PoolObject>();       //TODO Denes 这里可以尝试优化
        IzSpawnPool izSpawnPool = null;
        if (izPoolObject != null && this._dicSpawnPool.TryGetValue(izPoolObject.GetPoolName(), out izSpawnPool) && izSpawnPool != null)
        {
            izSpawnPool.DisposeTransform(transform, forceDestroy);
        }
        else 
        {
            UnityEngine.Object.Destroy(transform.gameObject);//不在池里的对象，直接destroy
        }
    }

    public void DisposeTransform(Transform transform)
    {
        if(null != transform)
            transform.gameObject.SetActive(false);
        DisposeTransform(transform, false);
    }

    public IzPoolFactory GetPoolFactory()
    {
        return this._izPoolFactory;
    }

    public void SetPoolFactory(IzPoolFactory izPoolFactory)
    {
        this._izPoolFactory = izPoolFactory;
    }

    public List<string> ReleasePool()
    {
        List<string> tempRemovePathList = new List<string>();

        Dictionary<string, IzSpawnPool>.Enumerator tempEnumerator = _dicSpawnPool.GetEnumerator();
        while(tempEnumerator.MoveNext())
        {
            tempRemovePathList.AddRange(tempEnumerator.Current.Value.ReleasePool());
        }
        return tempRemovePathList;
    }

    public void PrintAll()
    {

    }

    public void Print<T>()
    {
        string str = this._dicSpawnPool[typeof(T).Name].PrintString();
    }
}  
