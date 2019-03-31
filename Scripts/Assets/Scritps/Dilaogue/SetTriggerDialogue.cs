using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTriggerDialogue : MonoBehaviour
{
    private SuperDialogueManager dManager;
    public Dialogue dialogue;

    void Start()
    {
        dManager = FindObjectOfType<SuperDialogueManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            dManager.StartDialogue(dialogue);
        }
    }
}
