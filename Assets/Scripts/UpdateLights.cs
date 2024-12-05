using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class UpdateLights : MonoBehaviour
{
    [SerializeField] Material[] materials; 
    [SerializeField] Material referenceLights;

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
