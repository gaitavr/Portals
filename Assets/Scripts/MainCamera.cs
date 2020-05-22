using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] 
    private Portal[] _portals;
    [SerializeField]
    private Camera _portalCamera;

    private Camera _myCamera;

    private void Awake()
    {
        _myCamera = GetComponent<Camera>();
    }

    private void OnPreRender()
    {
        for (int i = 0; i < _portals.Length; i++)
        {
            _portals[i].Render(_myCamera, _portalCamera);
        }
    }
}
