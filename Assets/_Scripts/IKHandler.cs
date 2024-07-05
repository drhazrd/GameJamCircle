using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandler : MonoBehaviour
{
    Animator anim;
    public float LeftHandWeight = 1f;
    public Transform LeftHandTarget;
    public float RightHandWeight = 1f;
    public Transform RightHandTarget;
    public Transform weapon;
    public Vector3 lookPosition;

    void Start()
    {
        anim = GetComponent<Animator>();        
    }

    void Update()
    {
        
    }

    void OnAnimatorIK(){
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
        anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandWeight);
        anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation);
    }
}
