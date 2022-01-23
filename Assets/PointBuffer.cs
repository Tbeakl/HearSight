using UnityEngine;
using System.Collections.Generic;

public class PointBuffer
{
    Queue<ulong> idDQ;
    KdTree.KdTree<float,int> tree;
    Dictionary<ulong, Vector3> points;
    int maxLen;

    public PointBuffer(int max) {
        tree = new KdTree.KdTree<float, int>(3, new KdTree.Math.FloatMath());
        idDQ = new Queue<ulong>();
        points = new Dictionary<ulong, Vector3>();
        maxLen = max;
    }

    public void addPoints(ulong[] newPointIDs,Vector3[] newPoints) {
        for (int i = 0; i < newPointIDs.Length; i++) {
            if(newPoints[i].y<-1) continue;
            newPoints[i].y = 0;
            if(points.ContainsKey(newPointIDs[i])) continue;
            Vector3 closest = new Vector3(0,0,0);
            var nodes = tree.GetNearestNeighbours(new[] {
                newPoints[i].x,
                newPoints[i].y,
                newPoints[i].z
            },1);
            if(nodes.Length>0){
                closest = new Vector3(nodes[0].Point[0],nodes[0].Point[1],nodes[0].Point[2]);
            }
            if(Vector3.Distance(newPoints[i],closest)<0.10) continue;
            
            idDQ.Enqueue(newPointIDs[i]);
            points.Add(newPointIDs[i], newPoints[i]);
            tree.Add(new[] {
                    newPoints[i].x,
                    newPoints[i].y,
                    newPoints[i].z
                },
                0
            ); 
        }
        while (maxLen < idDQ.Count) {
            ulong toRemove = idDQ.Dequeue();
            tree.RemoveAt(new[] {
                points[toRemove].x,
                points[toRemove].y,
                points[toRemove].z
            });
            points.Remove(toRemove);
        }
    }

    public Vector3[] getClosest(Vector3 p, int count){
        var nodes = tree.GetNearestNeighbours(new[] {
            p.x,
            p.y,
            p.z
        },count);
        Vector3[] points = new Vector3[nodes.Length];
        for(int i = 0; i<nodes.Length;i++){
            points[i] = new Vector3(
                nodes[i].Point[0],
                nodes[i].Point[1],
                nodes[i].Point[2]
            );
        } 
        return points;
    }
}
