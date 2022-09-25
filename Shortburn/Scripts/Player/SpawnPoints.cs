using System.Collections;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints instance;

    [Header("References")]
    [SerializeField] private Animator anim;

    [Header("Settings")]
    public int CheckPoint = 1;
    [SerializeField] private Transform[] spawnPoints;

    private Transform currentSpawnPoint;
    private float respawnTimer = 3;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentSpawnPoint = spawnPoints[0];
    }

    void Update()
    {
        //if statement to check where to respawn the player
        if (CheckPoint == 2)
            currentSpawnPoint = spawnPoints[1];

        if (CheckPoint == 3)
            currentSpawnPoint = spawnPoints[2];

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(SpawnPlayer());
    }

    public IEnumerator SpawnPlayer()
    {
        //Lock player
        LockPlayer.instance.LockAll();

        //starts playing the fade animation
        anim.SetTrigger("Fade");

        yield return new WaitForSeconds(respawnTimer);

        //sets players new position
        transform.position = currentSpawnPoint.position;

        //Unlock player
        LockPlayer.instance.UnlockAll();
    }
}
