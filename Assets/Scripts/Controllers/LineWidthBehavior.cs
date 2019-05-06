using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LineWidthBehavior : MonoBehaviour
{
    private SteamVR_Action_Boolean westDPad;
    private SteamVR_Action_Boolean eastDPad;
    public GameObject textDisplay;

    public float[] widthSizes;
    public int initialWidth = 4;

    private int currentWidth = -1;
    private bool widthChanged = false;

    private Player player = null;
    private Coroutine textHint;

    public void Awake()
    {
        player = Valve.VR.InteractionSystem.Player.instance;

        textHint = StartCoroutine(widthHintCoroutine());

        currentWidth = initialWidth;
        westDPad = SteamVR_Actions._default.decreaseWidth;
        eastDPad = SteamVR_Actions._default.increaseWidth;

        if (widthSizes.Length == 0)
        {
            widthSizes = new float[12];
            widthSizes[0] = 0.001f;
            widthSizes[1] = 0.0025f;
            widthSizes[2] = 0.005f;
            widthSizes[3] = 0.0075f;
            widthSizes[4] = 0.01f;
            widthSizes[5] = 0.015f;
            widthSizes[6] = 0.021f;
            widthSizes[7] = 0.028f;
            widthSizes[8] = 0.036f;
            widthSizes[9] = 0.045f;
            widthSizes[10] = 0.055f;
            widthSizes[11] = 0.066f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (westDPad.GetStateDown(SteamVR_Input_Sources.Any) == true)
        {
            if (currentWidth > 0)
            {
                currentWidth--;
                widthChanged = true;
                StopCoroutine(textHint);
                ControllerButtonHints.HideTextHint(player.leftHand, SteamVR_Actions._default.decreaseWidth);
            }
        }

        if (eastDPad.GetStateDown(SteamVR_Input_Sources.Any) == true)
        {
            if (currentWidth < widthSizes.Length - 1)
            {
                currentWidth++;
                widthChanged = true;
                StopCoroutine(textHint);
                ControllerButtonHints.HideTextHint(player.leftHand, SteamVR_Actions._default.decreaseWidth);
            }
        }

        if (widthChanged == true)
        {

        }

        textDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Brush width: " + (currentWidth+1);

        widthChanged = false;
    }

    public float getWidth()
    {
        return widthSizes[currentWidth];
    }

    private IEnumerator widthHintCoroutine()
    {
        float prevBreakTime = Time.time;
        float prevHapticPulseTime = Time.time;

        while (true)
        {
            bool pulsed = false;

            //Show the hint on each eligible hand
            foreach (Hand hand in player.hands)
            {
                bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(hand, SteamVR_Actions._default.decreaseWidth));

                if (isShowingHint == false)
                {
                    ControllerButtonHints.ShowTextHint(hand, SteamVR_Actions._default.decreaseWidth, "Press sides to adjust brush width");
                    prevBreakTime = Time.time;
                    prevHapticPulseTime = Time.time;
                }

                if (Time.time > prevHapticPulseTime + 0.05f)
                {
                    //Haptic pulse for a few seconds
                    pulsed = true;

                    hand.TriggerHapticPulse(500);
                }
            }

            if (Time.time > prevBreakTime + 3.0f)
            {
                //Take a break for a few seconds
                yield return new WaitForSeconds(3.0f);

                prevBreakTime = Time.time;
            }

            if (pulsed)
            {
                prevHapticPulseTime = Time.time;
            }

            yield return null;
        }
    }
}
