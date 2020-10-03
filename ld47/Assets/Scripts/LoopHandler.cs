using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LoopHandler : MonoBehaviour
{

    // public memebers
    public Transform startPos;
    
    public GameObject player;
    public GameObject playerDummyPrefab;
    public int minReplayTimespan = 5;
    
     public int ReplayCount 
    { 
        get {
            return replayStack.Count;
        }
    }

    public float LastTime
    {
        get;
        private set;
    }

    // private memebers
    private List<Replay> replayStack;
    private List<Vector3> currentPositions;
    private int replayIndex = 0;
   

    private void Start() 
    {
        currentPositions = new List<Vector3>();
        replayStack = new List<Replay>();
    }
    private void Reset()
    {
        player.transform.position = startPos.position;
        LastTime = Time.time;
        var replay = new Replay(this);
        replay.positions = currentPositions;
        replayStack.Add(replay);
        currentPositions = new List<Vector3>();
        replayIndex = 0;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown("space")) {
            // put player to start
            if(Time.time - LastTime < (ReplayCount + 1) * minReplayTimespan)
                return;
            
            Reset();
        }
    }

    private void FixedUpdate()
    {
        currentPositions.Add(player.transform.position);
        ReplayFrame();
    }

    private void ReplayFrame()
    {
        foreach(var replay in replayStack) 
        {
            var replayLength = replay.positions.Count;
            if(replayIndex < replayLength) 
            {
                var pos = replay.positions[replayIndex];
                Debug.DrawLine(pos + Vector3.up * 5, pos + Vector3.down * 5, Color.green);
                Debug.DrawLine(pos + Vector3.right * 5, pos + Vector3.left * 5, Color.green);
                replay.Dummy.transform.position = pos;
            } 
            else 
            {
                replay.Dummy = null;
            }
        }
        replayIndex++;       
    }
    public class Replay 
    {

        public Replay(LoopHandler outer)
        {
            this.outer = outer;
        }

        public List<Vector3> positions;
        private GameObject dummy;
        private bool alive = false;
        private LoopHandler outer;
        public GameObject Dummy {
            get {
                if (!alive) 
                {
                    alive = true;
                    dummy = Instantiate(outer.playerDummyPrefab, Vector3.zero, Quaternion.identity);
                }
                return dummy;
            }
            set {
                if(value == null)
                {
                    Destroy(dummy);
                    alive = false;
                }
            }
        }
    }
}
