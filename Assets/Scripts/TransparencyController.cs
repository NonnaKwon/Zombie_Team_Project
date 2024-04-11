using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Camera mainCamera; // 메인 카메라
    public float transparency; // 적용할 투명도 값
    public LayerMask obstacleLayer; // 장애물 레이어

    [SerializeField] Material transparentMaterial; // Transparent Material
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>(); // 객체의 원래 Shader 저장용

    private void Update()
    {
        Vector3 directionToPlayer = player.position - mainCamera.transform.position;
        float distanceToPlayer = Vector3.Distance(player.position, mainCamera.transform.position);

        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // 모든 렌더러를 임시로 저장할 리스트
        List<Renderer> renderers = new List<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                // 기존 Shader를 저장 (처음 감지될 때만)
                if (!originalMaterials.ContainsKey(hitRenderer))
                {
                    originalMaterials[hitRenderer] = hitRenderer.material;
                }

                // Transparent Shader로 변경하고 투명도 적용
                hitRenderer.material = transparentMaterial;
                renderers.Add(hitRenderer);
            }
        }

        // 원래 셰이더로 복원해야 하는 렌더러 처리
        foreach (var pair in new Dictionary<Renderer, Material>(originalMaterials))
        {
            if (!renderers.Contains(pair.Key))
            {
                pair.Key.material = pair.Value;
                // 복원한 렌더러는 사전에서 제거
                originalMaterials.Remove(pair.Key);
            }
        }
    }

    /*private Shader transparentShader; // Transparent Shader
    private Dictionary<Renderer, Shader> originalShaders = new Dictionary<Renderer, Shader>(); // 객체의 원래 Shader 저장용

    private void Start()
    {
        transparentShader = Shader.Find("Assets/Resources/Materials/TransParent1");
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.position - mainCamera.transform.position;
        float distanceToPlayer = Vector3.Distance(player.position, mainCamera.transform.position);

        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // 모든 렌더러를 임시로 저장할 리스트
        List<Renderer> renderers = new List<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                // 기존 Shader를 저장 (처음 감지될 때만)
                if (!originalShaders.ContainsKey(hitRenderer))
                {
                    originalShaders[hitRenderer] = hitRenderer.material.shader;
                }

                // Transparent Shader로 변경하고 투명도 적용
                hitRenderer.material.shader = transparentShader;
                Color color = hitRenderer.material.color;
                color.a = transparency;
                hitRenderer.material.color = color;

                renderers.Add(hitRenderer);
            }
        }

        // 원래 셰이더로 복원해야 하는 렌더러 처리
        foreach (var pair in new Dictionary<Renderer, Shader>(originalShaders))
        {
            if (!renderers.Contains(pair.Key))
            {
                pair.Key.material.shader = pair.Value;
                Color color = pair.Key.material.color;
                color.a = 1.0f; // 원래 투명도로 복원
                pair.Key.material.color = color;

                // 복원한 렌더러는 사전에서 제거
                originalShaders.Remove(pair.Key);
            }
        }
    }*/
}
