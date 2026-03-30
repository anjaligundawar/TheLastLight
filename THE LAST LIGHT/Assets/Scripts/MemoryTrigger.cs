using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destination; // Drag the "Target_X" object here
    public float interactionDistance = 3f;

    [Header("References")]
    public GameObject promptUI; // Your "Press E" text
    
    private Transform playerTransform;
    private bool isPlayerNear = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance)
        {
            isPlayerNear = true;
            promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ExecuteTeleport();
            }
        }
        else if (isPlayerNear)
        {
            isPlayerNear = false;
            promptUI.SetActive(false);
        }
    }

    void ExecuteTeleport()
    {
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        
        // 1. Disable Physics (Required for CharacterController)
        if (cc != null) cc.enabled = false;

        // 2. Move Position and Match Rotation
        playerTransform.position = destination.position;
        playerTransform.rotation = destination.rotation;

        // 3. Re-enable Physics
        if (cc != null) cc.enabled = true;

        promptUI.SetActive(false);
        Debug.Log("Flashback Started!");
    }
}