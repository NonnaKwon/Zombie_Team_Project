using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Camera mainCamera; // ���� ī�޶�
    public float transparency; // ������ ���� ��
    public LayerMask obstacleLayer;

    private Shader transparentShader; // Transparent Shader
    private Shader originalShader; // ��ü�� ���� Shader �����

    private void Start()
    {
        transparentShader = Shader.Find("Transparent/Diffuse");
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.position - mainCamera.transform.position;
        float distanceToPlayer = Vector3.Distance(player.position, mainCamera.transform.position);

        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                // ���� Shader�� ���� (ó�� ������ ����)
                if (hitRenderer.material.shader != transparentShader)
                {
                    originalShader = hitRenderer.material.shader;
                }

                // Transparent Shader�� �����ϰ� ���� ����
                hitRenderer.material.shader = transparentShader;
                Color color = hitRenderer.material.color;
                color.a = transparency;
                hitRenderer.material.color = color;
            }
        }
    }
}
