using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBehaviour : MonoBehaviour
{

    private Image _parentImage;
    
    // Start is called before the first frame update
    void Start()
    {
        _parentImage = GetComponentInParent<Image>();
        
    }
    
    public void OnLoad()
    {
        float fillAmount = _parentImage.fillAmount;
        //position according to the partet fill amount
        RectTransform rectTransform = GetComponent<RectTransform>();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
