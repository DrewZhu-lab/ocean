using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle : MonoBehaviour
{
    public GameObject Self;
    public GameObject Field;
    private bool free = true;

    // Bottle only has onCollision function. Check what it’s colliding with and set parent as needed.
    void OnCollisionEnter(Collision collision)
    {
	    // If the bottle isn’t eaten by a fish (ie is ‘Free)
        if (free) {
	        // If it’s a hand, set parent to the colliding hand and remove rigidbody to stop onCollision detection.
            if (collision.gameObject.tag == "Hand") {
                transform.SetParent(collision.gameObject.transform);
                transform.localPosition = Vector3.zero;
                free = false;
                Destroy(GetComponent<Rigidbody>());
	        // If it’s a fish, set parent to fish and remove rigidbody. Also set the bottle’s rotation to match the fish facing angle.
            } else if (collision.gameObject.tag == "Fish") {
                transform.SetParent(collision.gameObject.transform);
                transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.Euler(90 + ((Random.value - 1f) * 20), transform.rotation.y, transform.rotation.z);
                free = false;
                Destroy(GetComponent<Rigidbody>());
            }
        }
    }
}
