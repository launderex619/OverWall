using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Spring_launcher : MonoBehaviour
{
    //resortera
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject leftSpring;
    [SerializeField]
    private GameObject rightSpring;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private GameObject[] criticalPoints;
    
    //constant points values { 0, 1, 2, 3}
    enum EPoints : int { LeftArm, LeftBase, RightArm, RightBase};


    private const float INITIAL_SIZE_SPRING = 1.70f;
    private const int CORRECTNESS_LEFT_ANGLE = 92;
    private const int CORRECTNESS_RIGHT_ANGLE = 110;
    private const int CORRECTNESS_PLAYER = 180;
    public Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        updateSprings();
    }

    //changes the angle, size and position of each spring
    private void updateSprings() {
        float leftSpringDistance, rightSpringDistance;
        //left spring position and angle
        leftSpring.transform.position =
            new Vector3((criticalPoints[(int)EPoints.LeftArm].transform.position.x + criticalPoints[(int)EPoints.LeftBase].transform.position.x) / 2f,
                (criticalPoints[(int)EPoints.LeftArm].transform.position.y + criticalPoints[(int)EPoints.LeftBase].transform.position.y) / 2f,
                0);
        leftSpring.transform.rotation = Quaternion.FromToRotation(transform.position - criticalPoints[(int)EPoints.LeftBase].transform.position,
            transform.position - criticalPoints[(int)EPoints.LeftArm].transform.position);
        leftSpring.transform.Rotate(0, 0, CORRECTNESS_LEFT_ANGLE);

        //rigth spring position and angle
        rightSpring.transform.position =
            new Vector3((criticalPoints[(int)EPoints.RightArm].transform.position.x + criticalPoints[(int)EPoints.RightBase].transform.position.x) / 2f,
                (criticalPoints[(int)EPoints.RightArm].transform.position.y + criticalPoints[(int)EPoints.RightBase].transform.position.y) / 2f,
                0);
        rightSpring.transform.rotation = Quaternion.FromToRotation(transform.position - criticalPoints[(int)EPoints.RightBase].transform.position,
            transform.position - criticalPoints[(int)EPoints.RightArm].transform.position);
        rightSpring.transform.Rotate(0, 0, CORRECTNESS_RIGHT_ANGLE);

        //left spring size
        leftSpringDistance = Vector3.Distance(transform.position - criticalPoints[(int)EPoints.LeftArm].transform.position,
            transform.position - criticalPoints[(int)EPoints.LeftBase].transform.position);
        leftSpring.transform.localScale = new Vector3(leftSpring.transform.localScale.x,
            leftSpringDistance / INITIAL_SIZE_SPRING, leftSpring.transform.localScale.z);

        //right spring size
        rightSpringDistance = Vector3.Distance(transform.position - criticalPoints[(int)EPoints.RightArm].transform.position,
            transform.position - criticalPoints[(int)EPoints.RightBase].transform.position);
        rightSpring.transform.localScale = new Vector3(rightSpring.transform.localScale.x,
            rightSpringDistance / INITIAL_SIZE_SPRING, rightSpring.transform.localScale.z);

        //player angle
        if (player != null && player.GetComponent<Player_Physics>().CanRotate) {
        player.transform.rotation = Quaternion.FromToRotation(transform.position - criticalPoints[(int)EPoints.LeftBase].transform.position,
            transform.position - criticalPoints[(int)EPoints.LeftArm].transform.position);
        player.transform.Rotate(0, 0, CORRECTNESS_PLAYER);
        }
    }
    private void OnMouseDown() {
        player.GetComponent<Player_Physics>().CanRotate = true;
    }

    private void OnMouseDrag() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePos.x, mousePos.y); //desperdicio de memoria aqui
    }

    private void OnMouseUp() {
        //super ineficiente, corregir en algun punto
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;
        player.AddComponent<Rigidbody2D>().mass = 10f;
        player.transform.parent = null;
        player.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        player.GetComponent<Player_Physics>().CanRotate = true;
        player.GetComponent<Player_Physics>().IsLaunched = true;
        player = null;
        Destroy(parent, 2f);
    }

}
