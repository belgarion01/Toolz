using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLord : MonoBehaviour
{
    public GameObject obj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ParticleManager.Instance.CreateEffect("Explosion", transform.position, Quaternion.LookRotation(obj.transform.position));
        }
    }
}
