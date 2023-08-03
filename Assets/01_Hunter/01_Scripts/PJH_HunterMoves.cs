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
        // 본체에게 CharacterController 컴포넌트를 얻어오고싶다.
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 중력의 힘이 y속도에 작용해야한다.
        // 9.81 m/s
        yVelocity += gravity * Time.deltaTime;

        // 땅에 닿았다면 점프카운트를 초기화 하고싶다.
        if (cc.isGrounded)
        {
            jumpCount = 0;
            // 땅에 서있을때는 y속도가 변화하지 않게하고싶다.
            yVelocity = 0;
        }
        // 만약 점프카운트가 최대 보다 작다 그리고 사용자가 점프버튼을 누르면
        if (jumpCount < maxJumpCount && Input.GetButtonDown("Jump"))
        {
            // JumpPower가 y속도에 작용해야한다.
            yVelocity = jumpPower;
            jumpCount++;
        }
        // 1. 사용자의 입력에따라
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 앞뒤좌우 방향을 만들고
        Vector3 dir = new Vector3(h, 0, v);
        // 3. 현재 방향을 카메라의 앞방향을 기준으로 변환하고싶다.
        dir = cam.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();// 벡터의 정규화

        // 결정된 y속도를 dir의 y항목에 반영되어야한다.
        Vector3 velocity = dir * speed;
        velocity.y = yVelocity;
        // 4. 그 방향으로 이동하고싶다.
        cc.Move(velocity * Time.deltaTime);
    }
}
