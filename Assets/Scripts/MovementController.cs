using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7.5f;
    [SerializeField]
    private float _rotationSpeed = 3.0f;

    private Vector3 _movingVector;
    private Quaternion _targetRotation;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        var rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        var targetEuler = _targetRotation.eulerAngles + rotation * _rotationSpeed;

        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        _targetRotation = Quaternion.Euler(targetEuler);
        transform.rotation = _targetRotation;//Quaternion.Slerp(transform.rotation, _targetRotation,
            //Time.deltaTime * 15.0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _movingVector = new Vector3(x, 0.0f, z) * _moveSpeed;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.TransformDirection(_movingVector);
    }
}