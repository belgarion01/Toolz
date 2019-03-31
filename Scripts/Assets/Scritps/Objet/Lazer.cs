using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    void Update()
    {
        //Raycast depuis la caméra contre le Layer whatCanBeHit
        //whatCanBeHit = Player et MorphableObject
        
        /*
        if(other.gameObject.GetComponent<MorphableObject>()){
            //objectToScale = other.gameObject
            //Scale objectToScale        
        }
        else if(Player){
            objectToScale = other.GetComponent<PlayerController>().objectHold;
        }
        */

        if (Input.GetKeyDown(KeyCode.E)) {
            //MorphableObject.ScaleUp()
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            //MorphableObject.ScaleDown()
        }
    }
}
