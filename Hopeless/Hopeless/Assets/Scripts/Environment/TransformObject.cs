using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment
{

    public class TransformObject : MonoBehaviour
    {
        public void Move(float x, float y, float z, float t)
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(MoveRoutine(new Vector3(x, y, z), t));
        }

        Coroutine _moveRoutine;
        IEnumerator MoveRoutine(Vector3 amount, float time)
        {
            float lerpPos = 0;
            Vector3 startingPos = transform.position;
            Vector3 targetPos = startingPos + amount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / time;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOutCubic(lerpPos);
                transform.position = Vector3.Lerp(startingPos, targetPos, t);
                yield return null;
            }
            _moveRoutine = null;
        }

        public void Scale(Vector3 amount, float time)
        {
            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleRoutine(amount, time));
        }

        Coroutine _scaleRoutine;
        IEnumerator ScaleRoutine(Vector3 amount, float time)
        {
            float lerpPos = 0;
            Vector3 startingScale = transform.localScale;
            Vector3 targetScale = startingScale + amount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / time;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOutCubic(lerpPos);
                transform.position = Vector3.Lerp(startingScale, targetScale, t);
                yield return null;
            }
            _scaleRoutine = null;
        }
    }
}