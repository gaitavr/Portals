using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _other;
    [SerializeField] private Collider[] _wallColliders;
    [SerializeField] private PortalRenderer _renderer;

    private Teleporter _enteredObject;

    public void Render(Camera mainCamera)
    {
        _renderer.Render(mainCamera, _other.transform);
    }

    private void OnTriggerEnter(Collider col)
    {
        var teleporter = col.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            _enteredObject = teleporter;
            teleporter.EnterPortal(this, _other, _wallColliders);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        var teleporter = col.GetComponent<Teleporter>();
        teleporter?.ExitPortal(_wallColliders);
        _enteredObject = null;
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
}
