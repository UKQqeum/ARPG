using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]//�ν����Ϳ����� ���� �����ϰ�
    private float smoothRotationTime;//target ������ ȸ���ϴµ� �ɸ��� �ð�
    [SerializeField]
    private float smoothMoveTime;//target �ӵ��� �ٲ�µ� �ɸ��� �ð�
    [SerializeField]
    private float moveSpeed;//�����̴� �ӵ�
    private float rotationVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float speedVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float currentSpeed;
    private float targetSpeed;

    //public float moveSpeed;// �̵� �ӵ�
    private Rigidbody rb;
    public float jumpSpeed;// ���� �ӵ�

    private int jumpcnt = 0;// ���� ī��Ʈ ���� ������ ���� ����

    private Transform cameraTrans;

    public bool run;// ������ ��
    public bool Sperun;// �޸� ��
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
        //GetAxisRaw("Horizontal") :������ ����Ű������ 1�� ��ȯ, �ƹ��͵� �ȴ����� 0, ���ʹ���Ű�� -1 ��ȯ
        //GetAxis("Horizontal"):-1�� 1 ������ �Ǽ����� ��ȯ
        //Vertical�� ���ʹ���Ű ������ 1,�ƹ��͵� �ȴ����� 0, �Ʒ��ʹ���Ű�� -1 ��ȯ

        Vector2 inputDir = input.normalized;
        //���� ����ȭ. ���� input=new Vector2(1,1) �̸� �������� �밢������ �����δ�.
        //������ ã���ش�

        if (inputDir != Vector2.zero)//�������� ������ �� �ٽ� ó�� ������ ���ư��°� ��������
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTrans.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }
        //������ �����ִ� �ڵ�, �÷��̾ ������ �� �밢������ �����Ͻ� �� ������ �ٶ󺸰� ���ش�
        //Mathf.Atan2�� ������ return�ϱ⿡ �ٽ� ������ �ٲ��ִ� Mathf.Rad2Deg�� �����ش�
        //Vector.up�� y axis�� �ǹ��Ѵ�
        //SmoothDampAngle�� �̿��ؼ� �ε巴�� �÷��̾��� ������ �ٲ��ش�.

        targetSpeed = moveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        //���罺�ǵ忡�� Ÿ�ٽ��ǵ���� smoothMoveTime ���� ���Ѵ�
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        //run = false;
    }
    void Jump()// ���� ����
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpcnt < 1)
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                jumpcnt++;// ������ �� �� ����
            }
        }
    }

}
