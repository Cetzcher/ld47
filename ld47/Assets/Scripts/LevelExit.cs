using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{

    public Level level;
    // start collider is acvive before the exit is hit to prevent the player from escaping the level
    // leaveCollider is active after that to stop the player going back
    public Collider2D startCollider;
    public Collider2D leaveCollider;
    
    private bool hasHit = false;

    void Awake()
    {
        startCollider.gameObject.SetActive(false);
        leaveCollider.gameObject.SetActive(true);
    }
    public void OnTriggerEnter2D()
    {
        if(!hasHit)
            level.Exit();
        hasHit = true;
        leaveCollider.gameObject.SetActive(false);
        startCollider.gameObject.SetActive(transform);
    }
    // Update is called once per frame
}
