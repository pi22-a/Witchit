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
    private MeshFilter disguiseMeshFilter;                                      // �������� �������� �� ����� �׸� �޽��� ��� �޽�����
    private MeshRenderer disguiseMeshRenderer;                                  // �������� �������� �� ����� �׷��ִ� �޽�������
    private MeshCollider probMeshCollider;
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // ���� ����� �׷��ִ� �޽�������
    private GameObject disguiseProb;
    private Collider collider;                                                  // ���� ��� �ݶ��̴�, �������� �����ϸ� ����

    // ���� ���� ����
    private GameObject decoyProb;
    private bool isDecoying = false;

    // ���� ���� ����
    private GameObject curRayProb = null;                                       // ���� ī�޶� �߽ɿ� �ִ� ������Ʈ
    private GameObject prevRayProb = null;                                      // ������ ī�޶� �߽ɿ� �ִ� ������Ʈ

    // ����ĳ��Ʈ ���� ����
    private float rayDist = 50f;
    private RaycastHit hit;

    // �ڷ�ƾ ���� ����
    private Coroutine coroutine;

    private MeshCollider meshCollider;
    private PEA_WitchMovement witchMovement;

    // �����Ϳ��� �������� ����
    public GameObject witchBody;                                               // ���� ��� ��ü
    public GameObject mushRoom;
    public Transform probBody;                                                // ���� ��� ��ü

    public bool IsChanged
    {
        get { return isChanged; }
    }

    void Start()
    {
        // ����
        disguiseProb = probBody.GetChild(0).gameObject;
        disguiseMeshFilter = disguiseProb.GetComponent<MeshFilter>();
        disguiseMeshRenderer = disguiseProb.GetComponent<MeshRenderer>();
        collider = witchBody.GetComponent<Collider>();
        probMeshCollider = disguiseProb.GetComponent<MeshCollider>();

        skinnedMeshRenderer = witchBody.GetComponent<SkinnedMeshRenderer>();
        originMesh = skinnedMeshRenderer.sharedMesh;

        meshCollider = GetComponent<MeshCollider>();
        witchMovement = GetComponent<PEA_WitchMovement>();
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
            collider.enabled = false;
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }
        else
        {
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve();
        }

        curRayProb.GetComponent<Outline>().enabled = false;

        /*
        disguiseMeshFilter.mesh = curRayProb.GetComponent<MeshFilter>().mesh;
        meshCollider.sharedMesh = disguiseMeshFilter.mesh;
        disguiseMeshRenderer.materials = curRayProb.GetComponent<MeshRenderer>().materials;
        probMeshCollider.sharedMesh = disguiseMeshFilter.mesh;
        probMeshCollider.convex = true;
        */

        GameObject disguise = Instantiate(curRayProb, probBody);
        disguise.transform.localPosition = Vector3.zero;
        disguise.transform.localEulerAngles = Vector3.zero;
        meshCollider.sharedMesh = disguise.GetComponent<MeshFilter>().mesh;
        //Rigidbody disguiseRig = disguise.GetComponent<Rigidbody>();

        curRayProb.GetComponent<Outline>().enabled = true;
        witchMovement.SetProbRigidbody(curRayProb.GetComponent<Rigidbody>());

        //disguiseMeshFilter.mesh = curRayProb.GetComponent<MeshFilter>().mesh;
        //disguiseMeshRenderer.materials = curRayProb.GetComponent<MeshRenderer>().materials;

        //curRayProb.transform.SetParent(probBody);
        //curRayProb.transform.localPosition = witchBody.transform.localPosition;
        //curRayProb.transform.localEulerAngles = transform.localEulerAngles;

    }
    
    // ���� - ���� <-> ���� �ٲٱ�
    private void Possess()
    {

        // �������� �ƴ� ��
        if (!isChanged)
        {
            isChanged = true;
            collider.enabled = false;
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }

        // �������� ��
        else
        {
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve(5f);
            probBody.GetChild(0).SetParent(null);
        }

        transform.position = curRayProb.transform.position;
        witchMovement.SetProbRigidbody(curRayProb.GetComponent<Rigidbody>());
        meshCollider.sharedMesh = curRayProb.GetComponent<MeshFilter>().mesh;
        curRayProb.transform.SetParent(probBody);
        curRayProb.transform.localPosition = witchBody.transform.localPosition;
        curRayProb.transform.localEulerAngles = transform.localEulerAngles;

        //Disguise();

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
            print(coroutine != null);
            if(coroutine == null)
            {
                print("111111");
                coroutine = StartCoroutine(Dissolve(true));
                //meshCollider.sharedMesh = 
                witchMovement.SetProbRigidbody(null);
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
        print("Dissolve : " + visible);
        Material[] mats = skinnedMeshRenderer.materials;

        if (visible)
        {
            while (mats[0].GetFloat("_Cutoff") > 0)
            {
                print("Dissolve true");
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
            print("Dissolve true end");
            yield return null;

        }
        else
        {
            while (mats[0].GetFloat("_Cutoff") < 1)
            {
                print("Dissolve false");
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
            print("Dissolve false end");
            yield return null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        print("cc");
    }
}
