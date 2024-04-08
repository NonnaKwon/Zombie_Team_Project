using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform _follow;
    Vector3 _distance;

    private void Awake()
    {
        _follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _distance = transform.position - _follow.position;
    }
    private void Update()
    {
        transform.position = new Vector3(_follow.position.x, 0, _follow.position.z) + _distance;
    }
}
