using UnityEngine;

public class MyWaypointScript : MonoBehaviour
{
    public static Transform[] points;

    void Awake()
    {
        points = new Transform[transform.childCount];   //Create an array size = amount of waypoints
        for (int i = 0; i < points.Length; i++)         //iterate through them
        {
            points[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
