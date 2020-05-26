﻿using UnityEngine;

public class PortalRenderer : MonoBehaviour
{
    [SerializeField] private Color _outlineColor;
    [SerializeField] private Renderer _outline;
    [SerializeField] private Camera _portalCamera;

    private Material _material;
    private Renderer _renderer;
    private RenderTexture _rTexture;

    private const int RENDER_ITERATIONS = 3;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _rTexture = new RenderTexture(Screen.width, Screen.height, 0);
        _material.mainTexture = _rTexture;
        _portalCamera.targetTexture = _rTexture;
        _outline.material.color = _outlineColor;
    }

    public void Render(Camera mainCamera, Transform otherPortal)
    {
        if (!_renderer.isVisible)
        {
            return;
        }

        for (int i = RENDER_ITERATIONS - 1; i >= 0; i--)
        {
            RenderInternal(mainCamera, otherPortal, i);
        }
    }
    private void RenderInternal(Camera mainCamera, Transform otherPortal, int iteration)
    {
        Transform enterPoint = transform;
        Transform exitPoint = otherPortal;

        Transform portalCamTransform = _portalCamera.transform;
        portalCamTransform.position = mainCamera.transform.position;
        portalCamTransform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i <= iteration; i++)
        {
            portalCamTransform.MirrorPosition(enterPoint, exitPoint);
            portalCamTransform.MirrorRotation(enterPoint, exitPoint);
        }

        SetupProjection(mainCamera, exitPoint);

        _portalCamera.Render();
    }

    private void SetupProjection(Camera mainCamera, Transform exitPoint)
    {
        Plane p = new Plane(-exitPoint.forward, exitPoint.position);
        Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(
                                           Matrix4x4.Inverse(_portalCamera.worldToCameraMatrix)) * clipPlane;
        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        _portalCamera.projectionMatrix = newMatrix;
    }
}