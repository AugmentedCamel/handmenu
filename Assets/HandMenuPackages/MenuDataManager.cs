using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace HandMenuPackages
{
    
    [System.Serializable]
    public struct MenuButton
    {
        public string text;
        public Sprite icon;
        public UnityEvent action;
    }
    
    [System.Serializable]
    public struct MenuPage
    {
        public string name;
        public List<MenuButton> menuItems;
    }
    
    
    public class MenuDataManager : MonoBehaviour
    {
        
        
        public List<MenuPage> menuPages;
        
        public int currentPage = 0;
        
        [Button]
        public void NextPage()
        {
            currentPage++;
            if (currentPage >= menuPages.Count)
            {
                currentPage = 0;
                OnPageChange();
            }
        }
        
        [Button]
        public void PreviousPage()
        {
            currentPage--;
            if (currentPage < 0)
            {
                currentPage = menuPages.Count - 1;
                OnPageChange();
            }
        }
        
        public int GetNumberOfButtons()
        {
            int numberOfButtons = menuPages[currentPage].menuItems.Count;
            return numberOfButtons;
        }
        
        public Sprite GetButtonIcon(int buttonIndex)
        {
            Sprite icon = menuPages[currentPage].menuItems[buttonIndex].icon;
            return icon;
        }
        
        public UnityEvent GetButtonAction(int buttonIndex)
        {
            UnityEvent action = menuPages[currentPage].menuItems[buttonIndex].action;
            return action;
        }
        
        private void OnPageChange()
        {
            //load the page
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
