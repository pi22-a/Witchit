using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchSkill : MonoBehaviour
{
    // ������ ���� ����
    private float t = 0f;
    private float cutoff = 0f;
    private float dissolveSpeed = 3f;

    // ���� ���� ����
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;                                             // ������ �������� Ȯ��
    private bool isDissolving = false;
    private Mesh originMesh;
    private MeshFilter meshFilter;                                              
    private MeshRenderer meshRenderer;                                          // �������� �������� �� ����� �׷��ִ� �޽�������
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // ���� ����� �׷��ִ� �޽�������
    private PEA_WitchDissolve witchDissolve;

    // ���� ���� ����
    private GameObject decoyProb;
    private bool isDecoying = false;

    // ���� ���� ����
    private GameObject curRayProb = null;                                       // ���� ī�޶� �߽ɿ� �ִ� ������Ʈ
    private GameObject prevRayProb = null;                                      // ������ ī�޶� �߽ɿ� �ִ� ������Ʈ

    // ����ĳ��Ʈ ���� ����
    private float rayDist = 10f;
    private RaycastHit hit;

    // �ڷ�ƾ ���� ����
    private Coroutine coroutine;

    // �����Ϳ��� �������� ����
    public GameObject witchBody;                                               // ���� ��� ��ü
    public GameObject mushRoom;
    public Transform probBody;                                                // ���� ��� ��ü

    void Start()
    {
        witchDissolve = GetComponent<PEA_WitchDissolve>();
        meshFilter = witchBody.GetComponent<MeshFilter>();
        skinnedMeshRenderer = witchBody.GetComponent<SkinnedMeshRenderer>();
        originMesh = skinnedMeshRenderer.sharedMesh;
        meshRenderer = probBody.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        RayCamera();

        // ���콺 ��Ŭ�� - ����
        if (Input.GetMouseButtonDown(0) && curRayProb != null)
        {
            Disguise();
        }

        // ���콺 ��Ŭ�� - ����
        if (Input.GetMouseButtonDown(1) && curRayProb != null)
        {
            Possess();
        }

        // ���콺 �� ��� - ���󺹱�
        if (Input.GetMouseButton(2) && isChanged)
        {
            ReturnOrigin();
        }

        // ���콺 �� ���� �ð� �ʱ�ȭ
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
        }

        // Q - ���� ������
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowMushRoom();
        }


        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if(coroutine == null)
        //    {
        //        visible = !visible;
        //        print("dissolve, " + visible);
        //        coroutine = StartCoroutine(nameof(Dissolve));
        //    }
        //}
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
        if (!isChanged)
        {
            isChanged = true;
        }
        else
        {
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve();
        }

        //meshFilter.mesh = curRayProb.GetComponent<MeshRenderer>()
        curRayProb.transform.SetParent(probBody);
        curRayProb.transform.localPosition = witchBody.transform.localPosition;
        curRayProb.transform.localEulerAngles = transform.localEulerAngles;

        coroutine = StartCoroutine(Dissolve(false));
    }
    
    // ���� - ���� <-> ���� �ٲٱ�
    private void Possess()
    {

        // �������� �ƴ� ��
        if (!isChanged)
        {
            isChanged = true;
        }

        // �������� ��
        else
        {
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve(5f);
            probBody.GetChild(0).SetParent(null);
        }

        transform.position = curRayProb.transform.position;
        curRayProb.transform.SetParent(probBody);
        curRayProb.transform.localPosition = witchBody.transform.localPosition;
        curRayProb.transform.localEulerAngles = transform.localEulerAngles;

        //Disguise();
        coroutine = StartCoroutine(Dissolve(false));

        //curRayProb.SetActive(false);
    }

    // ���󺹱� - ������ ������� ���ư�
    private void ReturnOrigin()
    {
        curTime += Time.deltaTime;
        if(curTime >= returnTime)
        {
            //meshFilter.mesh = originMesh;
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve();
            if(coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(true));
                isChanged = false;
            }
        }
    }

    // ���� ������ - ������ ������ ȥ�������� ������ ����
    private void ThrowMushRoom()
    {
        Instantiate(mushRoom, transform.position + Camera.main.transform.forward * 2, transform.rotation);
    }

    // ���� - ȭ�� �߾ӿ� �̳��� ����� ������ ���͸� ������
    // �̳��� 5�� �ڿ� �����
    private void Decoy()
    {
        if (!isDecoying && curRayProb == null)
        {
            return;
        }
        else if(!isDecoying && curRayProb != null)
        {
            isDecoying = true;
            decoyProb = curRayProb;
        }
    }

    // ������
    IEnumerator Dissolve(bool visible)
    {
        print(visible);
        Material[] mats = skinnedMeshRenderer.materials;

        if (visible)
        {
            while (mats[0].GetFloat("_Cutoff") > 0)
            {
                cutoff -= Time.deltaTime * dissolveSpeed;
                foreach (Material m in mats)
                {
                    m.SetFloat("_Cutoff", cutoff);
                }
                t += Time.deltaTime;

                skinnedMeshRenderer.materials = mats;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            t = 0f;
            coroutine = null;
            yield return null;

        }
        else
        {
            while (mats[0].GetFloat("_Cutoff") < 1)
            {
                cutoff += Time.deltaTime * dissolveSpeed;
                foreach (Material m in mats)
                {
                    m.SetFloat("_Cutoff", cutoff);
                }
                t += Time.deltaTime;

                skinnedMeshRenderer.materials = mats;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            t = 0f;
            coroutine = null;
            yield return null;
        }
    }
}
