using UnityEngine;

public class BoilingPuzzle : MonoBehaviour
{
    public bool IsBoiling;
    private bool launchPlayer;

    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject player;
    void Update()
    {
        if (launchPlayer)
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (player.transform.position == targetPos)
        {
            launchPlayer = false;

            LockPlayer.instance.FpsController.enabled = true;
            LockPlayer.instance.CharacterController.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player") && IsBoiling)
        {
            LockPlayer.instance.FpsController.enabled = false;
            LockPlayer.instance.CharacterController.enabled = false;

            launchPlayer = true;
        }
    }
}