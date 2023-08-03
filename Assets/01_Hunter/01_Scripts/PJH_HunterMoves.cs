using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_HunterMoves : MonoBehaviour
{
    int jumpCount;
    public int maxJumpCount = 1;

    public float jumpPower = 10;
    public float gravity = -9.81f;
    float yVelocity;

    CharacterController cc;

    public float speed = 5;
    Camera cam;  // cache
    void Start()
    {
        // ��ü���� CharacterController ������Ʈ�� ������ʹ�.
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // �߷��� ���� y�ӵ��� �ۿ��ؾ��Ѵ�.
        // 9.81 m/s
        yVelocity += gravity * Time.deltaTime;

        // ���� ��Ҵٸ� ����ī��Ʈ�� �ʱ�ȭ �ϰ�ʹ�.
        if (cc.isGrounded)
        {
            jumpCount = 0;
            // ���� ���������� y�ӵ��� ��ȭ���� �ʰ��ϰ�ʹ�.
            yVelocity = 0;
        }
        // ���� ����ī��Ʈ�� �ִ� ���� �۴� �׸��� ����ڰ� ������ư�� ������
        if (jumpCount < maxJumpCount && Input.GetButtonDown("Jump"))
        {
            // JumpPower�� y�ӵ��� �ۿ��ؾ��Ѵ�.
            yVelocity = jumpPower;
            jumpCount++;
        }
        // 1. ������� �Է¿�����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. �յ��¿� ������ �����
        Vector3 dir = new Vector3(h, 0, v);
        // 3. ���� ������ ī�޶��� �չ����� �������� ��ȯ�ϰ�ʹ�.
        dir = cam.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();// ������ ����ȭ

        // ������ y�ӵ��� dir�� y�׸� �ݿ��Ǿ���Ѵ�.
        Vector3 velocity = dir * speed;
        velocity.y = yVelocity;
        // 4. �� �������� �̵��ϰ�ʹ�.
        cc.Move(velocity * Time.deltaTime);
    }
}
