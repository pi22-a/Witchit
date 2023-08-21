using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PEA_WitchSkill : MonoBehaviourPun
{
    public enum SoundEffect
    {
        Disguise,
        Return,
        Mushroom
    }

    // 디졸브 관련 변수
    private float t = 0f;
    private float cutoff = 0f;
    private float dissolveSpeed = 3f;

    // 변신 관련 변수
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;                                             // 변신한 상태인지 확인
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // 마녀 모습을 그려주는 메쉬렌더러
    private GameObject disguiseProb;

    // 유인 관련 변수
    private GameObject decoyProb;
    private bool isDecoying = false;

    // 프랍 관련 변수
    private int curRayProbIndex = -1;
    private GameObject curRayProb = null;                                       // 현재 카메라 중심에 있는 오브젝트
    private GameObject prevRayProb = null;                                      // 이전에 카메라 중심에 있던 오브젝트

    // 마나 관련 변수
    private readonly int possessMP = 50;
    private readonly int mushroomMP = 70;
    private PEA_WitchMP witchMP;

    // 레이캐스트 관련 변수
    //private float rayDist = 50f;
    private RaycastHit hit;

    // 코루틴 관련 변수
    private Coroutine coroutine;

    private PEA_WitchMovement witchMovement;
    private GameObject[] changableObjects;
    private AudioSource audioSource;

    // 에디터에서 연결해줄 변수
    public GameObject witchBody;                                               // 마녀 모습 몸체
    public GameObject mushRoom;
    public Transform probBody;                                                 // 프랍 모습 몸체
    private  PEA_Camera pea_camera;
    public CapsuleCollider witchCollider;                                      // 마녀 모습 콜라이더, 프랍으로 변신하면 꺼줌
    public MeshCollider probCollider;                                          // 프랍 모습 콜라이더, 마녀모습으로 변시하면 꺼줌
    public Image returnWitchImage;
    public PEA_SkillCooltime possessCooltime;
    public PEA_SkillCooltime mushroomCooltime;
    public AudioClip[] soundEffects;

    public bool IsChanged
    {
        get { return isChanged; }
    }

    void Start()
    {
        // 변장
        disguiseProb = probBody.GetChild(0).gameObject;

        skinnedMeshRenderer = witchBody.GetComponent<SkinnedMeshRenderer>();

        witchMovement = GetComponent<PEA_WitchMovement>();
        witchMP = GetComponent<PEA_WitchMP>();
        changableObjects = GameObject.FindGameObjectsWithTag("Changable");
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            RayCamera();
            GetInputKeys();
        }
    }

    public void SetPeaCamera(PEA_Camera camera)
    {
        pea_camera = camera;
    }

    // 입력에 따라 스킬 사용
    private void GetInputKeys()
    {
        // 마우스 좌클릭 - 변장
        if (Input.GetMouseButtonDown(0) && curRayProb != null)
        {
            pea_camera.SetCamPos(isChanged);
            photonView.RPC(nameof(Disguise), RpcTarget.All, curRayProbIndex);
            //Disguise();
        }

        // 마우스 우클릭 - 빙의
        if (Input.GetMouseButtonDown(1) && curRayProb != null)
        {
            pea_camera.SetCamPos(isChanged);
            photonView.RPC(nameof(Possess), RpcTarget.All, curRayProbIndex);
            //Possess();
        }

        // 마우스 휠 길게 - 원상복구
        if (Input.GetMouseButton(2) && isChanged)
        {
            //photonView.RPC(nameof(ReturnOrigin), RpcTarget.All);
            //ReturnOrigin();
            CheckTime();
            returnWitchImage.gameObject.SetActive(true);
        }

        // 마우스 휠 떼면 시간 초기화
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
            //photonView.RPC(nameof(ResetCurtime), RpcTarget.All);
            returnWitchImage.gameObject.SetActive(false);
        }

        // Q - 버섯 던지기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            photonView.RPC(nameof(ThrowMushRoom), RpcTarget.All, transform.position + Camera.main.transform.forward * 2, transform.rotation);
            //ThrowMushRoom();
        }
    }

    private void RayCamera()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if(curRayProb != hit.transform.gameObject)
            {
                if(curRayProbIndex >= 0)
                {
                    prevRayProb = curRayProb;
                }
                else
                {
                    prevRayProb = null;
                }

                for(int i = 0; i < changableObjects.Length; i++)
                {
                    if (hit.transform.gameObject == changableObjects[i])
                    {
                        curRayProb = hit.transform.gameObject;
                        curRayProbIndex = i;
                        break;
                    }
                }
                
                if(curRayProb != hit.transform.gameObject)
                {
                    curRayProbIndex = -1;
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
            //changableObjects[curRayProbIndex].GetComponent<Outline>().enabled = true;
        }

        if(prevRayProb != null)
        {
            prevRayProb.GetComponent<Outline>().enabled = false;
        }
    }

    [PunRPC]
    //변장 - 마녀의 외형이 바뀜
    private void Disguise(int probIndex)
    {
        if (!isChanged)
        {
            isChanged = true;
            witchCollider.enabled = false;
            probCollider.enabled = true;
            //pea_camera.SetCamPos(isChanged);
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }
        else
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve();
        }

        //curRayProb.GetComponent<Outline>().enabled = false;
        changableObjects[probIndex].GetComponent<Outline>().enabled = false;

        //GameObject prob =  Instantiate(curRayProb, probBody.position, curRayProb.transform.rotation, probBody);
        GameObject prob =  Instantiate(changableObjects[probIndex], probBody.position, changableObjects[probIndex].transform.rotation, probBody);
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
        prob.GetComponent<Rigidbody>().useGravity = false;
        prob.GetComponent<Collider>().enabled = false;
        prob.tag = "Witch";

        probCollider.transform.localScale = prob.transform.lossyScale;
        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;
        GetComponent<Rigidbody>().useGravity = false;
        probCollider.GetComponent<Rigidbody>().useGravity = true;
        PlaySoundEffect(SoundEffect.Disguise);

        //witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }
    
    [PunRPC]
    // 빙의 - 마녀 <-> 프랍 바꾸기
    private void Possess(int probIndex)
    {
        if(witchMP.MP < possessMP || !possessCooltime.Available)
        {
            return;
        }
        else
        {
            witchMP.UseMP(possessMP);
            possessCooltime.UseSkill();
        }

        // 변장중이 아닐 떄
        if (!isChanged)
        {
            isChanged = true;
            witchCollider.enabled = false;
            probCollider.enabled = true;
            //pea_camera.SetCamPos(isChanged);
            if (coroutine == null)
            {
                print("dissolve");
                coroutine = StartCoroutine(Dissolve(false));
            }
        }

        // 변장중일 때
        else
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve(5f);
            probBody.GetChild(1).SetParent(null);
        }

        transform.position = changableObjects[probIndex].transform.position;

        GameObject prob = changableObjects[probIndex];
        prob.transform.SetParent(probBody);
        prob.transform.position = probBody.transform.position;
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
        prob.GetComponent<Rigidbody>().useGravity = false;
        prob.GetComponent<Collider>().enabled = false;
        prob.tag = "Witch";

        probCollider.transform.localScale = prob.transform.lossyScale;
        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;
        GetComponent<Rigidbody>().useGravity = false;
        probCollider.GetComponent<Rigidbody>().useGravity = true;
        PlaySoundEffect(SoundEffect.Disguise);

        //witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }

    private void CheckTime()
    {
        curTime += Time.deltaTime;
        returnWitchImage.fillAmount = (curTime / returnTime);
        if (curTime >= returnTime && isChanged)
        {
            photonView.RPC(nameof(ReturnOrigin), RpcTarget.All);
            pea_camera.SetCamPos(isChanged);
        }
    }


    [PunRPC]
    // 원상복귀 - 마녀의 모습으로 돌아감
    private void ReturnOrigin()
    {
        //curTime += Time.deltaTime;
        //returnWitchImage.fillAmount = (curTime / returnTime);
        //if(curTime >= returnTime)
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve();
            probCollider.sharedMesh = null;
            probCollider.GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().useGravity = true;
            if(coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(true));
                witchCollider.enabled = true;
                isChanged = false;
                //pea_camera.SetCamPos(isChanged);
                PlaySoundEffect(SoundEffect.Return);
            }
        }
    }

    [PunRPC]
    // 버섯 던지기 - 밞으면 정신이 혼미해지는 버섯을 던짐
    private void ThrowMushRoom(Vector3 firePos, Quaternion fireRot)
    {
        print(name);
        if(witchMP.MP < mushroomMP || !mushroomCooltime.Available)
        {
            return;
        }
        else
        {
            witchMP.UseMP(mushroomMP);
            mushroomCooltime.UseSkill();
        }

        Instantiate(mushRoom, firePos, fireRot);
        PlaySoundEffect(SoundEffect.Mushroom);
    }

    [PunRPC ]
    private void ResetCurtime()
    {
        curTime = 0f;
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

    private void PlaySoundEffect(SoundEffect soundEffect)
    {
        audioSource.PlayOneShot(soundEffects[(int)soundEffect]);
    }

    public void WitchDissolve(bool visible)
    {
        if( coroutine == null)
        {
            coroutine = StartCoroutine(Dissolve(visible));
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
