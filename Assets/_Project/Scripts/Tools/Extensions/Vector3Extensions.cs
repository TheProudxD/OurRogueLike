﻿using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 WithX(this Vector3 value, float x)
        {
            value.x = x;
            return value;
        }

        public static Vector3 WithY(this Vector3 value, float y)
        {
            value.y = y;
            return value;
        }

        public static Vector3 WithZ(this Vector3 value, float z)
        {
            value.z = z;
            return value;
        }

        public static Vector3 AddX(this Vector3 value, float x)
        {
            value.x += x;
            return value;
        }

        public static Vector3 AddY(this Vector3 value, float y)
        {
            value.y += y;
            return value;
        }

        public static Vector3 AddZ(this Vector3 value, float z)
        {
            value.z += z;
            return value;
        }

        public static Vector2 XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);

        public static Vector3 X0Z(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2
            {
                x = vector3.x,
                y = vector3.z
            };
        }

        public static Vector3 ToVector3(this Vector2 vector2, float y = 0)
        {
            return new Vector3
            {
                x = vector2.x,
                y = y,
                z = vector2.y
            };
        }

        public static Vector3Int FloorToVector3Int(this Vector3 vector)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(vector.x),
                y = Mathf.FloorToInt(vector.x),
                z = Mathf.FloorToInt(vector.x)
            };
        }
        
        public static Vector3Int FloorToVector3Int(this Vector3 vector, float offset)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(vector.x + offset),
                y = Mathf.FloorToInt(vector.y + offset),
                z = Mathf.FloorToInt(vector.z + offset),
            };
        }
        
        public static Vector3 ClampSimmetrical(this Vector3 value, Vector3 center,
            float marginX, float marginY, float marginZ)
        {
            return new Vector3(Mathf.Clamp(value.x, center.x - marginX, center.x + marginX),
                Mathf.Clamp(value.y, center.y - marginY, center.y + marginY),
                Mathf.Clamp(value.z, center.z - marginZ, center.z + marginZ));
        }

        public static Vector3 ClampNonSimmetrical(this Vector3 value, Vector2 xMargins, Vector2 yMargins,
            Vector2 zMargins)
        {
            if (xMargins.y < xMargins.x || yMargins.y < yMargins.x || zMargins.y < zMargins.x)
            {
                throw new System.Exception("wrong margin parameters - x element must be less then y");
            }

            return new Vector3(Mathf.Clamp(value.x, xMargins.x, value.y),
                Mathf.Clamp(value.y, yMargins.x, yMargins.y),
                Mathf.Clamp(value.z, zMargins.x, zMargins.y));
        }

        public static Vector3 GetRoundPosition(this Vector3 value)
        {
            value.x = Mathf.Round(value.x * 100f) / 100f;
            value.y = Mathf.Round(value.y * 100f) / 100f;
            value.z = Mathf.Round(value.z * 100f) / 100f;
            return value;
        }

        public static Vector3 MultiplyX(this Vector3 v, float val)
        {
            v = new Vector3(val * v.x, v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyY(this Vector3 v, float val)
        {
            v = new Vector3(v.x, val * v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyZ(this Vector3 v, float val)
        {
            v = new Vector3(v.x, v.y, val * v.z);
            return v;
        }

        public static Vector3 RandomizeWithinDistance(this Vector3 v, float minDistance, float maxDistance,
            bool doRandomizeY = false)
        {
            if (maxDistance < minDistance) throw new System.Exception("max distance can't be less then minimal");
            v.Set(Random.Range(0f, 1f), doRandomizeY ? Random.Range(0f, 1f) : 0f, Random.Range(0f, 1f));
            v = v.normalized * Random.Range(minDistance, maxDistance);
            return v;
        }

        public static float SubstractAbs(this Vector3 v, Vector3 vectorToSubstract)
        {
            var substractedVector = v - vectorToSubstract;
            return Mathf.Abs(substractedVector.x) + Mathf.Abs(substractedVector.y) + Mathf.Abs(substractedVector.z);
        }

        public static int GetAngleFromVector(this Vector3 dir)
        {
            dir = dir.normalized;
            var n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0)
                n += 360;
            var angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static int GetAngleFromVector180(this Vector3 dir)
        {
            dir = dir.normalized;
            var n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 GetRandomVector3(this Vector3 vector3) =>
            new(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        public static Vector3 GetNormalizedRandomVector3(this Vector3 vector3) =>
            GetRandomVector3(vector3).normalized;
    }
}