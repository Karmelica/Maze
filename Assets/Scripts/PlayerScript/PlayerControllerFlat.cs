using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerFlat : MonoBehaviour
{
    public List<Sprite> dialogues;
    public Image dialogueImage;
    private LevelLoader _loader;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    
    private void Dialogue()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        int dialogueIndex = 0;
        dialogueImage.enabled = true;

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
        _loader.LoadNextLevel();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hedge"))
        {
            other.gameObject.SetActive(false);
            Dialogue();
        }
        if(other.CompareTag("MovingPlatform"))
        {
            transform.parent = other.transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }

    void Start()
    {
        _loader = LevelLoader.instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        spriteRenderer.flipX = moveInput < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}