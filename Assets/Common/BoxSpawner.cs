using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPrefabs;
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] bool active = true;
    [SerializeField] bool speedUp = true;
    [SerializeField] bool randRotate = true;

    public float timeModifier = 1.0f;

    BoxCollider boxCollider = null;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        timer = Random.Range(minTime, maxTime);

        GameManager.Instance.startGameEvent += OnStartGame;
        GameManager.Instance.stopGameEvent += OnStopGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (speedUp)
            {
                timer = Random.Range(minTime, maxTime) * timeModifier;
            }
            else
            {
            timer = Random.Range(minTime, maxTime);
            }

            if (randRotate)
            {
            Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Length)], GetRandomPointInBoxCollider(), RandomizeRotation());
            } else
            {
            Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Length)], GetRandomPointInBoxCollider(), transform.rotation);
            }
        }
    }

    public void OnStartGame()
    {
        active = true;    
    }

    public void OnStopGame()
    {
        active = false;
    }

    public Quaternion RandomizeRotation()
    {
        Quaternion quat = Quaternion.identity;

        quat.x = Random.Range(0, 360);
        quat.y = Random.Range(0, 360);
        quat.z = Random.Range(0, 360);

        return quat;
    }

    Vector3 GetRandomPointInBoxCollider()
    {
        Vector3 point = Vector3.zero;

        Vector3 min = boxCollider.bounds.min;
        Vector3 max = boxCollider.bounds.max;

        point.x = Random.Range(min.x, max.x);
        point.y = Random.Range(min.y, max.y);
        point.z = Random.Range(min.z, max.z);

        return point;
    }
}
