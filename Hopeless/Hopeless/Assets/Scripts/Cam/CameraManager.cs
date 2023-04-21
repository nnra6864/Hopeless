using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cam
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;
        public static CameraManager Instance => _instance;

        public CinemachineVirtualCamera MainCamera;
        CinemachineTransposer _transposer;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
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
            if (time == 0)
            {
                Instance._transposer.m_FollowOffset = targetPosition;
                return;
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
            if (time == 0)
            {
                Instance.MainCamera.transform.rotation = Quaternion.Euler(targetRotation);
                return;
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
            if (time == 0)
            {
                Instance.MainCamera.m_Lens.OrthographicSize = targetSize;
                return;
            }
            Instance._lerpCameraSizeRoutine = _instance.StartCoroutine(_instance.LerpCameraSizeRoutine(targetSize, time));
        }

        private Coroutine _lerpCameraSizeRoutine;
        private IEnumerator LerpCameraSizeRoutine(float targetSize, float time)
        {
            float lerpPosition = 0;
            float startingSize = MainCamera.m_Lens.OrthographicSize;
            while (lerpPosition < 1)
            {
                lerpPosition += Time.deltaTime / time;
                lerpPosition = Mathf.Clamp01(lerpPosition);
                var t = NnUtils.EaseInOut(lerpPosition);
                MainCamera.m_Lens.OrthographicSize = Mathf.Lerp(startingSize, targetSize, t);
                yield return null;
            }
            _lerpCameraSizeRoutine = null;
        }
        #endregion
    }
}