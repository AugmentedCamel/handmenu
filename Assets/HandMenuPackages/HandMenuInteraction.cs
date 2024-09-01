using System;
using System.Collections;
using System.Collections.Generic;
using HandMenuPackages.HandCore;

using UnityEngine;

public class HandMenuInteraction : MonoBehaviour
{
    public bool isInteractable = false; //should be false if hand of palm is not detected
    public bool isPinched = false; //should be false if hand of palm is not detected
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _xrRig;
    [SerializeField] private MenuVisualController _menuVisualController;
    [SerializeField] private HandFollower _handFollower;
    [SerializeField] private RadialSelection _radialSelection;
    
    //[SerializeField] private hand
    //[SerializeField]
    //private GameObject _menuInteractable;

    private void Start()
    {
        //HideInteractable();
        HideInteractable();
        
        _handFollower = GetComponent<HandFollower>();
    }

    #region Toggling interactability
    
    public void ShowInteractable()
    {
        bool isHandFacingCamera = IsHandFacingCamera();
        
        if (!isHandFacingCamera)
        {
            //Debug.Log("Hand is not facing camera. Cannot show interactable.");
            return;
        }
        
        StopAllCoroutines();
        isInteractable = true;
        _menuVisualController.ActivateMenuCursor();
        
    }
    
 
    public void HideInteractable()
    {
        StartCoroutine(DelayedHideInteractable(1f));
        _menuVisualController.DeactivateMenuCursor();
    }

    private bool IsHandFacingCamera()
    {
        //compares the forward vector of the hand with the forward vector of the camera if the angle is less than 90 degrees, the hand is facing the camera
        Vector3 handForward = _leftHand.transform.forward;
        Vector3 cameraForward = _xrRig.transform.forward;
        float angle = Vector3.Angle(handForward, cameraForward);
        //Debug.Log("Angle: " + angle);
        return angle < 90;
    }

    private IEnumerator DelayedHideInteractable(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInteractable = false;
    }

    

    #endregion //making the menu interactable

    #region Pinch Menu Activation
    
    public void ActivatePinchMenu()
    {
        if (!isInteractable) return;
        //_menuVisualController.ActivateMenuCircle();
        _radialSelection.SpawnRadialPart();
        //now save that pinch is active
        isPinched = true;
        
        _handFollower.isFollowing = false;
        //store a begin point in menu behaviour
        //show a visual menu that is active

    }

    public void ReleasePinchMenu()
    {
        if (!isPinched) return;
        _menuVisualController.DeactivateMenuCircle();
        _handFollower.isFollowing = true;
        Debug.Log("Pinch released");
        _radialSelection.HideAndTriggerSelected();
        //something is chosen
    }
    

    public void DeactivatePinchMenu()
    {
        _menuVisualController.DeactivateMenuCircle();
        HandFollower handFollower = GetComponent<HandFollower>();
        handFollower.isFollowing = true;
    }
    

    #endregion
    
    
}
