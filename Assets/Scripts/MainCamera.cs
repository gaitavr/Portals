using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Portal _portal1;
    [SerializeField] private Portal _portal2;

    
    [SerializeField]
    private Camera _portalCamera;

    private Camera _myCamera;

    private void Awake()
    {
        _myCamera = GetComponent<Camera>();
    }

    private void OnPreRender()
    {
        _portal1.Render(_portal2, _myCamera, _portalCamera);
        _portal2.Render(_portal1, _myCamera, _portalCamera);
    }
}
