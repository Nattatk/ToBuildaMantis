using System.Collections.Generic;
using UnityEngine;

public class AbdomenManager : MonoBehaviour
{
    public Abdomen innerWallScript; // Reference to the inner wall's script
    public MeshFilter innerWallMeshFilter; // Reference to the inner wall's MeshFilter
    public MeshFilter exoskeletonMeshFilter; // Reference to the exoskeleton's MeshFilter

    public float exoskeletonThickness = 0.1f; // Thickness of the exoskeleton ring

    private Mesh exoskeletonMesh;

    void Start()
    {
        // Initialize the exoskeleton mesh
        exoskeletonMesh = new Mesh();
        exoskeletonMeshFilter.mesh = exoskeletonMesh;
    }

    void Update()
    {
        // Update the inner wall mesh
        innerWallScript.isFilled = true; // Ensure inner wall is filled
        innerWallScript.Update();

        // Get the current inner wall points
        Vector3[] innerWallPoints = innerWallScript.polygonPoints;

        // Update the exoskeleton mesh
        UpdateExoskeletonMesh(innerWallPoints, exoskeletonThickness);
    }

    void UpdateExoskeletonMesh(Vector3[] innerWallPoints, float thickness)
    {
        int sides = innerWallPoints.Length;

        // Create lists for the outer and inner ring vertices
        List<Vector3> outerRing = new List<Vector3>();
        List<Vector3> innerRing = new List<Vector3>(innerWallPoints); // Use inner wall points as the inner ring

        // Calculate the outer ring points by offsetting outward
        foreach (var point in innerWallPoints)
        {
            Vector3 normal = (point - Vector3.zero).normalized; // Direction from the center
            outerRing.Add(point + normal * thickness); // Offset outward
        }

        // Combine inner and outer ring points
        List<Vector3> allPoints = new List<Vector3>();
        allPoints.AddRange(outerRing);
        allPoints.AddRange(innerRing);

        // Generate triangles for the exoskeleton ring
        int[] triangles = GenerateExoskeletonTriangles(sides);

        // Update the exoskeleton mesh
        exoskeletonMesh.Clear();
        exoskeletonMesh.vertices = allPoints.ToArray();
        exoskeletonMesh.triangles = triangles;
        exoskeletonMesh.RecalculateNormals();
    }

    int[] GenerateExoskeletonTriangles(int sides)
    {
        List<int> triangles = new List<int>();

        // Generate two triangles for each segment
        for (int i = 0; i < sides; i++)
        {
            int outerA = i; // Current outer ring point
            int outerB = (i + 1) % sides; // Next outer ring point
            int innerA = i + sides; // Current inner ring point
            int innerB = (i + 1) % sides + sides; // Next inner ring point

            // First triangle (outerA, innerA, outerB)
            triangles.Add(outerA);
            triangles.Add(innerA);
            triangles.Add(outerB);

            // Second triangle (outerB, innerA, innerB)
            triangles.Add(outerB);
            triangles.Add(innerA);
            triangles.Add(innerB);
        }

        return triangles.ToArray();
    }
}
