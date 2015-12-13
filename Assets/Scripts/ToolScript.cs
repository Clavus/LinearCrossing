using UnityEngine;
using System.Collections;

public class ToolScript : MonoBehaviour
{

    public ToolType toolType;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Quaternion look = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime*0.5f);
    }

    public void OnDiscard(bool used)
    {
        GetComponent<CapsuleCollider>().enabled = true;
       
        var body = gameObject.AddComponent<Rigidbody>();
        body.useGravity = true;
        body.AddForce((transform.up * 1.5f + Random.onUnitSphere).normalized * 5, ForceMode.Impulse);

        // destroy all child transforms if it was 'used'
        if (used)
            foreach(Transform t in transform)
                Destroy(t.gameObject);

        Invoke("DestroyMe", 3f);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

}
