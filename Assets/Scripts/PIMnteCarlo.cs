using UnityEngine;
using System;
public class PIMnteCarlos : MonoBehaviour
{
    public int NumberOfCirclePoints = 0;
    public float radius = 1.0f;
    public int numberOfPoints = 16;

    public bool doIt = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (doIt)
        {
            doIt = false;
            
            Debug.Log("Radius: " + radius);
            Debug.Log("Center of the circle (eg. 0,0): ");
        
            float cx = 0;
            float cy = 0;
            float scx, scy;
            scx = scy = cx + radius;

            for (int i = 0; i < numberOfPoints; i++)
            {
                float cxfloat = UnityEngine.Random.Range(cx, scx);
                float cyfloat = UnityEngine.Random.Range(cy, scy);
                // calculate the distance between the circle center and the given point.
                float distance = Mathf.Sqrt(Mathf.Pow(cxfloat - cx, 2) + Mathf.Pow(cyfloat - cy, 2));
                // If the distance is less or equal to the radius of the circle then the given point is inside the circle area.
                // Else the given point is outisde the circle area.
                if (distance <= radius)
                    NumberOfCirclePoints++;
            }
            float PI = 4 * (NumberOfCirclePoints / numberOfPoints);
            Debug.Log($"PI = " + PI);
        }
        
    }
}
