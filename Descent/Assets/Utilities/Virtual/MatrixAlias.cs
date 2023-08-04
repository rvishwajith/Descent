using UnityEngine;

/* Components -> Virtual -> MatrixAlias */

namespace Components.Virtual
{
    public class MatrixAlias : MonoBehaviour
    {
        private VirtualTransform virtualTransform = new();

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

            Utilities.Labels.AtWorld(virtualTransform.matrix + "", virtualTransform.position + Vector3.up * 3);
            Utilities.Labels.AtWorld(transform.localToWorldMatrix + "", transform.position);
        }
    }
}