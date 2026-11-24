using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public static class RuntimeSplineGenerator
{
    /// <summary>
    /// Generate a random spline at runtime using Unity's built-in Spline API.
    /// </summary>
    public static Spline Generate(
        Vector3 origin,
        int controlPointCount = 8,
        float minSpacing = 2f,
        float maxSpacing = 5f,
        float heightAmplitude = 5f,
        float perlinFrequency = 0.25f,
        int seed = 0,
        bool closed = false,
        Vector3 forwardAxis = default)
    {
        if (forwardAxis == default)
            forwardAxis = Vector3.right; // default along X

        System.Random rng = (seed == 0) ? new System.Random() : new System.Random(seed);
        float perlinSeed = (float)rng.NextDouble() * 1000f;

        // We'll build the spline's control points
        List<BezierKnot> knots = new List<BezierKnot>();
        float cumulativeDist = 0f;

        for (int i = 0; i < controlPointCount; i++)
        {
            // Random forward spacing
            float step = Mathf.Lerp(minSpacing, maxSpacing, (float)rng.NextDouble());
            cumulativeDist += step;

            // Coherent Perlin height
            float heightNoise = (Mathf.PerlinNoise(perlinSeed + i * perlinFrequency, 0f) - 0.5f) * 2f;
            float height = heightNoise * heightAmplitude;

            // Position in XZ plane with Y height
            Vector3 pos = origin + forwardAxis.normalized * cumulativeDist + new Vector3(0f, 0f, height);

            // You can customize tangents for smoothness
            var knot = new BezierKnot(pos);
            knots.Add(knot);
        }

        // Create and populate Unity's Spline
        var spline = new Spline();
        foreach(var k in knots)spline.Add(k);
        spline.Closed = closed;

        // Optionally auto-smooth tangents
        spline.SetTangentMode(TangentMode.AutoSmooth);

        return spline;
    }
}
