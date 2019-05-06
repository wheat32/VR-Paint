using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float speed = 5f;
    public float startingRotation = 30f;
    private float xVal = 0;

    public void Start()
    {
        xVal = startingRotation;
    }

    // Update is called once per frame
    void Update()
    {
        xVal += Time.deltaTime * speed;
        this.transform.localRotation = Quaternion.Euler(xVal, 0, 0);
    }
}
