// DialogueUI.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    // ✅ ADDED — lets MemoryTrigger check if dialogue is active
    public bool IsOpen { get; private set; }

    [Header("Dialogue Panel")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI bodyText;
    public Button nextButton;
    public GameObject promptObject;

    [Header("Choice Panel")]
    public GameObject choicePanel;
    public Button choiceButtonA;
    public Button choiceButtonB;
    public TextMeshProUGUI choiceAText;
    public TextMeshProUGUI choiceBText;

    private DialogueNode currentNode;
    private int lineIndex;
    private bool typing;
    private bool waitingForChoice;
    private DialogueTrigger activeTrigger;
    private DialogueChoice pendingChoice;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        promptObject.SetActive(false);

        nextButton.onClick.AddListener(OnNextClicked);
        choiceButtonA.onClick.AddListener(() => OnChoiceSelected(0));
        choiceButtonB.onClick.AddListener(() => OnChoiceSelected(1));
    }

    void Update()
    {
        if (waitingForChoice)
        {
            if (Input.GetKeyDown(KeyCode.A)) OnChoiceSelected(0);
            if (Input.GetKeyDown(KeyCode.B)) OnChoiceSelected(1);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                waitingForChoice = false;
                Hide();
                activeTrigger?.OnDialogueEnd();
            }
            return;
        }

        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (typing)
                SkipTypewriter();
            else
                OnNextClicked();
        }
    }

    public void ShowPrompt(bool show) => promptObject.SetActive(show);

    public void StartNodeDialogue(DialogueNode startNode, DialogueTrigger trigger)
    {
        activeTrigger = trigger;
        currentNode = startNode;
        lineIndex = 0;
        waitingForChoice = false;

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        promptObject.SetActive(false);

        // ✅ ADDED
        IsOpen = true;

        ShowCurrentLine();
    }

    public void Hide()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        waitingForChoice = false;
        typing = false;

        // ✅ ADDED
        IsOpen = false;
    }

    void ShowCurrentLine()
    {
        if (lineIndex >= currentNode.npcLines.Count)
        {
            ShowChoicesOrEnd();
            return;
        }

        DialogueLine line = currentNode.npcLines[lineIndex];
        speakerNameText.text = line.speakerName;

        if (!string.IsNullOrEmpty(line.animationTrigger) && activeTrigger != null)
            activeTrigger.TriggerAnimation(line.animationTrigger);

        nextButton.gameObject.SetActive(true);
        StartCoroutine(TypeLine(line.dialogueText));
    }

    void OnNextClicked()
    {
        if (typing) { SkipTypewriter(); return; }
        lineIndex++;
        ShowCurrentLine();
    }

    void ShowChoicesOrEnd()
    {
        if (currentNode.choices == null || currentNode.choices.Count == 0)
        {
            Hide();
            activeTrigger?.OnDialogueEnd();
            return;
        }

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(true);
        waitingForChoice = true;

        choiceAText.text = $"[A]  {currentNode.choices[0].choiceText}";
        choiceButtonA.gameObject.SetActive(true);

        if (currentNode.choices.Count > 1)
        {
            choiceBText.text = $"[B]  {currentNode.choices[1].choiceText}";
            choiceButtonB.gameObject.SetActive(true);
        }
        else
        {
            choiceButtonB.gameObject.SetActive(false);
        }
    }

    void OnChoiceSelected(int index)
    {
        if (!waitingForChoice) return;
        if (index >= currentNode.choices.Count) return;

        waitingForChoice = false;
        choicePanel.SetActive(false);
        pendingChoice = currentNode.choices[index];

        if (!string.IsNullOrEmpty(pendingChoice.playerLine))
        {
            dialoguePanel.SetActive(true);
            speakerNameText.text = pendingChoice.playerSpeakerName;
            nextButton.gameObject.SetActive(true);
            StartCoroutine(TypeLine(pendingChoice.playerLine, () =>
            {
                StartCoroutine(WaitForEnterThenBranch());
            }));
        }
        else
        {
            StartCoroutine(DelayThenBranch());
        }
    }

    IEnumerator WaitForEnterThenBranch()
    {
        yield return new WaitUntil(() =>
            !Input.GetKey(KeyCode.Return) &&
            !Input.GetKey(KeyCode.Space) &&
            !Input.GetKey(KeyCode.E) &&
            !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.B));

        yield return new WaitUntil(() =>
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space));

        BranchToChoice(pendingChoice);
    }

    IEnumerator DelayThenBranch()
    {
        yield return new WaitUntil(() =>
            !Input.GetKey(KeyCode.Return) &&
            !Input.GetKey(KeyCode.Space) &&
            !Input.GetKey(KeyCode.E) &&
            !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.B));

        yield return null;
        BranchToChoice(pendingChoice);
    }

    void BranchToChoice(DialogueChoice chosen)
    {
        if (chosen.nextNode != null)
        {
            currentNode = chosen.nextNode;
            lineIndex = 0;
            dialoguePanel.SetActive(true);
            ShowCurrentLine();
        }
        else
        {
            Hide();
            activeTrigger?.OnDialogueEnd();
        }
    }

    void SkipTypewriter()
    {
        StopAllCoroutines();
        if (lineIndex < currentNode.npcLines.Count)
            bodyText.text = currentNode.npcLines[lineIndex].dialogueText;
        typing = false;
    }

    IEnumerator TypeLine(string line, System.Action onComplete = null)
    {
        typing = true;
        bodyText.text = "";
        foreach (char c in line)
        {
            bodyText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        typing = false;
        onComplete?.Invoke();
    }
}