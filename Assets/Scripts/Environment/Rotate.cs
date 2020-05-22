using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private Transform _cachedTr;

    private void Awake()
    {
        _cachedTr = transform;
    }

    private void Update()
    {
        _cachedTr.Rotate(Vector3.up, Time.deltaTime * _rotationSpeed);
    }
}
