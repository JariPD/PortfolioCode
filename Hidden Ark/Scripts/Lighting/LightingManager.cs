using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Camera cam;
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    //Variables
    [SerializeField, Range(0, 120)] private float timeOfDay;

    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            timeOfDay += Time.deltaTime;
            timeOfDay %= 120; //Modulus to ensure always between 0-24
            UpdateLighting(timeOfDay / 120);
        }
        else
        {
            UpdateLighting(timeOfDay / 120);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);
        cam.backgroundColor = preset.DayNightCycle.Evaluate(timePercent);
        //RenderSettings.fogDensity = 0.01f;

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);

            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}