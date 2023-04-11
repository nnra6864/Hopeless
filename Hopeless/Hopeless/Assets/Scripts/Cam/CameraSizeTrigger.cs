using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CameraSizeTrigger : MonoBehaviour
    {
        public static float TargetSize;
        [SerializeField] private float _leftSize, _rightSize, _leftTime, _rightTime;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            float targetSize;
            float targetTime;

            if (collision.transform.position.x < transform.position.x)
            {
                targetSize = _rightSize;
                targetTime = _rightTime;
            }
            else
            {
                targetSize = _leftSize;
                targetTime = _leftTime;
            }

            if (TargetSize == targetSize) return;
            CameraManager.LerpCameraSize(targetSize, targetTime);
            TargetSize = targetSize;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            float startingSize = CameraManager.Instance.MainCamera.m_Lens.OrthographicSize;
            float targetSize;
            float targetTime;

            if (collision.transform.position.x > transform.position.x)
            {
                targetSize = _rightSize;
                targetTime = _rightTime;
            }
            else
            {
                targetSize = _leftSize;
                targetTime = _leftTime;
            }

            if (TargetSize == targetSize) return;
            CameraManager.LerpCameraSize(targetSize, targetTime);
            TargetSize = targetSize;
        }
    }
}