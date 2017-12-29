using UnityEngine;
using System.Collections.Generic;

public interface IzSpawnPool
{
    /// <summary>
    /// 获得实例
    /// </summary>
    /// <param name="strPathName">资源路劲</param>
    /// <returns></returns>
    Transform GetTransform(string strPathName);

    ///<summary>回收实例</summary>
    ///<param name="transform"></param>
    ///<param name="forceDestroy">是否强制删除</param>
    void DisposeTransform(Transform transform,bool forceDestroy);

    /// <summary>
    /// 释放没有使用的GO，重置池状态
    /// </summary>
    /// <returns>被释放object的bundlePath</returns>
    List<string> ReleasePool();

    string PrintString();
}