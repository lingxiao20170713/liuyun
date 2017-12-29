using UnityEngine;
using System.Collections;

public class Bezier
{
    private Vector3[] m_points = new Vector3[4];
    private float[] m_constants = new float[9];

    public Bezier()
    {

    }

    // 三次贝塞尔曲线
    public Bezier(Vector3 A, Vector3 C1, Vector3 C2, Vector3 B)
    {
        SetBezier(A, C1, C2, B);
    }

    void CalcConstants()
    {
        m_constants[2] = 3 * (m_points[1].x - m_points[0].x);
        m_constants[1] = 3 * (m_points[2].x - m_points[1].x) - m_constants[2];
        m_constants[0] = m_points[3].x - m_points[0].x - m_constants[2] - m_constants[1];

        m_constants[5] = 3 * (m_points[1].y - m_points[0].y);
        m_constants[4] = 3 * (m_points[2].y - m_points[1].y) - m_constants[5];
        m_constants[3] = m_points[3].y - m_points[0].y - m_constants[5] - m_constants[4];

        m_constants[8] = 3 * (m_points[1].z - m_points[0].z);
        m_constants[7] = 3 * (m_points[2].z - m_points[1].z) - m_constants[8];
        m_constants[6] = m_points[3].z - m_points[0].z - m_constants[8] - m_constants[7];
    }

    float GetU(int u, float t)
    {
        int ofs = u * 3;
        float a = m_constants[ofs + 0];
        float b = m_constants[ofs + 1];
        float c = m_constants[ofs + 2];
        float u0 = m_points[0][u];
        float t2 = t * t;
        float t3 = t * t2;
        return a * t3 + b * t2 + c * t + u0;
    }

    public Vector3 GetPoint(float t)
    {
        return new Vector3(GetU(0, t), GetU(1, t), GetU(2, t));
    }

    public void SetBezier(Vector3 A, Vector3 C1, Vector3 C2, Vector3 B)
    {
        m_points[0] = A;
        m_points[1] = C1;
        m_points[2] = C2;
        m_points[3] = B;
        CalcConstants();
    }
}
