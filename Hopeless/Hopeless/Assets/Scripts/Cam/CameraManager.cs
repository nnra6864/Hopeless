using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;
        public static CameraManager Instance
        {
            get => _instance;
        }

        public CinemachineVirtualCamera MainCamera;
        CinemachineTransposer _transposer;

        private void Awake()
        {
            if (Instance != null)
                Destroy(_instance);
            _instance = this;
            _transposer = MainCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        #region Position
        public static void LerpCameraPosition(Vector3 targetPosition, float time = 1)
        {
            if (Instance._lerpCameraPositionRoutine != null)
            {
                Instance.StopCoroutine(Instance._lerpCameraPositionRoutine);
                Instance._lerpCameraPositionRoutine = null;
            }
            Instance._lerpCameraPositionRoutine = _instance.StartCoroutine(_instance.LerpCameraPositionRoutine(targetPosition, time));
        }

        private Coroutine _lerpCameraPositionRoutine;
        private IEnumerator LerpCameraPositionRoutine(Vector3 targetPosition, float time)
        {
            float lerpPosition = 0;
            while (lerpPosition < 1)
            {
                lerpPosition += Time.deltaTime / time;
                lerpPosition = Mathf.Clamp01(lerpPosition);
                var t = NnUtils.EaseInOut(lerpPosition);
                _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, targetPosition, t);
                yield return null;
            }
            _lerpCameraPositionRoutine = null;
        }
        #endregion

        #region Rotation
        public static void LerpCameraRotation(Vector3 targetRotation, float time = 1)
        {
            if (Instance._lerpCameraRotationRoutine != null)
            {
                Instance.StopCoroutine(Instance._lerpCameraRotationRoutine);
                Instance._lerpCameraRotationRoutine = null;
            }
            Instance._lerpCameraRotationRoutine = _instance.StartCoroutine(_instance.LerpCameraRotationRoutine(targetRotation, time));
        }

        private Coroutine _lerpCameraRotationRoutine;
        private IEnumerator LerpCameraRotationRoutine(Vector3 targetRotation, float time)
        {
            float lerpPosition = 0;
            while (lerpPosition < 1)
            {
                lerpPosition += Time.deltaTime / time;
                lerpPosition = Mathf.Clamp01(lerpPosition);
                var t = NnUtils.EaseInOut(lerpPosition);
                MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, Quaternion.Euler(targetRotation), t);
                yield return null;
            }
            _lerpCameraRotationRoutine = null;
        }
        #endregion

        #region Size
        public static void LerpCameraSize(float targetSize, float time = 1)
        {
            if (Instance._lerpCameraSizeRoutine != null)
            {
                Instance.StopCoroutine(Instance._lerpCameraSizeRoutine);
                Instance._lerpCameraSizeRoutine = null;
            }
            Instance._lerpCameraSizeRoutine = _instance.StartCoroutine(_instance.LerpCameraSizeRoutine(targetSize, time));
        }

        private Coroutine _lerpCameraSizeRoutine;
        private IEnumerator LerpCameraSizeRoutine(float targetSize, float time)
        {
            float lerpPosition = 0;
            while (lerpPosition < 1)
            {
                lerpPosition += Time.deltaTime / time;
                lerpPosition = Mathf.Clamp01(lerpPosition);
                var t = NnUtils.EaseInOut(lerpPosition);
                MainCamera.m_Lens.OrthographicSize = Mathf.Lerp(MainCamera.m_Lens.OrthographicSize, targetSize, t);
                yield return null;
            }
            _lerpCameraSizeRoutine = null;
        }
        #endregion
    }
}