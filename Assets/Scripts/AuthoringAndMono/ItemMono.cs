using UnityEngine;
using Unity.Entities;


namespace Team3
{

    public class ItemMono : MonoBehaviour
    {
    }

    public class ItemBaker : Baker<ItemMono>
    {

        public override void Bake( ItemMono authoring )
        {
        }
    }
}

