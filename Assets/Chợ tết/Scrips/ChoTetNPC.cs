using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoTetNPC : MonoBehaviour
{
    public GameObject[] citizenPrefabs;
    public Transform npcPrefab;
    [Header("Movement")]
    public float minSpeed = 0.8f;
    public float maxSpeed = 2.5f;
public float lifeDuration = 20f;
    private float speed;

    private Vector3 direction;
    public void SetUp(Vector3 position, Vector3 direction)
    {
        this.transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
        //this.transform.localPosition = Vector3.zero;
        this.direction = direction;

        transform.localScale = Vector3.one * Random.Range(0.7f, 0.8f);
        this.speed = Random.Range(minSpeed, maxSpeed);

        StartCoroutine(MoveAndDestroy(speed));
    }
    IEnumerator MoveAndDestroy(float speed)
    {
        float timer = 0;
        while (timer < lifeDuration)
        {
            transform.position += direction * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
