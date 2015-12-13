using UnityEngine;
using System.Collections;

[SelectionBase]
public class ChestScript : MonoBehaviour
{

    [Range(0,1)]
    public float chanceToSpawn = 1;
    public int coins = 10;

    [SerializeField]
    private ParticleSystem coinShower;

    private Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start ()
	{
	    if (Random.value > chanceToSpawn)
	        Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.GetComponent<PlayerScript>();

        if (player != null)
        {
            transform.parent = player.transform;
            player.CollectCoins(coins);
            coinShower.Play();
            body.useGravity = true;
            body.AddForce((transform.position - (player.transform.position + Vector3.down)).normalized * 15, ForceMode.Impulse);
            Invoke("DestroyMe",5f);
        }
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
