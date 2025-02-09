using UnityEngine;

public class BoxDragger : MonoBehaviour
{
    private AudioSource _audioSource;
    private Rigidbody _rb;
    private bool _isDragging;
    private Vector3 _dragOffset;
    public Transform player; // Reference to the player transform
    public float maxDistance = 1f; // Maximum distance to allow dragging
    public float dragForce = 20f; // Force applied to drag the box

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) && hit.transform == transform)
                if (IsBoxInFrontOfPlayer() && IsBoxWithinDistance())
                {
                    _audioSource.Play();
                    _isDragging = true;
                    _dragOffset = transform.position - hit.point;
                }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _audioSource.Stop();
        }
        
    }

    private void FixedUpdate()
    {
        if (_isDragging)
        {
            if (!IsBoxWithinDistance() || !IsBoxInFrontOfPlayer())
            {
                _isDragging = false;
                _audioSource.Stop();
            }
            var playerMovement = new Vector3(0, 0, Input.GetAxis("Vertical"));
            if (playerMovement.magnitude > 0)
            {
                var direction = player.TransformDirection(playerMovement.normalized);
                _rb.AddForce(direction * dragForce, ForceMode.Force);
            }
        }
    }

    private bool IsBoxInFrontOfPlayer()
    {
        var directionToBox = (transform.position - player.position).normalized;
        var dotProduct = Vector3.Dot(player.forward, directionToBox);
        return dotProduct > 0.5f; // Adjust the threshold as needed
    }

    private bool IsBoxWithinDistance()
    {
        var distanceToBox = Vector3.Distance(player.position, transform.position);
        return distanceToBox <= maxDistance;
    }
}