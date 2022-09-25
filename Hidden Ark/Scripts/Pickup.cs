using UnityEngine;
using System.Collections;
public class Pickup : MonoBehaviour
{
    public int amountPickedUp = 0;
    public bool doubleJumpAcquired = false;
    public bool dashAcquired = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            //Double Jump
            if (other.gameObject.CompareTag("DoubleJump"))
            {
                doubleJumpAcquired = true;
                amountPickedUp++;
                AudioManager.instance.Play("Pickup", false);
            }

            //Dash
            if (other.gameObject.CompareTag("Dash"))
            {
                dashAcquired = true;
                amountPickedUp++;
                AudioManager.instance.Play("Pickup", false);
            }

            other.gameObject.SetActive(false);

            if (amountPickedUp >= 2)
                StartCoroutine(WinCooldown());
        }
    }

    IEnumerator WinCooldown()
    {
        yield return new WaitForSeconds(3f);

        GameManager.instance.ActivateWin();
    }

}

