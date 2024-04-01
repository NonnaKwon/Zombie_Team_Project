using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] Vector3 _offset = new Vector3(0,10f,0);
    //�������� ������ ����Ʈ��
    bool _isActive;
    GameObject _uIData;
    Image _btnInfo;
    

    private void Start()
    {
        _isActive = false;
        _uIData = GetComponentInChildren<Canvas>().gameObject;
        _btnInfo = _uIData.GetComponentInChildren<Image>();
        _uIData.SetActive(false);
    }

    private void LateUpdate()
    {
        if(_isActive)
            _btnInfo.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);
    }

    public void OnActive()
    {
        _isActive = true;
        _uIData.SetActive(true);
    }

    public void OffActive()
    {
        _isActive = false;
        _uIData.SetActive(false);
    }
    public void Interact()
    {
        //�˾� ����
        Debug.Log("���� ��ȣ�ۿ�");
    }

    private void OnInteract(InputValue value)
    {
        if (_isActive)
            Interact();
    }

}
