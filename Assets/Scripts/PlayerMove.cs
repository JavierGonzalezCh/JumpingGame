using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [Header("Ball movement")]
    [Space(10)]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private bool _onFloor = true;
    private float _moveInputX, _moveInputZ;
    private const int yLimit = -2; 
    private Rigidbody _rb;
    private Vector3 _initialPosition;

    //Structure of the platforms that fall when the user collides 
    private struct FakePlatformInfo
    {
        public Rigidbody fakePlatform;
        public Vector3 initialPosition;
    }
    private List<FakePlatformInfo> _fakePlatforms = new List<FakePlatformInfo>();

    private void Start()
    {
        //Save the ball Position
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _moveInputX = Input.GetAxis("Horizontal");
        _moveInputZ = Input.GetAxis("Vertical");

        if (transform.position.y < yLimit) Restart();
    }

    private void FixedUpdate()
    {
        PlayerMotion();
        
        //Deactivate the platforms that fell
        for (int i = 0; i < _fakePlatforms.Count; i++)
        {
            if (_fakePlatforms[i].fakePlatform.gameObject.activeInHierarchy && 
                _fakePlatforms[i].fakePlatform.transform.position.y < yLimit) 
                _fakePlatforms[i].fakePlatform.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Only a Jump
        _onFloor = true;

        //Falling platforms
        if (collision.gameObject.CompareTag("FakePlatform"))
        {
            SaveFakePlatformsPositions(collision.gameObject);
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            CheckPointInteractions(other.gameObject);
        }
    }

    private void PlayerMotion()
    {
        //Motion
        Vector3 move = new Vector3(_moveInputX, 0f, _moveInputZ);
        _rb.AddForce(move * (moveSpeed * Time.deltaTime));
        
        //Jump
        if (_onFloor && Input.GetKey(KeyCode.Space))
        {
            _onFloor = false;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Restart()
    {
        //Relocate the player
        transform.position = _initialPosition;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        //Relocate FakePlatforms
        for (int i = 0; i < _fakePlatforms.Count; i++)
        {
            _fakePlatforms[i].fakePlatform.gameObject.SetActive(true);
            _fakePlatforms[i].fakePlatform.useGravity = false;
            _fakePlatforms[i].fakePlatform.velocity = Vector3.zero;
            _fakePlatforms[i].fakePlatform.angularVelocity = Vector3.zero;
            _fakePlatforms[i].fakePlatform.transform.position = _fakePlatforms[i].initialPosition;
            _fakePlatforms[i].fakePlatform.transform.rotation = Quaternion.identity;
        }
        _fakePlatforms.Clear();
    }

    private void SaveFakePlatformsPositions(GameObject platform)
    {
        FakePlatformInfo newPlatform = new FakePlatformInfo
        {
            fakePlatform = platform.GetComponent<Rigidbody>(),
            initialPosition = platform.transform.position
        };
        if (_fakePlatforms.Contains(newPlatform)) return;
        _fakePlatforms.Add(newPlatform);
    }

    private void CheckPointInteractions(GameObject checkpoint)
    {
        _initialPosition = transform.position;
        Destroy(checkpoint);
    }
}