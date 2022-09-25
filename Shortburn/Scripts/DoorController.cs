using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject particle;
    private Vector3 resetPos;

    public bool MoveObject;
    public bool ResetDoor;


    private void Start()
    {
        resetPos = transform.localPosition;
    }

    void Update()
    {
        if (MoveObject)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            particle.SetActive(true);
        }
        else if (ResetDoor)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, resetPos, moveSpeed * Time.deltaTime);
            particle.SetActive(true);
        }

        if (transform.localPosition == targetPos)
        {
            AudioManager.instance.Stop("Stone Door Opening");
            particle.SetActive(false);
        }

        if (transform.localPosition == resetPos && ResetDoor)
        {
            AudioManager.instance.Stop("Stone Door Opening");
            particle.SetActive(false);
        }
    }
}

