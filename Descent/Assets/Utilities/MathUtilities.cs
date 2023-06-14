using UnityEngine;

public static class MathUtil
{
    public static float Round(float value, float multiple = 1)
    {
        var remainder = value % multiple;
        var result = value - remainder;
        if (remainder >= (multiple / 2))
            result += multiple;
        return result;
    }

    public static Vector3 Round(Vector3 vector, float multiple = 1)
    {
        return new(
            Round(vector.x, multiple: multiple),
            Round(vector.y, multiple: multiple),
            Round(vector.z, multiple: multiple));
    }

    public static float Truncate(float value, float multiple = 1)
    {
        var remainder = value % multiple;
        return value - remainder;
    }

    public static Vector3 Truncate(Vector3 vector, float multiple = 1)
    {
        return new(
            Truncate(vector.x, multiple: multiple),
            Truncate(vector.y, multiple: multiple),
            Truncate(vector.z, multiple: multiple));
    }

    public static int Wrap(int index, int arrLength)
    {
        return index % arrLength;
    }
}

