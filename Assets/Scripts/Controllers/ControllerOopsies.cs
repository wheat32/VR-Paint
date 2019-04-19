using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerOopsies : MonoBehaviour
{
    public GameObject penguPoint;//Origin of the paint
    public bool isPenguVisible = false;
    public bool showVerticies = false;
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();
    private List<GameObject> parents = new List<GameObject>();
    public float lineWidth = 0.01f;
    public float timeBetweenDraws = 0.001f;//DEFAULT: 0.001f

    private SteamVR_Action_Single trigger;
    private float timestamp;
    private List<List<GameObject>> lastPengus = new List<List<GameObject>>();
    public float triggerThreshold = 0.7f;
    private bool triggered = false;

    private List<Gradient> gradients = new List<Gradient>();

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
            if (triggered == false)
            {
                parents.Add(new GameObject());
                parents[parents.Count - 1].name = "Vertex Parent " + parents.Count;
                lastPengus.Add(new List<GameObject>());
                lineRenderer.Add(new LineRenderer());
                gradients.Add(new Gradient());

                lineRenderer[lineRenderer.Count - 1] = parents[parents.Count - 1].AddComponent<LineRenderer>();
                lineRenderer[lineRenderer.Count - 1].material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer[lineRenderer.Count - 1].widthMultiplier = lineWidth;
                triggered = true;
            }
            
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(parents[parents.Count - 1].transform);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().enabled = showVerticies;
            sphere.name = "Paint Vertex";
            sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            sphere.transform.position = penguPoint.transform.position;
            lastPengus[lastPengus.Count - 1].Add(sphere);
            addColor(GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor());

            //Iterate through all of the lines
            for (int i = 0; i < lastPengus.Count; i++)
            {
                if (lastPengus[i].Count > 1)
                {
                    //holdTightAsnee will contain all of the verticies for a line
                    Vector3[] holdTightAsnee = new Vector3[lastPengus[i].Count];

                    //Iterate through the verticies on the indexed line
                    for (int j = 0; j < lastPengus[i].Count; j++)
                    {
                        holdTightAsnee[j] = lastPengus[i][j].transform.position;
                    }

                    lineRenderer[i].positionCount = lastPengus[i].Count;
                    lineRenderer[i].SetPositions(holdTightAsnee);
                    lineRenderer[i].colorGradient = gradients[i];
                }
            }

            timestamp = 0.0f;
        }
        else if (trigger.GetAxis(SteamVR_Input_Sources.Any) < triggerThreshold && triggered == true)
        {
            triggered = false;
        }
    }

    private void addColor(Color color)
    {
        Gradient newGradiant = new Gradient();
        Gradient currGradiant = gradients[gradients.Count - 1];

        GradientColorKey[] gck = new GradientColorKey[Mathf.Max(currGradiant.colorKeys.Length + 1, 2)];
        GradientAlphaKey[] gak = new GradientAlphaKey[gck.Length];

        float divisions = 1f / (gck.Length - 1);

        for (int i = 0; i < gck.Length - 1; i++)
        {
            gck[i] = new GradientColorKey(currGradiant.colorKeys[i].color, divisions * i);
            gak[i] = new GradientAlphaKey(1f, divisions * i);
        }

        if (lastPengus[lastPengus.Count - 1].Count == 1)
        {
            Debug.Log("here");
            gck[0] = new GradientColorKey(color, 0f);
            gak[0] = new GradientAlphaKey(1f, 0f);
            gck[1] = new GradientColorKey(color, 1f);
            gak[1] = new GradientAlphaKey(1f, 1f);
        }
        else
        {
            gck[gck.Length - 1] = new GradientColorKey(color, 1f);
            gak[gak.Length - 1] = new GradientAlphaKey(1f, 1f);
        }

        newGradiant.SetKeys(gck, gak);

        gradients[gradients.Count - 1] = newGradiant;
    }
}
