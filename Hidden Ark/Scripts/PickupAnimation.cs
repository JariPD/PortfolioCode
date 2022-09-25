using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool UpAndDown = false;       //turn this on in the inspector to move the object up and down
    [SerializeField] private bool Rotate = false;          //turn this on in the inspector to rotate the object
    [SerializeField] private float speed = 5f;             //the speed at wich the object will go up and down
    [SerializeField] private float height = 0.5f;          //the height of how high the object will go up and down
    private Vector3 pos;
    private void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        if (Rotate)
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime); //rotates the object

        if (UpAndDown)
        {
            //moves the object up and down
            float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
