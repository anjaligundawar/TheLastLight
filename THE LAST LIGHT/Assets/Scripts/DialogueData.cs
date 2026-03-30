using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)]
    public string dialogueText;
    public string animationTrigger;
}

[System.Serializable]
public class DialogueChoice
{
    [TextArea(1, 2)]
    public string choiceText;
    public string playerSpeakerName = "You";
    [TextArea(1, 3)]
    public string playerLine;
    public DialogueNode nextNode; // drag another asset here
}

// NOW a ScriptableObject — lives as an asset in your project
[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string nodeID;
    public List<DialogueLine> npcLines = new List<DialogueLine>();
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}