using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlayer : MonoBehaviour
{
    public GameObject objectHold;
    private MorphableObject objectHoldProperties;


    private void Update()
    {
        if (objectHold != null) {
            SizeProperties.ObjetType currentObjectType = objectHoldProperties.currentObjectType;
            switch ((currentObjectType)){
                case SizeProperties.ObjetType.SWORD:
                    UpdateSword();
                    break;
                case SizeProperties.ObjetType.BOX:
                    UpdateBox();
                    break;
            }
        }
    }

    void UpdateSword() {
        if (Input.GetMouseButtonDown(0)) {
            //Attack()
        }
        if (Input.GetMouseButtonDown(1)) {
            //ThrowObject()
        }
    }

    void UpdateBox() {
        if (Input.GetMouseButtonDown(1)) { }
        //ThrowObject()

    }

    void TakeObject() {
        objectHoldProperties.isHold = true;
    }

}
