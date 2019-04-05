using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PaintBehavior : MonoBehaviour
{
    public GameObject penguPoint;//Origin of the paint
    public bool isPenguVisible = false;
    public bool showVerticies = false;
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();
    private List<GameObject> parents = new List<GameObject>();
    public float lineWidth = 0.01f;
    public float timeBetweenDraws = 0.01f;

    private SteamVR_Action_Single trigger;
    private float timestamp;
    private List<List<GameObject>> lastPengus = new List<List<GameObject>>();
    public float triggerThreshold = 0.7f;
    private bool triggered = false;

    public void Awake()
    {
        penguPoint.GetComponent<MeshRenderer>().enabled = isPenguVisible;
        trigger = SteamVR_Actions._default.Squeeze;
    }

    // Update is called once per frame
    void Update()
    {
        timestamp += Time.deltaTime;

        if (trigger.GetAxis(SteamVR_Input_Sources.Any) >= triggerThreshold && triggered == false)
        {
            parents.Add(new GameObject());
            parents[parents.Count - 1].name = "Vertex Parent " + parents.Count;
            lastPengus.Add(new List<GameObject>());
            lineRenderer.Add(new LineRenderer());
            lineRenderer[lineRenderer.Count-1] = parents[parents.Count-1].AddComponent<LineRenderer>();
            lineRenderer[lineRenderer.Count-1].material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer[lineRenderer.Count-1].widthMultiplier = lineWidth;
            triggered = true;
        }
        else if (trigger.GetAxis(SteamVR_Input_Sources.Any) < triggerThreshold && triggered == true)
        {
            triggered = false;
        }

        if (trigger.GetAxis(SteamVR_Input_Sources.Any) >= triggerThreshold && timestamp >= timeBetweenDraws)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(parents[parents.Count - 1].transform);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().enabled = showVerticies;
            sphere.name = "Paint Vertex";
            sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            sphere.transform.position = penguPoint.transform.position;
            lastPengus[lastPengus.Count-1].Add(sphere);

            for (int i = 0; i < lastPengus.Count; i++)
            {
                if (lastPengus[i].Count > 1)
                {
                    Vector3[] holdTightAsnee = new Vector3[lastPengus[i].Count];

                    for (int j = 0; j < lastPengus[i].Count; j++)
                    {
                        holdTightAsnee[j] = lastPengus[i][j].transform.position;
                    }

                    lineRenderer[i].positionCount = lastPengus[i].Count;
                    lineRenderer[i].SetPositions(holdTightAsnee);
                    
                }
            }

            timestamp = 0.0f;
        }
    }
}
