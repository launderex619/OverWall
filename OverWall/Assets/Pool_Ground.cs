using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Ground : MonoBehaviour
{
    public GameObject player;
    private float startPos = -9f;
    private float limitPos = -19f;
    private Player_Physics playerInfo;
    public Vector3 pos;

    private void Start() {
        playerInfo = player.GetComponent<Player_Physics>();
        
    }

    void Update()
    {
        pos = GetComponent<Transform>().position;
        if (playerInfo.IsLaunched) {
            float velPlayerX = player.GetComponent<Rigidbody2D>().velocity.x;
            transform.localPosition += Vector3.left * velPlayerX * Time.deltaTime;

            if (transform.localPosition.x < limitPos) {
                transform.localPosition = Vector3.right * startPos;
            }
            else if (transform.localPosition.x > startPos) {
                transform.localPosition = Vector3.right * limitPos;
            }
        }
    }
}
