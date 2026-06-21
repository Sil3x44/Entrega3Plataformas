using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform visual;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialogueBubble;
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private string[] dialogueLines;

    private int currentLine;
    private bool isTalking;
    private PlayerMovement playerMovement;

    public void Interact(PlayerInteractor player)
    {
        playerMovement = player.GetComponent<PlayerMovement>();

        if (!isTalking)
        {
            StartDialogue(player.transform);
        }
        else
        {
            ContinueDialogue();
        }
    }

    private void StartDialogue(Transform player)
    {
        isTalking = true;
        currentLine = 0;

        interactionPrompt.SetActive(false);
        dialogueBubble.SetActive(true);

        LookAtPlayer(player);

        playerMovement.SetControlsLocked(true);

        dialogueText.text = dialogueLines[currentLine];
    }

    private void ContinueDialogue()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }

    private void EndDialogue()
    {
        isTalking = false;
        dialogueBubble.SetActive(false);

        if (playerMovement != null)
        {
            playerMovement.SetControlsLocked(false);
        }
    }

    private void LookAtPlayer(Transform player)
    {
        if (player.position.x > transform.position.x)
        {
            visual.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            visual.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void ShowPrompt()
    {
        if (!isTalking)
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        interactionPrompt.SetActive(false);
    }
}
