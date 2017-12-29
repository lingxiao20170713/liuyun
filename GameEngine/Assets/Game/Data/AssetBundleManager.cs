using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class AssetBundleManager
{
    public static readonly string localpersist_path = Application.persistentDataPath + "//";
    public static readonly string server_url =
#if UNITY_ANDROID
        "http://192.168.1.8/";
        //"192.168.1.21:8000" + "/Android/";
#elif UNITY_IPHONE
		"192.168.1.38:8000" + "/iPhone/";
#else
		"localhost" + "/";
#endif
    public static readonly string local_path = Application.persistentDataPath + "/";

    public static AssetBundle ab;

    public static string BytesToString(byte[] bt)
    {
        return Encoding.UTF8.GetString(bt);
    }

    public static byte[] StringToBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static List<checkData> NeedUpdateDataList = new List<checkData>();
    public static long NeedUpdateDataSize = 0;
    static Dictionary<string, checkData> serverCheckData = new Dictionary<string, checkData>();
    static Dictionary<string, checkData> localCheckData = new Dictionary<string, checkData>();

    public static IEnumerator WWWDataLoad()
    {
        //从http下载资源
        using (WWW www = new WWW(server_url + "GetNotice"/*+"serverdata"*/))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogWarning("Load Failed " + www.error);
                yield break;
            }

            string text = BytesToString(www.bytes);
            Debug.Log(text);
            List<checkData> serverFileInfos = fastJSON.JSON.ToObject<List<checkData>>(text);
            for (int i = 0; i < serverFileInfos.Count; i++)
            {
                serverCheckData[serverFileInfos[i].name] = serverFileInfos[i];
                Debug.Log(serverFileInfos[i].name + "||" + serverFileInfos[i].crc + "||" + serverFileInfos[i].size);
            }
        }
    }
    public static IEnumerator WWWDataDownCheck()
    {
        //从http下载资源
        using (WWW www = new WWW(server_url + "crc.txt"/*"GetNotice"*//*+"serverdata"*/))
        {
            Debug.Log(www.url);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogWarning("Load Failed " + www.error);
                yield break;
            }

            string text = BytesToString(www.bytes);
            Debug.Log(text);
            List<checkData> serverFileInfos = fastJSON.JSON.ToObject<List<checkData>>(text);
            for (int i = 0; i < serverFileInfos.Count; i++)
            {
                serverCheckData[serverFileInfos[i].name] = serverFileInfos[i];
                Debug.Log(serverFileInfos[i].name+"||"+ serverFileInfos[i].crc+"||"+ serverFileInfos[i].size);
            }
        }

        //从本地加载文件MD5表，可能没有
        try
        {
            List<checkData> checkDataList = fastJSON.JSON.ToObject<List<checkData>>(File.ReadAllText(local_path + "crc.txt"));
            for (int i = 0; i < checkDataList.Count; i++)
            {
                localCheckData[checkDataList[i].name] = checkDataList[i];
            }
            Debug.Log(local_path);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        //计算需要更新的文件
        foreach (KeyValuePair<string, checkData> data in serverCheckData)
        {
            //更新需要的文件
            if (!localCheckData.ContainsKey(data.Key) || localCheckData[data.Key].crc != data.Value.crc)
            {
                NeedUpdateDataList.Add(data.Value);
                NeedUpdateDataSize += data.Value.size;
            }
        }

        string down_size = string.Format("{0:0.0}M", (float)NeedUpdateDataSize / 1024 / 1024);
        Debug.Log("更新的大小："+ NeedUpdateDataSize+"||"+down_size);
    }


    public static IEnumerator updateFromServerByChecked()
    {
        for (int i = 0; i < NeedUpdateDataList.Count; i++)
        {
            using (WWW www = new WWW(server_url + NeedUpdateDataList[i].name))
            {
                byte[] bytes = null;
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning("Load Failed " + www.error);
                    updateLocalcrc();
                    yield break;
                }
                else
                {
                    if (www.isDone)
                    {
                        bytes = www.bytes;

                        string path = local_path + NeedUpdateDataList[i].name;
                        Debug.Log("下载文件："+path);

                        //创建目录，如果不存在
                        int index = NeedUpdateDataList[i].name.LastIndexOf('/');
                        if (index != -1)
                        {
                            string dir = local_path + NeedUpdateDataList[i].name.Substring(0, index);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            Debug.Log(dir);
                        }

                        FileStream fs = new FileStream(path, FileMode.Create);
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Flush();
                        fs.Close();

                        updateLocalcrc();
                    }
                }
            }
        }
        updateLocalcrc();
    }

    public static void updateLocalcrc()
    {
        List<checkData> listFiles = new List<checkData>();
        foreach (checkData uf in serverCheckData.Values)
        {
            listFiles.Add(uf);
        }
        byte[] bytes = StringToBytes(fastJSON.JSON.ToJSON(listFiles.ToArray()));

        //保存最新的文件MD5值表
        FileStream fs = new FileStream(local_path + "crc.txt", FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();
        fs.Close();
    }
}

public class checkData
{
    public string name;
    public long crc;
    public long size;
}
