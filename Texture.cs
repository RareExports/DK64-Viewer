// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Texture
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

namespace DK64Viewer
{
  public class Texture
  {
    public string file = "";
    public uint pointer;
    public uint indexID;
    public uint id;
    public int textureSize;
    public int textureWidth;
    public int textureHeight;
    public float textureHRatio;
    public float textureWRatio;
    public uint textureOffset;
    public uint indexOffset;
    public int palSize;
    public byte[] palette;
    public byte[] red;
    public byte[] green;
    public byte[] blue;
    public byte[] alpha;
    public int glIndex;
    public bool palLoaded;
    public byte[] pixels;

    public static Texture Clone(Texture t) => new Texture(t.pointer, t.textureOffset, t.textureWidth, t.textureHeight)
    {
      alpha = t.alpha,
      blue = t.blue,
      file = t.file,
      glIndex = t.glIndex,
      green = t.green,
      id = t.id,
      indexID = t.indexID,
      indexOffset = t.indexOffset,
      palette = t.palette,
      palLoaded = t.palLoaded,
      palSize = t.palSize,
      pixels = t.pixels,
      pointer = t.pointer,
      red = t.red,
      textureHRatio = t.textureHRatio,
      textureSize = t.textureSize,
      textureWRatio = t.textureWRatio,
      textureOffset = t.textureOffset
    };

    public Texture(uint pointer_, uint textureOffset_, int textureWidth_, int textureHeight_)
    {
      this.pointer = pointer_;
      this.textureOffset = textureOffset_;
      this.textureWidth = textureWidth_;
      this.textureHeight = textureHeight_;
      this.indexOffset = this.textureOffset + 32U;
      this.textureSize = this.textureWidth * this.textureHeight * 2;
    }

    public void setRatio(float sScale, float tScale)
    {
      this.textureHRatio = tScale / 32f / (float) this.textureHeight;
      this.textureWRatio = sScale / 32f / (float) this.textureWidth;
    }

    public void loadPalette(byte[] bytesInFile, int palSize)
    {
      this.palSize = palSize / 2;
      this.palette = new byte[palSize];
      this.red = new byte[palSize / 2];
      this.green = new byte[palSize / 2];
      this.blue = new byte[palSize / 2];
      this.alpha = new byte[palSize / 2];
      int index1 = 0;
      for (uint index2 = 0; (long) index2 < (long) palSize; ++index2)
      {
        this.palette[index1] = bytesInFile[(int) index2];
        ++index1;
      }
      int index3 = 0;
      for (int index4 = 0; index4 < palSize / 2; ++index4)
      {
        this.red[index4] = this.palette[index3];
        this.red[index4] >>= 3;
        this.red[index4] *= (byte) 8;
        ushort num = (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) this.palette[index3] * 256U + (uint) this.palette[index3 + 1]) << 5) >> 3) >> 8);
        this.green[index4] = (byte) num;
        this.green[index4] *= (byte) 8;
        this.blue[index4] = this.palette[index3 + 1];
        this.blue[index4] <<= 2;
        this.blue[index4] >>= 3;
        this.blue[index4] *= (byte) 8;
        this.alpha[index4] = this.palette[index3 + 1];
        this.alpha[index4] <<= 7;
        this.alpha[index4] >>= 7;
        this.alpha[index4] = this.alpha[index4] != (byte) 1 ? (byte) 0 : byte.MaxValue;
        index3 += 2;
      }
      this.palLoaded = true;
    }
  }
}
