using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchSkill : MonoBehaviour
{
    // 변신 관련 변수
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;
    private Mesh originMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // 프랍 관련 변수
    private GameObject curRayProb = null;                                       // 현재 카메라 중심에 있는 오브젝트
    private GameObject prevRayProb = null;                                      // 이전에 카메라 중심에 있던 오브젝트

    // 레이캐스트 관련 변수
    private float rayDist = 10f;
    private RaycastHit hit;

    // 에디터에서 연결해줄 변수
    public GameObject body;
    public GameObject mushRoom;

    void Start()
    {
        meshFilter = body.GetComponent<MeshFilter>();
        meshRenderer = body.GetComponent<MeshRenderer>();
        originMesh = meshFilter.mesh;
    }

    void Update()
    {
        RayCamera();
        if (Input.GetMouseButtonDown(0) && curRayProb != null)
        {
            Disguise();
        }

        if (Input.GetMouseButtonDown(1) && curRayProb != null)
        {
            Possess();
        }

        if (Input.GetMouseButton(2))
        {
            ReturnOrigin();
        }
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowMushRoom();
        }
    }

    private void RayCamera()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            print(hit.transform.gameObject.name);
            if(curRayProb != hit.transform.gameObject)
            {
                prevRayProb = curRayProb;

                if(hit.transform.CompareTag("Changable"))
                {
                    curRayProb = hit.transform.gameObject;
                }
                else
                {
                    curRayProb = null;                    
                }
                SetOutline();
            }
        }
    }

    private void SetOutline()
    {
        if(curRayProb != null)
        {
            curRayProb.GetComponent<Outline>().enabled = true;
        }

        if(prevRayProb != null)
        {
            prevRayProb.GetComponent<Outline>().enabled = false;
        }
    }

    //변장 - 마녀의 외형이 바뀜
    private void Disguise()
    {
        meshFilter.mesh = curRayProb.GetComponent<MeshFilter>().mesh;
    }
    
    // 빙의 - 마녀 <-> 프랍 바꾸기
    private void Possess()
    {
        Disguise();
        transform.position = curRayProb.transform.position;
        curRayProb.SetActive(false);
    }

    // 원상복귀 - 마녀의 모습으로 돌아감
    private void ReturnOrigin()
    {
        curTime += Time.deltaTime;
        if(curTime >= returnTime)
        {
            meshFilter.mesh = originMesh;
        }
    }

    // 버섯 던지기 - 밞으면 정신이 혼미해지는 버섯을 던짐
    private void ThrowMushRoom()
    {
        Instantiate(mushRoom, transform.position + Camera.main.transform.forward * 2, transform.rotation);
    }
}
