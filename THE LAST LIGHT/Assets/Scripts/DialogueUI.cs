using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI bodyText;
    public GameObject promptObject;
    public Button nextButton;

    private string[] lines;
    private int index;
    private bool typing;
    private DialogueTrigger activeTrigger;

    void Start()
    {
        dialoguePanel.SetActive(false);
        promptObject.SetActive(false);
        nextButton.onClick.AddListener(Next);
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (typing)
            {
                StopAllCoroutines();
                bodyText.text = lines[index]; // instantly complete line
                typing = false;
            }
            else Next();
        }
    }

    public void ShowPrompt(bool show) => promptObject.SetActive(show);

    public void Show(string speaker, string[] dialogueLines, DialogueTrigger trigger)
    {
        activeTrigger = trigger;
        lines = dialogueLines;
        index = 0;
        speakerNameText.text = speaker;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine(lines[0]));
    }

    public void Hide()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        typing = false;
    }

    void Next()
    {
        index++;
        if (index < lines.Length)
            StartCoroutine(TypeLine(lines[index]));
        else
        {
            Hide();
            activeTrigger?.OnDialogueEnd();
        }
    }

    IEnumerator TypeLine(string line)
    {
        typing = true;
        bodyText.text = "";
        foreach (char c in line)
        {
            bodyText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        typing = false;
    }
}
