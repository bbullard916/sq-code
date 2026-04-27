using UnityEngine;

[RequireComponent(typeof(Light))]
public class NaturalLightFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("Base intensity of the light.")]
    public float baseIntensity = 2.0f;

    [Tooltip("Maximum variation in intensity.")]
    public float intensityVariation = 0.5f;

    [Tooltip("Speed of the flicker (higher = faster changes).")]
    public float flickerSpeed = 2.0f;

    [Tooltip("Optional: vary light color for fire-like effect.")]
    public bool useColorVariation = false;
    public Color baseColor = Color.white;
    public float colorVariationStrength = 0.05f;

    private Light pointLight;
    private float noiseSeed;

    void Start()
    {
        pointLight = GetComponent<Light>();
        noiseSeed = Random.Range(0f, 100f); // unique flicker per light
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(noiseSeed, Time.time * flickerSpeed);
        float intensityOffset = (noise - 0.5f) * 2f * intensityVariation;
        pointLight.intensity = baseIntensity + intensityOffset;

        if (useColorVariation)
        {
            // Subtle shift toward orange/yellow
            float colorShift = (noise - 0.5f) * 2f * colorVariationStrength;
            pointLight.color = baseColor + new Color(colorShift, colorShift * 0.5f, -colorShift * 0.5f);
        }
    }
}
