using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK2 : MonoBehaviour
{
    Animator anim;

    [Range(0, 1)]
    public float distancToGround;
    private void Awake()
    {

        TryGetComponent<Animator>(out anim);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);


       //Vector3 leftFoot = GetHitPoint(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        //leftFoot.y += distancToGround;


        

        SetFootIK(AvatarIKGoal.LeftFoot);
        
        //leftFoot.y += distancToGround;
        //anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);

        //if (Physics.Raycast(ray,out RaycastHit hit, distancToGround +1f))
        //{
        //    Vector3 leftFoot = hit.point;
        //    leftFoot.y += distancToGround;
        //    anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);

        //}


    }

    void SetFootIK(AvatarIKGoal avatarIKGoal)
    {
        Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);


        if (Physics.Raycast(ray ,out RaycastHit hit, distancToGround + 1f))
        {
            Vector3 hitPostion = hit.point;
            hitPostion.y += distancToGround;
            anim.SetIKPosition(avatarIKGoal, hitPostion);
            anim.SetIKRotation(avatarIKGoal, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }

}
