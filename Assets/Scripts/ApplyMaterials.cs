using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMaterial : MonoBehaviour
{
    public Material newMaterial; // Inspector에서 할당할 Material 변수

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>(); // 현재 GameObject의 Renderer 컴포넌트를 가져옴
        if (renderer != null)
        {
            renderer.material = newMaterial; // newMaterial로 Renderer의 Material을 변경
        }
    }
}
