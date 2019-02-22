using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSwitch : MonoBehaviour
{

    public Material mat1;
    public Material mat2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RenderSettings.skybox = mat1;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            RenderSettings.skybox = mat2;
        }

    }
}






