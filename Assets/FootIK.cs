using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class FootIK : MonoBehaviour
    {
        public Animator anim;

        public Vector3 footIk_offset;

        public Transform bone;
        private StarterAssetsInputs _input;
        public float ik_Weight;
        public float lerpSpeed;

        public float rayDistance;

        public Transform leftToeEnd;
        public Transform rightToeEnd;

        private void Awake()
        {
            TryGetComponent<Animator>(out anim);
            TryGetComponent<StarterAssetsInputs>(out _input);
        }


        private void Update()
        {

            if (_input.move != Vector2.zero)
            {
                ik_Weight = Mathf.Lerp(ik_Weight, 0, Time.deltaTime * lerpSpeed);
            }
            else
            {
                //Debug.Log(1);
                ik_Weight = Mathf.Lerp(ik_Weight, 1, Time.deltaTime * lerpSpeed);
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {

            Vector3 leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            Vector3 rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;

            Vector3 l_Hit = GetHitPoint(leftFoot+Vector3.up, leftFoot + Vector3.down * rayDistance);
            Vector3 R_Hit = GetHitPoint(rightFoot+Vector3.up, rightFoot + Vector3.down * rayDistance);

            //float l_HItDis = GetHitDistance(leftFoot, leftFoot + Vector3.down * rayDistance) * -1f;
            //float R_HitDis = GetHitDistance(rightFoot, rightFoot + Vector3.down * rayDistance) * -1f;

            //Debug.Log("Left :" + l_HItDis);
            //Debug.Log("Right : " + R_HitDis);


            //leftFoot = new Vector3(leftFoot.x,leftFoot.y+ l_HItDis, leftFoot.z) + footIk_offset;
            //rightFoot = new Vector3(rightFoot.x, rightFoot.y + R_HitDis, rightFoot.z) + footIk_offset;
            leftFoot = l_Hit + footIk_offset;
            rightFoot = R_Hit + footIk_offset;

            //Debug.Log("left : " + leftFoot);
            //Debug.Log("right : " + rightFoot);


            //bone.localPosition = new Vector3(0, -Mathf.Abs(leftFoot.y - rightFoot.y) / 2 * ik_Weight, 0);

            //Debug.Log(-Mathf.Abs(leftFoot.y - rightFoot.y) / 2 * ik_Weight);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ik_Weight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);


            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot,1f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, ik_Weight);    
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot);

        }
        Vector3 GetHitPoint(Vector3 start, Vector3 end)
        {
            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                return hit.point;
            }
            return end;
        }

        float GetHitDistance(Vector3 start, Vector3 end)
        {
            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                return hit.distance;
            }
            return 0f;
        }
    }

}
