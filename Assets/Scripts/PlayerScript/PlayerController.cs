using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    public List<Sprite> dialogues;
    public Image dialogueImage;
    
    public AudioClip jumpSound;
    public AudioClip walkSound;
    private AudioSource _audioSource;
    public VideoPlayer videoPlayer;
    private AudioScript _audioScript;
    private LevelLoader _loader;
    static public bool canBeControlled = true;
    private Camera _lookCamera;
    private Rigidbody _rb;
    public float jumpForce = 250f; // siła skoku
    public float moveSpeed = 1000f; // prędkość poruszania się
    public float horizontalDrag = 0.5f; // współczynnik oporu poziomego
    public float minY = -80f; // minimalny kąt obrotu w osi Y
    public float maxY = 80f;  // maksymalny kąt obrotu w osi Y
    private float _currentXRotation; // bieżący kąt obrotu w osi X

    private void Dialogue()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        int dialogueIndex = 0;
        dialogueImage.enabled = true;
        canBeControlled = false;

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
        canBeControlled = true;
    }
    
    private IEnumerator CutScene()
    {
        if(videoPlayer != null)
        {
            canBeControlled = false;
            videoPlayer.enabled = true;
            videoPlayer.Play();
            yield return new WaitForSeconds((float)videoPlayer.clip.length);
            videoPlayer.Stop();
            videoPlayer.enabled = false;
            canBeControlled = true;
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                _loader.LoadLevel(2);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                _loader.LoadLevel(0);
            }
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            other.gameObject.SetActive(false);
            Dialogue();
        }

        if (other.CompareTag("Hedge"))
        {
            other.gameObject.SetActive(false);
            _loader.LoadNextLevel();
        }

        if (other.CompareTag("Maze"))
        {
            other.gameObject.SetActive(false);
            _audioScript.PlayMusic(0);
            StartCoroutine(CutScene());
        }

        if (other.CompareTag("MazeFinish"))
        {
            _loader.lvl2Unlocked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }

    private void Jump()
    {
        _rb.AddForce(new Vector3(0, jumpForce, 0));
        _audioSource.clip = jumpSound;
        _audioSource.Play();
    }

    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var movement = transform.forward * moveVertical + transform.right * moveHorizontal;

        _rb.AddForce(movement.normalized * (Time.deltaTime * moveSpeed));
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX);

        _currentXRotation -= mouseY;
        _currentXRotation = Mathf.Clamp(_currentXRotation, minY, maxY);
        _lookCamera.transform.localEulerAngles = new Vector3(_currentXRotation, 0, 0);
    }

    void FixedUpdate()
    {
        if(canBeControlled)
        {
            Look();
            Movement();
            ApplyHorizontalDrag();
        }
    }

    private void ApplyHorizontalDrag()
    {
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        Vector3 horizontalDragForce = -horizontalVelocity * horizontalDrag;
        _rb.AddForce(horizontalDragForce, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        if(_rb.velocity.magnitude > 0.1f && !_audioSource.isPlaying)
        {
            _audioSource.clip = walkSound;
            _audioSource.Play();
        }
        else if(_rb.velocity.magnitude < 0.1f && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        if(canBeControlled)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                if (Input.GetKeyDown(KeyCode.Space)) Jump();
            }
        }
        if(!canBeControlled)
        {
            _rb.velocity = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canBeControlled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _lookCamera = GetComponentInChildren<Camera>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _loader = LevelLoader.instance;
        _audioScript = AudioScript.instance;
    }
}