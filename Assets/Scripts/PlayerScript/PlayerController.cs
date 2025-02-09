using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
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
            SceneManager.LoadScene(2);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            other.gameObject.SetActive(false);
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
        if(canBeControlled)
        {
            Look();
            if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                if (Input.GetKeyDown(KeyCode.Space)) Jump();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _lookCamera = GetComponentInChildren<Camera>();
        _rb = GetComponent<Rigidbody>();
        _loader = LevelLoader.instance;
        _audioScript = AudioScript.instance;
    }
}