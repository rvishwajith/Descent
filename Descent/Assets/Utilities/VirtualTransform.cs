using UnityEngine;

// Reference: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Transform/ScriptBindings/Transform.bindings.cs

class VirtualTransform
{
    private Matrix4x4 internalMatrix;
    public Matrix4x4 matrix
    {
        get { return internalMatrix; }
    }

    private Vector3 internalPosition;
    public Vector3 position
    {
        get { return internalPosition; }
        set { internalPosition = value; }
    }

    private Vector3 internalForward;
    public Vector3 forward;

    private Quaternion internalRotation;
    public Quaternion rotation
    {
        get { return internalRotation; }
        set { internalRotation = value; }
    }
}