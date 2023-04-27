using UnityEngine;

public class SnapToVertex : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Transform objectToSnap;
    public int vertexIndex;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    private Mesh bakedMesh;
    public Transform targetJointRotation;

    private void Start()
    {
        // Get the mesh from the MeshFilter component
        bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);
    }

    private void Update()
    {
        Mesh bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh, true);
        // Get the position of the vertex in local space
        Vector3 localVertexPosition = bakedMesh.vertices[vertexIndex];

        // Convert the vertex position from local to world space
        Vector3 worldSpaceVertex = skinnedMeshRenderer.transform.TransformPoint(localVertexPosition);

        // Apply position offset to world space vertex position
        worldSpaceVertex += positionOffset*0.01f;

        // Set the position of the object to snap to the world space vertex position
        objectToSnap.position = worldSpaceVertex;
        objectToSnap.transform.rotation = Quaternion.Euler(TargetRotation.transform.rotation.eulerAngles+rotationOffset);
    }
}
