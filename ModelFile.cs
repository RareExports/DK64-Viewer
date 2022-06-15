// Decompiled with JetBrains decompiler
// Type: DK64Viewer.ModelFile
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System.Collections.Generic;

namespace DK64Viewer
{
  public class ModelFile
  {
    public string fileAddress = "";
    public ModelFileType FileType = ModelFileType.Invalid;
    public string fileName = "";
    public int F3DStart;
    public int F3DCommands;
    public int F3DEnd;
    public int vertStart;
    public int vertEnd;
    public List<byte[]> commands = new List<byte[]>();
    public F3DEX_VERT[] f3d_verts = new F3DEX_VERT[32];
    public List<ModelSectionGroup> groups = new List<ModelSectionGroup>();
    public List<ModelSection> sections = new List<ModelSection>();
    public int collStart;
    public int VTCount;
    public int textureCount;
  }
}
