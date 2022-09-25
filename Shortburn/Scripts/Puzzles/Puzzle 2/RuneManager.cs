using UnityEngine;
using UnityEngine.Events;

public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;

    public int AmountActive;

    [SerializeField] private UnityEvent winEvent;
    [SerializeField] private GameObject winPanel;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (AmountActive == 5)
            StartCoroutine(UIManager.instance.MoveToCenter());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (AmountActive == 5)
                Win();
        }
    }

    public void Win()
    {
        //locks player
        LockPlayer.instance.LockAll();

        //play win cutscene
        winEvent.Invoke();
    }
}
