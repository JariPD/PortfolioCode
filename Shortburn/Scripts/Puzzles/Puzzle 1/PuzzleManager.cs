using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    [SerializeField] private DoorController door;
    [SerializeField] private ParticleSystem particle;

    [Header("Settings")]
    public int AmountActive = 0;

    private Collider col;
    private bool coroutineAllowed = true;
    private bool winBool = true;
    private float timer;

    private void Awake()
    {
        instance = this;

        col = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        //win condition
        if (AmountActive >= 6 && winBool)
            Win();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("player") && AmountActive >= 6)
        {
            timer += Time.deltaTime;

            if (timer >= 0.1 && timer <= 4.8 && coroutineAllowed)
                StartCoroutine(OpenDoor());

            //plays a glass breaking sound effect after opening the door
            if (timer >= 1 && timer <= 2)
                AudioManager.instance.Play("Glass Break");

            if (timer >= 4.8)
            {
                timer = 0;

                //moves player to next area
                StartCoroutine(SpawnPoints.instance.SpawnPlayer());

                //turns off collider so player cant go back in
                col.enabled = false;

                //plays voiceover for puzzle 2
                AudioManager.instance.Play("Voiceover 2");

                //set the next checkpoint for respawning
                SpawnPoints.instance.CheckPoint += 1;

                //closes the door
                StartCoroutine(CloseDoor());
            }
        }
    }

    public void Win()
    {
        winBool = false;

        //plays big fireball particle
        particle.Play();

        //turn on text that say "Go to the middle of the circle"
        StartCoroutine(UIManager.instance.StayInCenter());
    }

    IEnumerator OpenDoor()
    {
        coroutineAllowed = false;

        //plays sound effect
        AudioManager.instance.Play("Stone Door Opening");

        //open door to next area
        door.MoveObject = true;

        //load cell 2-3
        CellLoader.instance.LoadCell();

        //screen shake to indicate something is moving
        StartCoroutine(CameraShake.instance.Shake(4.5f, 0.04f));

        yield break;
    }

    IEnumerator CloseDoor()
    {
        StartCoroutine(UIManager.instance.ShowRestartImage());

        yield return new WaitForSeconds(3.5f);

        //plays sound effect
        AudioManager.instance.Play("Stone Door Opening");

        //close door to previous area
        door.MoveObject = false;
        door.ResetDoor = true;

        //screen shake to indicate something is moving
        StartCoroutine(CameraShake.instance.Shake(4.5f, 0.04f));

        new WaitForSeconds(2);

        //unload cell 1
        CellLoader.instance.UnloadCell();

        yield break;
    }
}
