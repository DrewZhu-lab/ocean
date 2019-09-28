using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour
{
    public bool LeftHand = false;  //Check if hand is lefthand for values.
    public bool hasBottle = false;
    private bool grabbing = false;
    private float transSpeed = 0f;
    private float rotSpeed = 0f;
    private Vector3 startingPos;
    private Vector3 grabEnd_P;
    private Animator Anim;
    private int grabDuration = 0;
    private int handModifier = 1;

    void Start() {
        if (LeftHand) handModifier = -1;

        // store original position
        startingPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        // position for how far the hand moves.
        grabEnd_P = new Vector3(transform.localPosition.x - (0.2f * handModifier), transform.localPosition.y, transform.localPosition.z + 0.6f);
        // Get the animator component (to play animations). 
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset the position of all ‘grabbed’ bottles per frame.
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Bottle") {
                hasBottle = true;
                child.localPosition = new Vector3(-0.2f, -0.09f, 0.5f);
            }
        }

        // If hand is trying to grab, move hand towards the ‘grab’ destination.
        if (grabbing) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, grabEnd_P, transSpeed);

            grabDuration --;
            if (grabDuration == 0) { //if the time we’ve set to grab an object runs out, reset flags.
                grabbing = false;
                Anim.SetBool("IsGrabbing", false);
                transSpeed = 0.03f;
                transform.tag = "Untagged";
            }
        // otherwise, if the hand isn’t trying to grab, wait for ‘grab’ keyInput from user.
        } else {
            int keybind;
            if (LeftHand) { // Leftclick for lefthand, rightclick for righthand
                    keybind = 0;
            } else {
                    keybind = 1;
            }

	    	//set ‘grabbing’ state as true.
            if (Input.GetMouseButtonDown(keybind) && transform.localPosition == startingPos) {
                Anim.SetBool("IsGrabbing", true);
                grabbing = true;
                grabDuration = 25;
                transSpeed = 0.08f;
                transform.tag = "Hand"; //set tag as Hand to interact with bottle.cs and fish.cs
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startingPos, transSpeed);
        }
    }

    // Oncollision, if colliding object is bin, ‘delete’ the bottles we have.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bin") {
            if (hasBottle) {
                foreach (Transform child in transform) {
                    if (child.gameObject.tag == "Bottle") 
                        GameObject.Destroy(child.gameObject);
                }
                hasBottle = false;
            }
        }
    }
}
