using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steuerung der Spielfigur
/// </summary>
public class Player : MonoBehaviour {

    public float speed = 0.1f;
    public GameObject model = null;

    private float movingY = 0f;
    private float jumpPush = 6f;
    private float extraGravity = -3f;
    private bool isOnGround = false;

    private Rigidbody rigidb; // for jumping
    private Animator anim; // for animation blending

    private void Start() {
        rigidb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    //
    private void Update() {
        move();
        jump();
    }

    private void move() {
        float input = Input.GetAxis("Horizontal");
        anim.SetFloat("forward", Mathf.Abs(input));

        transform.position += input * speed * transform.forward;

        if (input > 0f) { // nach rechts gehen
            movingY = 0f;
        }
        else if (input < 0f) { // nach links gehen
            movingY = -180f;
        }

        // blend rotation
        //
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(0f, movingY, 0f), 0.3f);
    }

    private void jump() {
        RaycastHit hitInfo;
        isOnGround = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.12f);
        anim.SetBool("isIdling", isOnGround);

        if (Input.GetAxis("Jump") > 0f && isOnGround == true) {
            Vector3 jumpPower = rigidb.velocity;
            jumpPower.y = jumpPush;
            rigidb.velocity = jumpPower;
        }
        rigidb.AddForce(new Vector3(0f, extraGravity, 0f));
    }
}
