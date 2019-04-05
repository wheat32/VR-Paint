using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerOopsies : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject vertexGrouping;

    private SteamVR_Action_Single fuck;
    private SteamVR_Action_Vector2 radial;

    public void Awake()
    {
        fuck = SteamVR_Actions._default.Squeeze;
        radial = SteamVR_Actions._default.WheelTouch;
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log("Left hand: " + leftHand.transform.position.ToString("F4") + " | Right hand: " + rightHand.transform.position.ToString("F4"));

        if (fuck.GetAxis(SteamVR_Input_Sources.Any) > 0.5)
        {
            Debug.Log("SHIT-FUCK MY FACE");
        }

        Debug.Log(radial.GetAxis(SteamVR_Input_Sources.Any));
    }

}
