using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetBundle
{
#if UNITY_ANDROID
	static string targetDir = "Android/";
    static BuildTarget targetBuild = BuildTarget.Android;
#elif UNITY_IPHONE
	static string targetDir = "iPhone/";
    static BuildTarget targetBuild = BuildTarget.iOS;
#elif UNITY_STANDALONE//_WIN
    static string targetDir = "StandaloneWindows/";
    static BuildTarget targetBuild = BuildTarget.StandaloneWindows;
#endif

    [MenuItem("Tool/AssestTool/BuildAssest - Track dependencies")]
    static void BuildResourcesByDependencies()
    {
        //打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        string dirpath = path.Substring(0, path.LastIndexOf("/") + 1);//获取目录
        string filename = path.Substring(path.LastIndexOf("/") + 1);//获取文档名

        uint crc = 0;
        if (path.Length != 0)
        {
            //选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            //没有目录创建目录
            if (!Directory.Exists(dirpath + targetDir))
            {
                Directory.CreateDirectory(dirpath + targetDir);
            }

            //打包
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, dirpath+targetDir+filename,out crc,BuildAssetBundleOptions.CollectDependencies, targetBuild);
            Selection.objects = selection;

            //BuildPipeline.BuildPlayer(levels, Application.streamingAssetsPath + "/Lobby.unity3d", BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
            
            //生成校验文件
            Dictionary<string, object> dir = new Dictionary<string, object>();
            dir["name"] = filename;
            dir["crc"] = crc;
            dir["size"] = new FileInfo(dirpath + targetDir + filename).Length;
            string strjson = fastJSON.JSON.ToJSON(dir);
            Debug.Log(strjson);
            File.AppendAllText(dirpath + targetDir + "crc.txt", strjson);
        }
    }

    [MenuItem("Tool/AssestTool/BuildAssest - No Track dependencies")]
    static void BuildResourcesByNoDependencies()
    {
        //打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        string dirpath = path.Substring(0, path.LastIndexOf("/") + 1);//获取目录
        string filename = path.Substring(path.LastIndexOf("/") + 1);//获取文档名
        uint crc = 0;
        if (path.Length != 0)
        {
            //选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            //没有目录创建目录
            if (!Directory.Exists(dirpath + targetDir))
            {
                Directory.CreateDirectory(dirpath + targetDir);
            }

            //打包
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, dirpath + targetDir + filename, out crc, 0, targetBuild);
            Selection.objects = selection;

            Dictionary<string, object> dir = new Dictionary<string, object>();
            dir["name"] = filename;
            dir["crc"] = crc;
            dir["size:"] = new FileInfo(dirpath + targetDir + filename).Length;
            string strjson = fastJSON.JSON.ToJSON(dir);
            Debug.Log(strjson);
            File.AppendAllText(dirpath + targetDir + "crc.txt",strjson);
        }
    }
}
