using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleManager : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<string, ParticleSystem> VFX;

    public static ParticleManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void CreateEffect(string name, Vector3 pos)
    {
        ParticleSystem instance = MonoBehaviour.Instantiate(VFX[name], pos, VFX[name].transform.rotation);
    }

    public void CreateEffect(string name, Vector3 pos, Quaternion rot)
    {
        ParticleSystem instance = MonoBehaviour.Instantiate(VFX[name], pos, rot);
    }


    public void CreateEffect(string name, Vector3 pos, Transform parent)
    {
        ParticleSystem instance = MonoBehaviour.Instantiate(VFX[name], pos, VFX[name].transform.rotation, parent);
    }

    public void CreateEffect(string name, Vector3 pos, Quaternion rot, Transform parent)
    {
        ParticleSystem instance = MonoBehaviour.Instantiate(VFX[name], pos, rot, parent);
    }

}
