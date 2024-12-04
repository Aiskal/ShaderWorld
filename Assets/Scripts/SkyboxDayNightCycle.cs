using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class SkyboxDayNightCycle : MonoBehaviour
{
    //[SerializeField] private Material skyboxMaterial;
    [SerializeField] private Transform sunTransform;
    //[SerializeField] private UpdateLights updateLights;
    [SerializeField] private float sunArcHeight = 45f;
    [SerializeField] private float dayLength = 60f;
    [SerializeField][Range(0f, 0.25f)] private float transitionFraction = 0.1f;


    private Color zenithDayColor;
    private Color horizonDayColor;

    private Color zenithTwilightColor;
    private Color horizonTwilightColor;

    private Color zenithNightColor;
    private Color horizonNightColor;

    private Color zenithDawnColor;
    private Color horizonDawnColor;

    private Color zenithColor;
    private Color horizonColor;

    private float timeOfDay;
    private Coroutine sunCoroutine;

    private float dawnStart;
    private float dawnMid;
    private float dayStart;
    private float twilightStart;
    private float twilightMid;
    private float nightStart;

    void Start()
    {
        /*zenithDayColor = skyboxMaterial.GetColor("_ZenithDayColor");
        horizonDayColor = skyboxMaterial.GetColor("_HorizonDayColor");

        zenithTwilightColor = skyboxMaterial.GetColor("_ZenithTwilightColor");
        horizonTwilightColor = skyboxMaterial.GetColor("_HorizonTwilightColor");

        zenithNightColor = skyboxMaterial.GetColor("_ZenithNightColor");
        horizonNightColor = skyboxMaterial.GetColor("_HorizonNightColor");

        zenithDawnColor = skyboxMaterial.GetColor("_ZenithDawnColor");
        horizonDawnColor = skyboxMaterial.GetColor("_HorizonDawnColor"); */

        dawnStart = 0f;
        dawnMid = transitionFraction / 2f;
        dayStart = transitionFraction;
        twilightStart = 0.5f - transitionFraction;
        twilightMid = twilightStart + transitionFraction / 2f;
        nightStart = 0.5f;
        
        sunCoroutine = StartCoroutine(SunCycle());
    }


   /* void Update()
    {
        timeOfDay = Mathf.Repeat(Time.time / dayLength, 1f);

        zenithColor = CalculateColor(timeOfDay, zenithNightColor, zenithDawnColor, zenithDayColor, zenithTwilightColor);
        horizonColor = CalculateColor(timeOfDay, horizonNightColor, horizonDawnColor, horizonDayColor, horizonTwilightColor);

        skyboxMaterial.SetColor("_ZenithColor", zenithColor);
        skyboxMaterial.SetColor("_HorizonColor", horizonColor);
    }*/
    private IEnumerator SunCycle()
    {
        while (true)
        {
            if (timeOfDay >= dawnStart && timeOfDay <= nightStart) // Soleil visible
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

                float waitTime = CalculateWaitTime(timeOfDay, dawnStart, nightStart);
                yield return new WaitForSeconds(waitTime);

                if (!sunTransform.gameObject.activeSelf)
                {
                    sunTransform.gameObject.SetActive(true);
                }
            }
        }
    }

    private void UpdateSunPositionAndRotation()
    {

        float normalizedTime = (timeOfDay - dawnStart) / (nightStart - dawnStart);

        float angle = Mathf.Lerp(-90f, 90f, normalizedTime);

        float radians = angle * Mathf.Deg2Rad;
        float y = Mathf.Cos(radians) * sunArcHeight;
        float z = Mathf.Sin(radians) * sunArcHeight;

        sunTransform.localPosition = new Vector3(0, y, z);
        sunTransform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }

    private float CalculateWaitTime(float time, float dawnStart, float nightStart)
    {
        if (time > nightStart) // Nuit -> Attendre jusqu'à Mi-Dawn du prochain jour
        {
            return (1f - time + dawnStart) * dayLength;
        }
        else // Mi-Twilight -> Attendre jusqu'à Mi-Twilight
        {
            return (nightStart - time) * dayLength;
        }
    }

    /*private Color CalculateColor(float time, Color night, Color dawn, Color day, Color twilight)
    {
        if (time >= dawnStart && time < dawnMid) // Nuit -> Mi-Aube
        {
            updateLights.UpdateLightIntensityOverTime(transitionFraction * dayLength, true);
            float t = (time - dawnStart) / (dawnMid - dawnStart);
            return Color.Lerp(night, dawn, t);
        }
        else if (time >= dawnMid && time < dayStart) // Mi-Aube -> Jour
        {
            float t = (time - dawnMid) / (dayStart - dawnMid);
            return Color.Lerp(dawn, day, t);
        }
        else if (time >= dayStart && time < twilightStart) // Jour
        {
            return day;
        }
        else if (time >= twilightStart && time < twilightMid) // Jour -> Mi-Crépuscule
        {
            updateLights.UpdateLightIntensityOverTime(transitionFraction * dayLength, false);
            float t = (time - twilightStart) / (twilightMid - twilightStart);
            return Color.Lerp(day, twilight, t);
        }
        else if (time >= twilightMid && time <= nightStart) // Mi-Crépuscule -> Nuit
        {
            float t = (time - twilightMid) / (nightStart - twilightMid);
            return Color.Lerp(twilight, night, t);
        }
        else // Nuit
        {
            return night;
        }
    }*/
}
