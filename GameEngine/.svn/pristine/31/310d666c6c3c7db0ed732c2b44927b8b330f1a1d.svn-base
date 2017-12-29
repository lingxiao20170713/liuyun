using UnityEngine;
using System.Collections.Generic;
using System;

public static class CameraDef
{
    public const string Camera_Skybox = "Camera_Skybox";
    public const string Camera_Model = "Camera_Model"; // main camera
    public const string Camera_UI_Inner = "Camera_UI_Inner";
    public const string Camera_Model_Mid = "Camera_Model_Mid";
    public const string Camera_UI = "Camera_UI";
    public const string Camera_OverlayUI = "Camera_OverlayUI";

    public enum CameraDepth
    {
        Skybox = -5,
        Model = -4,
        Model2 = -3,
        UI_Inner = -2,
        Model_Mid = -1,
        UI = 0,
        OverlayUI = 1,
    }
}

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    private Dictionary<string, Camera> cameras_;

    protected override void Awake()
    {
        base.Awake();

        Init();

    }

    public Camera ModelCamera
    {
        get { return cameras_[CameraDef.Camera_Model]; }
    }

    public Camera UI_InnerCamera
    {
        get { return cameras_[CameraDef.Camera_UI_Inner]; }
    }

    public Camera Model_MidCamera
    {
        get { return cameras_[CameraDef.Camera_Model_Mid]; }
    }

    public Camera UICamera
    {
        get { return cameras_[CameraDef.Camera_UI]; }
    }

    public Camera OverlayUICamera
    {
        get { return cameras_[CameraDef.Camera_OverlayUI]; }
    }

    public void SetModelCamera(Vector3 pos, Vector3 rot, int fov, float near, float far)
    {
        SetCamera(CameraManager.Instance.ModelCamera, pos, rot, (int)CameraDef.CameraDepth.Model, near, far, false, fov);
    }

    public void SetModel_MidCamera(Vector3 pos, Vector3 rot, int size, int fov, float near, float far)
    {
        SetCamera(CameraManager.Instance.Model_MidCamera, pos, rot, (int)CameraDef.CameraDepth.Model_Mid, near, far, false, fov, size);
    }

    public void SetOverlayUICamera(Vector3 pos, Vector3 rot, int fov, float near, float far)
    {
        SetCamera(CameraManager.Instance.OverlayUICamera, pos, rot, (int)CameraDef.CameraDepth.OverlayUI, near, far);
    }

    public void SetDefaultModel_MidCamera()
    {
        SetCamera(CameraManager.Instance.Model_MidCamera, Vector3.zero, Vector3.zero, (int)CameraDef.CameraDepth.Model_Mid);
    }

    public void SetCamera(Camera cam, Vector3 pos, Vector3 rot, int depth, float near = -10, float far = 10, bool orthographic = true, int fov = 0, int size = 1)
    {
        cam.orthographic = orthographic;
        cam.transform.localPosition = pos;
        cam.transform.localRotation = Quaternion.Euler(rot);
        cam.nearClipPlane = near;
        cam.farClipPlane = far;
        cam.depth = depth;
        if (!orthographic)
            cam.fieldOfView = 4;
        else
            cam.orthographicSize = 1;
    }

    private void Init()
    {
        Camera cam = null;
        string cam_name = null;

        cameras_ = new Dictionary<string, Camera>();

        try
        {
            cam_name = CameraDef.Camera_Model;
            cam = GameObject.Find(string.Format("GameMgr/CameraMgr/{0}", cam_name)).GetComponent<Camera>();
            cam.depth = (int)CameraDef.CameraDepth.Model;
            cam.cullingMask = LayerDef.BitMask(LayerDef.Model);
            cameras_.Add(cam_name, cam);

            cam_name = CameraDef.Camera_Model_Mid;
            cam = GameObject.Find(string.Format("GameMgr/CameraMgr/{0}", cam_name)).GetComponent<Camera>();
            cam.depth = (int)CameraDef.CameraDepth.Model_Mid;
            cam.cullingMask = LayerDef.BitMask(LayerDef.Model_Mid);
            cameras_.Add(cam_name, cam);

            cam_name = CameraDef.Camera_OverlayUI;
            cam = GameObject.Find(string.Format("GameMgr/CameraMgr/{0}", cam_name)).GetComponent<Camera>();
            cam.depth = (int)CameraDef.CameraDepth.OverlayUI;
            cam.cullingMask = LayerDef.BitMask(LayerDef.OverlayUI);
            cameras_.Add(cam_name, cam);

            cam_name = CameraDef.Camera_UI_Inner;
            cam = GameObject.Find(string.Format("GUI/{0}", cam_name)).GetComponent<Camera>();
            cam.depth = (int)CameraDef.CameraDepth.UI_Inner;
            cam.cullingMask = LayerDef.BitMask(LayerDef.UI_Inner);
            cameras_.Add(cam_name, cam);

            cam_name = CameraDef.Camera_UI;
            cam = GameObject.Find(string.Format("GUI/{0}", cam_name)).GetComponent<Camera>();
            cam.depth = (int)CameraDef.CameraDepth.UI;
            cam.cullingMask = LayerDef.BitMask(LayerDef.UI);
            cameras_.Add(cam_name, cam);
        }
        catch (ApplicationException exception)
        {
            Debug.LogErrorFormat("Init camera failed - camera={0}\nexception={1}\nstacktrace={2}\n",
                cam_name, exception.Message, exception.StackTrace);
        }
    }
}

public static class LayerDef
{
    public const string Skybox = "Skybox";
    public const string Model = "Model";
    public const string UI_Inner = "UI_Inner";
    public const string Model_Mid = "Model_Mid";
    public const string UI = "UI";
    public const string OverlayUI = "OverlayUI";
    public const string Floor = "Floor";

    public static int BitMask(params string[] names)
    {
        int mask = 0;
        for (int i = 0; i < names.Length; ++i)
            mask |= (1 << LayerMask.NameToLayer(names[i]));
        return mask;
    }
}