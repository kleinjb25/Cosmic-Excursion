using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used to move the car along the track.
 * It uses the Spline class to calculate the position and orientation of the car.
 * It uses the PiecewiseCubic class to calculate the position and orientation of the car.
 * It uses the Spline and Vertex class to calculate the force of gravity along the track.
 */
public class Slide1 : MonoBehaviour
{
    public float p = 0;
    public Spline spline2;
    public float speed = 0;
    public GameObject player;

    void FixedUpdate()
    {
        float length = spline2.calculateLength(10000); //reset the length of the spline
        float t = spline2.getLengthwiseParameter(p, 0.0001f);
        this.transform.position = spline2.getSplinePosition(t);
        Vector3 delta = spline2.getSplineDelta(t);
        this.transform.forward = delta;
        if (delta.magnitude > 0)
        {
            speed += Physics.gravity.y * Time.fixedDeltaTime * delta.y / delta.magnitude; //calculate force of gravity along the track with g * sin(theta)
        }
        switch (spline2.getTrackType(t))
        {
            case "lift":
                speed = Mathf.Clamp(speed, 2f, Mathf.Infinity); //clamp the speed so it doesn't go up too slowly or fall back down
                break;
            case "brake":
                speed -= 1.25f * Time.fixedDeltaTime * speed;
                break;
            default:
                break;
        }
        p += speed * Time.fixedDeltaTime; //move the car along the track, reset if it goes past the end
        if (p > length)
        {
            p = 0;
        }
    }
}
