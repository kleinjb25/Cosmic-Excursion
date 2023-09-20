using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<GameObject> vertices;
    public List<string> trackTypes;
    public GameObject rendererPrefab;
    List<PiecewiseCubic> pieces = new List<PiecewiseCubic>();
    GameObject prevR;
    public new LineRenderer renderer;

    void Update()
    {
        pieces.Clear();
        for (int i = 0; i < vertices.Count - 1; ++i)
        {
            pieces.Add(new PiecewiseCubic(vertices[i].transform.position, vertices[i + 1].transform.position, vertices[i].transform.up * vertices[i].GetComponent<Vertex>().magnitudeForward, vertices[i + 1].transform.up * vertices[i + 1].GetComponent<Vertex>().magnitudeForward));
        }
        for (int i = 0; i < vertices.Count - 1; ++i)
        {
            pieces[i].setType(trackTypes[i]);
        }
        renderer.SetPositions(getPoints(pieces));
    }
    private class PiecewiseCubic
    {
        Vector3 a, b, c, d;
        string type = "normal";
        public PiecewiseCubic(Vector3 x0, Vector3 xf, Vector3 dx0, Vector3 dxf)
        {
            a = dxf + dx0 + 2 * (x0 - xf);
            b = 3 * (xf - x0) - 2 * dx0 - dxf;
            c = dx0;
            d = x0;
        }
        public Vector3 calculatePoint(float t)
        {
            return a * Mathf.Pow(t, 3) + b * Mathf.Pow(t, 2) + c * t + d;
        }
        public Vector3 calculateDelta(float t)
        {
            return 3 * a * Mathf.Pow(t, 2) + 2 * b * t + c;
        }
        public void setType(string t)
        {
            type = t;
        }
        public string getType()
        {
            return type;
        }
    }
    private Vector3[] getPoints(List<PiecewiseCubic> pcs)
    {
        int numPts = pieces.Count * 100;
        Vector3[] pts = new Vector3[numPts];
        renderer.positionCount = numPts;
        for (int i = 0; i < pieces.Count; ++i)
        {
            for (int j = 0; j < 100; ++j)
            {
                pts[100 * i + j] = pieces[i].calculatePoint(j / 100f);
            }
        }
        return pts;
    }
    public Vector3 getSplinePosition (float t)
    {
        if (pieces.Count > 0)
        {
            float x = Mathf.Clamp01(t) * pieces.Count;
            return (int) t < 1 ? pieces[(int)x].calculatePoint(x - Mathf.Floor(x)) : vertices[vertices.Count - 1].transform.position;
        } else
        {
            return Vector3.zero;
        }
    }
    public Vector3 getSplineDelta(float t)
    {
        if (pieces.Count > 0)
        {
            float x = Mathf.Clamp01(t) * pieces.Count;
            return (int)t < 1 ? pieces[(int)x].calculateDelta(x - Mathf.Floor(x)) : vertices[vertices.Count - 1].transform.forward;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public float calculateLength (int steps)
    {
        float precision = 1f / (float) steps;
        float length = 0f;
        for (int i = 0; i < steps; ++i)
        {
            length += (getSplinePosition(i * precision) - getSplinePosition((i + 1) * precision)).magnitude;
        }
        return length;
    }

    public Vector3 getLengthwisePosition(float desired, float precision)
    {
        float t = getLengthwiseParameter(desired, precision);
        return getSplinePosition(t);
    }
    public float getLengthwiseParameter(float desired, float precision)
    {
        bool found = false;
        float t = 0, length = 0;
        while (!found && t < 1)
        {
            length += (getSplinePosition(t) - getSplinePosition(t + precision)).magnitude;
            if (length > desired)
            {
                found = true;
            }
            else
            {
                t += precision;
            }
        }
        return t;
    }
    public string getTrackType(float t)
    {
        if (pieces.Count > 0)
        {
            float x = Mathf.Clamp01(t) * pieces.Count;
            return (int)t < 1 ? pieces[(int)x].getType() : "normal";
        }
        else
            return "normal";
    }
}

