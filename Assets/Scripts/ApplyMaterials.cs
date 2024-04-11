using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMaterial : MonoBehaviour
{
    public Material newMaterial; // Inspector���� �Ҵ��� Material ����

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>(); // ���� GameObject�� Renderer ������Ʈ�� ������
        if (renderer != null)
        {
            renderer.material = newMaterial; // newMaterial�� Renderer�� Material�� ����
        }
    }
}
