using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class RoundEdgeCollider2Ds
{
    [MenuItem("CONTEXT/EdgeCollider2D/Round Vertices")]
    public static void RoundEdgeCollider2D(MenuCommand menuCommand) {
        EdgeCollider2D ec2d = menuCommand.context as EdgeCollider2D;
        var points = ec2d.points;
        for (int i = 0; i < points.Length; i++) {
            points[i].x = Mathf.Round (points[i].x);
            points[i].y = Mathf.Round (points[i].y);
        }
        ec2d.points = points;
    }
    [MenuItem("CONTEXT/PolygonCollider2D/Round Vertices")]
    public static void RoundPolygonCollider2D(MenuCommand menuCommand) {
        PolygonCollider2D pc2d = menuCommand.context as PolygonCollider2D;
        var points = pc2d.points;
        for (int i = 0; i < points.Length; i++) {
            points[i].x = Mathf.Round (points[i].x);
            points[i].y = Mathf.Round (points[i].y);
        }
        pc2d.points = points;
    }
}
