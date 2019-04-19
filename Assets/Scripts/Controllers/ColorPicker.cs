using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ColorPicker : MonoBehaviour
{
    public float colorVisualMinSize = 0.02f;
    public float colorVisualMaxSize = 0.04f;

    private float hue = 1f;
    private float saturation = 1f;
    private float value = 1.0f;
    private Color color = Color.white;

    public GameObject colorVisual;
    private SteamVR_Action_Vector2 radial;
    public GameObject colorWheel;

    void Awake()
    {
        radial = SteamVR_Actions._default.WheelTouch;
    }


    
    void Update()
    {

        radial.GetAxis(SteamVR_Input_Sources.Any);
        float x = radial.GetAxis(SteamVR_Input_Sources.Any).x;
        float y = radial.GetAxis(SteamVR_Input_Sources.Any).y;
        
        if(x == 0 && 0 == y)
        {
            if (colorVisual.transform.localScale.x > colorVisualMinSize)
            {
                float scaleVal = colorVisual.transform.localScale.x;
                scaleVal -= Time.deltaTime * 0.04f;

                colorVisual.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            }
            else if (colorVisual.transform.localScale.x < colorVisualMinSize)
            {
                colorVisual.transform.localScale = new Vector3(colorVisualMinSize, colorVisualMinSize, colorVisualMinSize);
            }

            return;
        }

        //Size color visualizer
        if (colorVisual.transform.localScale.x < 0.04f)
        {
            float scaleVal = colorVisual.transform.localScale.x;
            scaleVal += Time.deltaTime * 0.07f;

            colorVisual.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        }
        else if (colorVisual.transform.localScale.x > 0.04f)
        {
            colorVisual.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        }

        float angle;

        if (x < 0)
        {
            angle = 270 - (Mathf.Atan(y / x) * 180 / Mathf.PI);
        }
        else
        {
            angle = 90 + (Mathf.Atan(y / -x) * 180 / Mathf.PI);
        }

        float normalAngle = angle - 90;
        if (normalAngle < 0)
        {
            normalAngle = 360 + normalAngle;
        }

        //Debug.Log(normalAngle);

        float rads = normalAngle * Mathf.PI / 180;
        float maxX = Mathf.Cos(rads);
        float maxY = Mathf.Sin(rads);

        float currX = x;
        float currY = y;

        float percentX = Mathf.Abs(currX / maxX);
        float percentY = Mathf.Abs(currY / maxY);

        hue = normalAngle / 360.0f;
        saturation = (percentX + percentY) / 2;

        hue = Mathf.Min(hue, 1);
        saturation = Mathf.Min(saturation, 1);

        //Debug.Log(hue + " " + saturation);

        color = Color.HSVToRGB(hue, saturation, value);
        colorVisual.GetComponent<Renderer>().material.color = color;
    }

    public Color getColor()
    {
        return color;
    }
}
