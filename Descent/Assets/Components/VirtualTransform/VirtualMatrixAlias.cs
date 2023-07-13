using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;
using Utilities;

public class VirtualMatrixAlias : MonoBehaviour
{
    VirtualTransform virtualTransform = new();

    void UpdateVirtualMatrix()
    {
        transform.LookAt(Camera.main.transform.position);

        virtualTransform.position = transform.position;
        virtualTransform.eulerAngles = transform.eulerAngles;
        virtualTransform.scale = transform.lossyScale;
    }

    private void OnDrawGizmos()
    {
        UpdateVirtualMatrix();
        virtualTransform.DrawGizmos();

        Labels.World(virtualTransform.matrix + "", virtualTransform.position + Vector3.up * 3);
        Labels.World(transform.localToWorldMatrix + "", transform.position);
    }
}
