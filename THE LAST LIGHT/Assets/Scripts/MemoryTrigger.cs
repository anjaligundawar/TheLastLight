using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    [Header("Destination")]
    public Transform destination; // Drag 'Target_beer' here

    [Header("Manual References")]
    public GameObject player;    // Drag 'test_player' here
    public GameObject promptUI;  // Drag 'PromptUI' here
    
    public float interactionDistance = 5f;

    void Update()
    {
        // Safety check to prevent red errors
        if (player == null || destination == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactionDistance)
        {
            if (promptUI != null) promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ExecuteTeleport();
            }
        }
        else
        {
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    void ExecuteTeleport()
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        
        if (cc != null) cc.enabled = false;

        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;
        
        if (promptUI != null) promptUI.SetActive(false);
        Debug.Log("Success! Teleported to " + destination.name);
    }
}