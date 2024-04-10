using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Camera mainCamera; // ���� ī�޶�
    public float transparency; // ������ ���� ��
    public LayerMask obstacleLayer; // ��ֹ� ���̾�

    private Shader transparentShader; // Transparent Shader
    private Dictionary<Renderer, Shader> originalShaders = new Dictionary<Renderer, Shader>(); // ��ü�� ���� Shader �����

    private void Start()
    {
        transparentShader = Shader.Find("TransParent");
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.position - mainCamera.transform.position;
        float distanceToPlayer = Vector3.Distance(player.position, mainCamera.transform.position);

        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // ��� �������� �ӽ÷� ������ ����Ʈ
        List<Renderer> renderers = new List<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                // ���� Shader�� ���� (ó�� ������ ����)
                if (!originalShaders.ContainsKey(hitRenderer))
                {
                    originalShaders[hitRenderer] = hitRenderer.material.shader;
                }

                // Transparent Shader�� �����ϰ� ���� ����
                hitRenderer.material.shader = transparentShader;
                Color color = hitRenderer.material.color;
                color.a = transparency;
                hitRenderer.material.color = color;

                renderers.Add(hitRenderer);
            }
        }

        // ���� ���̴��� �����ؾ� �ϴ� ������ ó��
        foreach (var pair in new Dictionary<Renderer, Shader>(originalShaders))
        {
            if (!renderers.Contains(pair.Key))
            {
                pair.Key.material.shader = pair.Value;
                Color color = pair.Key.material.color;
                color.a = 1.0f; // ���� ������ ����
                pair.Key.material.color = color;

                // ������ �������� �������� ����
                originalShaders.Remove(pair.Key);
            }
        }
    }
}
