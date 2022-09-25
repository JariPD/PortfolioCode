using UnityEngine;

public class LockPlayer : MonoBehaviour
{
    public static LockPlayer instance;

    public FirstPersonController FpsController;
    public MouseLook MouseLook;
    public Sway Sway;
    public Hand Hand;
    public CharacterController CharacterController;
    public GameObject BeamParticle;

    private void Awake()
    {
        instance = this;
    }

    public void LockAll()
    {
        FpsController.enabled = false;
        MouseLook.enabled = false;
        Sway.enabled = false;
        Hand.enabled = false;
        CharacterController.enabled = false;
        BeamParticle.SetActive(false);
    }

    public void UnlockAll()
    {
        FpsController.enabled = true;
        MouseLook.enabled = true;
        Sway.enabled = true;
        Hand.enabled = true;
        CharacterController.enabled = true;
        BeamParticle.SetActive(true);
    }
}
