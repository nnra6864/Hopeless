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

        private void Awake()
        {
            if (Instance != null)
                Destroy(_instance);
            _instance = this;
        }

        public static void LerpCameraSize(float targetSize, float time = 1)
        {
            if (Instance._lerpCameraRoutine != null)
            {
                Instance.StopCoroutine(Instance._lerpCameraRoutine);
                Instance._lerpCameraRoutine = null;
            }
            Instance._lerpCameraRoutine = _instance.StartCoroutine(_instance.LerpCameraSizeRoutine(targetSize, time));
        }

        private Coroutine _lerpCameraRoutine;
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
            _lerpCameraRoutine = null;
        }
    }
}