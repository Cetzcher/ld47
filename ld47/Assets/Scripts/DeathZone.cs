using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject == LoopHandler.Instance.player)
            LoopHandler.Instance.ResetFull();       
    }

}
