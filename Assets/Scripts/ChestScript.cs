using UnityEngine;
using System.Collections;

public class ChestScript : MonoBehaviour
{

    public int coins = 10;

    [SerializeField]
    private ParticleSystem coinShower;

    private Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
