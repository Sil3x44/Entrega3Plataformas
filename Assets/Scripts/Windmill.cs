using System;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed *Time.deltaTime, Space.Self);
    }
}
