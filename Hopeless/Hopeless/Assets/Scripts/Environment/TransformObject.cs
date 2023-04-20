using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment
{

    public class TransformObject : MonoBehaviour
    {
        [System.Serializable]
        public struct TransformStruct
        {
            public TransformObject ObjectScript;
            public Vector2 MoveAmount;
            public float MoveTime;
            public Vector2 RotateAmount;
            public float RotateTime;
            public Vector2 ScaleAmount;
            public float ScaleTime;

            public void Execute()
            {
                if (MoveAmount != Vector2.zero) ObjectScript.Move(MoveAmount, MoveTime);
                if (RotateAmount != Vector2.zero) ObjectScript.Rotate(RotateAmount, RotateTime);
                if (ScaleAmount != Vector2.zero) ObjectScript.Scale(ScaleAmount, ScaleTime);
            }
        }

        #region Move
        public void Move(Vector2 amount, float t)
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(MoveRoutine(amount, t));
        }

        Coroutine _moveRoutine;
        IEnumerator MoveRoutine(Vector2 amount, float time)
        {
            float lerpPos = 0;
            Vector2 startingPos = transform.position;
            Vector2 targetPos = startingPos + amount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / time;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOutCubic(lerpPos);
                transform.position = Vector2.Lerp(startingPos, targetPos, t);
                yield return null;
            }
            _moveRoutine = null;
        }
        #endregion
        #region Rotate
        public void Rotate(Vector2 amount, float t)
        {
            if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);
            _rotateRoutine = StartCoroutine(RotateRoutine(amount, t));
        }

        Coroutine _rotateRoutine;
        IEnumerator RotateRoutine(Vector2 amount, float time)
        {
            float lerpPos = 0;
            Vector2 startingPos = transform.position;
            Vector2 targetPos = startingPos + amount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / time;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOutCubic(lerpPos);
                transform.position = Vector2.Lerp(startingPos, targetPos, t);
                yield return null;
            }
            _rotateRoutine = null;
        }
        #endregion
        #region Scale
        public void Scale(Vector2 amount, float time)
        {
            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleRoutine(amount, time));
        }

        Coroutine _scaleRoutine;
        IEnumerator ScaleRoutine(Vector2 amount, float time)
        {
            float lerpPos = 0;
            Vector2 startingScale = transform.localScale;
            Vector2 targetScale = startingScale + amount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / time;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOutCubic(lerpPos);
                transform.localScale = Vector2.Lerp(startingScale, targetScale, t);
                yield return null;
            }
            _scaleRoutine = null;
        }
        #endregion
    }
}