using UnityEngine;

public class PortalRenderer : MonoBehaviour
{
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Renderer _outline;

    private Material _material;
    private Renderer _renderer;

    private RenderTexture _rTexture;

    private const int RENDER_ITERATION = 3;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _rTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        _material.mainTexture = _rTexture;
        _outline.material.SetColor("_MainColor", _color);
    }

    public void Render(Camera mainCamera, Camera portalCamera, Transform otherPortal)
    {
        if (!_renderer.isVisible)
        {
            return;
        }
        portalCamera.targetTexture = _rTexture;
        for (int i = RENDER_ITERATION - 1; i >= 0; --i)
        {
            RenderInternal(mainCamera, portalCamera, i, otherPortal);
        }
    }

    private void RenderInternal(Camera mainCamera, Camera portalCamera,
        int iteration, Transform otherPortal)
    {
        Transform enterPoint = transform;
        Transform exitPoint = otherPortal.transform;

        Transform portalCamTransform = portalCamera.transform;
        portalCamTransform.position = mainCamera.transform.position;
        portalCamTransform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i <= iteration; ++i)
        {
            portalCamTransform.MirrorPosition(enterPoint, exitPoint);
            portalCamTransform.MirrorRotation(enterPoint, exitPoint);
        }

        SetupProjection(mainCamera, portalCamera, exitPoint);

        portalCamera.Render();
    }

    private void SetupProjection(Camera mainCamera, Camera portalCamera, Transform exitPoint)
    {
        Plane p = new Plane(-exitPoint.forward, exitPoint.position);
        Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;

        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;
    }
}