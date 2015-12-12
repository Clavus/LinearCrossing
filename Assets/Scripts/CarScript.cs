using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour
{

    public float speed = 10;

    [SerializeField]
    private Transform[] wheels;

    [SerializeField]
    private GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	    transform.position += transform.forward*speed*Time.deltaTime;

	    foreach (Transform t in wheels)
	        t.rotation = t.rotation*Quaternion.AngleAxis(Mathf.PI*2*40*Time.deltaTime, Vector3.right);

	}

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            if (player.TooBigToFail())
                Explode();
            else
                player.Die();
        }
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
