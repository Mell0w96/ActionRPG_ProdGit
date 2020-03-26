using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXMesh : MonoBehaviour
{

    MeshCollider myMeshCollider;
    MeshFilter myMeshFilter;
  
    public Mesh[] PossibleMeshes;

    // Start is called before the first frame update
    public virtual void Start()
    {
        myMeshCollider = this.GetComponent<MeshCollider>();
        myMeshFilter = this.GetComponent<MeshFilter>();
        

        Mesh CurrentMesh = PossibleMeshes[Mathf.FloorToInt(Random.Range(0,PossibleMeshes.Length))];

        myMeshFilter.mesh = CurrentMesh;
        myMeshCollider.sharedMesh = CurrentMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
