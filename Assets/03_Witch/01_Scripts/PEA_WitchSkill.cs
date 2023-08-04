using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchSkill : MonoBehaviour
{
    // ���� ���� ����
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;
    private Mesh originMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // ���� ���� ����
    private GameObject curRayProb = null;                                       // ���� ī�޶� �߽ɿ� �ִ� ������Ʈ
    private GameObject prevRayProb = null;                                      // ������ ī�޶� �߽ɿ� �ִ� ������Ʈ

    // ����ĳ��Ʈ ���� ����
    private float rayDist = 10f;
    private RaycastHit hit;

    // �����Ϳ��� �������� ����
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

    //���� - ������ ������ �ٲ�
    private void Disguise()
    {
        meshFilter.mesh = curRayProb.GetComponent<MeshFilter>().mesh;
    }
    
    // ���� - ���� <-> ���� �ٲٱ�
    private void Possess()
    {
        Disguise();
        transform.position = curRayProb.transform.position;
        curRayProb.SetActive(false);
    }

    // ���󺹱� - ������ ������� ���ư�
    private void ReturnOrigin()
    {
        curTime += Time.deltaTime;
        if(curTime >= returnTime)
        {
            meshFilter.mesh = originMesh;
        }
    }

    // ���� ������ - ������ ������ ȥ�������� ������ ����
    private void ThrowMushRoom()
    {
        Instantiate(mushRoom, transform.position + Camera.main.transform.forward * 2, transform.rotation);
    }
}
