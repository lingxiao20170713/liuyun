using System;
using UnityEngine;
using System.Collections.Generic;

public class InputManager : Singleton<InputManager>
{
    public enum DirectionTypes
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    private const float DeltaX = 30f;
    private const float DeltaY = 30f;

    private bool m_activeInput = false;
    private Vector2 m_currentPos = Vector2.zero;
    private DirectionTypes m_directionType = DirectionTypes.None;
    private Action<DirectionTypes> m_OnTouchEvent = null;

    // 调试绘制变量
    private Material m_lineMaterial = null;
    private List<Vector2> m_touchPoints = new List<Vector2>(30);

    bool touchDown
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
        }
    }

    bool touchMove
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
#endif
        }
    }

    bool touchUp
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#else
            return Input.touchCount > 0 &&
                (Input.GetTouch(0).phase == TouchPhase.Ended ||
                Input.GetTouch(0).phase == TouchPhase.Canceled);
#endif
        }
    }

    Vector2 touchPosition
    {
        get
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            if (Input.touchCount > 0)
                return Input.GetTouch(0).position;
            return Vector2.zero;
#endif
        }
    }

    public void SetTouchEvent(Action<DirectionTypes> handle)
    {
        m_OnTouchEvent = handle;
    }

    void DirectionInputHandle()
    {
        if (touchDown)
        {
            m_currentPos = touchPosition;
            m_directionType = DirectionTypes.None;
            m_activeInput = true;
            m_touchPoints.Clear();
            m_touchPoints.Add(m_currentPos);
        }
        if (touchMove && m_activeInput)
        //if (touchUp)
        {
            Vector2 position = touchPosition;
            float angle = GetAngle(m_currentPos, position);
            m_touchPoints.Add(position);

            if ((position.x - m_currentPos.x) > DeltaX)
            {
                if ((angle < 45f) && (angle > -45f))
                {
                    m_directionType = DirectionTypes.Right;
                    m_activeInput = false;
                }
                else if (angle >= 45f)
                {
                    m_directionType = DirectionTypes.Up;
                    m_activeInput = false;
                }
                else if (angle <= -45f)
                {
                    m_directionType = DirectionTypes.Down;
                    m_activeInput = false;
                }
            }
            else if ((position.x - m_currentPos.x) < -DeltaX)
            {
                if ((angle < 45f) && (angle > -45f))
                {
                    m_directionType = DirectionTypes.Left;
                    m_activeInput = false;
                }
                else if (angle >= 45f)
                {
                    m_directionType = DirectionTypes.Down;
                    m_activeInput = false;
                }
                else if (angle <= -45f)
                {
                    m_directionType = DirectionTypes.Up;
                    m_activeInput = false;
                }
            }
            else if ((position.y - m_currentPos.y) > DeltaY)
            {
                if ((position.x - m_currentPos.x) >= 0f)
                {
                    if ((angle > 45f) && (angle <= 90f))
                    {
                        m_directionType = DirectionTypes.Up;
                        m_activeInput = false;
                    }
                    else if (angle <= 45f)
                    {
                        m_directionType = DirectionTypes.Right;
                        m_activeInput = false;
                    }
                }
                else
                {
                    if ((angle < -45f) && (angle >= -89f))
                    {
                        m_directionType = DirectionTypes.Up;
                        m_activeInput = false;
                    }
                    else if (angle >= -45f)
                    {
                        m_directionType = DirectionTypes.Left;
                        m_activeInput = false;
                    }
                }
            }
            else if ((position.y - m_currentPos.y) < -DeltaY)
            {
                if ((position.x - m_currentPos.x) >= 0f)
                {
                    if ((angle < -45f) && (angle >= -90f))
                    {
                        m_directionType = DirectionTypes.Down;
                        m_activeInput = false;
                    }
                    else if (angle >= -45f)
                    {
                        m_directionType = DirectionTypes.Right;
                        m_activeInput = false;
                    }
                }
                else
                {
                    if ((angle > 45f) && (angle < 89f))
                    {
                        m_directionType = DirectionTypes.Down;
                        m_activeInput = false;
                    }
                    else if (angle <= 45f)
                    {
                        m_directionType = DirectionTypes.Left;
                        m_activeInput = false;
                    }
                }
            }
            if (m_directionType != DirectionTypes.None && m_OnTouchEvent != null)
            {
                m_OnTouchEvent(m_directionType);
            }
        }
        if (touchUp)
        {
            m_touchPoints.Add(touchPosition);
            m_currentPos = Vector3.zero;
            m_directionType = DirectionTypes.None;
            m_activeInput = false;
        }
    }

    float GetAngle(Vector2 form, Vector2 to)
    {
        Vector2 zero = Vector2.zero;
        zero.x = to.x;
        zero.y = form.y;
        float num = to.y - zero.y;
        float num2 = zero.x - form.x;
        float f = num / num2;
        return Util.ToDegree(Mathf.Atan(f));
    }

    void KeyInputHandle()
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

            if (m_directionType != DirectionTypes.None && m_OnTouchEvent != null)
            {
                m_OnTouchEvent(m_directionType);
            }
        }
        else
        {
            m_directionType = DirectionTypes.None;
        }
    }

    public void Update()
    {
        DirectionInputHandle();
        KeyInputHandle();
    }

    public void TouchDraw()
    {
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

            float r1 = Util.ToRadian(45);
            float r2 = Util.ToRadian(225);
            DrawLine(
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r1), Mathf.Sin(r1)),
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r2), Mathf.Sin(r2)),
                Color.cyan);

            r1 = Util.ToRadian(135);
            r2 = Util.ToRadian(315);
            DrawLine(
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r1), Mathf.Sin(r1)),
                m_touchPoints[0] + 100 * new Vector2(Mathf.Cos(r2), Mathf.Sin(r2)),
                Color.cyan);

            for (int i = 0; i < m_touchPoints.Count; ++i)
            {
                if (i > 0)
                    DrawLine(m_touchPoints[i - 1], m_touchPoints[i], (i < m_touchPoints.Count - 1) ? Color.red : Color.yellow);
            }

            GL.PopMatrix();
        }
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
}