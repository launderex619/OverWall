using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Moovement : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
   
    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject ground;

    private Vector3 cameraPosition = Vector3.zero;
    private float groundPosY = -5.5f;
    private const float CAMERA_MARGIN = 5f; 

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        Rigidbody2D targetRigidBody = target.GetComponent<Rigidbody2D>();
        if (targetRigidBody != null) {
            /*  cameraPosition = new Vector3(
                  Mathf.SmoothStep(transform.position.x, target.position.x +4, 9f),
                  Mathf.SmoothStep(transform.position.y, target.position.y, 9f),
                  -10
                  );
                  */
            Vector3 pos = target.position;
            if(pos.y < -1f) {
                pos.y = -1f;
            }
            transform.position = pos + new Vector3(4, 0, -10);

        }
        ground.transform.position = new Vector2(transform.position.x, groundPosY);

    }
}
