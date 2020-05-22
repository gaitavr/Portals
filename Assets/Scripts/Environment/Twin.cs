using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twin : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _delta;

    private Transform _cachedTr;

    private bool _moveUp = true;

    private float _bottomPosition;
    private float _topPosition;
    private Vector3 _targetPos;

    private void Awake()
    {
        _cachedTr = transform;
        _bottomPosition = _cachedTr.position.y;
        _topPosition = _bottomPosition + _delta;
        _targetPos = _cachedTr.position;
    }

    private void Update()
    {
        if (_moveUp)
        {
            _targetPos.y += Time.deltaTime * _moveSpeed;
            if (_targetPos.y > _topPosition)
            {
                _moveUp = false;
            }
        }
        else
        {
            _targetPos.y -= Time.deltaTime * _moveSpeed;
            if (_targetPos.y < _bottomPosition)
            {
                _moveUp = true;
            }
        }

        _cachedTr.position = _targetPos;
    }
}
