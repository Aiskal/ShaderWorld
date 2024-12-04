using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class UpdateLights : MonoBehaviour
{
    [SerializeField] Material[] materials; 
    [SerializeField] Material referenceLights;

    private Coroutine lightIntensityCoroutine;
    [SerializeField] private float nightLightIntensity = 0.2f;
    [SerializeField] private float dayLightIntensity = 1f;

    private Color ambientLightColor;
    private float ambientLightIntensity;
    private Color lightColor; 
    private float lightIntensity;

    private void Start()
    {
        ambientLightColor = referenceLights.GetColor("_AmbientLightColor");
        ambientLightIntensity = referenceLights.GetFloat("_AmbientLightIntensity");
        lightColor = referenceLights.GetColor("_LightColor");
        lightIntensity = referenceLights.GetFloat("_LightIntensity");

        UpdateAllMaterialProperties();

        foreach (Material mat in materials)
        {
            SetMaterialProperty(mat, "_AmbientLightColor", ambientLightColor);
            SetMaterialProperty(mat, "_AmbientLightIntensity", ambientLightIntensity);
            SetMaterialProperty(mat, "_LightColor", lightColor);
            SetMaterialProperty(mat, "_LightIntensity", lightIntensity);
        }
    }

    public void UpdateLightIntensityOverTime(float transitionDuration, bool isNightToDay)
    {
        if (lightIntensityCoroutine != null)
        {
            return;
        }
        lightIntensityCoroutine = StartCoroutine(LerpLightIntensity(transitionDuration, isNightToDay));
    }

    private IEnumerator LerpLightIntensity(float duration, bool isNightToDay)
    {
        Debug.Log("LerpLightIntensity");
        float startIntensity = isNightToDay ? nightLightIntensity : dayLightIntensity;
        float endIntensity = isNightToDay ? dayLightIntensity : nightLightIntensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            lightIntensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / duration);
            UpdateMaterialPropertyForAll("_LightIntensity", lightIntensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lightIntensity = endIntensity; 
        UpdateMaterialPropertyForAll("_LightIntensity", lightIntensity);
    }

    private void SetMaterialProperty(Material mat, string propertyName, object value)
    {
        if (mat.HasProperty(propertyName))
        {
            if (value is Color)
            {
                mat.SetColor(propertyName, (Color)value);
            }
            else if (value is float)
            {
                mat.SetFloat(propertyName, (float)value);
            }
            else
            {
                Debug.LogWarning($"Type de propriété non pris en charge : {value.GetType()}");
            }
        }
        else
        {
            Debug.LogWarning($"La propriété {propertyName} n'existe pas sur le matériau {mat.name}");
        }
    }

    public void UpdateMaterialPropertyForAll(string propertyName, object value)
    {
        foreach (Material mat in materials)
        {
            SetMaterialProperty(mat, propertyName, value);
        }
    }

    public void UpdateAllMaterialProperties()
    {
        foreach (Material mat in materials)
        {
            SetMaterialProperty(mat, "_AmbientLightColor", ambientLightColor);
            SetMaterialProperty(mat, "_AmbientLightIntensity", ambientLightIntensity);
            SetMaterialProperty(mat, "_LightColor", lightColor);
            SetMaterialProperty(mat, "_LightIntensity", lightIntensity);
        }
    }
}
