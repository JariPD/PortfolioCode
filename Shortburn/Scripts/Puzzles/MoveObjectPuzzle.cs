using UnityEngine;

public class MoveObjectPuzzle : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Quaternion targetRot;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float moveSpeed;

    public bool MoveObject;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameObject.CompareTag("Brick") && MoveObject)
        {
            AudioManager.instance.Play("Brick Puzzle");

            if (transform.localPosition == targetPos)
                AudioManager.instance.Stop("Brick Puzzle");
        }

        if (MoveObject)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot, rotSpeed * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
        }
    }
}
