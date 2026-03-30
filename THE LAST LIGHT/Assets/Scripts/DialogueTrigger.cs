// DialogueTrigger.cs
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Caroline";
    public Animator npcAnimator;

    [Header("Dialogue Tree")]
    public DialogueNode startNode;

    [Header("References")]
    public FPSController fpsController;

    private bool playerNear = false;
    private bool talking = false;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, fpsController.transform.position);

        if (dist < 2.5f && !playerNear)
        {
            playerNear = true;
            DialogueUI.Instance.ShowPrompt(true);
        }
        else if (dist >= 2.5f && playerNear)
        {
            playerNear = false;
            talking = false;
            DialogueUI.Instance.ShowPrompt(false);
            DialogueUI.Instance.Hide();
            fpsController.canMove = true;
        }

        if (playerNear && !talking && Input.GetKeyDown(KeyCode.E))
            StartDialogue();
    }

    void StartDialogue()
    {
        if (startNode == null)
        {
            Debug.LogWarning($"[DialogueTrigger] No startNode assigned on {gameObject.name}!");
            return;
        }
        talking = true;
        fpsController.canMove = false;
        DialogueUI.Instance.StartNodeDialogue(startNode, this);
    }

    public void TriggerAnimation(string triggerName)
    {
        if (npcAnimator != null)
            npcAnimator.SetTrigger(triggerName);
    }

    public void OnDialogueEnd()
    {
        talking = false;
        fpsController.canMove = true;
    }
}