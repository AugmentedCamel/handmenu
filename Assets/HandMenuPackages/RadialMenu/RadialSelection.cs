using System.Collections;
using System.Collections.Generic;
using HandMenuPackages;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NaughtyAttributes;

public class RadialSelection : MonoBehaviour
{
    public OVRInput.Button spawnButton;
    [Required]
    [SerializeField] private MenuDataManager _menuDataManager;
    
    [Range(2,10)]
    public int numberOfRadialPart;
    public GameObject radialPartPrefab;
    public GameObject radialButtonPrefab; 
    
    public Transform radialPartCanvas;
    public float angleBetweenPart = 10;
    public Transform handTransform;

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private List<GameObject> spawnedButtons = new List<GameObject>();
    //private List<UnityAction> buttonActions = new List<UnityAction>();
    
    private int currentSelectedRadialPart = -1;
    private bool _isSelecting = false;
    
    [SerializeField] private Transform _thumbTip; 
    private Vector3 spawnPosition; // Add this line to store the position where the radial menu was spawned
    public float selectionDistanceThreshold = 0.1f; // Add this line to set the distance threshold for selection
    public float currentDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        _menuDataManager = GetComponent<MenuDataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(spawnButton))
        {
            SpawnRadialPart();
        }

        if(OVRInput.Get(spawnButton))
        {
            GetSelectedRadialPart();
        }

        if(OVRInput.GetUp(spawnButton))
        {
            HideAndTriggerSelected();
        }
        
        if(_isSelecting)
        {
            GetSelectedRadialPart();
        }
    }
    
    [Button]
    public void TriggerSpawnRadialPart()
    {
        SpawnRadialPart();
    }
    
    [Button]
    public void TriggerGetSelectedRadialPart()
    {
        GetSelectedRadialPart();
    }
    
    [Button]
    public void TriggerHideAndTriggerSelected()
    {
        HideAndTriggerSelected();
    }

    public void HideAndTriggerSelected()
    {
        
        OnPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
        
        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
                    
                spawnedButtons[i].GetComponent<Button>().OnDeselect(null);
            }
        }
        
        if(_menuDataManager.menuPages[_menuDataManager.currentPage].menuItems[currentSelectedRadialPart].action != null)
        {
            UnityAction action = _menuDataManager.menuPages[_menuDataManager.currentPage].menuItems[currentSelectedRadialPart].action.Invoke;
              action.Invoke();
              Debug.Log("Action invoked");
        }
        
        
        //disableSelecting
        _isSelecting = false;
         
    }
    
    private float GetDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }
    
    public void GetSelectedRadialPart()
    {
        currentDistance = GetDistance(_thumbTip.position, spawnPosition);


        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);
        
        if (currentDistance < selectionDistanceThreshold)
        {
            //Debug.Log("Distance less than threshold");
            //should unslecect all and return
            for (int i = 0; i < spawnedParts.Count; i++)
            {
                if (i == currentSelectedRadialPart)
                {
                    spawnedParts[i].GetComponent<Image>().color = Color.white;
                    spawnedParts[i].transform.localScale = Vector3.one;
                    
                    spawnedButtons[i].GetComponent<Button>().OnDeselect(null);
                    currentSelectedRadialPart = -1;
                }
            }

            return;
        }
        
        
        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);

        if (angle < 0)
            angle += 360;

        Debug.Log("ANGLE " + angle);


        currentSelectedRadialPart = (int) angle * numberOfRadialPart / 360;

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if(i == currentSelectedRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = Color.yellow;
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one;
                
                spawnedButtons[i].GetComponent<Button>().Select();
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialPart()
    {
        spawnPosition = handTransform.position; // Store the position where the radial menu was spawned
        
        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;

        foreach (var item in spawnedParts)
        {
            Destroy(item);
        }
        
        foreach (var item in spawnedButtons)
        {
            Destroy(item);
        }

        

        spawnedParts.Clear();
        spawnedButtons.Clear();
        
        
        //should check number of radial pars in the menu manager
        numberOfRadialPart = _menuDataManager.GetNumberOfButtons();
        
        
        for (int i = 0; i < numberOfRadialPart; i++)
        {
            float angle = - i * 360 / numberOfRadialPart - angleBetweenPart /2;
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;

            spawnedRadialPart.GetComponent<Image>().fillAmount = (1 / (float)numberOfRadialPart) - (angleBetweenPart/360);
            spawnedParts.Add(spawnedRadialPart);
            
            //spawn radial button
            Vector3 spawnposition = spawnedRadialPart.GetComponentInChildren<ButtonUILocation>().transform.position;
            
            
            GameObject spawnedRadialButton = Instantiate(radialButtonPrefab, radialPartCanvas);
            spawnedRadialButton.transform.position = spawnposition;
            spawnedRadialButton.transform.localEulerAngles = Vector3.zero;
            if (_menuDataManager.GetButtonIcon(i) != null)
            {
                spawnedRadialButton.GetComponent<Image>().sprite = _menuDataManager.GetButtonIcon(i);
            }
            
         
            spawnedButtons.Add(spawnedRadialButton);
            

            //pass the angle of the radial part and the original position of the radial menu, and the integer of the radial part
        }
        
        //enable selecting
        _isSelecting = true;
        
    }
}
