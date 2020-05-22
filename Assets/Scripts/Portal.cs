using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] 
    private Color _color;
    [SerializeField] 
    private Renderer _outline;
    [SerializeField] 
    private Portal _other;

    private Material _material;
    private Renderer _renderer;

    private RenderTexture _rTexture;

    private const int RENDER_ITERATION = 5;

    private Teleporter _currentObj;
    [SerializeField]
    private Collider[] _wallColliders;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _rTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        _material.mainTexture = _rTexture;
        _outline.material.SetColor("_MainColor", _color);
    }

    public void Render(Camera mainCamera, Camera portalCamera)
    {
        if (!_renderer.isVisible)
        {
            return;
        }
        portalCamera.targetTexture = _rTexture;
        for (int i = RENDER_ITERATION - 1; i >= 0; --i)
        {
            RenderInternal(mainCamera, portalCamera, i);
        }
    }

    private void RenderInternal(Camera mainCamera, Camera portalCamera, 
        int iteration)
    {
        Transform enterPoint = transform;
        Transform exitPoint = _other.transform;

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

    private void Update()
    {
        if (_currentObj != null)
        {
            Vector3 objPos = transform.InverseTransformPoint(_currentObj.transform.position);

            if (objPos.z > 0.0f)
            {
                _currentObj.Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.EnterPortal(this, _other, _wallColliders);
            _currentObj = teleporter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        teleporter.ExitPortal(_wallColliders);
        _currentObj = null;
    }
}
