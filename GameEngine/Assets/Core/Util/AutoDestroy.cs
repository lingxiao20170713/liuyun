/********************************************************************
 * Copyright (C) 2011 网络科技有限公司
 * 版权所有
 * 
 * 文件名：AutoDestroy.cs
 * 文件功能描述：自动销毁组件
 *******************************************************************/
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float delayTime;

    private void Start()
    {
        Destroy(gameObject, this.delayTime);
    }
}
