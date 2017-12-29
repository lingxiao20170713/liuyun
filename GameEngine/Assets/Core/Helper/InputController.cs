#define DEBUG_DRAW_ENABLE

using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchData
{
    public float Duration;
    public Vector2 End;
    public float EndTime;
    public bool IsTouching;
    public Vector2 LastPos;
    public Vector2 Start;
    public float StartTime;
    public int TapCount;
    public int TouchId = -1;
    public TouchType Type;
    public bool WasProcessed;

    public TouchData(int id)
    {
        TouchId = id;
    }

    public bool HasType()
    {
        return (Type != TouchType.None);
    }

    public void Reset()
    {
        Start = Vector2.zero;
        End = Vector2.zero;
        LastPos = Vector2.zero;
        StartTime = 0f;
        EndTime = 0f;
        Duration = 0f;
        IsTouching = false;
        WasProcessed = false;
        TapCount = 0;
        Type = TouchType.None;
    }

    public enum TouchType
    {
        None,
        Tap,//轻拍
        Swipe//猛击
    }
}


public class InputController : MonoBehaviour
{
    public enum DirectionTypes
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    protected OnTapEvent DoubleTapEvents;
    private int MaxMultiTap = 2;
    private float MaxTapTime = 0.25f;
    private float MaxTimeBetweenMultiTap = 0.2f;
    private int MaxTouches = 5;
    public float halfDownSector = 45f;
    public float halfUpSector = 35f;
    public float swipDist = 10f;
    protected OnSwipeEvent SwipeEvents;
    protected OnTapEvent TapEvents;
    private List<TouchData> Touches = new List<TouchData>();
    private List<OnTouchEvent> TouchEvents = new List<OnTouchEvent>();
    private bool TouchInput;
    protected bool WasInitialized;

    public static InputController Instance;

    // 调试绘制变量
#if DEBUG_DRAW_ENABLE
    private Material m_lineMaterial = null;
    private List<Vector2> m_touchPoints = new List<Vector2>(30);
#endif

    protected virtual void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();


        InputController.Instance.EnableTouchInput(true);
    }

    protected void CheckTouches()
    {
        if (TouchInput)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        TouchBegan(touch.fingerId, touch.position);
                        break;

                    case TouchPhase.Moved:
                        TouchMoved(touch.fingerId, touch.position);
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        TouchEnded(touch.fingerId, touch.position);
                        break;
                }
            }
        }
    }

#if UNITY_EDITOR
    private bool m_activeInput;
    private DirectionTypes m_directionType;
    protected void CheckKeys()
    {
        if (Input.anyKeyDown)
        {
            m_activeInput = true;
        }

        if (m_activeInput)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_directionType = DirectionTypes.Left;
                m_activeInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_directionType = DirectionTypes.Right;
                m_activeInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_directionType = DirectionTypes.Up;
                m_activeInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_directionType = DirectionTypes.Down;
                m_activeInput = false;
            }

            if (m_directionType != DirectionTypes.None && SwipeEvents != null)
            {
                SwipeEvents(m_directionType);
            }
        }
        else
        {
            m_directionType = DirectionTypes.None;
        }
    }
