using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text textUI;

    private int currentIndex;
    private int maxIndex;
    public List<string> currentLines;

    public float timeBetweenLines;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            NextLine();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        foreach (string line in dialogue.lines) {
            currentLines.Add(line);
        }
        currentIndex = 0;
        maxIndex = currentLines.Count-1;
        textUI.text = currentLines[0];
        StartCoroutine(WaitForNextLine());
    }

    public void NextLine() {
        currentIndex++;
        textUI.text = currentLines[currentIndex];
        StartCoroutine(WaitForNextLine());
    }

    public void EndDialogue() {
        textUI.text = "";
        ResetValues();
    }

    private void ResetValues() {
        currentIndex = 0;
        currentLines.Clear();
    }

    

    IEnumerator WaitForNextLine() {
        Debug.Log(currentIndex + "/" + maxIndex);
        float elapsedTime = 0f;
        while (elapsedTime <= timeBetweenLines) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (currentIndex == maxIndex)
        {
            EndDialogue();
        }
        else
        {
            NextLine();
        }
    }
}
