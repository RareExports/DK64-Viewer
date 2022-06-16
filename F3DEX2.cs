// Decompiled with JetBrains decompiler
// Type: DK64Viewer.F3DEX2
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using OpenTK.Graphics.OpenGL;
using System;

namespace DK64Viewer
{
  public class F3DEX2
  {
    public static int F3DEX2_MTX_STACKSIZE = 18;
    public static int F3DEX2_MTX_MODELVIEW = 0;
    public static int F3DEX2_MTX_PROJECTION = 4;
    public static int F3DEX2_MTX_MUL = 0;
    public static int F3DEX2_MTX_LOAD = 2;
    public static int F3DEX2_MTX_NOPUSH = 0;
    public static int F3DEX2_MTX_PUSH = 1;
    public static int F3DEX2_TEXTURE_ENABLE = 0;
    public static int F3DEX2_SHADING_SMOOTH = 2097152;
    public static int F3DEX2_CULL_FRONT = 512;
    public static int F3DEX2_CULL_BACK = 1024;
    public static int F3DEX2_CULL_BOTH = 1536;
    public static int F3DEX2_CLIPPING = 8388608;
    public static int F3DEX2_MV_VIEWPORT = 8;
    public static int F3DEX2_MWO_aLIGHT_1 = 0;
    public static int F3DEX2_MWO_bLIGHT_1 = 4;
    public static int F3DEX2_MWO_aLIGHT_2 = 24;
    public static int F3DEX2_MWO_bLIGHT_2 = 28;
    public static int F3DEX2_MWO_aLIGHT_3 = 48;
    public static int F3DEX2_MWO_bLIGHT_3 = 52;
    public static int F3DEX2_MWO_aLIGHT_4 = 72;
    public static int F3DEX2_MWO_bLIGHT_4 = 76;
    public static int F3DEX2_MWO_aLIGHT_5 = 96;
    public static int F3DEX2_MWO_bLIGHT_5 = 100;
    public static int F3DEX2_MWO_aLIGHT_6 = 120;
    public static int F3DEX2_MWO_bLIGHT_6 = 124;
    public static int F3DEX2_MWO_aLIGHT_7 = 144;
    public static int F3DEX2_MWO_bLIGHT_7 = 148;
    public static int F3DEX2_MWO_aLIGHT_8 = 168;
    public static int F3DEX2_MWO_bLIGHT_8 = 172;
    public static int F3DEX2_RDPHALF_2 = 241;
    public static int F3DEX2_SETOTHERMODE_H = 227;
    public static int F3DEX2_SETOTHERMODE_L = 226;
    public static int F3DEX2_RDPHALF_1 = 225;
    public static int F3DEX2_SPNOOP = 224;
    public static int F3DEX2_ENDDL = 223;
    public static int F3DEX2_DL = 222;
    public static int F3DEX2_LOAD_UCODE = 221;
    public static int F3DEX2_MOVEMEM = 220;
    public static int F3DEX2_MOVEWORD = 219;
    public static int F3DEX2_MTX = 218;
    public static int F3DEX2_GEOMETRYMODE = 217;
    public static int F3DEX2_POPMTX = 216;
    public static int F3DEX2_TEXTURE = 215;
    public static int F3DEX2_DMA_IO = 214;
    public static int F3DEX2_SPECIAL_1 = 213;
    public static int F3DEX2_SPECIAL_2 = 212;
    public static int F3DEX2_SPECIAL_3 = 211;
    public static int F3DEX2_VTX = 1;
    public static int F3DEX2_MODIFYVTX = 2;
    public static int F3DEX2_CULLDL = 3;
    public static int F3DEX2_BRANCH_Z = 4;
    public static int F3DEX2_TRI1 = 5;
    public static int F3DEX2_TRI2 = 6;
    public static int F3DEX2_QUAD = 7;
    public static int FMT_RGBA = 0;
    public static int FMT_YUV = 1;
    public static int FMT_CI = 2;
    public static int FMT_IA = 3;
    public static int FMT_I = 3;
    public static int PS_4 = 0;
    public static int PS_8 = 1;
    public static int PS_16 = 2;
    public static int PS_32 = 3;
    public static int textureFormat = -1;
    public static int texelSize = 0;
    public static int lineSize = 0;
    public static int cms = 0;
    public static int cmt = 0;
    public static int tileNo = -1;
    public static bool firstSetSize = true;

