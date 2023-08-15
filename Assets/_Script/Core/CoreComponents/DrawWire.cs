using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWire : CoreComponent
{
    private LineRenderer LR;
    private List<Vector3> points;

    protected override void Awake()
    {
        base.Awake();

        LR = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        points = new();
    }

    public void ChangeColor(Gradient gradient)
    {
        LR.colorGradient = gradient;
    }

    public void ClearPoints()
    {
        points.Clear();
        LR.positionCount = points.Count;
        LR.enabled = false;
    }

    public void SetPoints(Vector3 point1, Vector3 point2)
    {
        points.Clear();
        points.Add(point1);
        points.Add(point2);
        LR.positionCount = points.Count;
    }

    public void SetPoints(List<Vector3> points)
    {
        LR.positionCount = points.Count;
        this.points = points;
    }

    public void RenderLine()
    {
        if(LR.positionCount > 0)
        {
            LR.enabled = true;
            for (int i = 0; i < LR.positionCount; i++)
            {
                LR.SetPosition(i, points[i]);
            }
        }
    }

}
