using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public static config_world world;
    public static Dictionary<int, ConfigBuffData> _buffDataDict;
    public static Dictionary<int, ConfigSkillData> _skillDataDict;
    public static Dictionary<int, ConfigTipsData> _tipsDataDict;

    public static void Init()
    {
        Util.StartCoroutine(LoadConfigData());
    }

    static IEnumerator LoadConfigData()
    {
        //yield return new WaitForSeconds(1);
        LoadWorldData();
        //yield return new WaitForSeconds(1);
        LoadBuffData();
        //yield return new WaitForSeconds(1);
        LoadSkillData();
       // yield return new WaitForSeconds(1);
        yield return null;
    }

    static object Up(float progress)
    {
        LoadingPanel.Instance._progressValue = (int)(progress * 100);
        return progress;
    }

    private static void LoadWorldData()
    {
        if(world == null)
        {
            Debug.Log("加载世界数据");
            world = new config_world();
            TextAsset txt = Resources.Load<TextAsset>("config_world");
            world = fastJSON.JSON.ToObject<config_world>(txt.text);
        }
    }

    private static void LoadBuffData()
    {
        if (_buffDataDict == null)
        {
            ConfigBuffData[] buffDatas = ConfigManager.Instance.GetAllConfig<ConfigBuffData>();
            if (buffDatas != null && buffDatas.Length > 0)
            {
                Debug.Log("加载BUFF数据");
                _buffDataDict = new Dictionary<int, ConfigBuffData>();
                for (int i = 0; i < buffDatas.Length; ++i)
                {
                    if (_buffDataDict.ContainsKey(buffDatas[i].id))
                        _buffDataDict[buffDatas[i].id] = buffDatas[i];
                    else
                        _buffDataDict.Add(buffDatas[i].id, buffDatas[i]);
                }
            }
            else
                Debug.Log("BUFF信息配置表数据为null。");
        }
    }

    private static void LoadSkillData()
    {
        if (_skillDataDict == null)
        {
            ConfigSkillData[] skillDatas = ConfigManager.Instance.GetAllConfig<ConfigSkillData>();
            if (skillDatas != null && skillDatas.Length > 0)
            {
                Debug.Log("加载技能表数据");
                _skillDataDict = new Dictionary<int, ConfigSkillData>();
                for (int i = 0; i < skillDatas.Length; ++i)
                {
                    if (_skillDataDict.ContainsKey(skillDatas[i].id))
                        _skillDataDict[skillDatas[i].id] = skillDatas[i];
                    else
                        _skillDataDict.Add(skillDatas[i].id, skillDatas[i]);
                }
            }
            else
                Debug.Log("技能信息配置表数据为null。");
        }
    }

    private static void LoadTipsData()
    {
        if (_tipsDataDict == null)
        {
            ConfigTipsData[] tipsDatas = ConfigManager.Instance.GetAllConfig<ConfigTipsData>();
            if (tipsDatas != null && tipsDatas.Length > 0)
            {
                Debug.Log("加载提示数据");
                _tipsDataDict = new Dictionary<int, ConfigTipsData>();
                for (int i = 0; i < tipsDatas.Length; ++i)
                {
                    if (_tipsDataDict.ContainsKey(tipsDatas[i].id))
                        _tipsDataDict[tipsDatas[i].id] = tipsDatas[i];
                    else
                        _tipsDataDict.Add(tipsDatas[i].id, tipsDatas[i]);
                }
            }
            else
                Debug.Log("提示表为null。");
        }
    }
}

//密封类不能被继承
public sealed class Point
{
    public int x;
    public int y;
    public int z;
}

public class config_world
{
    public bool compress_network_data = false;

    public int[] id_card_map = new int[] { };

    public int ios_transaction_time = 0;

    public float[] client_auto_play_rate = new float[] { 1.0f, 1.5f, 2.0f };

    public int debug_player_gm_level = 0;
    public int debug_bot_count = 0;
    public bool debug_pay_test = false;
    public int[] level_chapter_open_level = new int[] { };

    public string client_sys_title = "二维码";
    public string client_sys_icon = "erweima";

    public float client_battler_action_height = 1.2f;
    public float client_move_click_valid_pressed_time = 0.5f;
    public bool client_skill_hide_scene = true;
    public bool client_battle_aways_hp = true;
    public bool client_battle_aways_en = false;
    public bool client_battle_aways_rage = false;
    public bool client_battle_show_quality_color = false;
    public float client_battle_action_bar_delay_hide_time = 0.8f;
    public bool client_battle_new_hand_system = true;
    public bool client_battle_new_hand_system_follow_camera = false;

    public bool client_battle_new_auto_system = true;

    public string client_battle_aoe_start_fx = "FX_guangxian";

    public string qqAppId = "100703379";
    public string qqAppKey = "4578e54fb3a1bd18e0681bc1c734514e";
    public string wxAppId = "wxcde873f99466f74a";
    public string wxAppKey = "bc0994f30c0a12a9908e353cf05d4dea";

    public string arena_static_map;
    public string arena_dynamic_map;
    public string arena_bgs = "";

    public string url_new_game_package = "http://www.yingyuegame.com";

    public config_world()
    {
        arena_static_map = "level1_1";
        arena_dynamic_map = "map_1_1";
    }
}

public class ConfigBuffData : ConfigBase
{
    public int id;
    public string name;
    public int type;
    public float duration; // 持续时间
    public float value; // 值
    public int overlap; // 叠加类型
    public int mutex; // 互斥标记
    public int flags; // 标记
    public int[] end_buffs; // 结束后的附加buff列表
    public string desc; // 说明
    public string icon; // 图标
    public int effect_pos; // 特效位置
    public string effect; // 特效名
    public string camera_effect; // 相机特效名
    public string snd; // buff状态音效
    public string voice;
}

public class ConfigSkillData : ConfigBase
{
    public int id;
    public string name;
    public int type;
    public int passive_type; // 被动类型
    public int job; // 职业类型
    public int open_lv; // 开放等级
    public float duration; // 持续时间
    public float cooldown; // 冷却时间
    public int flags; // 标记
    public int[] buffs; // buff列表
    public int buff_flags; // buff使用规则
    public string desc; // 说明
    public string icon; // 图标
    public int ui_pos; // 图标在界面上的位置
    public string snd; // 施放音效
    public string effect; // 特效文件
}

public class ConfigTipsData : ConfigBase
{
    public int id;
    public int[] type;
    public string caption;
    public string content;
    public int num;
}