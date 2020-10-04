using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LoopHandler : MonoBehaviour
{

    // public memebers
    public Transform startPos;
    public Level firstLevel;
    
    public GameObject player;
    public GameObject playerDummyPrefab;
    public int minReplayTimespan = 5;
    
     public int ReplayCount 
    { 
        get {
            return replayStack.Count;
        }
    }

    public bool CanReset
    {
        get {return !(Time.time - LastTime < (ReplayCount + 1) * minReplayTimespan); }
    } 

    public float LastTime
    {
        get;
        private set;
    }

    public int sampleFrequency = 20;

    // private memebers
    private List<Replay> replayStack;
    private List<Vector3> currentPositions;
    private int replayIndex = 0;

    private int sampleCount = 0;

    public void ResetFull()
    {
        Reset();
        ClearStack();
   }

    private List<IDynamic> registeredDyns;
    public void RegisterDynamic(IDynamic dyn)
    {
        registeredDyns.Add(dyn);
    }

   

    public void ClearStack()
    {
        replayIndex = 0;
        foreach (var r in replayStack)
        {
            r.Dummy = null;
            if(r.spwanedBlock)
                Destroy(r.spwanedBlock);
        }
        replayStack = new List<Replay>();
    }
    public void Reset()
    {
        foreach(var dyn in registeredDyns)
            dyn.Reset();
        player.transform.position = startPos.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        LastTime = Time.time;
        var replay = new Replay(this);
        replay.positions = currentPositions;
        replayStack.Add(replay);
        currentPositions = new List<Vector3>();
        replayIndex = 0;
    }
    
    public static LoopHandler Instance
    {
        get;
        private set;            
    }

    private void Awake()
    {
        Instance = this;
        firstLevel.StartLevel();
        currentPositions = new List<Vector3>();
        replayStack = new List<Replay>();
        registeredDyns = new List<IDynamic>();
    }


    private void FixedUpdate()
    {
        if(sampleCount == sampleFrequency) 
        {
            currentPositions.Add(player.transform.position);
            ReplayFrame();
            sampleCount = 0;
        }
        sampleCount ++;
    }

    public GameObject blockPrefab;
    private void ReplayFrame()
    {
        foreach(var replay in replayStack) 
        {
            
            var replayLength = replay.positions.Count;
            if(replayIndex < replayLength) 
            {
                if(replay.spwanedBlock != null)
                {
                    Destroy(replay.spwanedBlock);
                    replay.spwanedBlock = null;
                }
                var pos = replay.positions[replayIndex];
                Debug.DrawLine(pos + Vector3.up * 5, pos + Vector3.down * 5, Color.green);
                Debug.DrawLine(pos + Vector3.right * 5, pos + Vector3.left * 5, Color.green);
                replay.Dummy.transform.position = pos;
            } 
            else 
            {
                replay.Dummy = null;
                // create a block that can be used.
                if(replay.spwanedBlock == null)
                {
                    replay.spwanedBlock = Instantiate(blockPrefab, replay.positions.Last(), Quaternion.identity);
                }

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
        public GameObject spwanedBlock;
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