#endif

    public virtual void ClearInput()
    {
        for (int i = 0; i < MaxTouches; i++)
        {
            Touches[i].Reset();
        }
    }

    public virtual void ClearRegisteredInputs()
    {
        DoubleTapEvents = null;
        TapEvents = null;
        SwipeEvents = null;
    }

    public void EnableTouchInput(bool acceptTouch)
    {
        TouchInput = acceptTouch;
    }

    public int GetMaxTouches()
    {
        return MaxTouches;
    }

    public TouchData GetTouchData(int touchId)
    {
        for (int i = 0; i < MaxTouches; i++)
        {
            TouchData data = Touches[touchId];
            if ((data != null) && (data.TouchId == touchId))
            {
                return data;
            }
        }
        return null;
    }

    public virtual void Initialize()
    {
        InitializeTouchData();
    }

    protected void InitializeTouchData()
    {
        for (int i = 0; i < MaxTouches; i++)
        {
            Touches.Add(new TouchData(i));
        }
    }

    private void OnDisable()
    {
        ClearInput();
    }

    private Process ProcessTouchEvent(int touchID, Vector2 screenPos, TouchPhase phase)
    {
        Process process = Process.Continue;
        for (int i = 0; (i < TouchEvents.Count) && (process == Process.Continue); i++)
        {
            process = TouchEvents[i](touchID, screenPos, phase);
            switch (process)
            {
                case Process.Stop:
                    return process;

                case Process.Skip:
                    return process;
            }
        }
        return process;
    }

    public void RegisterSwipe(OnSwipeEvent callback)
    {
        SwipeEvents = (OnSwipeEvent)Delegate.Combine(SwipeEvents, callback);
    }

    public void RegisterTap(OnTapEvent callback)
    {
        TapEvents = (OnTapEvent)Delegate.Combine(TapEvents, callback);
    }

    public bool RegisterTouchEvent(OnTouchEvent newTouchEvent)
    {
        if (!TouchEvents.Contains(newTouchEvent))
        {
            TouchEvents.Add(newTouchEvent);
            return true;
        }
        return false;
    }

    public bool RemoveTouchEvent(OnTouchEvent touchEvent)
    {
        return TouchEvents.Remove(touchEvent);
    }

    private void SwipeDetected(TouchData touch)
    {
        if (SwipeEvents != null)
        {
            InputController.DirectionTypes direction = InputController.DirectionTypes.None;
            Vector2 vector = touch.End;
            vector -= touch.Start;
            vector.Normalize();
            float num = (Mathf.Atan2(1f, 0f) - Mathf.Atan2(vector.y, vector.x)) * 57.29578f;
            if (num < 0f)
            {
                num += 360f;
            }
            if ((num > (360f - this.halfUpSector)) || (num <= this.halfUpSector))
            {
                direction = DirectionTypes.Up;
            }
            else if ((num > (180f - this.halfDownSector)) && (num <= (180f + this.halfDownSector)))
            {
                direction = DirectionTypes.Down;
            }
            else if ((num > this.halfUpSector) && (num <= (180f - this.halfDownSector)))
            {
                direction = DirectionTypes.Right;
            }
            else if ((num > (180f + this.halfDownSector)) && (num <= (360f - this.halfUpSector)))
            {
                direction = DirectionTypes.Left;
            }

            SwipeEvents(direction);
        }
    }

    private void TapDetected(TouchData touch)
    {
        switch (touch.TapCount)
        {
            case 1:
                if (TapEvents != null)
                {
                    TapEvents(touch.Start);
                }
                break;

            case 2:
                if (DoubleTapEvents != null)
                {
                    DoubleTapEvents(touch.Start);
                }
                break;
        }
        if (touch.TapCount >= MaxMultiTap)
        {
            touch.TapCount = 0;
        }
    }

    private void TouchBegan(int id, Vector2 touchPoint)
    {
        if (id < MaxTouches)
        {
            TouchData data = Touches[id];
            if (!data.IsTouching)
            {
                data.IsTouching = true;
                data.Type = TouchData.TouchType.None;
                data.Start = touchPoint;
                data.StartTime = Time.time;
                data.Duration = 0f;
                data.WasProcessed = false;
                if ((data.StartTime - data.EndTime) > MaxTimeBetweenMultiTap)
                {
                    data.TapCount = 0;
                }
                if (ProcessTouchEvent(id, touchPoint, TouchPhase.Began) == Process.Stop)
                {
                    TouchProcessed(id);
                }
#if DEBUG_DRAW_ENABLE
                m_touchPoints.Clear();
                m_touchPoints.Add(touchPoint);
#endif
            }
        }
    }

    private void TouchEnded(int id, Vector2 touchPoint)
    {
        if (id < MaxTouches)
        {
            TouchData touch = Touches[id];
            if (touch.IsTouching)
            {
                touch.IsTouching = false;
                touch.End = touchPoint;
                touch.EndTime = Time.time;
                Process process = ProcessTouchEvent(id, touchPoint, TouchPhase.Ended);
                if (process == Process.Stop)
                {
                    TouchProcessed(id);
                }
                if ((!touch.WasProcessed && (process == Process.Continue)) && (!touch.HasType() && (touch.Duration < MaxTapTime)))
                {
                    touch.TapCount++;
                    TapDetected(touch);//点击次数判定处理
                }
                else
                {
                    touch.TapCount = 0;
                }
            }
        }
    }

    private void TouchMoved(int id, Vector2 touchPoint)
    {
        if (id < MaxTouches)
        {
            TouchData touch = Touches[id];
            if (touch.IsTouching && !touch.WasProcessed)
            {
                touch.LastPos = touch.End;
                touch.End = touchPoint;
                touch.EndTime = Time.time;
                touch.Duration += Time.deltaTime;
                Process process = ProcessTouchEvent(id, touchPoint, TouchPhase.Moved);
                if (process == Process.Stop)
                {
                    TouchProcessed(id);
                }
                if (!touch.WasProcessed && (process == Process.Continue))
                {
                    float num2 = 1f;
                    if (Screen.dpi != 0f)//每英寸分辨率点数
                    {
                        num2 = Screen.dpi / 160f;
                    }
                    float num = 10f * num2;

                    float num3 = Vector2.Distance(touch.End, touch.Start);
                    if (!touch.HasType() && (num3 >= num))
                    {
                        touch.Type = TouchData.TouchType.Swipe;
                        SwipeDetected(touch);
#if DEBUG_DRAW_ENABLE
                        m_touchPoints.Add(touchPoint);
#endif
                    }
                }
            }
        }
    }

    private void TouchProcessed(int id)
    {
        if (id < MaxTouches)
        {
            TouchData data = Touches[id];
            data.WasProcessed = true;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        CheckKeys();
#endif
        CheckTouches();
    }

    public void TouchDraw()
    {
#if DEBUG_DRAW_ENABLE
        if (m_lineMaterial == null)
        {
            m_lineMaterial = new Material(
                "Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass {" +
                "   BindChannels { Bind \"Color\",color }" +
                "   Blend SrcAlpha OneMinusSrcAlpha" +
                "   ZWrite Off Cull Off Fog { Mode Off }" +
                "} } }");
            m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            m_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        if (m_touchPoints.Count > 0)
        {
            m_lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();

            DrawCircle(m_touchPoints[0], 100, Color.cyan);

            //float r1 = Utils.ToRadian(45);
            //float r2 = Utils.ToRadian(225);
            //DrawLine(
            //    m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r1), Mathf.Sin(r1)),
            //    m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r2), Mathf.Sin(r2)),
            //    Color.cyan);

            //r1 = Utils.ToRadian(135);
            //r2 = Utils.ToRadian(315);
            //DrawLine(
            //    m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r1), Mathf.Sin(r1)),
            //    m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r2), Mathf.Sin(r2)),
            //    Color.cyan);

            float r = Util.ToRadian(90f - this.halfUpSector);
            DrawLine(
                m_touchPoints[0],
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r), Mathf.Sin(r)),
                Color.cyan);

            r = Util.ToRadian(90f + this.halfUpSector);
            DrawLine(
                m_touchPoints[0],
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r), Mathf.Sin(r)),
                Color.cyan);

            r = Util.ToRadian(270f - this.halfDownSector);
            DrawLine(
                m_touchPoints[0],
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r), Mathf.Sin(r)),
                Color.cyan);

            r = Util.ToRadian(270f + this.halfDownSector);
            DrawLine(
                m_touchPoints[0],
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r), Mathf.Sin(r)),
                Color.cyan);

            for (int i = 0; i < m_touchPoints.Count; ++i)
            {
                if (i > 0)
                    DrawLine(m_touchPoints[i - 1], m_touchPoints[i], Color.red);
            }

            GL.PopMatrix();
        }
