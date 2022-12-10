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

        public GameObject ExcludeParentObject;
        public static Vector3 ExcludeBoundsMax;
        public static Vector3 ExcludeBoundsMin;

        private void Awake()
        {
            Bounds bounds = GetBounds( gameObject );
            TrackBoundsMax = bounds.max;
            TrackBoundsMin = bounds.min;

            Bounds excludeBounds = GetBounds( ExcludeParentObject );
            ExcludeBoundsMax = excludeBounds.max;
            ExcludeBoundsMin = excludeBounds.min;
/*            Debug.Log(TrackBoundsMax + " Min: " + TrackBoundsMin);
            Debug.Log( ExcludeBoundsMax + " Min: " + ExcludeBoundsMin );*/
        }

        private Bounds GetBounds(GameObject obj) 
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

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
            return bounds;
        }
    }
    
}