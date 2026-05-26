using UnityEngine;

public class GlowZoneVisual : MonoBehaviour
{
    public Material glowMaterial;

    public float pulseSpeed = 2f;
    public float minEmission = 1f;
    public float maxEmission = 5f;

    private Color baseColor;

    void Start()
    {
        baseColor = glowMaterial.GetColor("_EmissionColor");
    }

    void Update()
    {
        float emission = Mathf.Lerp(
            minEmission,
            maxEmission,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f
        );

        glowMaterial.SetColor("_EmissionColor", baseColor * emission);
    }
}