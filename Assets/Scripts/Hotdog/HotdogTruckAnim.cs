using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotdogTruckAnim : MonoBehaviour
{
    public float speed = 1.0f;
    public float startZ;
    public float endZ;
    public float minTime = 6f;
    public float maxTime = 45f;
    public float timeUntilNextRun = 0f;

    public void Start()
    {
        timeUntilNextRun = Random.Range(minTime, maxTime);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, endZ);
    }

    void Update()
    {
        if (this.transform.localPosition.z < endZ && timeUntilNextRun <= 0)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + (Time.deltaTime * speed));
        }
        else if (this.transform.localPosition.z >= endZ && timeUntilNextRun <= 0)
        {
            timeUntilNextRun = Random.Range(minTime, maxTime);
        }
        else
        {
            timeUntilNextRun -= Time.deltaTime;

            if (timeUntilNextRun <= 0)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startZ);
            }
        }
    }
}
