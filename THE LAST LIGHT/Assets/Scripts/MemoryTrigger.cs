// MemoryTrigger.cs
using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destination;
    public float interactionDistance = 8f;

    [Header("References")]
    public GameObject promptUI;

    private Transform playerTransform;
    private bool isPlayerNear = false;
    private static MemoryTrigger currentActivePrompt = null;

    // ✅ ADDED
    private DialogueUI dialogueUI;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("MemoryTrigger: Player not found!");

        if (promptUI == null)
        {
            promptUI = GameObject.Find("PromptUI");
            if (promptUI == null)
                Debug.LogError("MemoryTrigger: PromptUI not found on " + gameObject.name);
        }

        if (promptUI != null)
            promptUI.SetActive(false);

        // ✅ ADDED
        dialogueUI = DialogueUI.Instance;
    }

    void Update()
    {
        if (playerTransform == null || promptUI == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance)
        {
            // ✅ ADDED — dialogue is open, hide prompt and skip entirely
            if (dialogueUI != null && dialogueUI.IsOpen)
            {
                if (isPlayerNear)
                {
                    isPlayerNear = false;
                    if (currentActivePrompt == this)
                        currentActivePrompt = null;
                    promptUI.SetActive(false);
                }
                return;
            }

            if (!isPlayerNear && currentActivePrompt == null)
            {
                isPlayerNear = true;
                currentActivePrompt = this;
                promptUI.SetActive(true);
            }

            if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                ExecuteTeleport();
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                if (currentActivePrompt == this)
                    currentActivePrompt = null;
                promptUI.SetActive(false);
            }
        }
    }

    void ExecuteTeleport()
    {
        if (destination == null)
        {
            Debug.LogError("MemoryTrigger: No destination assigned on " + gameObject.name);
            return;
        }

        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerTransform.position = destination.position;
        playerTransform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        promptUI.SetActive(false);
        isPlayerNear = false;
        if (currentActivePrompt == this)
            currentActivePrompt = null;

        Debug.Log("Teleported to: " + destination.name);
    }
}