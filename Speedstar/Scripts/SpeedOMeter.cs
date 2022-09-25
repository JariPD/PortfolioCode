//https://www.youtube.com/watch?v=CC8j_fU2GTQ credit to this guy for the speedOmeter
using UnityEngine;
using TMPro;

public class SpeedOMeter : MonoBehaviour
{
    public Rigidbody Target;

    public float MaxSpeed = 0.0f; //maximum speed of the target in KM/H

    public float MinSpeedArrowAngle;
    public float MaxSpeedArrowAngle;

    [Header("UI")]
    public TextMeshProUGUI SpeedLabel; //the label that dispalyes the speed
    public RectTransform Arrow;        //the arrow in the speedometer


    private void Update()
    {
        // 3.6f to convert in kilometers
        // ** The speed must be clamped by the car controller **
        /*speed = Target.velocity.magnitude * 3.6f;

        if (SpeedLabel != null)
            SpeedLabel.text = ((int)speed) + " km/h";
        if (Arrow != null)
            Arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(MinSpeedArrowAngle, MaxSpeed, speed / MaxSpeed));*/
    }
}
