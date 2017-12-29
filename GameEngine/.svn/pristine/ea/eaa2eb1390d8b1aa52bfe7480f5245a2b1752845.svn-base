using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public static Dictionary<string, Object> cacheResources = new Dictionary<string, Object>();
    public static T LoadFromResources<T>(string fullPath) where T: UnityEngine.Object
    {
        //if (ResourceCache.ContainsKey(path))
        //    return (T)ResourceCache[path];

        //ResourceCache.Add(path, ResourceManager.Load<T>(path));
        ////Debug.Log("Cache Resource " + path);
        //return (T)ResourceCache[path];

        if (cacheResources.ContainsKey(fullPath))
        {
            T ac = cacheResources[fullPath] as T;
            return ac;
        }
        else
        {
            T ac = Resources.Load<T>(fullPath) as T;
            cacheResources.Add(fullPath, ac);
            return ac;
        }
    }

    public static T LoadFromAssetBundle<T>(string fullPath) where T : UnityEngine.Object
    {
        T ac = Resources.Load<T>(fullPath);
        if (ac != null) return ac;
        AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + fullPath + ".unity3d");
        if(ab!=null)
        {
            ac = ab.mainAsset as T;
            ab.Unload(false);
            return ac;
        }
        return null;
    }
}
