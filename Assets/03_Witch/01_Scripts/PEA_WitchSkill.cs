using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private SkinnedMeshRenderer skinnedMeshRenderer;                            // ���� ����� �׷��ִ� �޽�������
    private GameObject disguiseProb;

    // ���� ���� ����
    private GameObject decoyProb;
    private bool isDecoying = false;

    // ���� ���� ����
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

    // �����Ϳ��� �������� ����
    public GameObject witchBody;                                               // ���� ��� ��ü
    public GameObject mushRoom;
    public Transform probBody;                                                // ���� ��� ��ü
    public PEA_Camera pea_camera;
    public CapsuleCollider witchCollider;                                      // ���� ��� �ݶ��̴�, �������� �����ϸ� ����
    public MeshCollider probCollider;                                          // ���� ��� �ݶ��̴�, ���������� �����ϸ� ����
    public Image returnWitchImage;
    public PEA_SkillCooltime possessCooltime;
    public PEA_SkillCooltime mushroomCooltime;

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
    }

    void Update()
    {
        RayCamera();
        GetInputKeys();
    }

    // �Է¿� ���� ��ų ���
    private void GetInputKeys()
    {
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
            returnWitchImage.gameObject.SetActive(true);
        }

        // ���콺 �� ���� �ð� �ʱ�ȭ
        else if (Input.GetMouseButtonUp(2))
        {
            curTime = 0f;
            returnWitchImage.gameObject.SetActive(false);
        }

        // Q - ���� ������
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

    //���� - ������ ������ �ٲ�
    private void Disguise()
    {
        if (!isChanged)
        {
            isChanged = true;
            witchCollider.enabled = false;
            probCollider.enabled = true;
            pea_camera.SetCamPos(isChanged);
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
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
        prob.GetComponent<Rigidbody>().useGravity = false;
        prob.GetComponent<Collider>().enabled = false;

        probCollider.transform.localScale = prob.transform.lossyScale;
        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;
        GetComponent<Rigidbody>().useGravity = false;
        probCollider.GetComponent<Rigidbody>().useGravity = true;

        //witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }
    
    // ���� - ���� <-> ���� �ٲٱ�
    private void Possess()
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
            pea_camera.SetCamPos(isChanged);
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(false));
            }
        }

        // �������� ��
        else
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve(5f);
            probBody.GetChild(1).SetParent(null);
        }

        transform.position = curRayProb.transform.position;

        GameObject prob = curRayProb;
        prob.transform.SetParent(probBody);
        prob.transform.position = probBody.transform.position;
        prob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
        prob.GetComponent<Rigidbody>().useGravity = false;
        prob.GetComponent<Collider>().enabled = false;

        probCollider.transform.localScale = prob.transform.lossyScale;
        probCollider.sharedMesh = prob.GetComponent<MeshFilter>().mesh;
        GetComponent<Rigidbody>().useGravity = false;
        probCollider.GetComponent<Rigidbody>().useGravity = true;

        //witchMovement.SetProbRigidbody(prob.GetComponent<Rigidbody>());
    }

    // ���󺹱� - ������ ������� ���ư�
    private void ReturnOrigin()
    {
        curTime += Time.deltaTime;
        returnWitchImage.fillAmount = (curTime / returnTime);
        if(curTime >= returnTime)
        {
            probBody.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve();
            probCollider.sharedMesh = null;
            GetComponent<Rigidbody>().useGravity = true;
            if(coroutine == null)
            {
                coroutine = StartCoroutine(Dissolve(true));
                witchCollider.enabled = true;
                isChanged = false;
                pea_camera.SetCamPos(isChanged);
            }
        }
    }

    // ���� ������ - ������ ������ ȥ�������� ������ ����
    private void ThrowMushRoom()
    {
        if(witchMP.MP < mushroomMP || !mushroomCooltime.Available)
        {
            return;
        }
        else
        {
            witchMP.UseMP(mushroomMP);
            mushroomCooltime.UseSkill();
        }

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
