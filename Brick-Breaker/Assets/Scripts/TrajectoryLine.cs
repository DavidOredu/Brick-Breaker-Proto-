﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private Transform trajectoryStart;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private int maxCollisions = 5;
    [SerializeField] 
    private float ballCheckSize = 0.5f;

    Vector2 pos;
    Vector2 force;

    int currentLine;
    // Start is called before the first frame update
    void Start()
    {
        PrepareLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowTrajectory()
    {
        line.enabled = true;
    }
    public void HideTrajectory()
    {
        line.enabled = false;
    }
    void PrepareLine()
    {
        line.SetPosition(0, trajectoryStart.position);
        line.positionCount = maxCollisions + 1;
    }
    public void UpdateLine(Vector2 force)
    {
        this.force = force;
        Collider2D previousCollision = null;
        for (int i = 0; i < maxCollisions; i++)
        {
            //var hits = Physics2D.RaycastAll(line.GetPosition(currentLine), this.force.normalized, Mathf.Infinity, LayerMask.GetMask("Wall", "Block"));
           var hits = Physics2D.CircleCastAll(line.GetPosition(currentLine), ballCheckSize, this.force.normalized, Mathf.Infinity, LayerMask.GetMask("Wall", "Block"));

            foreach (var hit in hits)
            {
                if(hit.collider == previousCollision) { continue; }

                var newDirection = Vector2.Reflect(this.force.normalized, hit.normal);
                Debug.Log("Magnitude of velocity, direction of travel:" + this.force.magnitude + ", " + newDirection);
                this.force = newDirection * this.force.magnitude;
                Debug.DrawRay(line.GetPosition(currentLine), newDirection, Color.green);

                currentLine++;
                line.SetPosition(currentLine, hit.point);
                Debug.Log("Current Line is: " + currentLine);
                previousCollision = hit.collider;
                break;
            }
        }
        currentLine = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(line.GetPosition(4), ballCheckSize);
    }
}
