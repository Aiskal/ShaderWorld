using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPos : MonoBehaviour
{
    [SerializeField] private Transform sunTransform;
    [SerializeField] private float sunArcHeight = 45f;
    [SerializeField] private float dayLength = 30f;

    private float timeOfDay;
    private Coroutine sunCoroutine;


    void Start()
    {
        sunCoroutine = StartCoroutine(SunCycle());
    }

    private IEnumerator SunCycle()
    {
        while (true)
        {
            timeOfDay = Mathf.Repeat(Time.time / dayLength, 1f);
            UpdateSunPositionAndRotation();
            yield return null; 
        }
    }

    private void UpdateSunPositionAndRotation()
    {
        float angle = timeOfDay * 180f - 90f;

        float radians = angle * Mathf.Deg2Rad;
        float y = Mathf.Cos(radians) * sunArcHeight;
        float z = Mathf.Sin(radians) * sunArcHeight;

        sunTransform.localPosition = new Vector3(0, y, z);
        sunTransform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }

}
