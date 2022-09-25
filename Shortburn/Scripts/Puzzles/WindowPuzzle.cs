using UnityEngine;

public class WindowPuzzle : MonoBehaviour
{
    public bool CloseWindow;

    [Header("References")]
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject rope;
    [SerializeField] private GameObject trigger;
    private Animator anim;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (CloseWindow)
        {
            //start animation
            anim.SetTrigger("CloseWindow");

            //start sound effect
            AudioManager.instance.Play("WindowSlam");

            rope.SetActive(false);
            trigger.SetActive(false);

            CloseWindow = false;
        }
    }
}