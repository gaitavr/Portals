using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] 
    private Portal _other;
    [SerializeField]
    private Collider[] _wallColliders;

    private PortalRenderer _renderer;

    private Teleporter _enteredObject;

    private void Awake()
    {
        _renderer = GetComponent<PortalRenderer>();
    }

    private void Update()
    {
        if (_enteredObject != null)
        {
            Vector3 relativePosition = transform.InverseTransformPoint(_enteredObject.transform.position);

            if (relativePosition.z > 0.0f)
            {
                _enteredObject.Teleport();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.EnterPortal(this, _other, _wallColliders);
            _enteredObject = teleporter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        teleporter.ExitPortal(_wallColliders);
        _enteredObject = null;
    }

    public void Render(Camera mainCamera)
    {
        _renderer.Render(mainCamera, _other.transform);
    }
}
