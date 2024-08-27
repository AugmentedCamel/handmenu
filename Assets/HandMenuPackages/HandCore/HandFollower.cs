using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace HandMenuPackages.HandCore
{
    /// <summary>
    /// This class is responsible for following the midpoint between the thumb and index finger.
    /// </summary>
    public class HandFollower : MonoBehaviour
    {
        
        
        [SerializeField]
        [Tooltip("Transform of the thumb tip.")]
        private Transform _thumbTip;

        [SerializeField]
        [Tooltip("Transform of the index tip.")]
        private Transform _indexTip;

        [Tooltip("Whether the object is currently following the midpoint.")]
        public bool isFollowing = true;

        private float startTime;

        [SerializeField]
        [Tooltip("The speed at which the object follows the midpoint.")]
        private float followingSpeed = 5.8f;
        
        
        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (isFollowing)
            {
                //get the target position
                Vector3 targetPos = Midpoint();

                Vector3 startpos = transform.position;
                Vector3 endpos = targetPos;

                transform.position = Vector3.Lerp(startpos, targetPos, followingSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Calculates the midpoint between the thumb and index finger.
        /// If either the thumb or index finger is not active, it returns the current position of the object.
        /// </summary>
        /// <returns>The calculated midpoint or the current position of the object.</returns>
        Vector3 Midpoint()
        {
            if (_thumbTip.gameObject.activeSelf && _indexTip.gameObject.activeSelf)
            {
                //calculate the transform in between
                return (_thumbTip.position + _indexTip.position) / 2;
            }
            else
            {
                return transform.position; //latest position of this handcore script
            }
        }
    } 
}