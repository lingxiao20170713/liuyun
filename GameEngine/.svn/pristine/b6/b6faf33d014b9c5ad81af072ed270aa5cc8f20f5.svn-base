


using System;

public class GameVersion
{
    public int main;
    public int sub;
    public int tiny;

    public void Copy(GameVersion v)
    {
        this.main = v.main;
        this.sub = v.sub;
        this.tiny = v.tiny;
    }

    public static GameVersion Create(string version)
    {
        GameVersion v = new GameVersion();
        string[] results = version.Split('.');
        if (results.Length == 3)
        { 
            v.main = int.Parse(results[0]);
            v.sub = int.Parse(results[1]);
            v.tiny = int.Parse(results[2]);
        }
        return v;
    }

    public override bool Equals(object obj)
    {
        GameVersion v1 = obj as GameVersion;
        return main == v1.main && sub == v1.sub && tiny == v1.tiny;
    }

    public static bool operator >(GameVersion v0, GameVersion v1)
    {
        if (v0.main > v1.main)
        {
            return true;
        }
        if (v0.main < v1.main)
        {
            return false;
        }
        if (v0.sub > v1.sub)
        {
            return true;
        }
        if (v0.sub < v1.sub)
        {
            return false;
        }
        return v0.tiny > v1.tiny;
    }

    public static bool operator <(GameVersion v0, GameVersion v1)
    {
        return !(v0 > v1 || v0.Equals(v1));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string[] texts = new string[] {main.ToString(),".",sub.ToString(),".",tiny.ToString() };
        return string.Concat(texts);
    }
}

