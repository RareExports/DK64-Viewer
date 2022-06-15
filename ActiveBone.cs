// Decompiled with JetBrains decompiler
// Type: DK64Viewer.ActiveBone
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

namespace DK64Viewer
{
  public class ActiveBone
  {
    public byte bone;
    public int length;
    public int parentCMDBoneID = -1;
    public int meshID;

    public ActiveBone(byte b, int l)
    {
      this.bone = b;
      this.length = l;
    }
  }
}
