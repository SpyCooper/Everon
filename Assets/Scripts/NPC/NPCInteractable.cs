using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private GameObject ui;
    [SerializeField] private string text;
    [SerializeField] private bool hasQuest;

    public Dialogue dialogue;
    public bool interacted = false;

    private void Start()
    {
        HideUI();
    }
    
    private void Update()
    {
        IInteractable interactable = playerInteract.GetInteractableObject();
        if(interactable != null && (object)interactable == this)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }
    }

    public void Interact()
    {
        if(interacted == false)
        {
            DialogueManager.Instance.StartDialogue(dialogue, this);
            Debug.Log("This NPC has a quest = " + hasQuest);
        }
    }

    private void ShowUI()
    {
        ui.SetActive(true);
    }

    private void HideUI()
    {
        ui.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
