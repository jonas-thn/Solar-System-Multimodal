using UnityEngine;

//Script um Himmelskörper um eigene Achse / Parent rotieren zu lassen
[ExecuteAlways]
public class RotatePlanet : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; 
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
