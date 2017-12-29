using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : Singleton<SceneMgr> {

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct ToLoadScene
    {
        public string name;
        public Action<int> on_progress;
    }

    public delegate void SceneLoadedEventHandler(string scene_name);
    public event SceneLoadedEventHandler on_scene_loaded_event;

    private string last_scene_;
    private string current_scene_;
    private AsyncOperation async_;
    private bool loading_ = false;
    private Action<int> on_loading_progress_;
    private List<ToLoadScene> to_load_scenes_ = new List<ToLoadScene>();

    public string CurrentSceneName
    {
        get { return current_scene_; }
    }

    public void Update()
    {
        if (!loading_ && !Application.isLoadingLevel && to_load_scenes_.Count > 0)
        {
            ToLoadScene scene = to_load_scenes_[0];
            to_load_scenes_.RemoveAt(0);
            if (scene.on_progress != null)
            {
                LoadScene(scene.name, scene.on_progress);
            }
            else
            {
                LoadScene(scene.name);
            }
        }
    }

    public void FireSceneLoadedEvent(string scene_name)
    {
        if (this.on_scene_loaded_event != null)
        {
            this.on_scene_loaded_event(scene_name);
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public void LoadScene(string scene_name)
    {
        if (!loading_ && !Application.isLoadingLevel)
        {
            AssetManager.Instance.ClearAll();
            Util.StartCoroutine(StartLoading(scene_name));
        }
        else
        {
            to_load_scenes_.Add(new ToLoadScene { name = scene_name, on_progress = null });
        }
    }

    public void LoadScene(string scene_name, System.Action<int> on_progress)
    {
        if (!loading_ && !Application.isLoadingLevel)
        {
            AssetManager.Instance.ClearAll();
            Util.StartCoroutine(StartLoading(scene_name, on_progress));
        }
        else
        {
            to_load_scenes_.Add(new ToLoadScene { name = scene_name, on_progress = on_progress });
        }
    }

    private IEnumerator StartLoading(string scene_name)
    {
        last_scene_ = current_scene_;
        current_scene_ = scene_name;
        loading_ = true;

        async_ = Application.LoadLevelAsync(scene_name);
        yield return async_;

        async_ = null;
        loading_ = false;

        FireSceneLoadedEvent(scene_name);
    }

    private IEnumerator StartLoading(string scene_name, System.Action<int> on_progress)
    {
        last_scene_ = current_scene_;
        current_scene_ = scene_name;
        on_loading_progress_ = on_progress;
        loading_ = true;

        if (on_loading_progress_ != null)
        {
            on_loading_progress_(0);
        }

        int display_progress = 0;
        int to_progress = 0;
        async_ = Application.LoadLevelAsync(scene_name);
        //_async.allowSceneActivation = false;
        while (async_.progress < 0.5f)
        {
            to_progress = (int)(async_.progress * 100);
            while (display_progress < to_progress)
            {
                display_progress += 3;
                if (on_loading_progress_ != null)
                {
                    on_loading_progress_(display_progress);
                }
                yield return new WaitForEndOfFrame();
            }
        }
        //_async.allowSceneActivation = true;
        to_progress = 100;
        while (display_progress < to_progress)
        {
            display_progress += 3;
            if (display_progress > 100) display_progress = 100;
            if (on_loading_progress_ != null)
            {
                on_loading_progress_(display_progress);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return async_;

        on_progress(100);

        async_ = null;
        loading_ = false;
        on_loading_progress_ = null;

        FireSceneLoadedEvent(scene_name);
    }
}