    public static void ResetTexture()
    {
      F3DEX2.textureFormat = -1;
      F3DEX2.tileNo = -1;
      F3DEX2.firstSetSize = true;
    }

    public static void GL_G_LOADBLOCK(ref Texture currentTexture, uint w1)
    {
    }

    public static void GL_G_SETTILESIZE(ref Texture currentTexture, uint w1)
    {
      if (F3DEX2.firstSetSize && F3DEX2.textureFormat != 0)
      {
        F3DEX2.firstSetSize = false;
        uint num1 = ((w1 >> 12 & 4095U) >> 2) + 1U;
        currentTexture.textureWidth = (int) num1;
        uint num2 = (uint) (float) ((w1 & 4095U) >> 2) + 1U;
        currentTexture.textureHeight = (int) num2;
      }
      else
      {
        if (F3DEX2.textureFormat != 0)
          return;
        uint num3 = ((w1 >> 12 & 4095U) >> 2) + 1U;
        currentTexture.textureWidth = (int) num3;
        uint num4 = (uint) (float) ((w1 & 4095U) >> 2) + 1U;
        currentTexture.textureHeight = (int) num4;
      }
    }

    public static void GL_G_SETTIMG(
      ref Texture currentTexture,
      uint w0,
      uint w1,
      byte[] commandCheck,
      ref bool newTexture,
      float sScale,
      float tScale)
    {
      if (w0 >> 16 == 80U)
        currentTexture.indexID = w1 << 8 >> 8;
      else
        currentTexture.id = w1 << 8 >> 8;
      newTexture = true;
    }

    public static void GL_G_Combine(uint w1)
    {
      if (w1 == 1058404863U)
        GL.Disable(EnableCap.Texture2D);
      else
        GL.Enable(EnableCap.Texture2D);
    }

    public static void GL_SETGEOMETRYMODE(uint w1)
    {
      GL.Disable(EnableCap.CullFace);
      int num = (int) ((uint) (((int) w1 & 16777215) << 8) >> 8);
      bool flag1 = (uint) (num & 4096) > 0U;
      bool flag2 = (uint) (num & 8192) > 0U;
      bool flag3 = (uint) (num & 12288) > 0U;
      if (flag1)
        GL.CullFace(CullFaceMode.Front);
      if (flag2)
        GL.CullFace(CullFaceMode.Back);
      if (flag2 & flag1)
        GL.CullFace(CullFaceMode.FrontAndBack);
      if (!(flag1 | flag2 | flag3))
        return;
      GL.Enable(EnableCap.CullFace);
    }

    public static void GL_G_SETTILE(byte[] command, ref Texture tex)
    {
      uint num1 = (uint) ((int) command[4] * 16777216 + (int) command[5] * 65536 + (int) command[6] * 256) + (uint) command[7];
      uint num2 = (uint) ((int) command[1] * 65536 + (int) command[2] * 256) + (uint) command[3];
      if (num1 == 117440512U)
        return;
      GL.Enable(EnableCap.Texture2D);
      if (F3DEX2.textureFormat == -1)
        F3DEX2.textureFormat = (int) (byte) ((uint) command[1] >> 5);
            F3DEX2.texelSize = (int)command[1] >> 3 & 3;
            F3DEX2.lineSize = (int)((uint)num2 >> 9) & 15;
            F3DEX2.cmt = (int) (num1 >> 18) & 2;
      F3DEX2.cms = (int) (num1 >> 8) & 3;
      if (F3DEX2.tileNo != -1)
        return;
      F3DEX2.tileNo = (int) (num1 >> 24);
    }

    public static void GL_G_LOADTLUT()
    {
    }

