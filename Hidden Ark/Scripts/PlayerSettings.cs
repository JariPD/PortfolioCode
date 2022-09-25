using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Settings")]
    public float speed = 6;
    public float sprintSpeed = 10;
    public float gravity = -19.62f;
    public float turnSmoothTime = 0.1f;

    [Header("Jump Settings")]
    public float jumpHeight = 2;
    public float doubleJumpHeight = 1;

    [Header("Dash Settings")]
    public float dashTime = 0.25f;
    public float dashSpeed = 20f;
    public float dashCooldown = 2f;

    [Header("GroundCheck Settings")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
}
