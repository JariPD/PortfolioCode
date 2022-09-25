using UnityEngine;

public class MovePlayerToNextArea : MonoBehaviour
{
    public static MovePlayerToNextArea instance;

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float moveSpeed;

    public bool MovePlayer;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (MovePlayer)
        {
            LockPlayer.instance.FpsController.enabled = false;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        if (transform.position == targetPos)
        {
            MovePlayer = false;

            LockPlayer.instance.FpsController.enabled = true;
        }
    }
}
