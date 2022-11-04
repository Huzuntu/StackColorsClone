using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColor : MonoBehaviour
{
    [SerializeField] Color newColor;

    void Start()
    {
        Color tempColor = newColor;
        tempColor.a = 0.5f;
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material.SetColor("_Color", tempColor);
    }
    public Color GetColor()
    {
        return newColor;
    }

}
