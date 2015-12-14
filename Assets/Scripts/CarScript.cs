using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour
{

    public float speed = 10;
    public float randSpeedIncreasePerDifficulty = 10;
    public int numDifficultyScaleLevels = 10;

    [HideInInspector]
    public float netSpeed;

    [SerializeField]
    private Renderer randomizeColorTarget;

    [SerializeField]
    private Transform[] wheels;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private AudioSource explosionAudio;

    // Use this for initialization
    void Start ()
	{
	    if (randomizeColorTarget != null)
	        randomizeColorTarget.material.color = new Color(Random.value, Random.value, Random.value);

        int difficulty = Mathf.Min(numDifficultyScaleLevels, WorldBuilderScript.instance.CurrentDifficulty());
        netSpeed = speed + (randSpeedIncreasePerDifficulty * difficulty / numDifficultyScaleLevels * Random.value);
    }

    // Update is called once per frame
    void Update ()
	{
        transform.position += transform.forward*netSpeed*Time.deltaTime;

	    foreach (Transform t in wheels)
	        t.rotation = t.rotation*Quaternion.AngleAxis(Mathf.PI*2*40*Time.deltaTime, Vector3.right);

	}

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            if (player.TooBigToFail())
            {
                player.Shrink();
                Explode();
            }
            else
                player.Die(CauseOfDeath.CarCrash);
        }

        CarScript car = other.gameObject.GetComponent<CarScript>();
        if (car != null)
        {
            netSpeed = Mathf.Min(netSpeed, car.netSpeed); // move speed set to slowest vehicle
        }
    }

    public void Explode()
    {
        Instantiate(explosionAudio.gameObject, transform.position, Quaternion.identity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
