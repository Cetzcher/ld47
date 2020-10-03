using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform nozzle;
    public float fuzzyness = 0.005f;
    private List<Vector3> points;
    public GameObject impactPrefab;
    private List<Vector3> lastPoints;
    private List<GameObject> oldParticles;
    void Start()
    {
        oldParticles = new List<GameObject>();
        points = new List<Vector3>();
        lastPoints = new List<Vector3>();
        lineRenderer = nozzle.GetComponent<LineRenderer>();
    }

    private void Prune() 
    {
        var pruned = new List<Vector3>();
        for(int i = 0; i < points.Count - 1; i += 2)
        {
            pruned.Add(points[i]);
        }
        pruned.Add(points[points.Count -1]);
        points = pruned;
    }

    // return false if  changed
    private bool HasChanged()
    {
        Debug.Log("Compares last = " + lastPoints.Count + " current " + points.Count);
        if(lastPoints.Count == points.Count)
        {
            for(int i = 0; i < lastPoints.Count; i++)
            {
                if(lastPoints[i] != points[i])
                    return true;
            }
            return false;
        }
        return true;
    }

    private void RenderLazer()
    {
        var pos = nozzle.position;
        var dir = (pos - transform.position).normalized;
        // raycast in that dir unitl we hit something
        lineRenderer.positionCount = 0; 
        points = new List<Vector3>();
        Reflect(pos, dir);
        if(points.Count == 0)
            return;
        // prune points
        Prune();
        bool hasChanged = HasChanged();
        // now copy points into lastPoints
        lastPoints = new List<Vector3>();
        foreach(var p in points) lastPoints.Add(p);
        if(hasChanged)
        {
            CreateParticles();
        }
        lineRenderer.positionCount = points.Count;
        for(int i = 0; i < points.Count; i++)
        {
            var perm = new Vector3(
                Random.Range(-fuzzyness, fuzzyness),
                Random.Range(-fuzzyness, fuzzyness)                
            );
            lineRenderer.SetPosition(i, points[i] + perm);
        }
    }

    private void CreateParticles()
    {
        Debug.Log("Creating paritcles");
        foreach(var p in oldParticles)
        {
            Destroy(p);
        }
        oldParticles = new List<GameObject>();
        for(int i = 1; i < points.Count; i++)
        {
            var pos = points[i];
            var spawn = new Vector3(pos.x, pos.y, 3);
            oldParticles.Add(Instantiate(impactPrefab, spawn, Quaternion.identity));
        }
    }

    private List<Vector3> Interpolate(List<Vector3> pos)
    {
        var modified = new List<Vector3>();
        for(int i = 0; i < pos.Count - 1; i++)
        {
            var cur = pos[i];
            var next = pos[i + 1];
            var t1 = Vector3.Lerp(cur, next, 1f/4f);
            var t2 = Vector3.Lerp(cur, next, 1f/2f);
            var t3 = Vector3.Lerp(cur, next, 3/4f);
            modified.Add(cur);
            modified.Add(t1);
            modified.Add(t2);
            modified.Add(t3);
            if(i == pos.Count - 2)
            {
                modified.Add(next); //  only add the next if it is the last
                // elem since the 'next' cur will be added by the loop anyways
            }
        }
        return modified;
    }

    private void Reflect(Vector3 position, Vector3 direction)
    {
        Debug.DrawRay(position, direction, Color.red);
        var hit = Physics2D.Raycast(position, direction);
        if(hit.collider != null) {
            var intersection = new Vector3(hit.point.x, hit.point.y);
            var surfaceNormal = hit.normal;
            Debug.DrawRay(intersection, surfaceNormal, Color.blue);
            var reflected = Vector3.Reflect(direction, surfaceNormal);
            points.Add(position);
            points.Add(intersection);
            // when we add points we 
            if(true /*magic*/)
            {
                var eps = reflected * 0.01f;
                Reflect(intersection + eps, reflected);
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        RenderLazer();
    }
}
