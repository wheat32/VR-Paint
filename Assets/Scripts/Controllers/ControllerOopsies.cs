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
    public float timeBetweenDraws = 0.05f;//DEFAULT: 0.05f

    private SteamVR_Action_Single trigger;
    private float timestamp;
    private List<List<GameObject>> lastPengus = new List<List<GameObject>>();
    private List<Gradient> gradients = new List<Gradient>();
    private GradientColorKey[] colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(1f, 1f, 1f, 0f), 0f), new GradientColorKey(new Color(1f, 1f, 1f, 0f), 1f) };
    private GradientAlphaKey[] alphaTest = new GradientAlphaKey[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 0.2f), new GradientAlphaKey(1f, 0.3f), new GradientAlphaKey(1f, 0.7f), new GradientAlphaKey(0f, 0.8f), new GradientAlphaKey(0f, 1f) };
    public float triggerThreshold = 0.7f;
    private bool triggered = false;
    private Color lastColor = Color.clear;
    private bool colorChanged = false;
    private GameObject lastVertex = null;
    private GameObject secondToLastVertex = null;

    public void Awake()
    {
        penguPoint.GetComponent<MeshRenderer>().enabled = isPenguVisible;
        trigger = SteamVR_Actions._default.Squeeze;
    }

    // Update is called once per frame
    void Update()
    {
        timestamp += Time.deltaTime;

        Color newColor = new Color();

        if (trigger.GetAxis(SteamVR_Input_Sources.Any) >= triggerThreshold && timestamp >= timeBetweenDraws)
        {
            if (triggered == false)
            {
                parents.Add(new GameObject());
                parents[parents.Count - 1].name = "Vertex Parent " + parents.Count;
                lastPengus.Add(new List<GameObject>());
                lineRenderer.Add(new LineRenderer());
                newColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();
                gradients.Add(new Gradient());
                gradients[gradients.Count - 1].SetKeys(
                    new GradientColorKey[] {new GradientColorKey(newColor, 0f), new GradientColorKey(newColor, 1f)},
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
                gradients[gradients.Count - 1].mode = GradientMode.Blend;
                lineRenderer[lineRenderer.Count - 1] = parents[parents.Count - 1].AddComponent<LineRenderer>();
                lineRenderer[lineRenderer.Count - 1].material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer[lineRenderer.Count - 1].widthMultiplier = lineWidth;
                lastColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();
                triggered = true;
            }
            else if (GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor() != lastColor && triggered == true)
            {
                lastColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();

                parents.Add(new GameObject());
                parents[parents.Count - 1].name = "Vertex Parent " + parents.Count;
                lastPengus.Add(new List<GameObject>());
                lineRenderer.Add(new LineRenderer());
                newColor = GameObject.Find("Scripts").GetComponent<ColorPicker>().getColor();
                lineRenderer[lineRenderer.Count - 1] = parents[parents.Count - 1].AddComponent<LineRenderer>();
                lineRenderer[lineRenderer.Count - 1].material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer[lineRenderer.Count - 1].widthMultiplier = lineWidth;
                colorChanged = true;
            }

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(parents[parents.Count - 1].transform);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().enabled = showVerticies;
            sphere.name = "Paint Vertex";
            sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            sphere.transform.position = penguPoint.transform.position;

            if (lastVertex != null && colorChanged == true)
            {
                if (secondToLastVertex != null)
                {
                    lastPengus[lastPengus.Count - 1].Add(secondToLastVertex);
                }

                lastPengus[lastPengus.Count - 1].Add(lastVertex);
            }

            lastPengus[lastPengus.Count - 1].Add(sphere);
            secondToLastVertex = lastVertex;
            lastVertex = sphere;//Save off the old vertex

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

                    lineRenderer[i].colorGradient = gradients[i];
                    lineRenderer[i].positionCount = lastPengus[i].Count;
                    lineRenderer[i].SetPositions(holdTightAsnee);
                }
            }

            timestamp = 0.0f;
            colorChanged = false;
        }
        else if (trigger.GetAxis(SteamVR_Input_Sources.Any) < triggerThreshold && triggered == true)
        {
            triggered = false;
            lastColor = Color.clear;
            lastVertex = null;
            secondToLastVertex = null;
            colorChanged = false;
        }
    }
}
