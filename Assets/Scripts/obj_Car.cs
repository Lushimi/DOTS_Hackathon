using UnityEngine;

[RequireComponent(typeof(Obj_car))]
public class Obj_car : MonoBehaviour
{
    private Obj_car car;
    public UnityEngine.Vector3 objVel;
    public float objSpeedLim;
    public Vector3 initSpeed;
    public float radar;
    public float cohesionWeight;
    public float separationWeight;
    public float alignmentWeight;
    public float originGravity;
    public float boundary;
    public UnityEngine.Vector3 origin;
    public float speedMultiplier;
    private uint cnt;
    public int cntLimit;
    
    void Start()
    {
        objVel= initSpeed;
        car= GetComponent<Obj_car>();    
        cnt=1;
    }
    void Update(){
        var deltaTime= Time.deltaTime;
        var cars= FindObjectsOfType<Obj_car>();
        var centroid= UnityEngine.Vector3.zero;
        var avgVel= UnityEngine.Vector3.zero;
        int sampling= 0;

        foreach(var car in cars){
            var selfPos= this.transform.position;
            var carPos= car.transform.position;
            var directionVector= carPos-selfPos;
            var dist= directionVector.magnitude;
            if(dist<radar){
                centroid+= directionVector;
                avgVel+= car.objVel;
                sampling+= 1;
            }
        }   
        centroid= centroid/sampling;
        avgVel= avgVel/sampling;
        var displacement= (origin - this.transform.position).magnitude;
        var position= this.transform.position;
        var direction= position.normalized;

        objVel+= (UnityEngine.Vector3.Lerp(origin, centroid, (centroid.magnitude/radar)))*cohesionWeight;
        objVel-= (UnityEngine.Vector3.Lerp(origin, centroid, (radar/centroid.magnitude)))*separationWeight;
        objVel+= (UnityEngine.Vector3.Lerp(this.objVel, avgVel, deltaTime))*alignmentWeight;
        

        if(displacement > boundary){
            car.objVel-= (direction* displacement) * originGravity;
        }
        if(objVel.magnitude>objSpeedLim){
            objVel= objVel.normalized*objSpeedLim;
        }
        this.transform.position+= objVel*deltaTime*speedMultiplier;
        this.transform.rotation= UnityEngine.Quaternion.LookRotation(objVel);



        cnt= (cnt<cntLimit)? cnt+1 : 1;
        
        if(cnt<750){
            radar= 5;
        }else{
            radar= 200;
        }
        // UnityEngine.Debug.Log(radar);
    }
}