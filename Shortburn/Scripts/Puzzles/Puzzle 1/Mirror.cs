using System.Collections;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [Header("Ray Settings")]
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private LayerMask layerToHit;
    private RaycastHit hit;
    private bool noHit = false;

    [Header("Mirror Settings")]
    public bool vertical;
    public bool horizontal;
    [SerializeField] private int mirrorIndex;

    [Header("References")]
    [SerializeField] private GameObject brokenMirror;
    [SerializeField] private GameObject chairMirror;
    [SerializeField] private GameObject reflection;
    [SerializeField] private AudioClip[] sounds;
    private GameObject rune;
    private ParticleSystem particle;
    private AudioSource audioSource;

    private bool getRune = true;
    private bool removeRune = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 origin = transform.position;                               //origin of the ray
        Vector3 direction = transform.TransformDirection(Vector3.forward); //direction for the ray

        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerToHit)) //draws a ray going forwards from the object
        {
            noHit = false;

            for (int i = 1; i <= 7; i++)
            {
                //checks if index is the same as the object that is hit if so start GetRune coroutine
                if (mirrorIndex == i && hit.transform.gameObject.name == $"Rune {i}" && getRune)
                {
                    if (hit.transform.gameObject.name == "Rune 6" && getRune)
                    {
                        hit.transform.gameObject.SetActive(false);

                        reflection?.SetActive(true);
                        brokenMirror?.SetActive(true);
                    }

                    if (hit.transform.gameObject.name == "Rune 7" && getRune)
                        chairMirror?.SetActive(true);

                    StartCoroutine(GetRune());
                }
            }
        }
        else
            noHit = true;

        if (noHit && removeRune)
            StartCoroutine(RemoveRune());
    }

    /// <summary>
    /// Coroutine that puts hit object into an empty gameobject
    /// </summary>
    /// <returns></returns>
    IEnumerator GetRune()
    {
        //bools to make sure coroutines is not started multiple times
        getRune = false;
        removeRune = true;

        //puts the hit object into an empty GameObject
        rune = hit.transform.gameObject;

        //plays fire sfx
        rune.GetComponentInChildren<AudioSource>()?.Play();

        //plays rune SFX
        audioSource.PlayOneShot(RandomClip());

        //changes the material of the object
        rune.GetComponent<Renderer>().material.color = new Color(255, 0, 0);

        //gets particle object and turns it or
        particle = rune.GetComponentInChildren<ParticleSystem>();
        particle?.Play();

        //increases the amount of active runes
        PuzzleManager.instance.AmountActive++;

        yield return null;
    }

    IEnumerator RemoveRune()
    {
        //bools to make sure coroutines is not started multiple times
        removeRune = false;
        getRune = true;

        //sets rune back to default if no longer selected
        rune.GetComponent<Renderer>().material.color = new Color(0, 0, 0);

        //stops playing fire sfx
        rune.GetComponentInChildren<AudioSource>()?.Stop();

        //gets particle object and turns it on
        particle?.Stop();

        //decreases the amount of active runes
        PuzzleManager.instance.AmountActive--;

        yield return null;
    }


    AudioClip RandomClip()
    {
        return sounds[Random.Range(0, sounds.Length)];
    }

    /// <summary>
    /// Editor only, Gizmos for raycast
    /// </summary>
#if (UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        Vector3 dir = transform.TransformDirection(Vector3.forward) * rayDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, dir);
    }
#endif
}