using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [Header("Player & Physics")]
    public GameObject player;
    public CharacterController playerController;

    [Header("Landing Pads")]
    public Transform landingPadFixed;  
    public Transform landingPadBroken; 

    [Header("State Control")]
    public bool isChoiceActive = false; // Turn this ON when the choice UI pops up

    void Update()
    {
        // Only listen for keys if the game is waiting for a choice
        if (isChoiceActive)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                SelectGoodOutcome();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                SelectBadOutcome();
            }
        }
    }

    public void SelectGoodOutcome()
    {
        isChoiceActive = false; // Lock the keys again
        TeleportPlayer(landingPadFixed);
        Debug.Log("Choice B: Mended Future.");
    }

    public void SelectBadOutcome()
    {
        isChoiceActive = false; // Lock the keys again
        TeleportPlayer(landingPadBroken);
        Debug.Log("Choice A: Broken Present.");
    }

    private void TeleportPlayer(Transform target)
    {
        if (playerController != null) playerController.enabled = false;

        player.transform.position = target.position;
        player.transform.rotation = target.rotation;

        if (playerController != null) playerController.enabled = true;
    }
}