using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOMover : MonoBehaviour
{
    public List<Transform> points; // List of points to move between
    public float speed = 1f; // Speed of movement
    public float delay = 3f; // Speed of movement
    public bool isActive = true; // Active status
    public bool randomizePoints = false; // Randomize points

    private int currentIndex = 0;
    private bool movingToNextPoint = true;
    private bool isWaiting;

    void Start()
    {
        if (randomizePoints)
        {
            ShufflePoints();
        }
    }

    void FixedUpdate()
    {
        if (!isActive || points.Count == 0 || isWaiting)
        {
            return;
        }

        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Transform targetPoint = points[currentIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % points.Count;
            StartCoroutine(DelayBeforeNextMove());
        }
    }

    IEnumerator DelayBeforeNextMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        isWaiting = false;
    }

    void ShufflePoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            int randomIndex = Random.Range(0, points.Count);
            Transform temp = points[i];
            points[i] = points[randomIndex];
            points[randomIndex] = temp;
        }
    }
}