using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class Util
{
    public static DateTime UnixEpoch = new DateTime(1970, 1, 1);

    public static MonoBehaviour Mono;

    static public string AssetRoot { get { return Application.persistentDataPath; } }

    //座机电话
    public static bool IsTelephone(string str_telephone)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
    }
    //手机号
    public static bool IsHandset(string str_handset)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^0{0,1}(13[0-9]|14[5-9]|15[0-9]|166|17[0-8]|18[0-9])[0-9]{8}$");
    }
    //身份证
    public static bool IsIDcard(string str_idcard)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
    }
    //数字
    public static bool IsNumber(string str_number)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
    }
    //邮编
    public static bool IsPostalcode(string str_postalcode)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
    }
    /// <summary>
    /// 验证邮箱
    /// </summary>
    /// <param name="str_Email"></param>
    /// <returns></returns>
    public static bool IsEmail(string str_Email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"\\w{1,}@\\w{1,}\\.\\w{1,}");
    }
    public static string StringToMD5(string pwd)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        byte[] b = Encoding.UTF8.GetBytes(pwd);
        byte[] m = md5.ComputeHash(b);
        return System.BitConverter.ToString(m).Replace("-", "");
    }


    public static Coroutine StartCoroutine(IEnumerator enumerator)
    {
        if (!ReferenceEquals(null, Mono) && !ReferenceEquals(null, enumerator))
        {
            return Mono.StartCoroutine(enumerator);
        }
        return null;
    }

    public static void StopCoroutine(IEnumerator enumerator)
    {
        if (!ReferenceEquals(null, Mono) && !ReferenceEquals(null, enumerator))
        {
            Mono.StopCoroutine(enumerator);
        }
    }
    public static void StopCoroutine(Coroutine cor)
    {
        if (cor != null)
        {
            Mono.StopCoroutine(cor);
        }
    }
    public static void InvokeRepeating(string strFunName, float fTime, float fRepeatRate)
    {
        if (!ReferenceEquals(null, Mono))
        {
            Mono.InvokeRepeating(strFunName, fTime, fRepeatRate);
        }
    }

    public static void CancelInvoke(string strFunName)
    {
        if (!ReferenceEquals(null, Mono))
        {
            Mono.CancelInvoke(strFunName);
        }
    }

    // 时间戳转为C#格式时间
    public static DateTime StampToDateTime(long timeStamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime dt = dateTimeStart.AddMilliseconds(timeStamp);
        return dt;
    }

    // 将Unix时间戳转换为DateTime类型时间
    public static DateTime ConvertIntToDateTime(double d)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime time = startTime.AddSeconds(d);
        return time;
    }

    public static long UnixNowTicks()
    {
        TimeSpan timeSpan = (DateTime.UtcNow - UnixEpoch);
        return (long)timeSpan.TotalMilliseconds;
    }

    public static int UnixNowTicksInSecond()
    {
        TimeSpan timeSpan = (DateTime.UtcNow - UnixEpoch);
        return (int)timeSpan.TotalSeconds;
    }

    public static int TimestampInSecond(int y, int m, int d, int t, int min, int s)
    {
        DateTime dt = new DateTime(y, m, d, t, min, s);
        TimeSpan timeSpan = dt - UnixEpoch;
        return (int)timeSpan.TotalSeconds - 28800; // 减去8小时，因为传进来的日期是东八区的时间
    }

    /// <summary>
    /// 生成范围内的随机数
    /// </summary>
    /// <param name="min">最小值（包含）</param>
    /// <param name="max">最大值（不包含）</param>
    /// <returns></returns>
    public static int Rnd(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float Rnd(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static int Rnd100()
    {
        return Rnd(1, 101);
    }

    public static bool RndBool()
    {
        return Rnd(0, 2) != 0;
    }

    /// <summary>
    /// 值类型包装类
    /// 使用此包装器可以像使用引用类型一样使用值类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueType<T>
    {
        public static implicit operator T(ValueType<T> x)
        {
            return x.Value;
        }

        public static implicit operator ValueType<T>(T x)
        {
            return new ValueType<T>() { Value = x };
        }

        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return (obj != null
                && obj.GetType() == this.GetType()
                && Value.Equals((obj as ValueType<T>).Value));
        }

        public static bool operator ==(ValueType<T> v1, ValueType<T> v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(ValueType<T> v1, ValueType<T> v2)
        {
            return !v1.Equals(v2);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    // 随机序列，生成不重复的随机数
    public class RndSequences<T>
    {
        private T[] m_sequences = null;
        private List<int> m_indices = new List<int>();

        public RndSequences(T[] sequences)
        {
            m_sequences = sequences;
        }

        public T Gen()
        {
            int idx = 0;
            if (m_indices.Count > 0)
            {
                int uidx = Util.Rnd(0, m_indices.Count);
                idx = m_indices[uidx];
                m_indices.RemoveAt(uidx);
            }
            else
                idx = Util.Rnd(0, m_sequences.Length);

            if (m_indices.Count <= 0)
            {
                for (int i = 0; i < m_sequences.Length; ++i)
                {
                    if (i != idx)
                        m_indices.Add(i);
                }
            }

            return m_sequences[idx];
        }
    }


    public static GameObject Instantiate(GameObject prefab, Transform parent)
    {
        if (prefab != null)
        {
            GameObject obj = UnityEngine.Object.Instantiate(prefab) as GameObject;
            obj.name = prefab.name;
            obj.transform.SetParent(parent);
            obj.transform.localPosition = prefab.transform.localPosition;
            obj.transform.localRotation = prefab.transform.localRotation;
            obj.transform.localScale = prefab.transform.localScale;
            return obj;
        }
        return null;
    }

    public static T Instantiate<T>(GameObject prefab, Transform parent) where T : Component
    {
        GameObject go = Instantiate(prefab, parent);
        if (go == null)
            return null;
        return go.GetComponent<T>();
    }

    public static T Instantiate<T>(T prefab, Transform parent) where T : Component
    {
        T t = UnityEngine.Object.Instantiate(prefab) as T;
        t.name = prefab.name;
        t.transform.parent = parent;
        t.transform.localPosition = prefab.transform.localPosition;
        t.transform.localRotation = prefab.transform.localRotation;
        t.transform.localScale = prefab.transform.localScale;
        return t;
    }

    public static GameObject Instantiate(GameObject prefab, Transform parent, bool linked)
    {
        if (prefab != null)
        {
            Vector3 position = parent.transform.TransformPoint(prefab.transform.localPosition);
            Quaternion rotation = parent.transform.rotation * prefab.transform.localRotation;
            GameObject obj = UnityEngine.Object.Instantiate(prefab, position, rotation) as GameObject;
            obj.transform.parent = linked ? parent : null;
            return obj;
        }
        return null;
    }

    //转换成弧度
    public static float ToRadian(float degree)
    {
        return degree * Mathf.Deg2Rad;
    }

    //转换成度数
    public static float ToDegree(float radian)
    {
        return radian * Mathf.Rad2Deg;
    }

    public static Vector3 Vibrate(Vector3 pos, Vector3 range, float scale = 1f)
    {
        float x = range.x * Rnd(-1f, 1f) * scale * Time.timeScale;
        float y = range.y * Rnd(-1f, 1f) * scale * Time.timeScale;
        float z = range.z * Rnd(-1f, 1f) * scale * Time.timeScale;
        return new Vector3(pos.x + x, pos.y + y, pos.z + z);
    }

    public static void PlayEffect(GameObject go, bool play = true)
    {
        if (go == null)
            return;

        ParticleSystem[] pss = go.GetComponentsInChildren<ParticleSystem>();
        if (play)
        {
            go.SetActive(true);
            for (int i = 0; i < pss.Length; ++i)
                pss[i].Play();
        }
        else
        {
            go.SetActive(false);
            for (int i = 0; i < pss.Length; ++i)
                pss[i].Stop();
        }
    }

    public static void PlayEffect(Transform trans, string path, bool play = true)
    {
        Transform t = trans.Find(path);
        if (t == null)
            return;

        PlayEffect(t.gameObject, play);
    }

    public static void PlayEffect(GameObject go, string path, bool play = true)
    {
        PlayEffect(go.transform, path, play);
    }


    public static long getSelfID()
    {
        long SelfID = 0;
        return SelfID;
    }

    public static void ResetPosition(this Transform trans)
    {
        trans.position = Vector3.zero;
    }

    public static void ResetRotation(this Transform trans)
    {
        trans.rotation = Quaternion.identity;
    }

    public static void ResetScale(this Transform trans)
    {
        trans.localScale = Vector3.one;
    }
    public static void ResetLocalPosition(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
    }
    public static void ResetLocalRotation(this Transform trans)
    {
        trans.localRotation = Quaternion.identity;
    }
    public static void ResetLocalScale(this Transform trans)
    {
        trans.localScale = Vector3.one;
    }
    public static void ResetTransform(this GameObject obj, Vector3 v = default(Vector3))
    {
        obj.ResetTransform();
    }
    public static void ResetTransform(this Transform trans, bool islocal = true)
    {
        if (islocal)
        {
            trans.ResetLocalPosition(); trans.ResetLocalRotation(); trans.ResetLocalScale();
        }
        else
        {
            trans.ResetPosition(); trans.ResetRotation(); trans.ResetScale();
        }
    }

    public static IEnumerator loadImage(RawImage image, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            Debug.Log("图片加载成功");
            //获取Texture
            //Texture2D texture = www.texture;
            //创建Sprite
            if (image != null)
            {
                image.texture = www.texture;
            }
        }
        else
        {
            Debug.LogError("图片加载出错");
        }
    }

    public static IEnumerator loadImage(RawImage image, string url, Action action)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            //Texture2D texture = www.texture;
            //创建Sprite
            if (image != null)
            {
                image.texture = www.texture;
                action();
            }
        }
        else
        {
            Debug.LogError("Load image fail!");
        }
    }

    public static Vector3 ScreenPositionNormalized(Vector3 pos)
    {
        Vector2 half = new Vector2(Screen.width / 2f, Screen.height / 2f);
        float x = (pos.x - half.x) / half.x;
        float y = (pos.y - half.y) / half.y;

        return new Vector3(x, y, 0);
    }

    // convert seconds to XX天XX小时XX分XX秒
    public enum FormatTimeFlags
    {
        Day = 1,
        Hour = 2,
        Minute = 4,
        Seconds = 8,
        All = Day | Hour | Minute | Seconds,
        All2 = Hour | Minute | Seconds,
        All3 = Hour | Minute,
    }
    public static string FormatTime(int seconds, FormatTimeFlags flags = FormatTimeFlags.All)
    {
        string str = "";
        int day = seconds / 86400;
        if (day > 0 && (flags & FormatTimeFlags.Day) != 0)
        {
            str = day + "天";
            seconds -= day * 86400;
        }
        int hour = seconds / 3600;
        if (hour > 0 && (flags & FormatTimeFlags.Hour) != 0)
        {
            str += hour + "小时";
            seconds -= hour * 3600;
        }
        int minute = seconds / 60;
        if (minute > 0 && (flags & FormatTimeFlags.Minute) != 0)
        {
            str += minute + "分";
            seconds -= minute * 60;
        }
        if ((seconds >= 0 && (flags & FormatTimeFlags.Seconds) != 0) ||
            string.IsNullOrEmpty(str)) // 没有任何值就显示0秒
        {
            str += seconds + "秒";
        }
        return str;
    }

    public static bool IsMemoryLow()
    {
        return SystemInfo.systemMemorySize < 750;
    }

    public static bool FloatEqual(float a, float b)
    {
        return Mathf.Abs(a - b) <= float.Epsilon;
    }

    static public string AssetsPath
    {
        get
        {
#if USE_S//使用editor下streamingAssets
        return string.Format("file:///{0}", Application.streamingAssetsPath);
#elif UNITY_ANDROID
            //return string.Format("jar:file://{0}!/assets", Application.dataPath);//读取apk包里面的资源路径
            return string.Format("file:///{0}", Application.persistentDataPath);
#elif UNITY_IPHONE
        return string.Format("file:///{0}/Raw", Application.dataPath);//Application.persistentDataPath  Application.dataPath
#else
        throw new NotImplementedException(string.Format("StreamingAssetsPath():不支持当前平台 - \"{0}\"", Application.platform));
#endif
        }
    }

#region 文件处理
    public static string GetFullName(GameObject go)
    {
        string name = go.name;
        var tempGo = go.transform;

        while (tempGo.parent != null)
        {
            name = string.Concat(tempGo.parent.name, ".", name);
            tempGo = tempGo.parent;
        }
        return name;
    }

    public static string GetDirectoryName(string fileName)
    {
        return fileName.Substring(0, fileName.LastIndexOf('/'));
    }
#endregion

#region 文本处理
    public static bool IsNullOrWhiteSpace(string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static int GetByteCount(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        int len = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            if (str[i] > 256)
                len += 2;
            else
                len += 1;
        }

        return len;
    }

    public static int CharCount(this string str, char c)
    {
        int cnt = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            if (str[i] == c)
                ++cnt;
        }
        return cnt;
    }

    /// <summary>
    /// 保存 Text 文档。
    /// </summary>
    /// <param name="fileName">文档名称</param>
    /// <param name="text">内容</param>
    public static void SaveText(string fileName, string text)
    {
        if (!Directory.Exists(GetDirectoryName(fileName)))
        {
            Directory.CreateDirectory(GetDirectoryName(fileName));
        }
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                //开始写入
                sw.Write(text);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
            }
            fs.Close();
        }
    }

    /// <summary>
    /// 替换字符串中的子字符串。
    /// </summary>
    /// <param name="input">原字符串</param>
    /// <param name="oldValue">旧子字符串</param>
    /// <param name="newValue">新子字符串</param>
    /// <param name="count">替换数量</param>
    /// <param name="startAt">从第几个字符开始</param>
    /// <returns>替换后的字符串</returns>
    public static string ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
    {
        int pos = input.IndexOf(oldValue, startAt);
        if (pos < 0)
        {
            return input;
        }
        return string.Concat(input.Substring(0, pos), newValue, input.Substring(pos + oldValue.Length));
    }
