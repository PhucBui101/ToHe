using UnityEngine;

public class GentleFloat : MonoBehaviour
{
    public float floatSpeed = 1f;    // How fast it bobs
    public float floatMagnitude = 10f; // How far it moves (pixels)
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Simple vertical bobbing motion
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}