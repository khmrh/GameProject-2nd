using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("�̵�����")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("���� ����")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.8f;                       //�߷°��ӵ� �߰�
    public float landingDuration = 0.3f;                //���� �� ã�� ��� ���� �ð� (��� �����ð� ���Ŀ� ĳ���Ͱ� �����ϼ� �ְ�

    [Header("���� ����")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("Ŀ����Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //���� ���� ����
    private float currenSpeed;
    private bool isAttacking = false;
    private bool isLanding = false;
    private float landingTimer;
        
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        UpdateAnimator();
        HandleAttack();
        HandleJump();   
    }

    void HandleMovement()            //�̵� �Լ� ����
    {

        //�������̰ų� ���� ���ϋ� ������ ����
        if (isAttacking && !canMoveWhileAttacking || isLanding)
        {
            currenSpeed = 0;
            return;
        }

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
        animator.SetBool("isGrounded",isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;
        animator.SetBool("isFalling",isFalling);
        animator.SetBool("isLanding",isLanding);
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (!isGrounded && wasGrounded)
        {
            Debug.Log("�������� ����");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;

            if (!wasGrounded && animator != null)
            {
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    void HandleJump()
    {

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;

            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R  ) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }
}
