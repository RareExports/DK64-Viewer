// Decompiled with JetBrains decompiler
// Type: DK64Viewer.RomHandler
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System;
using System.IO;

namespace DK64Viewer
{
  public static class RomHandler
  {
    public const int TABLE_SETUPS_START = 38776;
    private static byte[] rom;

    public static byte[] Rom
    {
      set => RomHandler.rom = value;
      get => RomHandler.rom;
    }

    public static int getNextPointer(int pntr_)
    {
      int num = (int) RomHandler.rom[pntr_] * 16777216 + (int) RomHandler.rom[pntr_ + 1] * 65536 + (int) RomHandler.rom[pntr_ + 2] * 256 + (int) RomHandler.rom[pntr_ + 3];
      int nextPointer = (int) RomHandler.rom[pntr_ + 8] * 16777216 + (int) RomHandler.rom[pntr_ + 1 + 8] * 65536 + (int) RomHandler.rom[pntr_ + 2 + 8] * 256 + (int) RomHandler.rom[pntr_ + 3 + 8];
      while (num - nextPointer == 0)
      {
        nextPointer = (int) RomHandler.rom[pntr_ + 8] * 16777216 + (int) RomHandler.rom[pntr_ + 1 + 8] * 65536 + (int) RomHandler.rom[pntr_ + 2 + 8] * 256 + (int) RomHandler.rom[pntr_ + 3 + 8];
        pntr_ += 8;
      }
      return nextPointer;
    }

    public static byte[] DecompressFileToByteArray(int address)
    {
      int compressedSize = 65536;
      byte[] Buffer = new byte[compressedSize];
      for (int index = 0; index < compressedSize; ++index)
        Buffer[index] = RomHandler.rom[address + index];
      GECompression geCompression = new GECompression();
      geCompression.SetCompressedBuffer(Buffer, Buffer.Length);
      int fileSize = 0;
      return geCompression.OutputDecompressedBuffer(ref fileSize, ref compressedSize);
    }

    public static byte[] GetDecompressedFile(int pntr, int length)
    {
      byte[] decompressedFile = new byte[length];
      for (int index = 0; index < length; ++index)
        decompressedFile[index] = RomHandler.rom[pntr + index];
      return decompressedFile;
    }

    public static void DecompressFileToHDD(int pntr)
    {
      if (File.Exists(Program.TMP + pntr.ToString("x")))
        return;
      try
      {
        byte[] byteArray = RomHandler.DecompressFileToByteArray(pntr);
        File.WriteAllBytes(Program.TMP + pntr.ToString("x"), byteArray);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static byte[] DecompressTextureFile(int fileNo)
    {
      int address = (fileNo + 7926 << 2) + 20872;
      try
      {
        return RomHandler.DecompressFileToByteArray(address);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
