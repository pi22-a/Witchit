using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbCollider : MonoBehaviour
{

    public PEA_WitchSkill witchSkill;
    public PEA_WitchMovement witchMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
