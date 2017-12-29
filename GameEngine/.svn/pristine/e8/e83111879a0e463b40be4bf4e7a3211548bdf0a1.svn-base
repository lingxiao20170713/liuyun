using UnityEngine;
using System.Collections.Generic;

public sealed class SpawnPool : IzSpawnPool
{
    #region 私有变量

    private Dictionary<string, List<Transform>> _dicSpawned = new Dictionary<string, List<Transform>>();

    private Dictionary<string, List<Transform>> _dicDespawned = new Dictionary<string, List<Transform>>();

    private IzPoolManager _izPoolManager = null;

    private string _strPoolName = "";

    private GameObject _parentGameObject;

    private GameObject _rootGameObject;

    #endregion


    #region 构造方法

    public SpawnPool(string strPoolName, IzPoolManager izPoolManager, GameObject parentGameObject)
    {
        this._strPoolName = strPoolName;
        this._izPoolManager = izPoolManager;

        this._parentGameObject = parentGameObject;
        this._rootGameObject = new GameObject(strPoolName);
        this._rootGameObject.transform.parent = this._parentGameObject.transform;
    }

    #endregion


    #region 接口实现

    public Transform GetTransform(string strPathName)
    {
        Transform transform = null;
        List<Transform> list = null;
        if (this._dicDespawned.TryGetValue(strPathName, out list) && list.Count > 0)
        {
            transform = list[0];
        }
        
        if(transform == null)
        {
            transform = this._izPoolManager.GetPoolFactory().CreatePrefab(strPathName);
        }
        if (null == transform)
        {
            return null;
        }
        IzPoolObject izPoolObject = transform.GetComponent<PoolObject>();
        if (izPoolObject == null)
        {
            izPoolObject = transform.gameObject.AddComponent<PoolObject>();
        }
        izPoolObject.SetPoolName(this._strPoolName);
        izPoolObject.SetPathName(strPathName);
        AddToSpawnedDic(izPoolObject);
        RemoveFromDespawnedDic(izPoolObject);
        return transform;
    }

    public void DisposeTransform(Transform transform,bool forceDestroy)
    {
        IzPoolObject izPoolObject = transform.GetComponent<PoolObject>();
        if (izPoolObject != null)
        {
            if(forceDestroy)//强制清除,从字典中去掉,并且destroy
            {
                RemoveFromSpawnedDic(izPoolObject);
                RemoveFromDespawnedDic(izPoolObject);

                UnityEngine.Object.Destroy(transform.gameObject);
                //UnityEngine.Object.DestroyImmediate(transform.gameObject);
            }
            else
            {
                AddToDespawnedDic(izPoolObject);
                RemoveFromSpawnedDic(izPoolObject);
            }
        }
        else
        {
            Debug.LogFormat(">>>>该obj {0} 尚未没存到对象池中，无法回收，并将直接删除！ ",transform.name);
            UnityEngine.Object.Destroy(transform.gameObject);
            //UnityEngine.Object.DestroyImmediate(transform.gameObject);
        }
    }

    public static IzSpawnPool CreateSpawnPool(string strPoolName, IzPoolManager izPoolManager, GameObject parentGameObject)
    {
        return new SpawnPool(strPoolName, izPoolManager, parentGameObject);
    }

