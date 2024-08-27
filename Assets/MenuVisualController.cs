using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVisualController : MonoBehaviour
{
    [SerializeField] private GameObject _menuCursor;
    [SerializeField] private GameObject _menuCircle;
    
    public void ActivateMenuCursor()
    {
        _menuCursor.SetActive(true);
    }
    public void DeactivateMenuCursor()
    {
        _menuCursor.SetActive(false);
    }
    
    public void ActivateMenuCircle()
    {
        _menuCircle.SetActive(true);
    }
    public void DeactivateMenuCircle()
    {
        _menuCircle.SetActive(false);
    }
}
