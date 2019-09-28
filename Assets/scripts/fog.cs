using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fogColor = Camera.main.backgroundColor;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fog = true;
    }
}
