using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform nozzle;
    private LineRenderer lineRenderer;
    public int segmentCount = 10;
    public float fuzzyness = 0.02f;
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
        lineRenderer.positionCount = points.Count / 2 + 1;
        lineRenderer.SetPosition(0, points[0]);
        lineRenderer.SetPosition(lineRenderer.positionCount -1, points[points.Count - 1]);
        for(int i = 1; i < points.Count / 2; i ++) {
            var cur = points[i * 2];
            lineRenderer.SetPosition(i, cur);
        }
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
