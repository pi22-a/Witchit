using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lantern : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject lightObject;
    bool lightONOFF;
    public float maxRaycastDistance = 3f;

    void Update()
    {
        if (photonView.IsMine)  
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Vector3 directionToPlayer = Camera.main.transform.position - transform.position;
                    float distanceToPlayer = directionToPlayer.magnitude;

                    if (distanceToPlayer <= maxRaycastDistance)
                    {
                        photonView.RPC("ToggleLanternRPC", RpcTarget.AllBuffered);
                    }
                }
            }
        }
    }

    [PunRPC]
    void ToggleLanternRPC()
    {
        lightONOFF = !lightONOFF;
        lightObject.SetActive(lightONOFF);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(lightONOFF);
        }
        else
        {
            lightONOFF = (bool)stream.ReceiveNext();
            lightObject.SetActive(lightONOFF);
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class Lantern : MonoBehaviour
//{
//    //public GameObject player1;
//    //public GameObject player2;
//    public GameObject lightObject;
//    bool lightONOFF;
//    public float maxRaycastDistance = 3f;

//    void Update()
//    {
//        //Vector3 directionToPlayer = player1.transform.position - transform.position;
//        //Vector3 directionToPlayer2 = player2.transform.position - transform.position;
//        float distanceToPlayer = directionToPlayer.magnitude;
//        //float distanceToPlayer2 = directionToPlayer2.magnitude;

//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, directionToPlayer, out hit, maxRaycastDistance))
//        {
//            if (hit.collider.gameObject == player1 && Input.GetKeyDown(KeyCode.E))
//            {
//                Debug.Log("dd");
//                ToggleLantern();
//            }
//        }
//        //if (Physics.Raycast(transform.position, directionToPlayer2, out hit, maxRaycastDistance))
//        //{
//        //    if (hit.collider.gameObject == player2 && Input.GetKeyDown(KeyCode.E))
//        //    {
//        //        ToggleLantern();
//        //    }
//        //}
//    }

//    void ToggleLantern()
//    {
//        lightONOFF = !lightONOFF;
//        lightObject.SetActive(lightONOFF);
//    }
//}



