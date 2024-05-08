using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public bool isCollected;
    private Transform player;
    private void Start()
    {
        isCollected = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        if (isCollected)
        {
            Vector3 target = player.position;
            target.y = player.position.y + 1f;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10f);
            if (Vector3.Magnitude(gameObject.transform.position - target) < 0.2f)
            {
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 10);
                Destroy(gameObject);
            }
        }
    }
}
