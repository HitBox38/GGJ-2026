using System;
using UnityEngine;

public class Goose : MonoBehaviour
{
    [SerializeField] private float ravenPatrolRadius = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    
    public Transform GetPointA() => pointA;
    public Transform GetPointB() => pointB;
    
    public event Action OnCallRaven;

    private void Start()
    {
        pointA.position = transform.position + new Vector3(ravenPatrolRadius, 3);
        pointB.position = transform.position + new Vector3(-ravenPatrolRadius, 3);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            OnCallRaven?.Invoke();
        }
    }
}
