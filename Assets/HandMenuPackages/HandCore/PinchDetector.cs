using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinchDetector : MonoBehaviour
{
    [SerializeField] private bool _hasPinched;
    [SerializeField] private OVRHand _thisHand;
    
    public UnityEvent OnPinch;
    public UnityEvent OnRelease;
    
    private bool _isIndexFingerPinching;
    private float _pinchStrength;
    private OVRHand.TrackingConfidence _confidence;

    void Update() => CheckPinch(_thisHand);

    void CheckPinch(OVRHand hand)
    {
        _pinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        _isIndexFingerPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        _confidence = hand.GetFingerConfidence(OVRHand.HandFinger.Index);

        if (!_hasPinched && _isIndexFingerPinching && _confidence == OVRHand.TrackingConfidence.High)
        {
            _hasPinched = true;
            OnPinch.Invoke();
            //handPointer.CurrentTarget.GetComponent<AudioSource>().PlayOneShot(pinchSound);
        }
        else if (_hasPinched && !_isIndexFingerPinching)
        {
            _hasPinched = false;
            OnRelease.Invoke();
            //handPointer.CurrentTarget.GetComponent<AudioSource>().PlayOneShot(releaseSound);
        }
    }

}
