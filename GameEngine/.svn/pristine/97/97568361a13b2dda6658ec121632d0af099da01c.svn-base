using UnityEngine;
using System.Collections.Generic;

public interface IzPoolManager
{
    /// <summary>
    /// 获得实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strPathName"></param>
    /// <returns></returns>
    Transform GetTransform<T>(string strPathName);

    ///<summary>回收实例</summary>
    ///<param name="transform"></param>
    void DisposeTransform(Transform transform);

    ///<summary>回收实例</summary>
    ///<param name="transform"></param>
    ///<param name="forceDestroy">是否强制删除</param>
    void DisposeTransform(Transform transform, bool forceDestroy);

    /// <summary>
    /// 释放没有使用的对象
    /// </summary>
    /// <returns>被释放的资源路径</returns>
    List<string> ReleasePool();

    /// <summary>
    /// 获得对象池工厂
    /// </summary>
    /// <returns></returns>
    IzPoolFactory GetPoolFactory();

    /// <summary>
    /// 设置对象池工厂
    /// </summary>
    /// <param name="izPoolFactory"></param>
    void SetPoolFactory(IzPoolFactory izPoolFactory);
}