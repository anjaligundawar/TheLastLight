using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(2,4)]
    public string[] lines = {
        "Oh! I didn't hear you coming...",
        "I've been standing here for so long.",
        "Do you know what's going on?"
    };

    public string npcName = "Caroline";

    private bool playerNear = false;
    private bool talking = false;

    void Update()
    {
        if (playerNear && !talking && Input.GetKeyDown(KeyCode.E))
            StartDialogue();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNear = true;
        DialogueUI.Instance.ShowPrompt(true);  // show "Press E"
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNear = false;
        talking = false;
        DialogueUI.Instance.ShowPrompt(false); // hide "Press E"
        DialogueUI.Instance.Hide();            // hide dialogue box
        FindObjectOfType<FPSController>().canMove = true;
    }

    void StartDialogue()
    {
        talking = true;
        DialogueUI.Instance.Show(npcName, lines, this);
        FindObjectOfType<FPSController>().canMove = false;
    }

    public void OnDialogueEnd()
    {
        talking = false;
        FindObjectOfType<FPSController>().canMove = true;
    }
}