using System.Collections;
using UnityEngine;

public class ProceduralMovement : MonoBehaviour
{
    public Animator animator;
    public Transform leftFootTarget, rightFootTarget;
    public LayerMask groundLayer;
    public float footOffset = 0.1f;
    public float stepHeight = 0.2f;
    public float stepSpeed = 5f;
    public float footSpacing = 0.2f;

    private Vector3 leftFootPosition, rightFootPosition;
    private Quaternion leftFootRotation, rightFootRotation;
    private Vector3 leftFootIKPos, rightFootIKPos;
    private Quaternion leftFootIKRot, rightFootIKRot;

    void Start()
    {
        leftFootPosition = leftFootTarget.position;
        rightFootPosition = rightFootTarget.position;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            UpdateFootTarget(ref leftFootIKPos, ref leftFootIKRot, leftFootTarget);
            UpdateFootTarget(ref rightFootIKPos, ref rightFootIKRot, rightFootTarget);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPos);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootIKRot);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKPos);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootIKRot);
        }
    }

    void UpdateFootTarget(ref Vector3 footPosition, ref Quaternion footRotation, Transform footTransform)
    {
        RaycastHit hit;
        Vector3 origin = footTransform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 1.5f, groundLayer))
        {
            footPosition = hit.point + Vector3.up * footOffset;
            footRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * footTransform.rotation;
        }
    }
}