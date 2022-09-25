using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private GameObject hand;
    private GameObject mirrorObject;

    [Header("Ray Settings")]
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private float rayDistance;
    [SerializeField] private float rotSpeed;

    [Header("Cam Settings")]
    [SerializeField] private Vector3 camTargetPos;
    [SerializeField] private Quaternion camTargetRot;

    private RaycastHit hit;

    [Header("Selected Bools")]
    public bool ZoomSelected;
    public bool PrismSelected;

    void Update()
    {
        Vector3 origin = transform.position;                               //origin of the ray
        Vector3 direction = transform.TransformDirection(Vector3.forward); //direction for the ray

        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerToHit)) //draws a ray going forwards from the object
        {
            //enables interact text if object is not untagged
            if (hit.transform.gameObject.CompareTag("Untagged"))
                return;
            UIManager.instance.IsInteracting = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.gameObject.CompareTag("Mirror"))
                {
                    //sets reference to selected mirror
                    mirrorObject = hit.transform.gameObject;
                }

                if (hit.transform.gameObject.CompareTag("Zoom"))
                    ZoomSelected = true;

                if (hit.transform.gameObject.CompareTag("Prism"))
                    PrismSelected = true;
            }
        }
        else
            UIManager.instance.IsInteracting = false;

        if (mirrorObject)
        {
            //sets hand GameObject to off
            hand.gameObject.SetActive(false);

            //lock player movement and camera
            LockPlayer.instance.FpsController.enabled = false;
            LockPlayer.instance.MouseLook.enabled = false;

            //move camera up
            transform.localPosition = camTargetPos;
            transform.localRotation = camTargetRot;

            //turns interact text off and exit interact on
            UIManager.instance.IsInteracting = false;
            UIManager.instance.ObjectSelected = true;

            //turn input
            float mouseX = (Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime * Mathf.Rad2Deg);
            float mouseY = (Input.GetAxis("Mouse Y") * -rotSpeed * Time.deltaTime * Mathf.Rad2Deg);

            //turns object
            if (mirrorObject.GetComponentInChildren<Mirror>().vertical)
                mirrorObject.transform.Rotate(Vector3.right, mouseY);

            if (mirrorObject.GetComponentInChildren<Mirror>().horizontal)
                mirrorObject.transform.Rotate(Vector3.up, mouseX);

            //unselect object
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //remove reference to mirrorObject
                mirrorObject = null;

                //turns off the exit interact text
                UIManager.instance.ObjectSelected = false;

                //unlocks camera
                LockPlayer.instance.UnlockAll();

                //resets camera
                cameraZoom.Reset();

                //turns hand GameObject back on
                hand.gameObject.SetActive(true);
            }
        }

        if (PrismSelected)
        {
            var prism = hit.transform.gameObject;

            prism.transform.Rotate(0, 90, 0);

            PrismSelected = false;
        }

        if (ZoomSelected)
        {
            //sets hand GameObject to off
            hand.gameObject.SetActive(false);

            //lock player movement and camera
            LockPlayer.instance.FpsController.enabled = false;
            LockPlayer.instance.MouseLook.enabled = false;

            //zooms the camera to another position
            cameraZoom.Zoom();

            //unselect object
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ZoomSelected = false;

                //resets camera's zoom back to player
                cameraZoom.Reset();

                //turns camera movement back on
                LockPlayer.instance.MouseLook.enabled = true;
                LockPlayer.instance.FpsController.enabled = true;

                //turns hand GameObject back on
                hand.gameObject.SetActive(true);
            }
        }
    }

#if (UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        Vector3 dir = transform.TransformDirection(Vector3.forward) * rayDistance;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, dir);
    }
#endif
}
