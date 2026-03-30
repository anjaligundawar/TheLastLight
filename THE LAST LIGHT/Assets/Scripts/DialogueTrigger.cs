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
    private DialogueUI dialogueUI;

    void Start()
    {
        dialogueUI = FindObjectOfType<DialogueUI>();

        // Debug checks
        if (dialogueUI == null)
            Debug.LogError("❌ DialogueUI not found in scene!");
        else
            Debug.Log("✅ DialogueUI found");

        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("❌ No collider on Girl_NPC!");
        else if (!col.isTrigger)
            Debug.LogError("❌ Collider exists but Is Trigger is OFF!");
        else
            Debug.Log("✅ Trigger collider found");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log($"E pressed | playerNear={playerNear} | talking={talking}");

        if (playerNear && !talking && Input.GetKeyDown(KeyCode.E))
            StartDialogue();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name} | Tag: {other.tag}");

        if (!other.CompareTag("Player")) return;
        playerNear = true;
        dialogueUI.ShowPrompt(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNear = false;
        talking = false;
        dialogueUI.ShowPrompt(false);
        dialogueUI.Hide();
        FindObjectOfType<FPSController>().canMove = true;
    }

    void StartDialogue()
    {
        Debug.Log("✅ Starting dialogue!");
        talking = true;
        dialogueUI.ShowPrompt(false);
        dialogueUI.Show(npcName, lines, this);
        FindObjectOfType<FPSController>().canMove = false;
    }

    public void OnDialogueEnd()
    {
        talking = false;
        FindObjectOfType<FPSController>().canMove = true;
    }
}