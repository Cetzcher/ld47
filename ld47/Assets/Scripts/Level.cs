using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public Transform cameraPosition;
    public Camera camera;
    public Level nextLevel;
    public Transform spawn;

    // Start is called before the first frame update

    public void StartLevel()
    {
        camera.transform.position = cameraPosition.position;
        LoopHandler.Instance.startPos = spawn;
    }

    private IEnumerator LevelTransition(Level to)
    {
        Vector3 dist;
        do
        {
            // freeze the player
            LoopHandler.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            dist = to.cameraPosition.position - camera.transform.position;    
            var dir = dist.normalized;
            camera.transform.position += 12 * dir * Time.deltaTime;
            yield return null;
        } while(dist.magnitude > 0.1f);
        nextLevel.StartLevel();
        LoopHandler.Instance.ClearStack();
    }
    // Update is called once per frame
    public void Exit()
    {
        Debug.Log("level Exit");
        if(nextLevel != null)
        {
            StartCoroutine(LevelTransition(nextLevel));
        }
    }
}
