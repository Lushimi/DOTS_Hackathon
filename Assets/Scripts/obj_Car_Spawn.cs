using UnityEngine;

public class Obj_Car_Spawn : MonoBehaviour
{
    public GameObject prefab;
    public uint popCap;
    public uint randerosity;
    uint cnt= 0;
    void Update(){
        var selfPos= this.transform.position;
        Unity.Mathematics.Random rand= new Unity.Mathematics.Random(1);
        Vector3 randRange= new Vector3(1,0,1)*randerosity;
        Vector3 randPos= rand.NextFloat3(selfPos-randRange, selfPos+randRange);

        if(cnt<popCap){
            Instantiate(prefab, randPos, UnityEngine.Random.rotation);
        }
        cnt= (cnt<popCap)? cnt+1 : popCap;
    }
}