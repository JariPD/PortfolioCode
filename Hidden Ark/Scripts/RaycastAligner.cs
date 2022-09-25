using UnityEngine;

public class RaycastAligner : MonoBehaviour
{
    private float raycastDistance = 100f;

    void Start()
    {
        PositionRayCast();
    }

    //function that shoots a ray down to align trees
   private void PositionRayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = rot;
            transform.position = hit.point;
        }
    }
}
