using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerOopsies : MonoBehaviour
{
    public GameObject penguPoint;//Origin of the paint
    public bool isPenguVisible = false;
    public bool showVerticies = false;
    private int parents = 0;
    public float timeBetweenDraws = 0.05f;//DEFAULT: 0.05f

    private SteamVR_Action_Single trigger;
    private float timestamp;
    public float triggerThreshold = 0.7f;
    private bool triggered = false;
    private Color lastColor = Color.clear;
    private bool colorChanged = false;
    private GameObject lastVertex = null;

    private List<Color> colors = new List<Color>();
    private List<GameObject> lastPengu = new List<GameObject>();
    private LineRenderer line;
    private GameObject dad;

    public void Awake()
    {
        penguPoint.GetComponent<MeshRenderer>().enabled = isPenguVisible;
        trigger = SteamVR_Actions._default.Squeeze;
    }

    // Update is called once per frame
    void Update()
    {
        timestamp += Time.deltaTime;

        if (trigger.GetAxis(SteamVR_Input_Sources.Any) >= triggerThreshold && timestamp >= timeBetweenDraws)
        {
            //Solid color
            if (triggered == false)
            {
                dad = new GameObject();
                dad.name = "Vertex Parent " + ++parents;
                dad.transform.localPosition = penguPoint.transform.position;

                lastColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();
                colors.Add(lastColor);

                line = new LineRenderer();
                lastPengu.Clear();
                colors.Clear();

                line = dad.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.numCornerVertices = 4;
                line.numCapVertices = 4;
                line.widthMultiplier = GameObject.Find("Scripts").GetComponent<LineWidthBehavior>().getWidth();

                triggered = true;
            }
            //For gradient colors
            else if (GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor() != lastColor && triggered == true)
            {
                lastColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();

                dad = new GameObject();
                dad.name = "Vertex Parent " + ++parents;
                dad.transform.localPosition = penguPoint.transform.position;
                colors.Add(lastColor);

                lastPengu.Clear();

                line = dad.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.numCornerVertices = 4;
                line.numCapVertices = 4;
                line.widthMultiplier = GameObject.Find("Scripts").GetComponent<LineWidthBehavior>().getWidth();
                colorChanged = true;
            }

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(dad.transform);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().enabled = showVerticies;
            sphere.name = "Paint Vertex";
            sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            sphere.transform.position = penguPoint.transform.position;

            if (lastVertex != null && colorChanged == true)
            {
                lastPengu.Add(lastVertex);
            }

            lastPengu.Add(sphere);
            lastVertex = sphere;//Save off the old vertex

            
            if (lastPengu.Count > 1)
            {
                //holdTightAsnee will contain all of the verticies for a line
                Vector3[] holdTightAsnee = new Vector3[lastPengu.Count];

                //Iterate through the verticies on the indexed line
                for (int i = 0; i < lastPengu.Count; i++)
                {
                    holdTightAsnee[i] = lastPengu[i].transform.position;
                }

                line.material.color = lastColor;
                line.positionCount = lastPengu.Count;
                line.SetPositions(holdTightAsnee);
            }

            timestamp = 0.0f;
            colorChanged = false;
        }
        else if (trigger.GetAxis(SteamVR_Input_Sources.Any) < triggerThreshold && triggered == true)
        {
            triggered = false;
            lastColor = Color.clear;
            lastVertex = null;
            colorChanged = false;
        }
    }
}
