using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform nozzle;
    private LineRenderer lineRenderer;
    public int segmentCount = 10;
    public float fuzzyness = 0.005f;
    private List<Vector3> points;
    private int c = 0;
    void Start()
    {
        points = new List<Vector3>();
        lineRenderer = nozzle.GetComponent<LineRenderer>();
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
        List<Vector3> actualPos = new List<Vector3>();
        actualPos.Add(points[0]);
        for(int i = 1; i < points.Count / 2; i ++) {
            var cur = points[i * 2];
            actualPos.Add(cur);
        }
        actualPos.Add(points[points.Count - 1]);
        actualPos = Interpolate(actualPos);
        /*
        lineRenderer.positionCount = points.Count / 2 + 1;
        lineRenderer.SetPosition(0, points[0]);
        lineRenderer.SetPosition(lineRenderer.positionCount -1, points[points.Count - 1]);
        for(int i = 1; i < points.Count / 2; i ++) {
            var cur = points[i * 2];
            lineRenderer.SetPosition(i, cur);
        }
        Permutate();
        */
        lineRenderer.positionCount = actualPos.Count;
        for(int i = 0; i < actualPos.Count; i++)
        {
            var perm = new Vector3(
                Random.Range(-fuzzyness, fuzzyness),
                Random.Range(-fuzzyness, fuzzyness)                
            );
            lineRenderer.SetPosition(i, actualPos[i] + perm);
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
