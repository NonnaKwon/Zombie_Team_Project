using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Camera mainCamera; // 메인 카메라
    public float transparency; // 적용할 투명도 값
    public LayerMask obstacleLayer;

    private Shader transparentShader; // Transparent Shader
    private Shader originalShader; // 객체의 원래 Shader 저장용

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
                // 기존 Shader를 저장 (처음 감지될 때만)
                if (hitRenderer.material.shader != transparentShader)
                {
                    originalShader = hitRenderer.material.shader;
                }

                // Transparent Shader로 변경하고 투명도 적용
                hitRenderer.material.shader = transparentShader;
                Color color = hitRenderer.material.color;
                color.a = transparency;
                hitRenderer.material.color = color;
            }
        }
    }
}
