using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossDialogue : MonoBehaviour
{
    private LevelLoader _levelLoader;
    public List<Sprite> dialogues;
    public Image dialogueImage;
    
    private void Dialogue()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        int dialogueIndex = 0;
        dialogueImage.enabled = true;
        PlayerController.canBeControlled = false;

        while (dialogueIndex < dialogues.Count)
        {
            dialogueImage.sprite = dialogues[dialogueIndex];

            // Czekaj na naciśnięcie spacji
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            // Dodaj małe opóźnienie, aby uniknąć natychmiastowego wykrycia ponownego naciśnięcia spacji
            yield return new WaitForSeconds(0.1f);

            dialogueIndex++;
        }

        dialogueImage.enabled = false;
        PlayerController.canBeControlled = true;
        _levelLoader.LoadLevel(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Final"))
        {
            other.gameObject.SetActive(false);
            Dialogue();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelLoader = LevelLoader.instance;
    }
}
