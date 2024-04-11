using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    private Color initialColor;
    private Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        initialColor = material.color;
    }

    public void MakeTransparent()
    {
        Color color = initialColor;
        color.a = 0.3f;
        material.color = color;
    }

    public void MakeOpaque()
    {
        material.color = initialColor;
    }
}
