using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpikes : MonoBehaviour, IDynamic
{
    // Start is called before the first frame update

    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Awake()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    private void Start()
    {
        LoopHandler.Instance.RegisterDynamic(this);

    }


    public void Reset()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        Rigidbody2D rb;
        if(TryGetComponent<Rigidbody2D>(out rb))
            rb.velocity = Vector2.zero;
    }
}
