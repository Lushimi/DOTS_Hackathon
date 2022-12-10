using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/getting-the-bounds-of-the-group-of-objects.70979/
namespace Team3
{
    public class GetTrackBounds : MonoBehaviour
    {
        public static Vector3 TrackBoundsMax;
        public static Vector3 TrackBoundsMin;

        private void Awake()
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            if ( renderers.Length > 0 )
            {
                //Find first enabled renderer to start encapsulate from it
                foreach ( Renderer renderer in renderers )
                {
                    if ( renderer.enabled )
                    {
                        bounds = renderer.bounds;
                        break;
                    }
                }
                //Encapsulate for all renderers
                foreach ( Renderer renderer in renderers )
                {
                    if ( renderer.enabled )
                    {
                        bounds.Encapsulate( renderer.bounds );
                    }
                }
            }
            TrackBoundsMax = bounds.max;
            TrackBoundsMin = bounds.min;
            //Debug.Log(TrackBoundsMax + " Min: " + TrackBoundsMin);
        }
    }
    
}