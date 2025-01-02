using System.Collections.Generic;
using UnityEngine;

public class Abdomen : MonoBehaviour
{
    // Mesh properties
    Mesh mesh;
    public Vector3[] polygonPoints;
    public int[] polygonTriangles;

    // Polygon properties
    public bool isFilled;
    public int polygonSides;
    public float radiusX = 1f; // X-axis radius
    public float radiusY = 2f; // Y-axis radius
    public float centerRadius;

    void Start()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void Update()
    {
        if (isFilled)
        {
            DrawFilled(polygonSides, radiusX, radiusY);
        }
        else
        {
            DrawHollow(polygonSides, radiusX, radiusY, centerRadius);
        }
    }

    void DrawFilled(int sides, float radiusX, float radiusY)
    {
        polygonPoints = GetOvalPoints(sides, radiusX, radiusY).ToArray();
        polygonTriangles = DrawFilledTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
    }

    void DrawHollow(int sides, float outerRadiusX, float outerRadiusY, float innerRadius)
    {
        List<Vector3> pointsList = new List<Vector3>();
        List<Vector3> outerPoints = GetOvalPoints(sides, outerRadiusX, outerRadiusY);
        pointsList.AddRange(outerPoints);

        List<Vector3> innerPoints = GetOvalPoints(sides, innerRadius, innerRadius); // Inner radius is circular
        pointsList.AddRange(innerPoints);

        polygonPoints = pointsList.ToArray();

        polygonTriangles = DrawHollowTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
    }

    int[] DrawHollowTriangles(Vector3[] points)
    {
        int sides = points.Length / 2;
        int triangleAmount = sides * 2;

        List<int> newTriangles = new List<int>();
        for (int i = 0; i < sides; i++)
        {
            int outerIndex = i;
            int innerIndex = i + sides;

            // First triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(innerIndex);
            newTriangles.Add((i + 1) % sides);

            // Second triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(sides + ((sides + i - 1) % sides));
            newTriangles.Add(outerIndex + sides);
        }
        return newTriangles.ToArray();
    }

    List<Vector3> GetOvalPoints(int sides, float radiusX, float radiusY)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(
                Mathf.Cos(currentRadian) * radiusX,
                Mathf.Sin(currentRadian) * radiusY,
                0
            ));
        }
        return points;
    }

    int[] DrawFilledTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length - 2;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        return newTriangles.ToArray();
    }
}
