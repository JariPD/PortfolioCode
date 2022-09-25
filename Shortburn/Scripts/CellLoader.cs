using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLoader : MonoBehaviour
{
    public static CellLoader instance;


    [SerializeField] private GameObject cell1;
    [SerializeField] private GameObject cell2;

    private void Awake()
    {
        instance = this;
    }

    public void LoadCell()
    {
        cell2.SetActive(true);
    }

    public void UnloadCell()
    {
        cell1.SetActive(false);
    }
}
