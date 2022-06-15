// Decompiled with JetBrains decompiler
// Type: DK64Viewer.F3DEX_VERT
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

namespace DK64Viewer
{
  public class F3DEX_VERT
  {
    public short x;
    public short y;
    public short z;
    public short u;
    public short v;
    public float r;
    public float g;
    public float b;
    public float a;

    public F3DEX_VERT(
      short x_,
      short y_,
      short z_,
      short u_,
      short v_,
      float r_,
      float g_,
      float b_,
      float a_)
    {
      this.x = x_;
      this.y = y_;
      this.z = z_;
      this.u = u_;
      this.v = v_;
      this.r = r_;
      this.g = g_;
      this.b = b_;
      this.a = a_;
    }

    public static F3DEX_VERT Clone(F3DEX_VERT v) => new F3DEX_VERT(v.x, v.y, v.z, v.u, v.v, v.r, v.g, v.b, v.a);
  }
}
