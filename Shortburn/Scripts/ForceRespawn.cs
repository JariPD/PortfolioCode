using UnityEngine;

public class ForceRespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            StartCoroutine(SpawnPoints.instance.SpawnPlayer());
        }
    }
}