#endif
    }

    void DrawCircle(Vector2 center, float radius, Color color)
    {
        GL.Begin(GL.LINES);
        GL.Color(color);

        Vector3 first = Vector3.zero;
        Vector3 start = Vector3.zero;
        for (int i = 0; i < 360; ++i)
        {
            float r = Util.ToRadian(i);
            float x = center.x + radius * Mathf.Cos(r);
            float y = center.y + radius * Mathf.Sin(r);
            Vector3 end = new Vector3(x / Screen.width, y / Screen.height, 0);
            if (i > 0)
            {
                GL.Vertex(start);
                GL.Vertex(end);
            }
            else
                first = end;
            start = end;
        }

        GL.Vertex(first);
        GL.Vertex(start);
        GL.End();
    }

    void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        GL.Begin(GL.LINES);
        GL.Color(color);
        GL.Vertex(new Vector3(start.x / Screen.width, start.y / Screen.height, 0));
        GL.Vertex(new Vector3(end.x / Screen.width, end.y / Screen.height, 0));
        GL.End();
    }

    public delegate void OnDoubleTapEvent(Vector2 tapPoint);

    public delegate void OnKey(KeyCode key);

    public delegate void OnKeyDown(KeyCode key);

    public delegate void OnKeyUp(KeyCode key);

    public delegate void OnSwipeEvent(DirectionTypes direction);

    public delegate void OnTapEvent(Vector2 tapPoint);

    public delegate InputController.Process OnTouchEvent(int touchID, Vector2 screenPos, TouchPhase phase);

    public enum Process
    {
        Stop,
        Continue,
        Skip
    }
}
