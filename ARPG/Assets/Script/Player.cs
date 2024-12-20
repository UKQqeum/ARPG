using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]//인스펙터에서만 참조 가능하게
    private float smoothRotationTime;//target 각도로 회전하는데 걸리는 시간
    [SerializeField]
    private float smoothMoveTime;//target 속도로 바뀌는데 걸리는 시간
    [SerializeField]
    private float moveSpeed;//움직이는 속도
    private float rotationVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float speedVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float currentSpeed;
    private float targetSpeed;

    //public float moveSpeed;// 이동 속도
    private Rigidbody rb;
    public float jumpSpeed;// 점프 속도

    private int jumpcnt = 0;// 점프 카운트 연속 점프를 막기 위함

    private Transform cameraTrans;

    public bool run;// 움직일 때
    public bool Sperun;// 달릴 때
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTrans = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //if (Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    anim.SetBool("SpedeRun", true);
        //    anim.SetBool("Run", false);
        //    Move();
        //}
        //else
        //{
        //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A)
        //     || Input.GetKeyDown(KeyCode.D))
        //    {
        //        anim.SetBool("Run", true);
        //        anim.SetBool("SpedeRun", false);
        //        Move();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //    Move();
        //if (Input.GetKeyDown(KeyCode.A))
        //    Move();
        //if (Input.GetKeyDown(KeyCode.D))
        //    Move();
        Jump();
    }
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            anim.SetBool("SpedeRun", true);
        }
        else
        {
            anim.SetBool("SpedeRun", false);
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A)
             || Input.GetKeyDown(KeyCode.D))
            {
                anim.SetBool("Run", true);
                anim.SetBool("SpedeRun", false);
                Move();
            }
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //GetAxisRaw("Horizontal") :오른쪽 방향키누르면 1을 반환, 아무것도 안누르면 0, 왼쪽방향키는 -1 반환
        //GetAxis("Horizontal"):-1과 1 사이의 실수값을 반환
        //Vertical은 위쪽방향키 누를시 1,아무것도 안누르면 0, 아래쪽방향키는 -1 반환

        Vector2 inputDir = input.normalized;
        //벡터 정규화. 만약 input=new Vector2(1,1) 이면 오른쪽위 대각선으로 움직인다.
        //방향을 찾아준다

        if (inputDir != Vector2.zero)//움직임을 멈췄을 때 다시 처음 각도로 돌아가는걸 막기위함
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTrans.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }
        //각도를 구해주는 코드, 플레이어가 오른쪽 위 대각선으로 움직일시 그 방향을 바라보게 해준다
        //Mathf.Atan2는 라디안을 return하기에 다시 각도로 바꿔주는 Mathf.Rad2Deg를 곱해준다
        //Vector.up은 y axis를 의미한다
        //SmoothDampAngle을 이용해서 부드럽게 플레이어의 각도를 바꿔준다.

        targetSpeed = moveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        //현재스피드에서 타겟스피드까지 smoothMoveTime 동안 변한다
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        //run = false;
    }
    void Jump()// 점프 관련
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpcnt < 1)
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                jumpcnt++;// 점프를 한 번 했음
            }
        }
    }

}
