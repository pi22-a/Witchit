using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbCollider : MonoBehaviour
{
    public PEA_WitchMovement witchMovement;
    public PEA_WitchSkill witchSkill;

    private void OnCollisionEnter(Collision collision)
    {
        if (witchSkill.IsChanged)
        {
            if (collision.contacts[0].point.y < transform.position.y)
            {
                witchMovement.OnGround();
            }
        }
    }
}
