#if !UNITY_5_3_OR_NEWER
using System; 

namespace CrawfisSoftware.Tiling.UnityEngine
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Scale(Vector2 scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public static Vector2 Scale(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Scale(Vector3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public static Vector3 Scale(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }

    public static class Mathf
    {
        public const float PI = (float)Math.PI;

        public static float Cos(float f) => (float)Math.Cos(f);

        public static float Sin(float f) => (float)Math.Sin(f);
    }
}
#endif
