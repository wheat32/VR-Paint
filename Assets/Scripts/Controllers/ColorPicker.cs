using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ColorPicker : MonoBehaviour
{
    private float hue = 1f;
    private float saturation = 1f;
    //TODO private float value = 1.0f;

    private SteamVR_Action_Vector2 radial;
    public GameObject colorWheel;

    void Awake()
    {
        radial = SteamVR_Actions._default.WheelTouch;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: get angle from 2D axis
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
