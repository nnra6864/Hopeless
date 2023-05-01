using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cam
{
    public class CameraTransitionTrigger : MonoBehaviour
    {
        [Header("Transition Time")]
        [SerializeField] private float _leftTime, _rightTime;

        [Header("Position")]
        [SerializeField] private bool _position;
        [SerializeField] private Vector3 _leftPosition, _rightPosition;
        public static Vector3 TargetPosition;

        [Header("Rotation")]
        [SerializeField] private bool _rotate;
        [SerializeField] private Vector3 _leftRotation, _rightRotation;
        public static Vector3 TargetRotation;

        [Header("Size")]
        [SerializeField] private bool _size;
        [SerializeField] private float _leftSize, _rightSize;
        public static float TargetSize;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Vector3 targetPosition, targetRotation;
            float targetSize;
            float targetTime;

            if (collision.transform.position.x < transform.position.x)
            {
                targetPosition = _rightPosition;
                targetRotation = _rightRotation;
                targetSize = _rightSize;
                targetTime = _rightTime;
            }
            else
            {
                targetPosition = _leftPosition;
                targetRotation = _leftRotation;
                targetSize = _leftSize;
                targetTime = _leftTime;
            }

            if (_position && TargetPosition != targetPosition)
                CameraManager.LerpCameraPosition(targetPosition, targetTime);
            if (_rotate && TargetRotation != targetRotation)
                CameraManager.LerpCameraRotation(targetRotation, targetSize);
            if (_size && TargetSize != targetSize)
                CameraManager.LerpCameraSize(targetSize, targetTime);
            TargetPosition = targetPosition;
            TargetRotation = targetRotation;
            TargetSize = targetSize;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Vector3 targetPosition, targetRotation;
            float targetSize;
            float targetTime;

            if (collision.transform.position.x > transform.position.x)
            {
                targetPosition = _rightPosition;
                targetRotation = _rightRotation;
                targetSize = _rightSize;
                targetTime = _rightTime;
            }
            else
            {
                targetPosition = _leftPosition;
                targetRotation = _leftRotation;
                targetSize = _leftSize;
                targetTime = _leftTime;
            }

            if (_position && TargetPosition != targetPosition)
                CameraManager.LerpCameraPosition(targetPosition, targetTime);
            if (_rotate && TargetRotation != targetRotation)
                CameraManager.LerpCameraRotation(targetRotation, targetSize);
            if (_size && TargetSize != targetSize)
                CameraManager.LerpCameraSize(targetSize, targetTime);
            TargetPosition = targetPosition;
            TargetRotation = targetRotation;
            TargetSize = targetSize;
        }
    }
}