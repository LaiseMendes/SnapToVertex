using UnityEngine;
using System.Collections;

public class SnapToVertex_Corotine : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Transform objectToSnap;
    public int vertexIndex;
    public float intervalSeconds = 0.1f;
    public Vector3 rotationOffset;
    public Vector3 translateOffset;
    public Space rotationSpace;

    public Transform targetJointRotation;

    private Mesh bakedMesh;

    public float snapDistance = 0.1f;

    public Animator TargetAnim;

    private void Start()
    {
        // Bake the SkinnedMeshRenderer into a static mesh
        bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);

        // Start the coroutine
        StartCoroutine(UpdateSnapObject());
    }

    private IEnumerator UpdateSnapObject()
    {
        while (true)
        {
            Transform transformMesh;
            transformMesh = skinnedMeshRenderer.transform;

            // Re-bake the SkinnedMeshRenderer into a static mesh
            Mesh bakedMesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(bakedMesh, true);

            // Get the vertices and normals of the baked mesh
            Vector3[] vertices = bakedMesh.vertices;

            // Get the world space position of the specified vertex
            Vector3 worldPosVertex = transformMesh.localToWorldMatrix.MultiplyPoint3x4(vertices[vertexIndex]);

            // Check the distance between the objectToSnap and the vertex position
            float distance = Vector3.Distance(objectToSnap.position, worldPosVertex);
            if (distance <= snapDistance)
            {
                // Set the position of the object to snap to the world space vertex position
                objectToSnap.position = worldPosVertex+translateOffset*0.01f;

                // Get joint or object orientation and add an option for offset
                objectToSnap.transform.rotation = Quaternion.Euler(targetJointRotation.transform.rotation.eulerAngles+rotationOffset);
                if(!TargetAnim.GetCurrentAnimatorStateInfo(0).IsName("AnimationFileName"))
                {
                    // Trigger the animation on the animator component
                    TargetAnim.SetTrigger("TriggerName");

                }
                
            }

            else
            {
                if(!TargetAnim.GetCurrentAnimatorStateInfo(0).IsName("CurrentAnimationName"))
                {
                    TargetAnim.SetTrigger("CurrentTriggerName");
                }
            }

            // Wait for the specified interval
            yield return new WaitForSeconds(intervalSeconds);
        }
    }
}