#endregion

    public static void LogFormat(this Debug debug,string format,object[] orgs)
    {
        UnityEngine.Debug.Log(string.Format(format,orgs));
    }

    static public T AddMissingComponent<T>(this GameObject go) where T : Component
    {
        T comp = go.GetComponent<T>();
        if (comp == null)
        {
            comp = go.AddComponent<T>();
        }

        return comp;
    }

    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    //控制动画不受timescale影响
    public static IEnumerator Play(this Animation animation, string clipName, bool ignoreTimeScale, Action onComplete)
    {
        //We don't want to use timeScale, so we have to animate by frame..
        if (ignoreTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;

            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            bool _inReversePlaying = false;

            float _deltaTime = 0F;
            animation.Play(clipName);
            _timeAtLastFrame = Time.realtimeSinceStartup;

            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                _deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += _deltaTime;

                _currState.normalizedTime = _inReversePlaying ? 1.0f - (_progressTime / _currState.length)
                                                              : _progressTime / _currState.length;
                animation.Sample();

                if (_progressTime >= _currState.length)
                {
                    switch (_currState.wrapMode)
                    {
                        case WrapMode.Loop:
                            //Loop anim, continue.
                            _progressTime = 0.0f;
                            break;
                        case WrapMode.PingPong:
                            //PingPong anim, reversing continue.
                            _progressTime = 0.0f;
                            _inReversePlaying = !_inReversePlaying;
                            break;
                        case WrapMode.ClampForever:
                            //ClampForever anim, keep the last frame.
                            break;
                        case WrapMode.Default:
                            //We don't know how to handle it.
                            //In most time, this means it's a Once anim.
                            //Animation should be played with wrap mode specified.
                            Debug.LogWarning("A Default Anim Finished. Animation should be played with wrap mode specified.");
                            isPlaying = false;
                            break;
                        default:
                            //Once anim, kill it.
                            isPlaying = false;
                            break;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;

            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
        {
            if (onComplete != null)
            {
                Debug.LogWarning("onComplete will not be called when you set \"ignoreTimeScale\" to true. Use Unity's animation handler instead!)");
                animation.Play(clipName);
            }
        }
    }
}

public class ResourceGC : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private readonly float gc_cd = 5.0f;

    void Start() { }

    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= gc_cd)
        {
            Resources.UnloadUnusedAssets();
            deltaTime = 0.0f;
        }
    }
}

