using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] 
    private Color _color;
    [SerializeField] 
    private Renderer _outline;

    private Material _material;
    private Renderer _renderer;

    private RenderTexture _rTexture;

    private const int RENDER_ITERATION = 5;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _rTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        _material.mainTexture = _rTexture;
        _outline.material.SetColor("_MainColor", _color);
    }

    public void Render(Portal other, Camera mainCamera, Camera portalCamera)
    {
        if (!_renderer.isVisible)
        {
            return;
        }
        portalCamera.targetTexture = _rTexture;
        for (int i = RENDER_ITERATION - 1; i >= 0; --i)
        {
            RenderInternal(other, mainCamera, portalCamera, i);
        }
    }

    private void RenderInternal(Portal destination, Camera mainCamera, Camera portalCamera, 
        int iteration)
    {
        Transform enterPoint = transform;
        Transform exitPoint = destination.transform;

        Transform portalCamTransform = portalCamera.transform;
        portalCamTransform.position = mainCamera.transform.position;
        portalCamTransform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i <= iteration; ++i)
        {
            Vector3 relativePos = enterPoint.InverseTransformPoint(portalCamTransform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            portalCamTransform.position = exitPoint.TransformPoint(relativePos);

            Quaternion relativeRot = Quaternion.Inverse(enterPoint.rotation) * portalCamTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            portalCamTransform.rotation = exitPoint.rotation * relativeRot;
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
