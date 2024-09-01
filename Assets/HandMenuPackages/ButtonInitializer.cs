using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInitializer : MonoBehaviour
{
    [SerializeField] private Image _thisImage;
    [SerializeField] private TextMeshProUGUI _thisText;
    
    public void Initialize(Sprite sprite, string text)
    {
        _thisImage.sprite = sprite;
        _thisText.text = text;
    }
}
