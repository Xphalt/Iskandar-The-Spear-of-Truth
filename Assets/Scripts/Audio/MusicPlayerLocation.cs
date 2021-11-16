using UnityEngine;

public class MusicPlayerLocation : MonoBehaviour
{
    public BoxCollider[] nodes;
    public float nodeWidth = 2;

    float distanceBetweenNodes = 0;

    void Start()
    {      
        float angle = 0;
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            distanceBetweenNodes = Vector3.Distance(nodes[i].transform.position, nodes[i+1].transform.position);

            nodes[i].size = new Vector3(nodeWidth, 2, distanceBetweenNodes);
            nodes[i].center = new Vector3(0, 0, distanceBetweenNodes / 2);
            if (IsGreaterOrEqual(nodes[i+1].transform.position, nodes[i].transform.position))
                angle = Vector3.Angle(nodes[i+1].transform.position - nodes[i].transform.position, nodes[i].transform.TransformDirection(Vector3.forward));
            else angle = 360 - (Vector3.Angle(nodes[i + 1].transform.position - nodes[i].transform.position, nodes[i].transform.TransformDirection(Vector3.forward)));
            nodes[i].transform.eulerAngles = new Vector3(0, angle, 0);
        }
        nodes[nodes.Length-1].transform.eulerAngles = new Vector3(0, angle, 0);
        nodes[nodes.Length-1].size = new Vector3(nodeWidth, 1, 1);
        nodes[nodes.Length-1].center = new Vector3(0, 0, nodes[nodes.Length-1].size.z / 2);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i <= nodes.Length - 2; i++)
        {
            Debug.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position, Color.red);
        }
    }

    public bool IsGreaterOrEqual(Vector3 local, Vector3 other)
    {
        if (local.x >= other.x)
            return true;
        else return false;
    }    
}