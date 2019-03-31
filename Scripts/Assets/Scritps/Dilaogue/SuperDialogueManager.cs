using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperDialogueManager : MonoBehaviour
{
    //public Text textUI;
    public SuperTextMesh textUI;

    private int currentIndex;
    private int maxIndex;
    public List<string> currentLines;

    public float timeBetweenLines;
    public bool inDialogue = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(currentIndex + "/" + maxIndex);
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        if (!inDialogue)
        {
            inDialogue = true;
            foreach (string line in dialogue.lines)
            {
                currentLines.Add(line);
            }
            currentIndex = 0;
            if (currentLines.Count > 0)
            {
                maxIndex = currentLines.Count - 1;
            }
            textUI.text = currentLines[0];
            textUI.Rebuild();
        }
    }

    public void NextLine() {
        currentIndex++;
        textUI.text = currentLines[currentIndex];
        textUI.Rebuild();
    }

    IEnumerator FadeDialogue() {
        //inDialogue = false;
        textUI.drawAnimName = "FadeOut";
        textUI.Rebuild();
        int numberOfLetter = textUI.text.ToCharArray().Length;
        yield return new WaitForSeconds(textUI.readDelay*numberOfLetter+0.25f);
        EndDialogue();
    }

    private void ResetValues() {
        currentIndex = 0;
        currentLines.Clear();
        textUI.drawAnimName = "Spring";
    }

    public void ActionWaitForNextLine()
    {
        if (inDialogue)
        {
            Debug.Log("End of Line");
            StartCoroutine(WaitForNextLine());
        }
    }

    IEnumerator WaitForNextLine() {
        float elapsedTime = 0f;
        while (elapsedTime <= timeBetweenLines) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (currentIndex == maxIndex)
        {
            StartCoroutine(FadeDialogue());
        }
        else
        {
            NextLine();
        }
    }

    void EndDialogue() {
        inDialogue = false;
        textUI.text = "";
        textUI.Rebuild();
        ResetValues();
    }
}
