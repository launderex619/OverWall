using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Physics : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool CanRotate { set; get; }
    public bool IsLaunched { set; get; }
    public float speed;
    public float rotation;
    public Vector2 velocity;
    public float fartEnergy = 20f;
    public float fartConsumerPerSecond = 15f;

    private enum RotationalState {RIGHT, LEFT, NONE }
    private enum BostQuadrant { TOP_LEFT, TOP_RIGHT, DOWN_LEFT, DOWN_RIGHT, TOP, LEFT, RIGHT, DOWN, NONE }
    private int actualRotationalState;
    private int actualQuadrant;
    private float rotationForce;

    public float BoostForce { set; get; }
    public float ROTATION_FORCE { get; private set; }

    void Start()
    {
        BoostForce = 1500f;
        CanRotate = false;
        IsLaunched = false;
        rotationForce = 0f;
        ROTATION_FORCE = .9f;
        actualRotationalState = (int)RotationalState.NONE;
        actualQuadrant = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsLaunched) {
            speed = GetComponent<Rigidbody2D>().velocity.magnitude;
            velocity = GetComponent<Rigidbody2D>().velocity;
            rotation = GetComponent<Rigidbody2D>().rotation;
            if(transform.position.y <= -1f) {
                GetComponent<Rigidbody2D>().drag = .25f;
            }
            else {
                GetComponent<Rigidbody2D>().drag = 0f;
            }
        }
        //for rotation 
        switch (actualRotationalState) {
            case (int)RotationalState.RIGHT:
                transform.Rotate(0, 0, -1);
                rotationForce = -1f;
                break;
            case (int)RotationalState.LEFT:
                transform.Rotate(0, 0, 1);
                rotationForce = 1f;
                break;
            case (int)RotationalState.NONE:
                transform.Rotate(0, 0, 0);
                break;
        }
        if (rotationForce < -.1f) {
            transform.Rotate(0, 0, rotationForce);
            rotationForce += ROTATION_FORCE * Time.deltaTime;
        }
        else if (rotationForce > .1f) {
            transform.Rotate(0, 0, rotationForce);
            rotationForce -= ROTATION_FORCE * Time.deltaTime;
        }

        //for boost
        if (fartEnergy > 0) {
            makeBoost();
        }
        else {
            fartEnergy = 0;
        }
        
    }

    private void makeBoost() {
        float modDegree;
        if (rotation <= 180f && rotation >= 0f) {
            modDegree = Mathf.Abs(rotation) % 90f;
        }
        else {
            modDegree = Mathf.Abs(rotation + 180f) % 90f;
        }
        Debug.Log(fartEnergy);
        switch (actualQuadrant) {
            case (int)BostQuadrant.TOP:
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, BoostForce));
                fartEnergy -= fartConsumerPerSecond * Time.deltaTime;
                break;
            case (int)BostQuadrant.DOWN:
                fartEnergy -= fartConsumerPerSecond * Time.deltaTime;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, (-1 * BoostForce)));
                break;
            case (int)BostQuadrant.RIGHT:
                fartEnergy -= fartConsumerPerSecond * Time.deltaTime;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(BoostForce, 0));
                break;
            case (int)BostQuadrant.LEFT:
                fartEnergy -= fartConsumerPerSecond * Time.deltaTime;
                GetComponent<Rigidbody2D>().AddForce(new Vector2((-1 * BoostForce), 0));
                break;
            case (int)BostQuadrant.TOP_LEFT:
                GetComponent<Rigidbody2D>().AddForce(getForceInQuadrant(actualQuadrant, modDegree));
                break;
            case (int)BostQuadrant.TOP_RIGHT:
                GetComponent<Rigidbody2D>().AddForce(getForceInQuadrant(actualQuadrant, modDegree));
                break;
            case (int)BostQuadrant.DOWN_LEFT:
                GetComponent<Rigidbody2D>().AddForce(getForceInQuadrant(actualQuadrant, modDegree));
                break;
            case (int)BostQuadrant.DOWN_RIGHT:
                GetComponent<Rigidbody2D>().AddForce(getForceInQuadrant(actualQuadrant, modDegree));
                break;
        }
    }

    private Vector2 getForceInQuadrant(int quadrant, float modDegree) {
        Vector2 force = new Vector2();

        fartEnergy -= fartConsumerPerSecond * Time.deltaTime;
        switch (quadrant) {
            case (int)BostQuadrant.TOP_LEFT:
                force.x = ((Mathf.Sin(modDegree * Mathf.Deg2Rad) * BoostForce) * -1) + velocity.x;
                force.y = (Mathf.Cos(modDegree * Mathf.Deg2Rad) * BoostForce) + velocity.y;
                break;
            case (int)BostQuadrant.TOP_RIGHT:
                force.x = (Mathf.Cos(modDegree * Mathf.Deg2Rad) * BoostForce);
                force.y = (Mathf.Sin(modDegree * Mathf.Deg2Rad) * BoostForce);
                break;
            case (int)BostQuadrant.DOWN_LEFT:
                force.x = -1 * (Mathf.Cos(modDegree * Mathf.Deg2Rad) * BoostForce);
                force.y = -1 * (Mathf.Sin(modDegree * Mathf.Deg2Rad) * BoostForce);
                break;
            case (int)BostQuadrant.DOWN_RIGHT:
                force.x = (Mathf.Sin(modDegree * Mathf.Deg2Rad) * BoostForce) + velocity.x;
                force.y = ((Mathf.Cos(modDegree * Mathf.Deg2Rad) * BoostForce) * -1) + velocity.y;

                break;
        }
        Debug.Log("mod: " + modDegree + " Velocidad.x: " + velocity.x + " Velocidad.y: " + velocity.y + " Fuerza: " + force);
        return force;
    }

    public void RotateToRight() {
        actualRotationalState = (int)RotationalState.RIGHT;
    }
    public void RotateToLeft() {
        actualRotationalState = (int)RotationalState.LEFT;
    }
    public void StopRotating() {
        actualRotationalState = (int)RotationalState.NONE;
    }
    public void Boost() {
        if (rotation > -1f && rotation < 1f) {
            actualQuadrant = (int)BostQuadrant.TOP;
        }
        else if (rotation > -91f && rotation < -89f) {
            actualQuadrant = (int)BostQuadrant.RIGHT;
        }
        else if (rotation > 89f && rotation < 91f) {
            actualQuadrant = (int)BostQuadrant.LEFT;
        }
        else if (rotation > 179f && rotation < -179f) {
            actualQuadrant = (int)BostQuadrant.DOWN;
        }
        else if (rotation >= 1f && rotation <= 89f) {
            actualQuadrant = (int)BostQuadrant.TOP_LEFT;
        }
        else if (rotation <= -1f && rotation >= -89f) {
            actualQuadrant = (int)BostQuadrant.TOP_RIGHT;
        }
        else if (rotation >= 91f && rotation <= 179f) {
            actualQuadrant = (int)BostQuadrant.DOWN_LEFT;
        }
        else if (rotation >= -179f && rotation <= -91f) {
            actualQuadrant = (int)BostQuadrant.DOWN_RIGHT;
        }
    }
    public void StopBoost() {
        actualQuadrant = (int)BostQuadrant.NONE;
    }
}
