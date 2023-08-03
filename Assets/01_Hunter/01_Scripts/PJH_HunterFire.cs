using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PJH_HunterFire : MonoBehaviour
{
    public GameObject grenadeUIFactory;
    public ScrollRect scrollRectSkill;

    enum BImpactName
    {
        Floor,
        Enemy
    }

    public GameObject grenadeFactory;
    public Transform firePosition;
    // �Ѿ��ڱ�����
    public GameObject[] bImpactFactorys;
    //SkillItem item;
    private void Awake()
    {
        GameObject ui = Instantiate(grenadeUIFactory);
       // item = ui.GetComponent<SkillItem>();
        ui.transform.parent = scrollRectSkill.content;
    }


    void Start()
    {
    }

    void Update()
    {
        UpdateFire();
        UpdageGrenade();
    }
    public float cameraShakeTime = 0.2f;
    public float cameraShakeIntensicy = 2;
    private void UpdateFire()
    {
        // 0. ����ڰ� ���콺 ���� ��ư�� ������
        if (Input.GetButtonDown("Fire1"))
        {
            //CameraShake.instance.Play(cameraShakeTime, cameraShakeIntensicy);

            // 1. ī�޶󿡼� ī�޶��� �չ������� �ü��� �����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo;
            // 2. �ٶ󺸰�ʹ�.
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                bool isEnemy = false;
                // 3. �ü��� ���� ���� �Ѿ��ڱ����忡�� �Ѿ��ڱ��� ���� ��ġ�ϰ�ʹ�.
               // ObjectPool.FactoryName key = ObjectPool.FactoryName.BImpactFloor;
                // ���� hitInfo�� ��ü�� ���̾ Enemy���
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                   // key = ObjectPool.FactoryName.BImpactEnemy;
                    isEnemy = true;
                }

              //  GameObject bulletImpact = ObjectPool.instance.GetDeactiveObject(key);
              //  if (bulletImpact != null)
              //  {
                  //  bulletImpact.GetComponent<BulletImpactAutoDeactive>().Play(2);

                  //  bulletImpact.transform.position = hitInfo.point;
                    // ������ ȸ���ϰ�ʹ�. Ƣ�¹���(forward�� �ε��� ���� Normal��������
                   // bulletImpact.transform.forward = hitInfo.normal;
              //  }

                // ���� �ѿ� �´°��� ���̶��
                if (isEnemy)
                {
                    // ��(Enemy2)���� �� �ѿ� �¾Ҿ�!(DamageProcess()) ��� �˷��ְ�ʹ�.
                   // Enemy2 enemy = hitInfo.transform.GetComponent<Enemy2>();

              //      enemy.DamageProcess();

                }

            }
            else
            {
                // ���
            }
        }
    }

    private void UpdageGrenade()
    {
        // ���� ��ų�� ����� �� �ִٸ� �׸��� ����ڰ� GŰ�� ������ ��ź�� ������ �ʹ�.
    //    if (item.CanDoIt() && Input.GetKeyDown(KeyCode.G))
    //    {
    //        item.DoIt();
    //        GameObject g = Instantiate(grenadeFactory);
    //        g.transform.position = firePosition.position;
    //        g.transform.forward = firePosition.forward;

    //        g.GetComponent<Grenade>().speed = 10;
    //    }
    }
}
