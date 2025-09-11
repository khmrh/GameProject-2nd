using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Ÿ�ټ���")]
    public Transform target;

    [Header("ī�޶� �Ÿ� ����")]
    public float distance = 8.0f;
    public float height = 5.0f;

    [Header("���콺 ����")]
    public float mouseSensivitiy = 2.0f;
    public float minVectiaclAngle = -30.0f;
    public float maxVectiaclAngle = 60.0f;

    [Header("�ε巯�� ����")]
    public float positionSmoothTime = 0.2f;
    public float rotationSmoothTime = 0.1f;

    //ȸ������
    private float horizontalAnigle = 0.0f;
    private float verticalAnigle = 0.0f;

    //�����ӿ� ����
    private Vector3 currentVelocity;
    private Vector3 currentPosition;
    private Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) { ToggleCursor(); }
    }

    void LateUpdate()
    {
        if (target == null) return;
        HandleMouseInput();
        UpdateCameraSmooth();
    }

    void HandleMouseInput()
    {
        //Ŀ���� ����������� ���콺 �Է� ó��
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensivitiy;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivitiy;

        horizontalAnigle += mouseX;
        verticalAnigle -= mouseY;  

       verticalAnigle = Mathf.Clamp(verticalAnigle , minVectiaclAngle , maxVectiaclAngle);
    }

    void UpdateCameraSmooth()
    {
        Quaternion rotation = Quaternion.Euler(verticalAnigle, horizontalAnigle, 0);
        Vector3 rotateOffset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosstion = target.position + rotateOffset;    

        Vector3 looktarget = target.position + Vector3.up * height;
        Quaternion targetRotation = Quaternion.LookRotation(looktarget - targetPosstion);

        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosstion, ref currentVelocity, positionSmoothTime);

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime / rotationSmoothTime);

        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }

    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
