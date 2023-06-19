using System.Collections;
using UnityEngine;

public class PositionMarker : MonoBehaviour
{
    public Vector3 _TargetPos;

    public float _Duration = 1f;
    public Vector3 _originalScale;
    public Vector3 _targetScale;
    public static PositionMarker Instance;

    private void Awake()
    {
        Instance = this;
        _originalScale = transform.localScale;
    }

    public void SetNewPosition(Vector3 targetPos)
    {

        _TargetPos = targetPos;
        transform.position = targetPos;
        transform.transform.localScale = _originalScale;

        //reset the coroutine
        StopAllCoroutines();
        StartCoroutine(FadeOut());

    }

    private IEnumerator FadeOut()
    {

        //set the scale to the original scale
        transform.localScale = _originalScale;
        transform.position = _TargetPos;
        //set the target scale to the target scale
        float startTime = Time.time;
        float endTime = startTime + _Duration;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / _Duration;
            // transform.position = Vector3.Lerp(_TargetPos, _TargetPos + -Vector3.up * 2f, t);
            transform.localScale = Vector3.Lerp(_originalScale, _targetScale, t);
            yield return null;
        }
        //hide the marker
        transform.localScale = Vector3.zero;
    }
}