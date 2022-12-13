using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    float timeCountForForce;
        
    public void TrailSetColor(Color changeTrailColor)
    {
        Color fullChangeTrailColor = changeTrailColor;
        fullChangeTrailColor.a = 0.9f;
        SetTrails(fullChangeTrailColor, changeTrailColor);
    }

    void SetTrails(Color starterColor, Color enderColor)
    {
        foreach(TrailRenderer trails in this.gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            trails.startColor = starterColor;
            trails.endColor = starterColor;
        }
    }

    public void EnableDisableTrails(bool enabled)
    {
        foreach(TrailRenderer trails in this.gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            trails.enabled = enabled;
        }
    }
    
    public void SetTrailTime(float timeToChange)
    {
        timeCountForForce += Time.deltaTime;
        foreach (TrailRenderer trails in this.gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            if(timeCountForForce < timeToChange)
            {
                trails.time = 0.7f;
            }
            else 
            {
                trails.time = 0.3f;
            }
        }
    }
}