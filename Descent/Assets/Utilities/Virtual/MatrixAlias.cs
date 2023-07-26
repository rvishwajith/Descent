using UnityEngine;
using Utilities;

namespace Components.Virtual
{
    public class VirtualMatrixAlias : MonoBehaviour
    {
        VirtualTransform virtualTransform = new();

        void UpdateVirtualMatrix()
        {
            transform.LookAt(UnityEngine.Camera.main.transform.position);

            virtualTransform.position = transform.position;
            virtualTransform.eulerAngles = transform.eulerAngles;
            virtualTransform.scale = transform.lossyScale;
        }

        private void OnDrawGizmos()
        {
            UpdateVirtualMatrix();
            virtualTransform.DrawGizmos();

            Labels.AtWorld(virtualTransform.matrix + "", virtualTransform.position + Vector3.up * 3);
            Labels.AtWorld(transform.localToWorldMatrix + "", transform.position);
        }
    }
}