    public static void F3DEX_2_GL_TEXTURE(
      ref byte[] bytesInFile,
      ref Texture texture,
      ref int texturesGL,
      bool deleteTextureGL)
    {
      GL.Enable(EnableCap.Texture2D);
      byte[] numArray1 = new byte[texture.textureWidth * texture.textureHeight * 4];
      byte[] numArray2 = new byte[texture.textureSize];
      int num = 0;
      byte[] pixels = bytesInFile;
      switch (F3DEX2.textureFormat)
      {
        case 0:
          if (F3DEX2.texelSize == 2)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.CONVERT_RGBA5551_RGBA8888(ref texture, ref pixels);
          }
          if (F3DEX2.texelSize == 3)
          {
            GL.Enable(EnableCap.Texture2D);
            try
            {
              for (int index = 0; index < numArray1.Length; ++index)
                numArray1[index] = bytesInFile[num + index];
              break;
            }
            catch (Exception ex)
            {
              break;
            }
          }
          else
            break;
        case 2:
          if (F3DEX2.texelSize == 0)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.CONVERT_CI4_RGBA8888(ref texture, ref pixels);
          }
          if (F3DEX2.texelSize == 2)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.ConvertCI4ToRGBA8888_2(ref texture, ref pixels);
          }
          if (F3DEX2.texelSize == 1)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.CONVERT_CI8_RGBA8888(ref texture, ref pixels);
            break;
          }
          break;
        case 3:
          if (F3DEX2.texelSize == 0)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.CONVERT_IA4_RGBA8888(ref texture, ref pixels);
          }
          if (F3DEX2.texelSize == 1)
          {
            GL.Enable(EnableCap.Texture2D);
            numArray1 = F3DEX2.CONVERT_IA8_RGBA8888(ref texture, ref pixels);
            break;
          }
          break;
      }
      texture.pixels = numArray1;
      if (F3DEX2.texelSize > 2 && F3DEX2.textureFormat != 0)
      {
        if (deleteTextureGL)
          GL.DeleteTextures(1, ref texturesGL);
        GL.Enable(EnableCap.Texture2D);
        GL.GenTextures(1, out texturesGL);
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, texturesGL);
        try
        {
          GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.textureWidth, texture.textureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        if (deleteTextureGL)
          GL.DeleteTextures(1, ref texturesGL);
        GL.Enable(EnableCap.Texture2D);
        GL.GenTextures(1, out texturesGL);
        Console.Write((object) GL.GetError());
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, texturesGL);
        GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.textureWidth, texture.textureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.pixels);
        Console.Write(GL.GetError().ToString());
      }
      GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName) 34046, 16);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
      if (F3DEX2.cms == 0)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32((object) TextureWrapMode.Repeat));
      if (F3DEX2.cms == 2)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 33071);
      if (F3DEX2.cmt == 0)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32((object) TextureWrapMode.Repeat));
      if (F3DEX2.cmt != 2)
        return;
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 33071);
    }

    public static byte[] CONVERT_RGBA5551_RGBA8888(ref Texture texture, ref byte[] textureN64Bytes)
    {
      int index1 = 0;
      uint index2 = 0;
      if (texture.textureWidth == 4)
      {
        texture.textureWidth = 32;
        texture.textureHeight = 32;
      }
      byte[] numArray = new byte[texture.textureHeight * texture.textureWidth * 4];
      try
      {
        for (int index3 = 0; index3 < texture.textureHeight; ++index3)
        {
          for (int index4 = 0; index4 < texture.textureWidth; ++index4)
          {
            ushort num1 = (ushort) ((uint) textureN64Bytes[(int) index2] * 256U + (uint) textureN64Bytes[(int) index2 + 1]);
            byte num2 = (byte) ((uint) (byte) ((uint) textureN64Bytes[(int) index2 + 1] << 7) >> 7) != (byte) 0 ? byte.MaxValue : (byte) 0;
            numArray[index1] = (byte) (((int) num1 & 63488) >> 8);
            numArray[index1 + 1] = (byte) (((int) num1 & 1984) << 5 >> 8);
            numArray[index1 + 2] = (byte) (((int) num1 & 62) << 18 >> 16);
            numArray[index1 + 3] = num2;
            index2 += 2U;
            index1 += 4;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return numArray;
    }

        public static byte[] CONVERT_CI4_RGBA8888(ref Texture texture, ref byte[] indices)
        {
            int index1 = 0;
            uint num1 = 0;
            int textureWidth = texture.textureWidth;
            int textureHeight = texture.textureHeight;
            byte[] numArray = new byte[textureWidth * textureHeight * 4];
            try
            {
                for (int index2 = 0; index2 < textureHeight; ++index2)
                {
                    for (int index3 = 0; index3 < textureWidth / 2; ++index3)
                    {
                        int num2 = (int)((uint)indices[(int)num1] >> 4);
                        int num3 = (int)((uint)(byte)((uint)indices[(int)num1] << 4) >> 4);
                        numArray[index1] = texture.red[(int)num2];
                        numArray[index1 + 1] = texture.green[(int)num2];
                        numArray[index1 + 2] = texture.blue[(int)num2];
                        numArray[index1 + 3] = texture.alpha[(int)num2];
                        numArray[index1 + 4] = texture.red[(int)num3];
                        numArray[index1 + 5] = texture.green[(int)num3];
                        numArray[index1 + 6] = texture.blue[(int)num3];
                        numArray[index1 + 7] = texture.alpha[(int)num3];
                        index1 += 8;
                        ++num1;
                    }
                    num1 += (uint)(F3DEX2.lineSize * 8 - textureWidth / 2);
                }
            }
            catch (Exception ex)
            {
            }
            return numArray;
        }

        public static byte[] ConvertCI4ToRGBA8888_2(ref Texture texture, ref byte[] indices)
        {
            int index1 = 0;
            uint num1 = 0;
            int textureWidth = texture.textureWidth;
            int textureHeight = texture.textureHeight;
            byte[] numArray = new byte[textureWidth * textureHeight * 4];
            try
            {
                for (int index2 = 0; index2 < textureHeight; ++index2)
                {
                    if (index1 + 7 < numArray.Length)
                    {
                        for (int index3 = 0; index3 < textureWidth / 2 && index1 + 7 < numArray.Length; ++index3)
                        {
                            byte num2 = (byte)((uint)indices[(int)num1] >> 4);
                            byte num3 = (byte)((uint)(byte)((uint)indices[(int)num1] << 4) >> 4);
                            numArray[index1] = texture.red[(int)num2];
                            numArray[index1 + 1] = texture.green[(int)num2];
                            numArray[index1 + 2] = texture.blue[(int)num2];
                            numArray[index1 + 3] = texture.alpha[(int)num2];
                            numArray[index1 + 4] = texture.red[(int)num3];
                            numArray[index1 + 5] = texture.green[(int)num3];
                            numArray[index1 + 6] = texture.blue[(int)num3];
                            numArray[index1 + 7] = texture.alpha[(int)num3];
                            index1 += 8;
                            ++num1;
                        }
                        if (index2 + 1 == textureHeight)
                            index2 = 0;
                    }
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return numArray;
        }

        public static byte[] CONVERT_CI8_RGBA8888(ref Texture texture, ref byte[] textureN64Bytes)
    {
      int index1 = 0;
      uint index2 = 0;
            texture.textureHeight = (textureN64Bytes.Length - 1) / texture.textureWidth;
            byte[] numArray = new byte[texture.textureWidth * texture.textureHeight * 4];
      try
      {
        for (int index3 = 0; index3 < texture.textureHeight; ++index3)
        {
          for (int index4 = 0; index4 < texture.textureWidth; ++index4)
          {
            numArray[index1] = texture.red[(int) textureN64Bytes[(int) index2]];
            numArray[index1 + 1] = texture.green[(int) textureN64Bytes[(int) index2]];
            numArray[index1 + 2] = texture.blue[(int) textureN64Bytes[(int) index2]];
            numArray[index1 + 3] = texture.alpha[(int) textureN64Bytes[(int) index2]];
            index1 += 2;
            ++index2;
          }
          if (F3DEX2.lineSize != 0)
            index2 += (uint) (F3DEX2.lineSize * 8 - texture.textureWidth);
        }
      }
      catch (Exception ex)
      {
      }
      return numArray;
    }

    public static byte[] CONVERT_IA4_RGBA8888(ref Texture texture, ref byte[] textureN64Bytes)
    {
      byte[] numArray = new byte[texture.textureWidth * texture.textureHeight * 4];
      texture.textureHeight = textureN64Bytes.Length * 2 / texture.textureWidth;
      try
      {
        for (int index = 0; index < textureN64Bytes.Length / 2; ++index)
        {
          byte num1 = (byte) ((uint) textureN64Bytes[index] >> 4);
          numArray[index * 8] = (byte) ((uint) num1 * 17U);
          numArray[index * 8 + 1] = (byte) ((uint) num1 * 17U);
          numArray[index * 8 + 2] = (byte) ((uint) num1 * 17U);
          numArray[index * 8 + 3] = (byte) ((uint) num1 * 17U);
          byte num2 = (byte) ((uint) (byte) ((uint) textureN64Bytes[index] << 4) >> 4);
          numArray[index * 4] = (byte) ((uint) num2 * 17U);
          numArray[index * 5 + 1] = (byte) ((uint) num2 * 17U);
          numArray[index * 6 + 2] = (byte) ((uint) num2 * 17U);
          numArray[index * 7 + 3] = (byte) ((uint) num2 * 17U);
        }
      }
      catch (Exception ex)
      {
      }
      return numArray;
    }

    public static byte[] CONVERT_IA8_RGBA8888(ref Texture texture, ref byte[] textureN64Bytes)
    {
      byte[] numArray = new byte[texture.textureWidth * texture.textureHeight * 4];
      int index1 = 0;
      texture.textureHeight = textureN64Bytes.Length / texture.textureWidth;
      try
      {
        int index2 = 0;
        for (int index3 = 0; index3 < texture.textureHeight; ++index3)
        {
          for (int index4 = 0; index4 < texture.textureWidth; ++index4)
          {
            byte num1 = (byte) ((uint) textureN64Bytes[index2] >> 4);
            byte num2 = (byte) ((int) textureN64Bytes[index2] << 4 >> 4);
            numArray[index1] = (byte) ((uint) num1 * 17U);
            numArray[index1 + 1] = (byte) ((uint) num1 * 17U);
            numArray[index1 + 2] = (byte) ((uint) num1 * 17U);
            numArray[index1 + 3] = (byte) ((uint) num2 * 17U);
            index1 += 4;
            ++index2;
          }
          index2 += F3DEX2.lineSize * 8 - texture.textureWidth;
        }
      }
      catch (Exception ex)
      {
      }
      return numArray;
    }

    public static byte[] CONVERT_IA16_RGBA8888(ref Texture texture, ref byte[] textureN64Bytes)
    {
      byte[] numArray = new byte[texture.textureWidth * texture.textureHeight * 4];
      int index1 = 0;
      texture.textureHeight = textureN64Bytes.Length / 2 / texture.textureWidth;
      try
      {
        int index2 = 0;
        for (int index3 = 0; index3 < texture.textureHeight; ++index3)
        {
          for (int index4 = 0; index4 < texture.textureWidth; ++index4)
          {
            byte num1 = textureN64Bytes[index2];
            byte num2 = textureN64Bytes[index2 + 1];
            numArray[index1] = num1;
            numArray[index1 + 1] = num1;
            numArray[index1 + 2] = num1;
            numArray[index1 + 3] = num2;
            index1 += 4;
            index2 += 2;
          }
          index2 += F3DEX2.lineSize * 4 - texture.textureWidth;
        }
      }
      catch (Exception ex)
      {
      }
      return numArray;
    }
  }
}
