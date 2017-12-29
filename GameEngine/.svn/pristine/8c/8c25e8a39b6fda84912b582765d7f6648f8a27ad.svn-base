using System;
using UnityEngine;

public sealed class PoolObject : MonoBehaviour , IzPoolObject
{
    #region 参数

    public string poolName;                             //所在的对象池名称

    public string pathName;                             //资源路劲

    private Transform _cachedTrans;

    private GameObject _cachedGameObject;

    #endregion

    void Awake()
    {
        this._cachedTrans = this.transform;
        this._cachedGameObject = this.gameObject;
    }

    #region 接口实现

    public string GetPoolName()
    {
        return this.poolName;
    }

    public void SetPoolName(string strPoolName)
    {
        this.poolName = strPoolName;
    }

    public string GetPathName()
    {
        return this.pathName;
    }

    public void SetPathName(string strPathName)
    {
        this.pathName = strPathName;
    }

    public Transform GetTransform()
    {
        return this._cachedTrans;
    }

    public GameObject GetGameObject()
    {
        return this._cachedGameObject;
    }

    #endregion
}  
