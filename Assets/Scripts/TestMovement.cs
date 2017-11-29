using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    float runSpeed = 5;
    Rigidbody2D rb;

    Vector2 movement;
    float xMove;
    float xMovePrev;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        xMovePrev = xMove;

        float h = Input.GetAxis("Left Hor");

        ////store wish move Right Hor
        xMove = h * runSpeed;

        ////move Right Horly
        ////rb.velocity += new Vector2(xMove - rb.velocity.x, 0);


        //    //Store the current horizontal input in the float moveHorizontal.
        //    float moveHorizontal = Input.GetAxis("Left Hor");

        //    //Store the current vertical input in the float moveVertical.
        //    //float moveVertical = Input.GetAxis("Vertical");

        //    //Use the two store floats to create a new Vector2 variable movement.


        float force = xMove - xMovePrev;

            Vector2 movement = new Vector2((0.4f * force) + force, 0);



        //    //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //    rb.AddForce(movement * runSpeed, ForceMode2D.Impulse);


        float targetX = xMove - rb.velocity.x;

        //we want constant velocity
        //but we dont want to subtract the rigidbody velocity

        //rb.velocity += new Vector2(xMove - xMovePrev, 0);
        rb.AddForce(movement, ForceMode2D.Impulse);

        print(rb.velocity);
    }
}
