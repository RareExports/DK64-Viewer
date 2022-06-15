// Decompiled with JetBrains decompiler
// Type: DK64Viewer.GLCamera
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System;

namespace DK64Viewer
{
  public class GLCamera
  {
    private Vector3 position = new Vector3(0.0f, 0.0f, -2000f);
    private Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 up = new Vector3(0.0f, 1f, 0.0f);
    private Matrix4 ryMatrix = Matrix4.I;
    private Matrix4 rxMatrix = Matrix4.I;
    public float movementSpeed = 10f;

    public Vector3 Position => this.position;

    public Vector3 RotationDegrees => new Vector3(GLCamera.RadianToDegree((double) this.rotation.x), GLCamera.RadianToDegree((double) this.rotation.y), GLCamera.RadianToDegree((double) this.rotation.z));

    private static float DegreeToRadian(double angle) => (float) (Math.PI * angle / 180.0);

    private static float RadianToDegree(double angle) => (float) (angle * (180.0 / Math.PI));

    public void SetView(Vector3 position, Vector3 rotation)
    {
      this.position = position;
      this.rotation = new Vector3(GLCamera.DegreeToRadian((double) rotation.x), GLCamera.DegreeToRadian((double) rotation.y), GLCamera.DegreeToRadian((double) rotation.z));
      this.ryMatrix = Matrix4.GetRotationMatrixY((double) this.rotation.y);
      this.rxMatrix = Matrix4.GetRotationMatrixX((double) this.rotation.x);
    }

    public void LookAt(Vector3 target)
    {
      this.rotation.x = 0.22f;
      this.rotation.y = 3.1f;
      this.rotation.z = 0.0f;
      this.position.x = target.x * -1f;
      this.position.y = (float) ((double) target.y * -1.0 - 1000.0);
      this.position.z = target.z + 2000f;
      this.ryMatrix = Matrix4.GetRotationMatrixY((double) this.rotation.y);
      this.rxMatrix = Matrix4.GetRotationMatrixX((double) this.rotation.x);
    }

    public void PanUpdate(bool forward, bool back, bool left, bool right)
    {
      float movementSpeed = this.movementSpeed;
      float num1 = (float) Math.Sin((double) this.rotation.y) * movementSpeed;
      float num2 = (float) Math.Cos((double) this.rotation.y) * movementSpeed;
      float num3 = (float) Math.Sin((double) this.rotation.x) * movementSpeed;
      Vector3 vector3 = new Vector3();
      if (forward)
      {
        vector3.x -= num1;
        vector3.z += num2;
        vector3.y += num3;
      }
      if (back)
      {
        vector3.x += num1;
        vector3.z -= num2;
        vector3.y -= num3;
      }
      if (left)
      {
        vector3.x -= num2;
        vector3.z -= num1;
      }
      if (right)
      {
        vector3.x += num2;
        vector3.z += num1;
      }
      this.position += vector3;
    }

    public void MouseUpdate(int mouseDeltaX, int mouseDeltaY)
    {
      this.rotation.y += (float) mouseDeltaX * 0.01f;
      this.rotation.x += (float) mouseDeltaY * 0.005f;
      this.ryMatrix = Matrix4.GetRotationMatrixY((double) this.rotation.y);
      this.rxMatrix = Matrix4.GetRotationMatrixX((double) this.rotation.x);
    }

    public float[] GetWorldToViewMatrix() => (this.rxMatrix * this.ryMatrix * Matrix4.GetTranslationMatrix(this.position.x, this.position.y, this.position.z)).ToGLMatrix();
  }
}
