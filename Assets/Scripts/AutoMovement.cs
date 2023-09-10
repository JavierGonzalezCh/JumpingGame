using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 0.0f;
    public float maxDistance = 0.0f;
    public Vector3 direction = Vector3.zero;

    private Vector3 _initialPosition;
    private float _sense = 1.0f;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        Vector3 motion = direction * (_sense * speed);
        transform.Translate(motion);

        if (Mathf.Abs(transform.position.x - _initialPosition.x) >= maxDistance ||
            Mathf.Abs(transform.position.y - _initialPosition.y) >= maxDistance ||
            Mathf.Abs(transform.position.z - _initialPosition.z) >= maxDistance)
        {
            _sense *= -1.0f;
        }
    }
}
