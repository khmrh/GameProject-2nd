using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("공격 설정")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("커포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //현재 상태 값들
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

    void HandleMovement()            //이동 함수 재작
    {
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
    }
}