    public List<string> ReleasePool()
    {
        Debug.Log("readly to realse spawnPool：\n" + PrintString());

        List<string> tempRemovePathList = new List<string>();//将要清除的资源路径列表
        List<string> tempReservePathList = new List<string>();//将要保留的资源路径列表

        Dictionary<string, List<Transform>>.Enumerator tempEnumerator;

        #region 去掉_dicSpawned中多余的值
        tempEnumerator = _dicSpawned.GetEnumerator();
        while (tempEnumerator.MoveNext())
        {
            if (CheckPoolObjectAvailable(tempEnumerator.Current.Key,tempEnumerator.Current.Value) == false)
            {
                if (tempRemovePathList.Contains(tempEnumerator.Current.Key) == false)
                    tempRemovePathList.Add(tempEnumerator.Current.Key);

            }
            else
            {
                if (tempReservePathList.Contains(tempEnumerator.Current.Key) == false)
                    tempReservePathList.Add(tempEnumerator.Current.Key);
            }
        }
        for (int i = 0; i < tempRemovePathList.Count;i++ )
        {
            _dicSpawned.Remove(tempRemovePathList[i]);//从字典_dicSpawned中移除
        }
        #endregion

        #region 清除字典_dicDespawned

        List<Transform> tempTransformList;

        tempEnumerator = _dicDespawned.GetEnumerator();
        while (tempEnumerator.MoveNext())
        {
            if (tempRemovePathList.Contains(tempEnumerator.Current.Key) == false)
                tempRemovePathList.Add(tempEnumerator.Current.Key);

            tempTransformList = tempEnumerator.Current.Value;

            if (tempTransformList == null) continue;

            for (int i = 0; i < tempTransformList.Count;i++ )
            {
                if (tempTransformList[i] != null)
                    UnityEngine.Object.Destroy(tempTransformList[i].gameObject);//destroy池里缓存的GO

            }
        }

        _dicDespawned.Clear();//清空重置

        for (int i = tempRemovePathList.Count -1 ; i >= 0 ; i--)
        {
            if (tempReservePathList.Contains(tempRemovePathList[i]) == true)//筛选出需要保留的资源路径
            {
                tempRemovePathList.RemoveAt(i);
            }
            else
                Debug.Log(">>>null reference res: " + tempRemovePathList[i]);
        }

        #endregion

        return tempRemovePathList;
    }

    public string PrintString()
    {
        string str = "";
        str += "poolName = " + this._strPoolName + " ";
        str += "spawned num = " + this._dicSpawned.Count + " ";
        str += "despawned num = " + this._dicDespawned.Count;
        return str;
    }

    #endregion


    #region 内部方法

    private void AddToDespawnedDic(IzPoolObject izPoolObject)
    {
        if (izPoolObject != null && this._strPoolName == izPoolObject.GetPoolName())
        {
            string strPathName = izPoolObject.GetPathName();
            List<Transform> list = null;
            if (!this._dicDespawned.TryGetValue(strPathName, out list) || list == null)
            {
                list = new List<Transform>();
            }
            list.Add(izPoolObject.GetTransform());
            this._dicDespawned[strPathName] = list;

            izPoolObject.GetTransform().parent = this._rootGameObject.transform;
        }
    }

    private void RemoveFromDespawnedDic(IzPoolObject izPoolObject)
    {
        if (izPoolObject != null && this._strPoolName == izPoolObject.GetPoolName())
        {
            string strPathName = izPoolObject.GetPathName();
            List<Transform> list = null;
            if (this._dicDespawned.TryGetValue(strPathName, out list) && list != null)
            {
                list.Remove(izPoolObject.GetTransform());
                this._dicDespawned[strPathName] = list;
            }
        }
    }

    private void AddToSpawnedDic(IzPoolObject izPoolObject)
    {
        if (izPoolObject != null && this._strPoolName == izPoolObject.GetPoolName())
        {
            string strPathName = izPoolObject.GetPathName();
            List<Transform> list = null;
            if (!this._dicSpawned.TryGetValue(strPathName, out list) || list == null)
            {
                list = new List<Transform>();
            }
            list.Add(izPoolObject.GetTransform());
            this._dicSpawned[strPathName] = list;
        }
    }

    private void RemoveFromSpawnedDic(IzPoolObject izPoolObject)
    {
        if (izPoolObject != null && this._strPoolName == izPoolObject.GetPoolName())
        {
            string strPathName = izPoolObject.GetPathName();
            List<Transform> list = null;
            if (this._dicSpawned.TryGetValue(strPathName, out list) && list != null)
            {
                list.Remove(izPoolObject.GetTransform());
                this._dicSpawned[strPathName] = list;
            }
        }
    }

    //检查_dicSpawned和_dicDespawned中的值是否有效(true：有效)
    private bool CheckPoolObjectAvailable(string bundlePath,List<Transform> transform)
    {
        if (transform == null) return false;

        if (transform.Count == 0) return false;

        for (int i = transform.Count - 1; i >= 0; i--)
        {
            if(transform[i] == null)
            {
                transform.RemoveAt(i);
            }
        }

        if (transform.Count == 0) return false;
        else return true;

    }

    #endregion
}  
