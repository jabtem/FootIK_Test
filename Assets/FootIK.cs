using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

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

        private Vector3 leftFoot;
        private Vector3 rightFoot;

        private Vector3 l_Hit;
        private Vector3 l_Noraml;

        private Vector3 R_Hit;
        private Vector3 R_Normal;

        private Vector3 l_hit2;
        private Vector3 l_normal2;

        public float fallMin = 0.7f;
        public float fallMax = 0.9f;

        public Transform leftKneeDummy;

        private void Awake()
        {
            TryGetComponent<Animator>(out anim);
            TryGetComponent<StarterAssetsInputs>(out _input);
        }


        private void Update()
        {

            if (_input.move != Vector2.zero)
            {
                ik_Weight = Mathf.Lerp(ik_Weight, 0.2f, Time.deltaTime * lerpSpeed);
            }
            else
            {
                //Debug.Log(1);
                ik_Weight = Mathf.Lerp(ik_Weight, 1, Time.deltaTime * lerpSpeed);
            }

            if(Mathf.Abs(leftFoot.y - rightFoot.y) < fallMin)
            {
                bone.transform.localPosition = new Vector3(0, -Mathf.Abs(leftFoot.y - rightFoot.y) * ik_Weight, 0);
            }
            else if(Mathf.Abs(leftFoot.y - rightFoot.y) >= fallMin && Mathf.Abs(leftFoot.y - rightFoot.y) < fallMax)
            {
                bone.transform.localPosition = new Vector3(0, -Mathf.Abs(leftFoot.y - rightFoot.y)/2 * ik_Weight, 0);
            }


        }

        private void OnAnimatorIK(int layerIndex)
        {

            leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;



            GetHitInfo(leftFoot, ref l_Hit,ref l_Noraml);

            GetHitInfo(rightFoot, ref R_Hit, ref R_Normal);

            //Debug.DrawRay(leftFoot+ Vector3.up, transform.forward);
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

            //Debug.Log(-Mathf.Abs(leftFoot.y - rightFoot.y) / 2 * ik_Weight);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ik_Weight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ik_Weight);

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, l_Noraml));


            //발부터 무릎까지 높이

            float test = anim.GetIKHintPosition(AvatarIKHint.LeftKnee).y - anim.GetIKPosition(AvatarIKGoal.LeftFoot).y;

            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, ik_Weight);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, ik_Weight);   


            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, R_Normal));
        }
        void GetHitInfo(Vector3 start, Vector3 end ,ref Vector3 point, ref Vector3 normal)
        {
            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                point = hit.point;
                normal = hit.normal;
            }
        }

        void GetHitInfo(Vector3 origin, ref Vector3 point, ref Vector3 normal)
        {
            //Bottom Check
            if (Physics.Linecast(origin + Vector3.up, origin+Vector3.down * rayDistance, out RaycastHit hit))
            {
                point = hit.point;
                normal = hit.normal;

            }

            //Forward Check
            Debug.DrawRay(leftKneeDummy.position, transform.forward);
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
