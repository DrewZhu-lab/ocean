using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : MonoBehaviour
{
    public GameObject Self;
    public GameObject Field;

    private float max_X = 0;
    private float max_Y = 0;
    private float max_Z = 0;
    private int dir_Z = 1;
    private float MySpeed = 0.04f;
    public bool filled = false;
    private Vector3 destPos;
    private Vector3 startPos;
    private float swimDuration = 0;
    private bool inTravel = false;
    private float swimRotation = 0;
    private float rotationIncrement = 1;
    private MeshRenderer myRenderer;

   void Start() {
        //  Y =4 means 4 units tall  and fish can only swim between 0-4 
        max_X = Field.transform.localScale.x * 2 / 4;
        max_Z = Field.transform.localScale.z * 2/ 4;
        max_Y = 4;
    }
    void Update() {
        //Self.GetComponentInChildren<Animation>().Play("bigfishanimation");
        //this is the rotation animation for fish in y-axis (x and z direction), currently just finished the y-axis rotation
        // Root （0，0）
        //   |__
        //      |
	    //   Parent （LocalTransform = -1，0） - World（-1，0）
		//      |__
		//         |
		//      Child （LocalTransform= 0，2）- World（-1， 2）
        transform.Rotate(0f, rotationIncrement, 0f, Space.Self);
        // how much fish turns every update.
        swimRotation += rotationIncrement;
        // 15 degree is a good angle.
        if (swimRotation >= 15f || swimRotation <= -15f) {
            rotationIncrement = rotationIncrement * -1f;
        }
        // flag that idetify the fish is moving.
        if (inTravel) {

            transform.position = Vector3.MoveTowards(transform.position, destPos, MySpeed);
            swimDuration --; // how long the fish can swim. for edge cases : that give the fish a very fast speed. If the fish has reached destination, or has exceeded the time we’ve given it, set inTravel to false and find new destination
            if (swimDuration <= 0 || transform.position == destPos) inTravel = false;
        } else {
            // randomize the fishes’ direction and speed.
            if (Random.value > 0.4f) {
	            // Random.value gives a float between 0 < value < 1. minus 0.5 from this to give -0.5 < value < 0.5;
                startPos = transform.position;
                destPos = new Vector3((float)(Random.value - 0.5f) * 2 * max_X, (float)(Random.value * max_Y), (float)(Random.value - 0.5f) * 2 * max_Z);
                Vector3 relativePos = destPos - startPos;
                transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);

                float travelSpeed = Random.value + 0.1f;
                MySpeed = travelSpeed / 10f;
                rotationIncrement = travelSpeed * 1.4f;

	            // Random value between 0 < value < 400;
                swimDuration = Random.value * 400;

                inTravel = true;
                swimRotation = 0f; // reset the rotation angle every time           
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // if onCollision the fish already contains the bottle then check the the object is hand or not
        if (filled) {  
            if (collision.gameObject.tag == "Hand") {
                Transform[] Children = transform.GetComponentsInChildren<Transform>();
	            //if hand, then change all bottle parents to the new hand.
                foreach(Transform child in Children) {
                    if (child.gameObject.tag == "Bottle") {
                        child.SetParent(collision.gameObject.transform); 
                        child.localPosition = Vector3.zero;
                    }
                }
                filled = false;
                ChangeMaterialToMode("Opaque");
            }
        // if the fish doesn’t contain the bottle then set the bottle as the child object of the and set the mode as the transparent
        } else {  
            if (collision.gameObject.tag == "Bottle") {
                filled = true;
                ChangeMaterialToMode("Transparent");
            }
        }

        if (collision.gameObject.tag == "Environment") {
            destPos = startPos;
            Vector3 relativePos = destPos - transform.position;
            transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            MySpeed = 0.025f;
            rotationIncrement = 0.28f;
            swimDuration = 60;
        }
    }

    // Change between opaque and transparency material for fish. Make a copy per change so each fish has their own material.
    void ChangeMaterialToMode(string Mode) {
        Component[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children) {
            Material temp = child.material;
            Material newMat = new Material(child.material);
            SetMaterialMode(newMat, Mode);
            child.material = newMat;
            Destroy(temp);    //Garbage Collection
        }
    }

    //Set all the flags in the material to reflect opaque / transparent.
    void SetMaterialMode(Material myMaterial, string Mode) {
        switch(Mode) {
            case "Opaque":
                myMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                myMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                myMaterial.SetInt("_ZWrite", 1);
                myMaterial.DisableKeyword("_ALPHATEST_ON");
                myMaterial.DisableKeyword("_ALPHABLEND_ON");
                myMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                myMaterial.renderQueue = -1;
                break;
            case "Transparent":
                myMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                myMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                myMaterial.SetInt("_ZWrite", 0);
                myMaterial.DisableKeyword("_ALPHATEST_ON");
                myMaterial.DisableKeyword("_ALPHABLEND_ON");
                myMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                myMaterial.renderQueue = 3000;
                break;
            default:
                break;
        }
    }
}
