using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

/* VirtualTransform
 * 
 * Description:
 * A custom version of the Transform component that works without any Instantiation required (independent of GameObjects). Since it does not use any GameObjects, it also cannot have any parents/children, which means all transofrmations are world space only and local position/rotation/lossy scale cannot exist.
 * 
 * Use in combination with GPU mesh instancing, because a transformation matrix array is required but using thousands of standard Transforms for matrices is very expensive. The planned use cases are:
 * 1. Flocking objects (boid simulations)
 *    Mesh instancing has already been setup using regular instantiation for these, but will be changed when this class is complete.
 * 2. Kelp stalks/leaves (verlet simulations)
 *    Currently functions partially using only a position, so neither virtual transforms nor instances are beign sued here. 
 * 
 * Attributions:
 * 1. Unity - Transform (MonoBehaviour) Source Code:
 * https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Transform/ScriptBindings/Transform.bindings.cs
 */

namespace Components
{

    class VirtualTransform
    {
        private InternalData innerData = new();
        class InternalData
        {
            public Vector3 position = Vector3.zero;
            public Quaternion rotation = Quaternion.identity;
            public Vector3 scale = Vector3.one;

            public InternalData()
            {
                // Debug.Log("VirtualTransform.(): Created virtual transform.");
            }
        }

        public Vector3 position
        {
            get { return innerData.position; }
            set { innerData.position = value; }
        }
        public Quaternion rotation
        {
            get { return innerData.rotation; }
            set { innerData.rotation = value; }
        }
        public Vector3 scale
        {
            get { return innerData.scale; }
            set { innerData.scale = value; }
        }
        public Vector3 eulerAngles
        {
            get { return rotation.eulerAngles; }
            set { rotation = Quaternion.Euler(value); }
        }

        public Vector3 right
        {
            get { return rotation * Vector3.right; }
            set { rotation = Quaternion.FromToRotation(Vector3.right, value); }
        }
        public Vector3 up
        {
            get { return rotation * Vector3.up; }
            set { rotation = Quaternion.FromToRotation(Vector3.up, value); }
        }
        public Vector3 forward
        {
            get { return rotation * Vector3.forward; }
            set { rotation = Quaternion.LookRotation(value); }
        }

        public Matrix4x4 matrix
        {
            get
            {
                Matrix4x4 result = new();
                result.SetTRS(position, rotation, scale);
                return result;
            }
        }

        public void LookAt(Vector3 point)
        {
            LookAt(position, Vector3.up);
        }

        public void LookAt(Vector3 point, Vector3 up)
        {
            Vector3 relativePos = point - position;
            if (relativePos.magnitude == 0)
                return;
            rotation = Quaternion.LookRotation(relativePos, up);
            // Debug.Log("VirtualTransform.LookAt(): Looking at " + Format.Vector(point, 2));
        }

        public void DrawGizmos()
        {
            void Directions(Vector3 origin)
            {
                float lineLength = 1.5f;
                Gizmos.color = Color.red;
                Gizmo.Arrow(origin, right * lineLength * scale.x);
                Gizmos.color = Color.green;
                Gizmo.Arrow(origin, up * lineLength * scale.y);
                Gizmos.color = Color.cyan;
                Gizmo.Arrow(origin, forward * lineLength * scale.z);
            }

            void Label(Vector3 labelPos)
            {
                string posText = "Position: " + Format.Vector(position, 2),
                    rotText = "Rotation: " + Format.Quaternion(rotation, 2),
                    anglesText = "Euler Angles: " + Format.Vector(eulerAngles, 2),
                    scaleText = "Scale: " + Format.Vector(scale, 2);

                string result = posText + "\n"
                    + rotText + "\n"
                    + anglesText + "\n"
                    + scaleText;
                Labels.World(result, labelPos);
            }

            Directions(position);
            Label(position + up * 1f);
        }
    }
}