using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵�����")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("���� ����")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("Ŀ����Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //���� ���� ����
    private float currenSpeed;
    private bool isAttackin = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateAnimator();

    }

    void HandleMovement()            //�̵� �Լ� ����
    {
        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)
        {
            //ī�޶� ���� ������ �������� ����
            Vector3 carmeraFoward = playerCamera.transform.forward;
            Vector3 carmeraRight = playerCamera.transform.right;
            carmeraFoward.y = 0;
            carmeraRight.y = 0;
            carmeraFoward.Normalize();
            carmeraRight.Normalize();

            Vector3 moveDirection = carmeraFoward * verical + carmeraRight * horizontal;        //�̵����� ����

            if (Input.GetKey(KeyCode.LeftShift))                                //��Shift�� ������ 
            {
                currenSpeed = runSpeed;
            }
            else
            {
                currenSpeed = walkSpeed;
            }
            controller.Move(moveDirection * currenSpeed * Time.deltaTime);      //ĳ���� ��Ʈ�ѷ��� �̵��Է�

            //�̵� ������ �ٶ󺸸� �̵�
            Quaternion tagetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation , tagetRotation, rotationSpeed *  Time.deltaTime);
        }
        else
        {
            currenSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        //��ü �ִ� �ӵ� ����
        float animatorSpeed = Mathf.Clamp01(currenSpeed / runSpeed);
        animator.SetFloat("Speed", animatorSpeed);
    }
}
