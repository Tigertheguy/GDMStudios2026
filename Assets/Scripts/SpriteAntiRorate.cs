using UnityEngine;

public class SpriteAntiRorate : MonoBehaviour
{
    Quaternion initialRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
