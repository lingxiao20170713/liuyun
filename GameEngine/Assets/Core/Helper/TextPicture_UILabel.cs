using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

/**
 * 实现图文混排的功能，支持pivot左上对齐一种格式；支持label alignment的中对齐和左对齐
 * NGUIText.CalculatePrintedSize得到的尺寸与屏幕无关
 * 一个空格占9个像素
 **/
public class TextPicture_UILabel : Text 
{
    public Sprite atlas;
    [HideInInspector]
    public string srcText 
    {
        set 
        {
            if (string.IsNullOrEmpty(value))
                return;
            m_srcText = value;
            NormalizeText();
        }
        get { return m_srcText; }
    }
    private string m_srcText;
    private bool need2Normalize = false;
    private bool startExcuted = false;
    private int oneSpaceWidth = 9;
    private bool initEror = false;
    private GameObject spriteTemplate;
    private List<int> symbolIndexList = new List<int>();     //标记在m_srcText中的位置
    private List<Vector3> posList = new List<Vector3>();     //图片生成的位置,优化，看看能不能将picId保存在z
    private List<Vector2> rowInforList = new List<Vector2>();//行信息，x此行的起始ID，y此行的结束ID
    private float pictureModify = 1.2f;                     //图片高度相对于字体大小的放大倍数
    private List<Transform> spritesTrList = new List<Transform>();  //生成的图片List，可重复使用

    private string[] picName = new string[] {
											"item_1",
                                            "item_2",
                                            "item_3",
                                            "icon_hulu_1",
                                            "item_21015",
                                            "item_21005",
    };

    private static int[] replaceItemIds = new int[] {               
                                            1,              //#e000         "钻石"
                                            2,              //#e001         "金币"
                                            3,              //#e002         "仙桃"
                                            20001,          //#e003         "得分加倍"
                                            20002,          //#e004         "开局体力"
                                            20003,          //#e005         "开局技能"
    };

    private static string[] replaceItemNames = new string[] {
                                            "钻石",
                                            "金币",
                                            "体力",
                                            "得分加倍",
                                            "开局体力",
                                            "开局技能",
    };

    public static bool ReplaceItemId(int _itemId,ref string _replacedName)
    {
        if (_itemId < 0)
            return false;
        for (int i = 0; i < replaceItemIds.Length; i++)
        {
            if (replaceItemIds[i] == _itemId)
            {
                _replacedName = string.Format((i < 10 ? "#e00{0}" : (i < 100 ? "#e0{0}" : (i < 1000 ? "#e{0}" : "#e{0}"))), i);
                return true;
            }
        }
        return false;
    }

    public static string ReplaceItemName(string _itemName)
    {
        if (string.IsNullOrEmpty(_itemName))
            return _itemName;
        for (int i = 0; i < replaceItemNames.Length; i++)
        {
            if (replaceItemNames[i].Equals(_itemName))
            {
                return string.Format((i < 10 ? "#e00{0}" : (i < 100 ? "#e0{0}" : (i < 1000 ? "#e{0}" : "#e{0}"))), i);
            }
        }
        return _itemName;
    }
    protected override void Start()
    {
        base.Start();
        startExcuted = true;
        //Init();
        if (need2Normalize && !initEror)
        {
            _NormalizeText();
            ShowSpirte();
            need2Normalize = false;
        }
    }

