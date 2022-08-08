using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindIntersectionArea : MonoBehaviour
{
    [SerializeField] MeshCollider thisMesh;
    private Mesh mesh;
    private MeshCollider otherMesh;
    private Bounds bounds;
    [SerializeField] GameObject cube;
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    private void OnTriggerStay(Collider other)
    {
        otherMesh = other.gameObject.GetComponent<MeshCollider>();
        FindArea(otherMesh);
    }

    public void FindArea(MeshCollider otherMesh)
    {
        Vector2 firstPoint;
        Vector2 secondPoint;
        FindCircleIntersections(gameObject.transform.position,5, otherMesh.gameObject.transform.position,5,out firstPoint, out secondPoint);
        Vector2 intersectionCenter = firstPoint+secondPoint/2f;
        print(firstPoint);
        print(secondPoint);
        print(intersectionCenter);
        Instantiate(cube, intersectionCenter, Quaternion.identity);
    }
  private void OnDrawGizmos()
    {
        if (mesh != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(mesh,bounds.center);
        }
       
    }

    private int FindCircleIntersections(Vector2 c0, float r0, Vector2 c1, float r1, out Vector2 intersection1, out Vector2 intersection2)
    {
        // Find the distance between the centers.
        double dx = c0.x - c1.x;
        double dy = c0.y - c1.y;
        double dist = Math.Sqrt(dx * dx + dy * dy);

        if (Math.Abs(dist - (r0 + r1)) < 0.00001)
        {
            intersection1 = Vector2.Lerp(c0, c1, r0 / (r0 + r1));
            intersection2 = intersection1;
            return 1;
        }

        // See how many solutions there are.
        if (dist > r0 + r1)
        {
            // No solutions, the circles are too far apart.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if (dist < Math.Abs(r0 - r1))
        {
            // No solutions, one circle contains the other.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if ((dist == 0) && (r0 == r1))
        {
            // No solutions, the circles coincide.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else
        {
            // Find a and h.
            double a = (r0 * r0 -
                        r1 * r1 + dist * dist) / (2 * dist);
            double h = Math.Sqrt(r0 * r0 - a * a);

            // Find P2.
            double cx2 = c0.x + a * (c1.x - c0.x) / dist;
            double cy2 = c0.y + a * (c1.y - c0.y) / dist;

            // Get the points P3.
            intersection1 = new Vector2(
                (float)(cx2 + h * (c1.y - c0.y) / dist),
                (float)(cy2 - h * (c1.x - c0.x) / dist));
            intersection2 = new Vector2(
                (float)(cx2 - h * (c1.y - c0.y) / dist),
                (float)(cy2 + h * (c1.x - c0.x) / dist));

            return 2;
        }
    }

}
