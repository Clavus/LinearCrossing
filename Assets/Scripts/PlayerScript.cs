using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    private float jumpDistance = 2;

	// Use this for initialization
	void Start () {
	    


	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetButtonDown("Start"))
	    {
            Debug.Log("Input tracking reset");
	        InputTracking.Recenter();
	    }

        if (Input.GetButtonDown("Back"))
        {
            transform.position += new Vector3(0,0,-jumpDistance);
        }

        if (Input.GetButtonDown("Right"))
        {
            transform.position += new Vector3(jumpDistance, 0, 0);
        }

        if (Input.GetButtonDown("Left"))
        {
            transform.position += new Vector3(-jumpDistance, 0, 0);
        }

        if (Input.GetButtonDown("Front"))
        {
            transform.position += new Vector3(0, 0, jumpDistance);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If other is a car
        CarScript car = other.gameObject.GetComponent<CarScript>();
        if (car != null)
        {
            car.Explode();
        }
    }

}
