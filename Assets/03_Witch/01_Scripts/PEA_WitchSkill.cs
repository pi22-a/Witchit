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

    // ������ ���� ����
    private float t = 0f;
    private float cutoff = 0f;
    private float dissolveSpeed = 3f;

    // ���� ���� ����
    private float curTime = 0f;
    private readonly float returnTime = 1f;
    private bool isChanged = false;                                             // ������ �������� Ȯ��
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // ���� ����� �׷��ִ� �޽�������
    private GameObject disguiseProb;

    // ���� ���� ����
    private GameObject decoyProb;
    private bool isDecoying = false;

    // ���� ���� ����
    private int curRayProbIndex = -1;
    private GameObject curRayProb = null;                                       // ���� ī�޶� �߽ɿ� �ִ� ������Ʈ
    private GameObject prevRayProb = null;                                      // ������ ī�޶� �߽ɿ� �ִ� ������Ʈ

    // ���� ���� ����
    private readonly int possessMP = 50;
    private readonly int mushroomMP = 70;
    private PEA_WitchMP witchMP;

    // ����ĳ��Ʈ ���� ����
    //private float rayDist = 50f;
    private RaycastHit hit;

    // �ڷ�ƾ ���� ����
    private Coroutine coroutine;

    private PEA_WitchMovement witchMovement;
    private GameObject[] changableObjects;
    private AudioSource audioSource;

    // �����Ϳ��� �������� ����
    public GameObject witchBody;                                               // ���� ��� ��ü
    public GameObject mushRoom;
    public Transform probBody;                                                 // ���� ��� ��ü
    private  PEA_Camera pea_camera;
    public CapsuleCollider witchCollider;                                      // ���� ��� �ݶ��̴�, �������� �����ϸ� ����
    public MeshCollider probCollider;                                          // ���� ��� �ݶ��̴�, ���������� �����ϸ� ����
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
        // ����
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

    // �Է¿� ���� ��ų ���
    private void GetInputKeys()
    {
        // ���콺 ��Ŭ�� - ����
        if (Input.GetMouseButtonDown(0) && curRayProb != null)
        {
            pea_camera.SetCamPos(isChanged);
            photonView.RPC(nameof(Disguise), RpcTarget.All, curRayProbIndex);
            //Disguise();
        }

        // ���콺 ��Ŭ�� - ����
        if (Input.GetMouseButtonDown(1) && curRayProb != null)
        {
            pea_camera.SetCamPos(isChanged);
            photonView.RPC(nameof(Possess), RpcTarget.All, curRayProbIndex);
            //Possess();
        }

        // ���콺 �� ��� - ���󺹱�
        if (Input.GetMouseButton(2) && isChanged)
        {
            //photonView.RPC(nameof(ReturnOrigin), RpcTarget.All);
            //ReturnOrigin();
            CheckTime();
            returnWitchImage.gameObject.SetActive(true);
        }

        // ���콺 �� ���� �ð� �ʱ�ȭ
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
            //photonView.RPC(nameof(ResetCurtime), RpcTarget.All);
            returnWitchImage.gameObject.SetActive(false);
        }

        // Q - ���� ������
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
    //���� - ������ ������ �ٲ�
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
    // ���� - ���� <-> ���� �ٲٱ�
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

        // �������� �ƴ� ��
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

        // �������� ��
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
    // ���󺹱� - ������ ������� ���ư�
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
    // ���� ������ - ������ ������ ȥ�������� ������ ����
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

    // ������
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
