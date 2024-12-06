using UnityEngine;
using System.Collections.Generic;

public class LineToMesh : MonoBehaviour
{
    public LineRenderer lineRenderer; // Assign in the Inspector
    public Material meshMaterial;    // Material for the generated mesh
    public float width = 0.1f;       // Width of the generated mesh
    public bool use2DPhysics = true; // Toggle for 2D or 3D physics

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) // On mouse release
        {
            CreateMeshFromLine();
        }
    }

    private void CreateMeshFromLine()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned!");
            return;
        }

        if (lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("Not enough points in LineRenderer to create a mesh!");
            return;
        }

        // Get positions from the LineRenderer
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        // Create arrays for vertices and triangles
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 start = positions[i];
            Vector3 end = positions[i + 1];

            // Calculate direction and perpendicular for width
            Vector3 direction = (end - start).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * width * 0.5f;

            // Add vertices for the quad
            vertices.Add(start + perpendicular);
            vertices.Add(start - perpendicular);
            vertices.Add(end + perpendicular);
            vertices.Add(end - perpendicular);

            // Add UVs
            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 0));
            uv.Add(new Vector2(1, 1));

            // Add triangles for the quad
            int baseIndex = i * 4;
            triangles.Add(baseIndex);
            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);

            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 3);
        }

        // Create the mesh
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uv.ToArray()
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Create a new GameObject for the mesh
        GameObject meshObject = new GameObject("TracheaMesh", typeof(MeshFilter), typeof(MeshRenderer));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = meshMaterial;

        // Add Rigidbody based on physics type
        if (use2DPhysics)
        {
            Rigidbody2D rb = meshObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;

            // Add an EdgeCollider2D
            AddEdgeCollider2D(lineRenderer, meshObject);
        }
        else
        {
            Rigidbody rb = meshObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            // Add a MeshCollider
            MeshCollider collider = meshObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
        }

        // Clear the LineRenderer after creating the mesh
        lineRenderer.positionCount = 0;
    }

    private void AddEdgeCollider2D(LineRenderer lineRenderer, GameObject meshObject)
    {
        if (lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("Not enough points in LineRenderer to create an EdgeCollider2D.");
            return;
        }

        // Retrieve positions from LineRenderer
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        // Convert positions to 2D points
        Vector2[] edgePoints = new Vector2[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            edgePoints[i] = new Vector2(positions[i].x, positions[i].y);
        }

        // Add and configure EdgeCollider2D
        EdgeCollider2D edgeCollider = meshObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = edgePoints;
    }
}
