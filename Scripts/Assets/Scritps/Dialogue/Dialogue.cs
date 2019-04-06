using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueTruc", menuName = "DialogueTruc")]
public class Dialoguetruc : ScriptableObject
{
    [TextArea]
    public string text;
}
