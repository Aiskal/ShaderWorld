using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxDayNightCycle : MonoBehaviour
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

            if (timeOfDay >= 0 && timeOfDay <= 1f) // Soleil visible
            {
                UpdateSunPositionAndRotation();
                yield return null;            
            }
            else
            {
                // Soleil sous l'horizon

                if (sunTransform.gameObject.activeSelf)
                {
                    sunTransform.gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.5f);

                if (!sunTransform.gameObject.activeSelf)
                {
                    sunTransform.gameObject.SetActive(true);
                }
            }
        }
    }

    private void UpdateSunPositionAndRotation()
    {
        float angle = Mathf.Lerp(-90f, 90f, timeOfDay);

        float radians = angle * Mathf.Deg2Rad;
        float y = Mathf.Cos(radians) * sunArcHeight;
        float z = Mathf.Sin(radians) * sunArcHeight;

        sunTransform.localPosition = new Vector3(0, y, z);
        sunTransform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }

}
