using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Quaternion targetRot;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float rotSpeed = 1.5f;

    private Vector3 origin;
    private Quaternion rot;

    private bool activateZoom = false;

    public void Start()
    {
        origin = transform.localPosition;
        rot = transform.localRotation;
    }

    private void Update()
    {
        if (activateZoom)
        {
            //smoothly moves the camera's position to allocated position
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot, rotSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// function to zoom camera to allocated position
    /// </summary>
    public void Zoom()
    {
        activateZoom = true;
    }

    /// <summary>
    /// function to reset camera's position
    /// </summary>
    public void Reset()
    {
        activateZoom = false;

        transform.localPosition = origin;
        transform.localRotation = rot;
    }
}
