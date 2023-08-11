using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchSkill : MonoBehaviour
{
    // 디졸브 관련 변수
    private float t = 0f;
    private float cutoff = 0f;
    private float dissolveSpeed = 3f;

    // 변신 관련 변수
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;                                             // 변신한 상태인지 확인
    private MeshFilter disguiseMeshFilter;                                      // 프랍으로 변신했을 때 모습을 그릴 메쉬를 담는 메쉬필터
    private MeshRenderer disguiseMeshRenderer;                                  // 프랍으로 변신했을 때 모습을 그려주는 메쉬렌더러
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // 마녀 모습을 그려주는 메쉬렌더러
    private GameObject disguiseProb;
    private MeshCollider probCollider;                                          // 프랍 모습 콜라이더, 마녀모습으로 변시하면 꺼줌
    private CapsuleCollider witchCollider;                                      // 마녀 모습 콜라이더, 프랍으로 변신하면 꺼줌

    // 유인 관련 변수
    private GameObject decoyProb;
    private bool isDecoying = false;

    // 프랍 관련 변수
    private GameObject curRayProb = null;                                       // 현재 카메라 중심에 있는 오브젝트
    private GameObject prevRayProb = null;                                      // 이전에 카메라 중심에 있던 오브젝트

    // 레이캐스트 관련 변수
    private float rayDist = 50f;
    private RaycastHit hit;

    // 코루틴 관련 변수
    private Coroutine coroutine;

    private MeshCollider meshCollider;
    private PEA_WitchMovement witchMovement;

    // 에디터에서 연결해줄 변수
    public GameObject witchBody;                                               // 마녀 모습 몸체
    public GameObject mushRoom;
    public Transform probBody;                                                // 프랍 모습 몸체

    public bool IsChanged
    {
        get { return isChanged; }
    }

    void Start()
    {
        // 변장
        disguiseProb = probBody.GetChild(0).gameObject;
        disguiseMeshFilter = disguiseProb.GetComponent<MeshFilter>();
        disguiseMeshRenderer = disguiseProb.GetComponent<MeshRenderer>();
        witchCollider = GetComponent<CapsuleCollider>();
        probCollider = GetComponent<MeshCollider>();

        skinnedMeshRenderer = witchBody.GetComponent<SkinnedMeshRenderer>();

        meshCollider = GetComponent<MeshCollider>();
        witchMovement = GetComponent<PEA_WitchMovement>();
    }

    void Update()
    {
        RayCamera();

        // 마우스 좌클릭 - 변장
        if (Input.GetMouseButtonDown(0) && curRayProb != null)
        {
            Disguise();
        }

        // 마우스 우클릭 - 빙의
        if (Input.GetMouseButtonDown(1) && curRayProb != null)
        {
            Possess();
        }

        // 마우스 휠 길게 - 원상복구
        if (Input.GetMouseButton(2) && isChanged)
        {
            ReturnOrigin();
        }

        // 마우스 휠 떼면 시간 초기화
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
        }

        // Q - 버섯 던지기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowMushRoom();
        }
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

    //변장 - 마녀의 외형이 바뀜
    private void Disguise()
    {
        if (!isChanged)
        {
            isChanged = true;
            witchCollider.enabled = false;
            probCollider.enabled = true;
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }
        else
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve();
        }

        curRayProb.GetComponent<Outline>().enabled = false;

        GameObject prob =  Instantiate(curRayProb, probBody.position, curRayProb.transform.rotation, probBody);
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        prob.GetComponent<Rigidbody>().useGravity = true;
        prob.GetComponent<Collider>().enabled = false;

        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;

        witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }
    
    // 빙의 - 마녀 <-> 프랍 바꾸기
    private void Possess()
    {

        // 변장중이 아닐 떄
        if (!isChanged)
        {
            isChanged = true;
            witchCollider.enabled = false;
            probCollider.enabled = true;
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }

        // 변장중일 때
        else
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve(5f);
            probBody.GetChild(1).SetParent(null);
        }

        transform.position = curRayProb.transform.position;

        GameObject prob = curRayProb;
        prob.transform.SetParent(probBody);
        prob.transform.position = probBody.transform.position;
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        prob.GetComponent<Rigidbody>().useGravity = true;
        prob.GetComponent<Collider>().enabled = false;

        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;

        witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }

    // 원상복귀 - 마녀의 모습으로 돌아감
    private void ReturnOrigin()
    {
        curTime += Time.deltaTime;
        if(curTime >= returnTime)
        {
            probBody.GetChild(0).GetComponent<PEA_ProbDissolve>().ProbDissolve();
            print(coroutine != null);
            if(coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(true));
                witchMovement.SetProbRigidbody(null);
                isChanged = false;
            }
        }
    }

    // 버섯 던지기 - 밞으면 정신이 혼미해지는 버섯을 던짐
    private void ThrowMushRoom()
    {
        Instantiate(mushRoom, transform.position + Camera.main.transform.forward * 2, transform.rotation);
    }

    // 유인 - 화면 중앙에 미끼를 만들고 조종해 헌터를 유인함
    // 미끼는 5초 뒤에 사라짐
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

    // 디졸브
    IEnumerator Dissolve(bool visible)
    {
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
