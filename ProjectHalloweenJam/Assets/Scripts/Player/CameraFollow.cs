using System;
using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField, Range(2f, 100f)] private float _followSensitivity = 3f;
        [SerializeField] private Transform _playerTransform;
        
        private Camera _camera;
        private Rect _screenRect;
        private Vector3 _targetPosition;

        private void Awake()
        {
            _camera = Camera.main;
            _screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
        }

        private void OnValidate()
        {
            transform.position = _playerTransform.position;
        }

        private void LateUpdate()
        {
            if (!_screenRect.Contains(Input.mousePosition))
                return;

            _targetPosition =  _camera.ScreenToWorldPoint(Input.mousePosition);
            var followObjectPosition = (_targetPosition + (_followSensitivity - 1) * _playerTransform.position) / _followSensitivity;

            transform.position = followObjectPosition;
        }
    }
}