    private void Init()
    {
        lineSpacing = pictureModify > 1 ? ((int)((float)fontSize * (pictureModify - 1)) + 3) : 3;

        if (atlas == null)
        {
            Debug.LogError("atlas == null");
            enabled = false;
            initEror = true;
            return;
        }
        if (spriteTemplate == null)
        {
            Transform spriteTemplateTr = transform.FindChild("SpriteTemplate");
            if (spriteTemplateTr == null)
            {
                spriteTemplateTr = new GameObject("SpriteTemplate").transform;
                spriteTemplateTr.parent = transform;
                spriteTemplateTr.localPosition = Vector3.zero;
                spriteTemplateTr.localRotation = Quaternion.identity;
                spriteTemplateTr.localScale = Vector3.one;
                spriteTemplateTr.gameObject.layer = transform.gameObject.layer;
                Image tempSprite = spriteTemplateTr.gameObject.AddComponent<Image>();
                if (tempSprite == null)
                {
                    enabled = false;
                    initEror = true;
                    return;
                }
                //tempSprite.depth = depth + 1;
                spriteTemplateTr.gameObject.SetActive(false);
            }
            spriteTemplate = spriteTemplateTr.gameObject;
        }
        //计算空格在当前字体大小下的占用像素
        //UpdateNGUIText();
        //Vector2 range = NGUIText.CalculatePrintedSize(" ");
        //oneSpaceWidth = (int)range.x;
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 100, 30), "Cal"))
    //    {
    //        srcText = text;
    //        //NormalizeText();
    //    }
    //    if (GUI.Button(new Rect(10, 50, 100, 30), "Cal   2 "))
    //    {
    //        string txt = text;
    //        GetInformation(ref txt);
    //    }
    //}



    public void NormalizeText()
    {
        if (!startExcuted)
        {
            need2Normalize = true;
            return;
        }
        if (initEror)
            return;
        _NormalizeText();
        ShowSpirte();
    }

    private int maxLineRow = 0;
    private void _NormalizeText()
    {
        string preText = srcText;
        //UpdateNGUIText();
        posList.Clear();
        symbolIndexList.Clear();
        rowInforList.Clear();
        string space = string.Empty;
        Vector2 preRange = Vector2.zero;
        Vector2 rangeRect = Vector2.zero;
        int lastStartIndex = 0; //始终为当前行的第一个字符下标
        int length = preText.Length;
        int row = 1;
        bool hasCalculateTheLastRow = false;//标志最后一行是否要在循环结束后处理
        for (int i = 0; i < length; i++)
        {
            if (preText[i] == '\n')    //处理换行符
            {
                Vector2 tempRowInfor = new Vector2(lastStartIndex, i - 1);//i-1小于0的情况即第一个字符为\n
                rowInforList.Add(tempRowInfor);
                row++;
                lastStartIndex = i + 1;

                if (lastStartIndex >= length) //如果下行开头已经大于length，则处理结束
                {
                    hasCalculateTheLastRow = true;
                    row--;
                    break;
                }
                //if (lastStartIndex < length)
                //{
                //    Debug.LogError("换行符，下一行第一个字符 ： " + lastStartIndex + " char = " + preText[lastStartIndex]);
                //}
                continue;
            }
            Vector2 range = CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex + 1));
            if ((int)range.y > fontSize + lineSpacing)  //此ID的字符已经显示到下一行了
            {
                //Debug.LogError("换行，下一行第一个字符 ： " + i + " char = " + preText[i]);
                
                //插入换行符
                preText = preText.Insert(i, "\n");
                length = preText.Length;
                i++;
                Vector2 tempRowInfor = new Vector2(lastStartIndex, i - 2);
                rowInforList.Add(tempRowInfor);
                lastStartIndex = i;
                row++;
                hasCalculateTheLastRow = false;
            }
            if (preText[i] == '#' && preText[i + 1] == 'e' && i + 4 < length)
            {
                string picName = preText.Substring(i + 2, 3);
                int picId = 0;
                if (int.TryParse(picName, out picId))
                {
                    preRange = CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex));

                    preText = preText.Remove(i, 5);
                    GetRightPicLength(ref space, picId);
                    preText = preText.Insert(i, space);
                    length = preText.Length;
                    rangeRect = CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex + space.Length));

                    //已经产生换行，即本行不够空间显示图片
                    if (rangeRect.y > preRange.y && i > lastStartIndex) //i > lastStartIndex排除此行第一个字符为#的情况
                    {
                        preText = preText.Insert(i, "\n");
                        length = preText.Length;
                        Vector2 tempRowInfor = new Vector2(lastStartIndex, i - 1);
                        rowInforList.Add(tempRowInfor);
                        lastStartIndex = i + 1;
                        row++;
                        hasCalculateTheLastRow = false;
                        symbolIndexList.Add(lastStartIndex);
                        Vector3 tempPos = new Vector3(0, row, picId); //new Vector3(0, rangeRect.y, picId);
                        posList.Add(tempPos);
                        i += space.Length;
                    }
                    else
                    {
                        symbolIndexList.Add(i);
                        //用行号表示高度
                        Vector3 tempPos = new Vector3(preRange.x + (((int)preRange.x == 0) ? 0 : 0), row, picId);
                        //new Vector3(preRange.x + (((int)preRange.x == 0)? 0 : uiLabel.spacingX), ((int)preRange.y == 0 ? rangeRect.y: preRange.y), picId);
                        posList.Add(tempPos);
                        i += space.Length - 1;
                    }
                }
            }
        }
        if (!hasCalculateTheLastRow)
        {
            Vector2 tempRowInfor = new Vector2(lastStartIndex, preText.Length - 1);
            rowInforList.Add(tempRowInfor);
        }
        maxLineRow = row;
        text = preText;
    }
    //private void _NormalizeText_bak()
    //{
    //    string preText = srcText;
    //    UpdateNGUIText();
    //    posList.Clear();
    //    symbolIndexList.Clear();
    //    rowInforList.Clear();
    //    string space = string.Empty;
    //    Vector2 preRange = Vector2.zero;
    //    Vector2 rangeRect = Vector2.zero;
    //    int lastStartIndex = 0; //始终为当前行的第一个字符下标
    //    int length = preText.Length;
    //    int row = 1;
    //    bool hasCalculateTheLastRow = false;//标志最后一行是否要在循环结束后处理
    //    for (int i = 0; i < length; i++)
    //    {
    //        if (preText[i] == '\n')    //处理换行符
    //        {
    //            Vector2 tempRowInfor = new Vector2(lastStartIndex, i -1);//i-1小于0的情况即第一个字符为\n
    //            rowInforList.Add(tempRowInfor);
    //            row++;
    //            lastStartIndex = i + 1;

    //            if (lastStartIndex >= length) //如果下行开头已经大于length，则处理结束
    //            {
    //                hasCalculateTheLastRow = true;
    //                row--;
    //                break;
    //            }
    //            //if (lastStartIndex < length)
    //            //{
    //            //    Debug.LogError("换行符，下一行第一个字符 ： " + lastStartIndex + " char = " + preText[lastStartIndex]);
    //            //}
    //            continue;
    //        }
    //        Vector2 range = NGUIText.CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex + 1));
    //        if ((int)range.y > fontSize + spacingY)  //此ID的字符已经显示到下一行了
    //        {
    //            //Debug.LogError("换行，下一行第一个字符 ： " + i + " char = " + preText[i]);

    //            //在上一行的末尾补齐空格,　空格个数　＝　地板值　（总长度/空格长度）  +１
    //            //特殊情况，如果label是居中对齐，则需要在两头补齐空格，目前暂不实现
    //            Vector2 rangeV2 = NGUIText.CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex));
    //            int needSpaceCount = Mathf.FloorToInt((width - rangeV2.x) / oneSpaceWidth) + 1;
    //            //Debug.LogError((width - rangeV2.x).ToString("f5"));
    //            string needSpaceString = string.Empty;
    //            GetTargetCountSpace(ref needSpaceString, needSpaceCount);
    //            preText = preText.Insert(i, needSpaceString);
    //            length = preText.Length;
    //            i += needSpaceCount;
    //            //Debug.LogError(needSpaceCount + " ; " + preText[i]);
    //            Vector2 tempRowInfor = new Vector2(lastStartIndex, i - 1);
    //            rowInforList.Add(tempRowInfor);
    //            lastStartIndex = i;
    //            row++;
    //            hasCalculateTheLastRow = false;
    //        }
    //        if (preText[i] == '#' && preText[i + 1] == 'e' && i + 4 < length)
    //        {
    //            string picName = preText.Substring(i + 2, 3);
    //            int picId = 0;
    //            if (int.TryParse(picName, out picId))
    //            {
    //                preRange = NGUIText.CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex));

    //                preText = preText.Remove(i, 5);
    //                GetRightPicLength(ref space, picId);
    //                //Debug.LogError("space.length = " + space.Length);
    //                preText = preText.Insert(i, space);
    //                length = preText.Length;
    //                rangeRect = NGUIText.CalculatePrintedSize(preText.Substring(lastStartIndex, i - lastStartIndex + space.Length));

    //                //已经产生换行，即本行不够空间显示图片
    //                if (rangeRect.y > preRange.y && i > lastStartIndex) //i > lastStartIndex排除此行第一个字符为#的情况
    //                {
    //                    int differ = width - (int)preRange.x;
    //                    int spaceCount = Mathf.FloorToInt(differ / oneSpaceWidth) + 1;
    //                    //Debug.LogError("spaceCount = " + spaceCount + " ; differ = " + differ);
    //                    Vector2 tempRowInfor = new Vector2(lastStartIndex, i + spaceCount - 1);
    //                    rowInforList.Add(tempRowInfor);
    //                    lastStartIndex = i + spaceCount;
    //                    row++;
    //                    hasCalculateTheLastRow = false;
    //                    symbolIndexList.Add(lastStartIndex);
    //                    Vector3 tempPos = new Vector3(0, row, picId); //new Vector3(0, rangeRect.y, picId);
    //                    posList.Add(tempPos);
    //                    //补齐不够的空格
    //                    string _differSpace = string.Empty;
    //                    GetTargetCountSpace(ref _differSpace, spaceCount);
    //                    preText = preText.Insert(i, _differSpace);
    //                    length = preText.Length;
    //                    i += (spaceCount + space.Length - 1);
    //                    //Debug.LogError("spaceCount = " + spaceCount + " ; space.length = " + space.Length);
    //                }
    //                else
    //                {
    //                    symbolIndexList.Add(i);
    //                    //用行号表示高度
    //                    Vector3 tempPos = new Vector3(preRange.x + (((int)preRange.x == 0) ? 0 : spacingX), row, picId);
    //                    //new Vector3(preRange.x + (((int)preRange.x == 0)? 0 : uiLabel.spacingX), ((int)preRange.y == 0 ? rangeRect.y: preRange.y), picId);
    //                    posList.Add(tempPos);
    //                    i += space.Length - 1;
    //                }
    //            }
    //        }
    //    }
    //    if (!hasCalculateTheLastRow)
    //    {
    //        Vector2 tempRowInfor = new Vector2(lastStartIndex,preText.Length -1);
    //        rowInforList.Add(tempRowInfor);
    //    }
    //    maxLineRow = row;
    //    text = preText;
    //}
    
    protected void GetInformation(ref string preText)
    {
        Vector2 textWidth = Vector2.zero;
        fontSize = font.fontSize;//设置当前使用字体大小

        //UpdateNGUIText();
        //textWidth = NGUIText.CalculatePrintedSize(preText);
        Debug.LogError("text Width = " + textWidth.ToString("f5"));
    }

    Vector2 CalculatePrintedSize(string str)
    {
        return new Vector2(0,0);
    }
    private void ShowSpirte()
    {
        if (initEror)
            return;
        if (spriteTemplate == null)
        {
            Transform spriteTemplateTr = transform.FindChild("SpriteTemplate");
            if (spriteTemplateTr == null)
            {
                Debug.LogError("spriteTemplateTr == null");
                return;
            }
            spriteTemplate = spriteTemplateTr.gameObject;
        }
        
        foreach (Transform tempTr in spritesTrList)
        {
            tempTr.gameObject.SetActive(false);
        }
        float spaceOffset = 3 * fontSize / (float)34;
        //UpdateNGUIText();
        //float maxHeight = maxLineRow * (fontSize + spacingY) * 0.5f;
        for (int i = 0; i < posList.Count; i++)
        {
            if (i > spritesTrList.Count - 1)
            {
                GameObject tempGo = Instantiate(spriteTemplate) as GameObject;
                tempGo.transform.parent = transform;
                tempGo.transform.localScale = Vector3.one;
                spritesTrList.Add(tempGo.transform);
            }
            Image tempSprite = spritesTrList[i].GetComponent<Image>();
            tempSprite.sprite = null;//picName[(int)posList[i].z];
            tempSprite.SetNativeSize();
            spritesTrList[i].gameObject.SetActive(true);
            switch (alignment)
            {
                case TextAnchor.MiddleCenter:
                    int rowNum = (int)posList[i].y;
                    //float y = maxHeight - (rowNum - 1) / (float)maxLineRow * (fontSize + spacingY);
                    float x = 1;
                    if (rowNum > rowInforList.Count)
                    {
                        Debug.LogError("TextPicture_UILabel： rowNum 大于 rowInforList.Count!");
                        return;
                    }

                    Vector2 rowLength = CalculatePrintedSize(text.Substring((int)rowInforList[rowNum - 1].x, (int)rowInforList[rowNum - 1].y - (int)rowInforList[rowNum - 1].x + 1));
                    if ((int)rowLength.x < flexibleWidth)// && (int)rowLength.y <= fontSize + spacingY)//小于最大宽度，且无换行，表示此行未被填满
                    {
                        x = posList[i].x + (flexibleWidth - rowLength.x) * 0.5f;
                    }
                    else
                    {
                        x = posList[i].x;
                    }
                    //spritesTrList[i].localPosition = new Vector3(x + tempSprite.width * 0.5f + 3, y - fontSize * 0.5f + 1f, -1f);
                    spritesTrList[i].localPosition = new Vector3(x + tempSprite.flexibleWidth * 0.5f + spaceOffset, (1 - posList[i].y) * (fontSize + lineSpacing) - fontSize * 0.5f + 1f, -1f);
                    break;
                case TextAnchor.MiddleLeft:
                    spritesTrList[i].localPosition = new Vector3(posList[i].x + tempSprite.flexibleWidth * 0.5f + spaceOffset, (1 - posList[i].y) * (fontSize + lineSpacing) - fontSize * 0.5f + 1f, -1f);
                    break;
                default:
                    Debug.Log("TextPicture_UILabel unsupport alignment model = " + alignment.ToString());
                    break;
            }
        }
    }

    private void GetRightPicLength(ref string _space, int _picId)
    {
        if (_picId > picName.Length - 1 || _picId < 0)
        {
            Debug.LogError("不存在的图集Id！_picId = " + _picId);
            return;
        }
        int _width = GetTargetPicWidth(picName[_picId]);
        int count = Mathf.CeilToInt((float)_width / (fontSize * oneSpaceWidth / 35));
        //Debug.LogError("_width = " + _width + " ; count = " + count);
        //if (count % 2 == 1)
        //    count++;
        GetTargetCountSpace(ref _space, count);
    }
    private void GetTargetCountSpace(ref string _space, int _count)
    {
        if (_count <= 0)
        {
            return;
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _count; i++)
        {
            sb.Append(" ");
        }
        _space = sb.ToString();
    }

    private int GetTargetPicWidth(string _picName)
    {
        //if (string.IsNullOrEmpty(_picName) || atlas == null)
        //{
        //    Debug.LogError("_picName == 空 或者 atlas == null");
        //    return 0;
        //}
        //Sprite _data = atlas.pivot(_picName);
        //if (_data == null)
        //{
        //    Debug.LogError("指定的名字的图片在图集中找不到，name = " + _picName);
        //    return 0;
        //}
        //int _width = (int)((float)_data.width * fontSize * pictureModify / _data.height);
        //if (_width > width)
        //{
        //    _width = width;
        //}
        //return _width;
        return 0;
    }
}
