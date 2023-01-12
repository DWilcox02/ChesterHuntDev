using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSound : MonoBehaviour
{
    public GameObject quiet;
    public GameObject medium;
    public GameObject loud;
    // Start is called before the first frame update
    void Start()
    {
        quiet.SetActive(true);
        medium.SetActive(false);
        loud.SetActive(false);
    }

    public void SetQuiet()
    {
        quiet.SetActive(true);
        medium.SetActive(false);
        loud.SetActive(false);
    }

    public void SetMedium()
    {
        quiet.SetActive(false);
        medium.SetActive(true);
        loud.SetActive(false);
    }

    public void SetLoud()
    {
        quiet.SetActive(false);
        medium.SetActive(false);
        loud.SetActive(true);
    }
}
