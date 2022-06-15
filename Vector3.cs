﻿// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Vector3
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System;

namespace DK64Viewer
{
  public class Vector3
  {
    public static Vector3 Zero = Vector3.NewZero();
    public static Vector3 One = Vector3.NewOne();
    public float x;
    public float y;
    public float z;

    public Vector3()
    {
      this.x = 0.0f;
      this.y = 0.0f;
      this.z = 0.0f;
    }

    public Vector3(float x, float y, float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public Vector3(float xyz)
    {
      this.x = xyz;
      this.y = xyz;
      this.z = xyz;
    }

    public Vector3(Vector3 v)
    {
      this.x = v.x;
      this.y = v.y;
      this.z = v.z;
    }

    public static Vector3 NewZero() => new Vector3(0.0f);

    public static Vector3 NewOne() => new Vector3(1f);

    public float DotProduct(Vector3 other) => (float) ((double) this.x * (double) other.x + (double) this.y * (double) other.y + (double) this.z * (double) other.z);

    public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

    public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

    public static Vector3 operator -(Vector3 v) => new Vector3(-v.x, -v.y, -v.z);

    public static Vector3 operator *(Vector3 v, float scalar) => new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);

    public static Vector3 operator /(Vector3 v, float scalar) => new Vector3(v.x / scalar, v.y / scalar, v.z / scalar);

    public static bool operator ==(Vector3 v1, Vector3 v2) => (double) v1.x == (double) v2.x && (double) v1.y == (double) v2.y && (double) v1.z == (double) v2.z;

    public static bool operator !=(Vector3 v1, Vector3 v2) => (double) v1.x != (double) v2.x || (double) v1.y != (double) v2.y || (double) v1.z != (double) v2.z;

    public static Vector3 CrossProduct(Vector3 a, Vector3 b) => new Vector3((float) ((double) a.y * (double) b.z - (double) a.z * (double) b.y), (float) ((double) a.z * (double) b.x - (double) a.x * (double) b.z), (float) ((double) a.x * (double) b.y - (double) a.y * (double) b.x));

    public Vector3 Add(Vector3 v)
    {
      this.x += v.x;
      this.y += v.y;
      this.z += v.z;
      return this;
    }

    public float DistanceTo(Vector3 v)
    {
      double num1 = (double) this.x - (double) v.x;
      float num2 = this.y - v.y;
      float num3 = this.z - v.z;
      return (float) Math.Sqrt(num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public float Size() => this.DistanceTo(Vector3.Zero);

    public Vector3 Normalize()
    {
      float num = this.Size();
      this.x /= num;
      this.y /= num;
      this.z /= num;
      return this;
    }

    public Vector3 Clone() => new Vector3(this);

    public override string ToString() => "(" + (object) this.x + ", " + (object) this.y + ", " + (object) this.z + ")";
  }
}
