using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
    public GameObject eyes;

    // senses
    public float viewDistance = 10;
    public float viewFov = 30;
    public float viewHeight = 1;

    public Color wedgeColor = Color.red;



    public LayerMask layers;
    public LayerMask losLayers;

    public Collider[] colliders = new Collider[50];
    Mesh viewMesh;
    int count;

    // check for suppression

    // get nearest collider and assume its the cover

    // while its being shot at, take cover

    // raycast. check if it hits both a sphere around the bot and a collider that isn't the bot


    // the target must have a collider. y-bot has one on his hip, will want to add more colliders later
    public List<AiAgent> ScanForEnemies(bool team)
    {
        List<AiAgent> targetList = new List<AiAgent>();

        Array.Clear(colliders, 0, colliders.Length);
        // check for collisions in a sphere
        count = Physics.OverlapSphereNonAlloc(transform.position, viewDistance, colliders, layers, QueryTriggerInteraction.Collide);

        // for each object in the detection distance
        for (int i = 0; i < count; i++)
        {
            if (!BelongsToChildOfParent(colliders[i].gameObject) && IsInSight(colliders[i].gameObject))
                ;
            else
                continue;

            // Debug.Log("colliders: " + colliders[i].gameObject.name);

            if (colliders[i].gameObject.TryGetComponent(out AiAgent bot))
            {
                // Debug.Log("Found: " + colliders[i].gameObject.name);
                if (team != bot.friendly)
                {
                    targetList.Add(bot);
                }
            }
            else
            {
                // Debug.Log("Couldn't find it.");
            }
        }
        return targetList;
    }

    // prevents bot from Seeing his own colliders
    private bool BelongsToChildOfParent(GameObject obj)
    {
        // iterate up until found collider's name == this parent object with the AiSensor script on it
        Transform iterator = obj.transform;

        // check if called by intended caller, ie Rifleman
        if (obj.transform.name == gameObject.name)
        {
            // Debug.Log("called by intended caller: " + gameObject.name);
            return false;
        }



        // iterate up tree til name is found belonging to parent
        while (iterator.parent != null)
        {
            iterator = iterator.parent;
            if (iterator.name == gameObject.name)
            {
                // Debug.Log("Called by child collider of agent: " + gameObject.name + " from Child:  " + obj.transform.name);
                return true;
            }
        }

        return false;
    }

    private bool IsInSight(GameObject obj)
    {
        Vector3 origin = eyes.transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = origin - dest;



        if (direction.y < 0 || direction.y > viewHeight)
        {

            return false;
        }
        direction.y = 0;

        float deltaAngle = Vector3.Angle(direction, -eyes.transform.forward);
        if (deltaAngle > viewFov)
        {
/*            Debug.Log("delta angle:" + deltaAngle);
            Debug.Log("here2");*/
            return false;
        }

        // get side of object collider
        // get other side of object collider

/*        Collider collider = obj.GetComponent<Collider>();
        float distanceFromCenter = collider.bounds.size.x / 2f;
        Vector3 leftSideOfCollider = new Vector3(collider.bounds.center + distanceFromCenter_;
        Vector3 rightSideOfCollider = collider.bounds.x - distanceFromCenter*/

        if (Physics.Linecast(origin, dest, losLayers)) // change this to 3. center to center, center to left and right
        {
            
/*            Debug.Log("here3");
            Debug.Log("delta angle:" + (deltaAngle));
*/
            return false;
        }
 
        // Debug.Log("delta angle:" + (deltaAngle));

        return true;

    }

    // displaying FOV for debugging

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;


        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -viewFov, 0) * Vector3.forward * viewDistance;
        Vector3 bottomRight = Quaternion.Euler(0, viewFov, 0) * Vector3.forward * viewDistance;

        Vector3 topCenter = bottomCenter + Vector3.up * viewHeight;
        Vector3 topRight = bottomRight + Vector3.up * viewHeight;
        Vector3 topLeft = bottomLeft + Vector3.up * viewHeight;

        int vert = 0;
        // left
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -viewFov;
        float deltaAngle = (viewFov * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            

            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * viewDistance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * viewDistance;

            topRight = bottomRight + Vector3.up * viewHeight;
            topLeft = bottomLeft + Vector3.up * viewHeight;

            // far

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom

            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;

        }



        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        viewMesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (viewMesh)
        {
            Gizmos.color = wedgeColor;
            Gizmos.DrawMesh(viewMesh, eyes.transform.position, gameObject.transform.rotation);
        }

    }
}
