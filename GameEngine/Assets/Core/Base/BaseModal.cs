using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseModal:IModal
{
    public BaseModal()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) { }
    public virtual void Update() { }

    public virtual void Destroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
