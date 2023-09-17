using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set;}

    public GameObject dialougeUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;

    private NPCInteractable currentNPC;

    private Queue<string> sentences;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
        HideUIDialogueUI();
    }

    public void StartDialogue(Dialogue dialogue, NPCInteractable npcInteractable)
    {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        nameText.text = dialogue.name;
        currentNPC = npcInteractable;
        currentNPC.interacted = true;

        ShowDialogueUI();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private void EndDialogue()
    {
        HideUIDialogueUI();
        currentNPC.interacted = false;
    }

    private void ShowDialogueUI()
    {
        dialougeUI.SetActive(true);
        PlayerManager.Instance.player.GetComponent<PlayerController>().LockPlayerMovement();
    }
    
    private void HideUIDialogueUI()
    {
        dialougeUI.SetActive(false);
        PlayerManager.Instance.player.GetComponent<PlayerController>().UnlockPlayerMovement();
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }
}
