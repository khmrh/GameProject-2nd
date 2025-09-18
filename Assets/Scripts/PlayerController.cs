using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("이동설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("점프 설정")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.8f;                       //중력가속도 추가
    public float landingDuration = 0.3f;                //착지 후 찾지 모션 지속 시간 (헤당 시족시간 이후에 캐릭터가 움직일수 있게

    [Header("공격 설정")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("커포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //현재 상태 값들
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

    void HandleMovement()            //이동 함수 재작
    {

        //공격중이거나 착지 중일떄 움직임 제한
        if (isAttacking && !canMoveWhileAttacking || isLanding)
        {
            currenSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)
        {
            //카메라가 보는 방향의 앞쪽으로 설정
            Vector3 carmeraFoward = playerCamera.transform.forward;
            Vector3 carmeraRight = playerCamera.transform.right;
            carmeraFoward.y = 0;
            carmeraRight.y = 0;
            carmeraFoward.Normalize();
            carmeraRight.Normalize();

            Vector3 moveDirection = carmeraFoward * verical + carmeraRight * horizontal;        //이동방향 설정

            if (Input.GetKey(KeyCode.LeftShift))                                //왼Shift를 눌러서 
            {
                currenSpeed = runSpeed;
            }
            else
            {
                currenSpeed = walkSpeed;
            }
            controller.Move(moveDirection * currenSpeed * Time.deltaTime);      //캐릭터 컨트롤러의 이동입력

            //이동 진행을 바라보며 이동
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
        //전체 최대 속도 기준
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
            Debug.Log("떨어지기 시작");
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
