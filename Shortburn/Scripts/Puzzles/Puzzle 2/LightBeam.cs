using UnityEngine;

public class LightBeam : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    private RaycastHit hit;
    private GameObject runeObject;
    private bool runeHit;

    void Update()
    {
        Vector3 origin = transform.position;                               //origin of the ray
        Vector3 direction = transform.TransformDirection(Vector3.forward); //direction for the ray

        if (Physics.Raycast(origin, direction, out hit, rayDistance)) //draws a ray going forwards from the object
        {
            if (hit.transform.gameObject.CompareTag("Rune"))
            {
                runeHit = true;

                if (runeHit)
                {
                    //sets reference to object you pressed
                    runeObject = hit.transform.gameObject;

                    Rune();
                }
            }
        }
    }

    /// <summary>
    /// function to increase amount of runes pressed and handles disabling them when pressed
    /// </summary>
    private void Rune()
    {
        //increases the rune count
        RuneManager.instance.AmountActive++;

        runeObject.tag = "Untagged";

        //change runes appearance to indicate it being pressed
        runeObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0);

        runeHit = false;
    }

#if (UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        Vector3 dir = transform.TransformDirection(Vector3.forward) * rayDistance;

        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, dir);
    }
#endif
}




