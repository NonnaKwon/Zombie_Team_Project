using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        renderer.material.shader = Shader.Find("TransParent");

        // 반투명으로 만듬
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.5f);
    }
}
