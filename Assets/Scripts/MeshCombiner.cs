using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private MeshFilter combinedMeshFilter;

    public void CombineMeshes() {
        if (combinedMeshFilter == null) combinedMeshFilter = GetComponent<MeshFilter>();

        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3();

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[filters.Length];
        for (int i = 0; i < filters.Length; i++) {
            combineInstances[i] = new CombineInstance() {
                mesh = filters[i].sharedMesh,
                transform = filters[i].transform.localToWorldMatrix
            };
        }
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances);
        combinedMeshFilter.sharedMesh = combinedMesh;

        transform.rotation = oldRot;
        transform.position = oldPos;
    }
}
