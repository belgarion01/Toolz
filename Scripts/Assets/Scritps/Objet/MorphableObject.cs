using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphableObject : MonoBehaviour
{
    public enum Size { SMALL, MEDIUM, BIG };
    public Size currentSize;

    public SizeProperties.ObjetType currentObjectType;

    public bool canBeHold;

    public SizeProperties smallProperties;
    public SizeProperties mediumProperties;
    public SizeProperties bigProperties;

    private SizeProperties[] sizeProperties = new SizeProperties[3];

    private Rigidbody rb;

    private void Awake()
    {
        sizeProperties[0] = smallProperties;
        sizeProperties[1] = mediumProperties;
        sizeProperties[2] = bigProperties;
    }

    private void Start()
    {
        SetProperties((int)currentSize);
    }

    IEnumerator ScaleUp() {
        if ((int)currentSize < 2) {
            //ScaleUp()
            //SetProperties()
        }
        yield return null;
    }

    IEnumerator ScaleDown()
    {
        if ((int)currentSize > 0)
        {
            //ScaleDown()
            //SetProperties()
        }
        yield return null;
    }

    public void SetProperties(int sizeIndex) {
        currentObjectType = sizeProperties[sizeIndex].objectType;
        if (GetComponent<Rigidbody>()){
            GetComponent<Rigidbody>().mass = sizeProperties[sizeIndex].mass;
        }

    }
}

[System.Serializable]
public class SizeProperties {
    public enum ObjetType { NULL, SWORD, BOX }
    public ObjetType objectType;

    public float mass;

    public bool canBeHold;
}
