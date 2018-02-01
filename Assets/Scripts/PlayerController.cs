using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Movement {

    private float moveInput;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !falling)
        {
            jump = true;
            fallThrough = false;
            falling = true;
        }
        if (Input.GetAxisRaw("Vertical") == -1 && Input.GetKeyDown(KeyCode.Space)) //if crouching and jump is pressed
        {
            jump = false;
            fallThrough = true;
        }
        moveInput = Input.GetAxis("Horizontal");
    }
    
    new private void FixedUpdate()
    {
        if (jump)
            _velocity = new Vector3(_velocity.x, 0.23f, 0); //0.23
        _velocity = new Vector3(moveInput / 10, _velocity.y, 0); //if input
        if (falling)
            _velocity += new Vector3(0, -0.009f, 0);
        transform.position += _velocity;
        CollisionAdjust();
    }
}
