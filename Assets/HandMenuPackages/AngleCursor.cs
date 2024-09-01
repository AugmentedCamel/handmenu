using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class AngleCursor : MonoBehaviour
{
    //this script is only for moving the angle to a desired angle in a smooth motion
    [SerializeField] private float _angle;
    [SerializeField] private float _duration = 1f; // Duration of the rotation animation
    
    
    public void RotateToAngle()
    {
        RotateToAngle(_angle);
    }
    
    private void RotateToAngle(float targetAngle)
    {
        // Calculate the difference between the current local Z rotation and the target angle
        float angleDifference = targetAngle - transform.localEulerAngles.z;

        // If the angle difference is greater than 180, subtract 360 to ensure the rotation is in the shortest direction
        if (Mathf.Abs(angleDifference) > 180)
        {
            angleDifference -= 360 * Mathf.Sign(angleDifference);
        }

        // Calculate the new target angle in local space
        float newTargetAngle = transform.localEulerAngles.z + angleDifference;

        // Animate the rotation to the new target angle in local space
        transform.DOLocalRotate(new Vector3(0, 0, newTargetAngle), _duration, RotateMode.FastBeyond360);
    }
}