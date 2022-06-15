// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Core
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;

namespace DK64Viewer
{
  public class Core
  {
    public static void InitGl()
    {
      GL.Enable(EnableCap.DepthTest);
      GL.Enable(EnableCap.CullFace);
      GL.Enable(EnableCap.Texture2D);
      GL.ShadeModel(ShadingModel.Smooth);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
      GL.ClearColor(0.2f, 0.5f, 1f, 0.0f);
      GL.BindTexture(TextureTarget.Texture2D, 0);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9728);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9728);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 33071);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 33071);
      GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName) 34046, 16);
    }

    public static void SetView(int height, int width)
    {
      GL.Viewport(0, 0, width, height);
      GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
      GL.LoadIdentity();
      Glu.gluPerspective(45.0, 1.0 * (double) ((float) width / (float) height), 1.0, 100000.0);
      GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
      GL.LoadIdentity();
    }

    public static void ClearScreen()
    {
      GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
      GL.LoadIdentity();
    }

    public static void PopMatrix() => GL.PopMatrix();

    public static void PushMatrix() => GL.PushMatrix();

    public static void DeleteDL(uint dl) => GL.DeleteLists(dl, 1);

    public static void DeleteDLs(List<uint> dls)
    {
      foreach (uint dl in dls)
        GL.DeleteLists(dl, 1);
    }

    public static void DrawCube(int sx, int sy, int sz, int lx, int ly, int lz)
    {
      GL.Begin(BeginMode.Quads);
      GL.Vertex3(lx, sy, sz);
      GL.Vertex3(lx, ly, sz);
      GL.Vertex3(lx, ly, lz);
      GL.Vertex3(lx, sy, lz);
      GL.Vertex3(sx, sy, sz);
      GL.Vertex3(sx, sy, lz);
      GL.Vertex3(sx, ly, lz);
      GL.Vertex3(sx, ly, sz);
      GL.Vertex3(lx, sy, lz);
      GL.Vertex3(lx, ly, lz);
      GL.Vertex3(sx, ly, lz);
      GL.Vertex3(sx, sy, lz);
      GL.Vertex3(sx, sy, sz);
      GL.Vertex3(sx, ly, sz);
      GL.Vertex3(lx, ly, sz);
      GL.Vertex3(lx, sy, sz);
      GL.Vertex3(sx, ly, sz);
      GL.Vertex3(sx, ly, lz);
      GL.Vertex3(lx, ly, lz);
      GL.Vertex3(lx, ly, sz);
      GL.Vertex3(sx, sy, lz);
      GL.Vertex3(sx, sy, sz);
      GL.Vertex3(lx, sy, sz);
      GL.Vertex3(lx, sy, lz);
      GL.End();
    }

    public static void DeleteTextures(List<int> dls)
    {
      for (int index = 0; index < dls.Count; ++index)
      {
        if (index != -1)
        {
          uint dl = (uint) dls[index];
          GL.DeleteTextures(1, ref dl);
        }
      }
    }

    public static void DeleteBuffers(List<uint> buffers, bool arb)
    {
      for (int index = 0; index < buffers.Count; ++index)
      {
        if (index != -1)
        {
          uint buffer = buffers[index];
          if (arb)
            GL.Arb.DeleteBuffers(1, ref buffer);
          else
            GL.DeleteBuffers(1, ref buffer);
        }
      }
    }

    public static void DeleteBuffer(uint buffer, bool arb)
    {
      if (arb)
        GL.Arb.DeleteBuffers(1, ref buffer);
      else
        GL.DeleteBuffers(1, ref buffer);
    }

    public static ModelFile ReadModel(ref byte[] bytesInFile)
    {
      ModelFile modelFile = new ModelFile();
      if (((int) bytesInFile[8] << 8) + (int) bytesInFile[9] == (int) ushort.MaxValue)
      {
        modelFile.FileType = ModelFileType.Level;
        int sourceIndex = 12;
        byte[] numArray = new byte[6];
        Array.Copy((Array) bytesInFile, sourceIndex, (Array) numArray, 0, 6);
        modelFile.fileName = Core.convertHexString(numArray);
        int index1 = 64;
        modelFile.F3DStart = ((int) bytesInFile[index1] << 24) + ((int) bytesInFile[index1 + 1] << 16) + ((int) bytesInFile[index1 + 2] << 8) + (int) bytesInFile[index1 + 3];
        int index2 = index1 + 4 + 4;
        modelFile.vertStart = ((int) bytesInFile[index2] << 24) + ((int) bytesInFile[index2 + 1] << 16) + ((int) bytesInFile[index2 + 2] << 8) + (int) bytesInFile[index2 + 3];
        int index3 = index2 + 4;
        int num = ((int) bytesInFile[index3] << 24) + ((int) bytesInFile[index3 + 1] << 16) + ((int) bytesInFile[index3 + 2] << 8) + (int) bytesInFile[index3 + 3];
        modelFile.F3DCommands = (modelFile.vertStart - modelFile.F3DStart) / 8;
        modelFile.F3DEnd = modelFile.vertStart;
        modelFile.VTCount = (num - modelFile.vertStart) / 16;
        modelFile.textureCount = 0;
      }
      else if (((int) bytesInFile[128] << 8) + (int) bytesInFile[129] == (int) ushort.MaxValue && ((int) bytesInFile[130] << 8) + (int) bytesInFile[131] == (int) ushort.MaxValue)
      {
        modelFile.FileType = ModelFileType.Level;
        int index4 = 52;
        modelFile.F3DStart = ((int) bytesInFile[index4] << 24) + ((int) bytesInFile[index4 + 1] << 16) + ((int) bytesInFile[index4 + 2] << 8) + (int) bytesInFile[index4 + 3];
        int index5 = index4 + 4;
        modelFile.vertStart = ((int) bytesInFile[index5] << 24) + ((int) bytesInFile[index5 + 1] << 16) + ((int) bytesInFile[index5 + 2] << 8) + (int) bytesInFile[index5 + 3];
        int index6 = index5 + 4 + 4;
        int num1 = ((int) bytesInFile[index6] << 24) + ((int) bytesInFile[index6 + 1] << 16) + ((int) bytesInFile[index6 + 2] << 8) + (int) bytesInFile[index6 + 3];
        modelFile.F3DCommands = (modelFile.vertStart - modelFile.F3DStart) / 8;
        modelFile.F3DEnd = modelFile.vertStart;
        modelFile.VTCount = (num1 - modelFile.vertStart) / 16;
        modelFile.textureCount = 0;
        int num2 = ((int) bytesInFile[88] << 24) + ((int) bytesInFile[89] << 16) + ((int) bytesInFile[90] << 8) + (int) bytesInFile[91];
        int num3 = ((int) bytesInFile[92] << 24) + ((int) bytesInFile[93] << 16) + ((int) bytesInFile[94] << 8) + (int) bytesInFile[95];
        int num4 = num2 + 4;
        for (int index7 = 0; num4 + index7 < num3; index7 += 28)
          modelFile.sections.Add(new ModelSection()
          {
            sectionID = ((int) bytesInFile[num4 + index7] << 8) + (int) bytesInFile[num4 + index7 + 1],
            meshID = ((int) bytesInFile[num4 + index7 + 2] << 8) + (int) bytesInFile[num4 + index7 + 3],
            vertStart = ((int) bytesInFile[num4 + index7 + 8] << 8) + (int) bytesInFile[num4 + index7 + 9],
            unknownVertStart = ((int) bytesInFile[num4 + index7 + 10] << 8) + (int) bytesInFile[num4 + index7 + 11],
            vertStart2 = ((int) bytesInFile[num4 + index7 + 12] << 8) + (int) bytesInFile[num4 + index7 + 13],
            vertStart3 = ((int) bytesInFile[num4 + index7 + 14] << 8) + (int) bytesInFile[num4 + index7 + 15],
            numVerts1 = ((int) bytesInFile[num4 + index7 + 16] << 8) + (int) bytesInFile[num4 + index7 + 17],
            numVertsUnknown = ((int) bytesInFile[num4 + index7 + 18] << 8) + (int) bytesInFile[num4 + index7 + 19],
            numVerts2 = ((int) bytesInFile[num4 + index7 + 20] << 8) + (int) bytesInFile[num4 + index7 + 21],
            numVerts3 = ((int) bytesInFile[num4 + index7 + 22] << 8) + (int) bytesInFile[num4 + index7 + 23]
          });
        List<int> intList = new List<int>();
        foreach (ModelSection section in modelFile.sections)
        {
          if (!intList.Contains(section.sectionID))
          {
            intList.Add(section.sectionID);
            modelFile.groups.Add(new ModelSectionGroup()
            {
              sectionID = section.sectionID
            });
          }
        }
        modelFile.groups = modelFile.groups.OrderBy<ModelSectionGroup, int>((Func<ModelSectionGroup, int>) (o => o.sectionID)).ToList<ModelSectionGroup>();
        foreach (ModelSectionGroup group in modelFile.groups)
        {
          foreach (ModelSection section in modelFile.sections)
          {
            if (section.sectionID == group.sectionID)
              group.endOffset += section.numVerts1 + section.numVerts2 + section.numVerts3 + section.numVertsUnknown;
          }
        }
      }
      else
      {
        modelFile.FileType = ModelFileType.Character;
        modelFile.fileName = "Character Format?";
        modelFile.vertStart = 40;
        modelFile.F3DStart = modelFile.vertStart;
        while (modelFile.F3DStart + 16 < bytesInFile.Length && (int) bytesInFile[modelFile.F3DStart] + (int) bytesInFile[modelFile.F3DStart + 1] + (int) bytesInFile[modelFile.F3DStart + 2] + (int) bytesInFile[modelFile.F3DStart + 3] != 231)
          modelFile.F3DStart += 16;
        int f3Dstart = modelFile.F3DStart;
        modelFile.F3DEnd = f3Dstart;
        while (modelFile.F3DEnd + 8 < bytesInFile.Length && bytesInFile[modelFile.F3DEnd] != (byte) 16)
          modelFile.F3DEnd += 8;
        modelFile.F3DCommands = (modelFile.F3DEnd - modelFile.F3DStart) / 8;
        modelFile.VTCount = (f3Dstart - modelFile.vertStart) / 16;
        modelFile.textureCount = 0;
      }
      modelFile.f3d_verts = new F3DEX_VERT[modelFile.VTCount];
      Core.RipVerts(ref bytesInFile, ref modelFile.f3d_verts, modelFile.VTCount, modelFile.vertStart);
      int num5 = 0;
      int f3Dstart1 = modelFile.F3DStart;
      for (; num5 < modelFile.F3DCommands; ++num5)
      {
        byte[] numArray = new byte[8];
        for (int index = 0; index < 8; ++index)
          numArray[index] = bytesInFile[f3Dstart1 + index];
        f3Dstart1 += 8;
        modelFile.commands.Add(numArray);
      }
      return modelFile;
    }

    private static void RipVerts(
      ref byte[] bytesInFile,
      ref F3DEX_VERT[] verts,
      int VTCount,
      int offset)
    {
      for (int index = 0; index < VTCount; ++index)
      {
        int x_ = (int) (short) ((int) bytesInFile[offset] * 256 + (int) bytesInFile[offset + 1]);
        short num1 = (short) ((int) bytesInFile[offset + 2] * 256 + (int) bytesInFile[offset + 3]);
        short num2 = (short) ((int) bytesInFile[offset + 4] * 256 + (int) bytesInFile[offset + 5]);
        short num3 = (short) ((int) bytesInFile[offset + 8] * 256 + (int) bytesInFile[offset + 9]);
        short num4 = (short) ((int) bytesInFile[offset + 10] * 256 + (int) bytesInFile[offset + 11]);
        float num5 = (float) bytesInFile[offset + 12] / (float) byte.MaxValue;
        float num6 = (float) bytesInFile[offset + 13] / (float) byte.MaxValue;
        float num7 = (float) bytesInFile[offset + 14] / (float) byte.MaxValue;
        float num8 = (float) bytesInFile[offset + 15] / (float) byte.MaxValue;
        int y_ = (int) num1;
        int z_ = (int) num2;
        int u_ = (int) num3;
        int v_ = (int) num4;
        double r_ = (double) num5;
        double g_ = (double) num6;
        double b_ = (double) num7;
        double a_ = (double) num8;
        F3DEX_VERT f3DexVert = new F3DEX_VERT((short) x_, (short) y_, (short) z_, (short) u_, (short) v_, (float) r_, (float) g_, (float) b_, (float) a_);
        verts[index] = f3DexVert;
        offset += 16;
      }
    }

    public static string convertHexString(byte[] hex)
    {
      string str = "";
      for (int index = 0; index < hex.Length; ++index)
      {
        if (hex[index] >= (byte) 32 && hex[index] <= (byte) 126)
        {
          char ch = (char) hex[index];
          str += ch.ToString();
        }
      }
      return str;
    }

    private static void LoadNewTexture(
      ref byte[] textureTable,
      List<Texture> textures,
      ref Texture currentTexture,
      ref int glTextureName,
      float sScale,
      float tScale,
      bool writeTextures,
      string exportDir)
    {
      int index = currentTexture.indexID == 0U ? (int) currentTexture.id << 2 : (int) currentTexture.indexID << 2;
      int pntr = ((int) textureTable[index] << 24) + ((int) textureTable[index + 1] << 16) + ((int) textureTable[index + 2] << 8) + (int) textureTable[index + 3] + 1055824;
      RomHandler.DecompressFileToHDD(pntr);
      currentTexture.pointer = (uint) pntr;
      currentTexture.file = currentTexture.pointer.ToString("x");
      bool flag = false;
      foreach (Texture texture in textures)
      {
        if ((int) currentTexture.pointer == (int) texture.pointer)
        {
          flag = true;
          glTextureName = texture.glIndex;
          currentTexture.textureHeight = texture.textureHeight;
          currentTexture.textureWidth = texture.textureWidth;
          currentTexture.textureWRatio = texture.textureWRatio;
          currentTexture.textureHRatio = texture.textureHRatio;
        }
      }
      if (!flag)
      {
        byte[] bytesInFile = File.ReadAllBytes(Program.TMP + currentTexture.file);
        F3DEX2.F3DEX_2_GL_TEXTURE(ref bytesInFile, ref currentTexture, ref glTextureName, false);
        if (writeTextures)
          Core.writeTexture(currentTexture.pixels, currentTexture.textureWidth, currentTexture.textureHeight, glTextureName, exportDir);
        currentTexture.glIndex = glTextureName;
        currentTexture.setRatio(sScale, tScale);
        textures.Add(Texture.Clone(currentTexture));
      }
      F3DEX2.ResetTexture();
      currentTexture.indexID = 0U;
      currentTexture.id = 0U;
    }

    public static void GetBuffersFromDKModelFile(
      ref ModelFile file,
      ref byte[] bytesInFile,
      ref uint vboVertexHandle,
      ref float[] vertexData,
      ref uint vboColorHandle,
      ref uint vboTexCoordHandle,
      ref List<uint> iboHandles,
      ref List<ushort[]> iboData,
      ref List<int> texturesGL,
      bool exportModel)
    {
      string str1 = Program.EXPORT + file.fileAddress + "//";
      if (exportModel)
        Directory.CreateDirectory(str1);
      List<ushort> ushortList = new List<ushort>();
      bool newTexture = false;
      float sScale = 0.0f;
      float tScale = 0.0f;
      ushort[] numArray = new ushort[32];
      F3DEX2 f3DeX2 = new F3DEX2();
      Texture texture = new Texture(0U, 0U, 0, 0);
      List<Texture> textures = new List<Texture>();
      byte[] textureTable = File.ReadAllBytes(".\\resources\\texturetable.bin");
      try
      {
        Hashtable hashtable = new Hashtable();
        List<uint> uintList1 = new List<uint>();
        List<uint> uintList2 = new List<uint>();
        int f3Dstart = file.F3DStart;
        int f3Dend = file.F3DEnd;
        int f3Dcommands = file.F3DCommands;
        int vertStart = file.vertStart;
        int vtCount = file.VTCount;
        int textureCount = file.textureCount;
        F3DEX_VERT[] f3dVerts = file.f3d_verts;
        List<byte[]> commands = file.commands;
        vertexData = new float[file.VTCount * 3];
        float[] data1 = new float[file.VTCount * 4];
        float[] data2 = new float[file.VTCount * 2];
        int index1 = 0;
        int index2 = 0;
        int index3 = 0;
        foreach (F3DEX_VERT f3DexVert in f3dVerts)
        {
          vertexData[index1] = (float) f3DexVert.x;
          vertexData[index1 + 1] = (float) f3DexVert.y;
          vertexData[index1 + 2] = (float) f3DexVert.z;
          data1[index2] = f3DexVert.r;
          data1[index2 + 1] = f3DexVert.g;
          data1[index2 + 2] = f3DexVert.b;
          data1[index2 + 3] = f3DexVert.a;
          data2[index3] = (float) f3DexVert.u;
          data2[index3 + 1] = (float) f3DexVert.v;
          index1 += 3;
          index2 += 4;
          index3 += 2;
        }
        GL.GenBuffers(1, out vboVertexHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertexHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (vertexData.Length * 4), vertexData, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.GenBuffers(1, out vboColorHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboColorHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (data1.Length * 4), data1, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        uint key = 0;
        for (int index4 = 0; index4 < file.F3DCommands; ++index4)
        {
          byte[] command1 = file.commands[index4];
          uint num4 = (uint) ((int) command1[4] * 16777216 + (int) command1[5] * 65536 + (int) command1[6] * 256) + (uint) command1[7];
          uint w0 = (uint) ((int) command1[1] * 65536 + (int) command1[2] * 256) + (uint) command1[3];
          if (command1[0] == (byte) 0 && num4 <= (uint) byte.MaxValue)
          {
            key = num4;
            int num5;
            if (uintList1.Contains(num4))
            {
              int useNumber = 0;
              foreach (int num6 in uintList1)
              {
                if ((long) num6 == (long) num4)
                  ++useNumber;
              }
              num5 = Core.CalcSectionVtxOffset(file.groups, file.sections, (int) num4, useNumber);
              hashtable[(object) num4] = (object) 0;
              uintList1.Add(num4);
            }
            else
            {
              hashtable.Add((object) num4, (object) 0);
              uintList1.Add(num4);
              num5 = Core.CalcSectionVtxOffset(file.groups, file.sections, (int) num4, 0);
            }
            num1 = num5 + num3;
            num2 = num1;
          }
          if (command1[0] == (byte) 223)
            key = 0U;
          if (command1[0] == (byte) 253)
            F3DEX2.GL_G_SETTIMG(ref texture, w0, num4, file.commands[index4 + 2], ref newTexture, sScale, tScale);
          if (command1[0] == (byte) 243)
            F3DEX2.GL_G_LOADBLOCK(ref texture, num4);
          if (command1[0] == (byte) 242)
            F3DEX2.GL_G_SETTILESIZE(ref texture, num4);
          int num7 = (int) command1[0];
          int num8 = (int) command1[0];
          int num9 = (int) command1[0];
          if (command1[0] == (byte) 245)
            F3DEX2.GL_G_SETTILE(command1, ref texture);
          if (command1[0] == (byte) 240)
          {
            int palSize = (int) ((num4 << 8 >> 8 & 16773120U) >> 14) * 2 + 2;
            int index5 = (int) texture.id << 2;
            int pntr = ((int) textureTable[index5] << 24) + ((int) textureTable[index5 + 1] << 16) + ((int) textureTable[index5 + 2] << 8) + (int) textureTable[index5 + 3] + 1055824;
            RomHandler.DecompressFileToHDD(pntr);
            texture.pointer = (uint) pntr;
            texture.file = texture.pointer.ToString("x");
            byte[] bytesInFile1 = File.ReadAllBytes(Program.TMP + texture.file);
            texture.loadPalette(bytesInFile1, palSize);
            if (file.commands[index4 + 4][0] == (byte) 186)
              newTexture = true;
          }
          if (command1[0] == (byte) 215)
          {
            sScale = (float) (num4 >> 16) / 65536f;
            tScale = (float) (num4 & (uint) ushort.MaxValue) / 65536f;
          }
          if ((int) command1[0] == F3DEX2.F3DEX2_VTX)
          {
            byte[] command2 = file.commands[index4];
            int num10 = (int) command1[4] * 16777216 + (int) command1[5] * 65536 + (int) command1[6] * 256 + (int) command1[7];
            int num11 = (int) command1[1] * 65536 + (int) command1[2] * 256 + (int) command1[3];
            byte num12 = (byte) ((uint) num11 >> 12 & (uint) byte.MaxValue);
            byte num13 = (byte) (((uint) num11 >> 1 & (uint) sbyte.MaxValue) - (uint) num12);
            if (num13 > (byte) 63)
              num13 = (byte) 63;
            uint num14 = ((uint) (num10 << 8) >> 8) / 16U;
            if (key != 0U)
            {
              int num15 = (int) num14 + (int) num12;
              if ((int) hashtable[(object) key] < num15)
                hashtable[(object) key] = (object) num15;
            }
            else
            {
              int num16 = (int) num14 + (int) num12;
              if (num3 < num16)
                num3 = num16;
            }
            uint num17;
            if (key != 0U)
            {
              num17 = num14 + (uint) num1;
              if ((long) num2 < (long) (num17 + (uint) num12))
                num2 = (int) num17 + (int) num12;
            }
            else
              num17 = num14 + (uint) num2;
            uint num18 = num17;
            try
            {
              for (int index6 = (int) num13; index6 < (int) num12 + (int) num13; ++index6)
              {
                if ((long) num18 < (long) file.f3d_verts.Length)
                  numArray[index6] = (ushort) num18;
                ++num18;
              }
            }
            catch (Exception ex)
            {
            }
          }
          if ((int) command1[0] == F3DEX2.F3DEX2_TRI1)
          {
            if (newTexture)
            {
              if (texturesGL.Count > 0)
              {
                ushort[] array = ushortList.ToArray();
                uint buffers = 0;
                GL.GenBuffers(1, out buffers);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffers);
                GL.BufferData<ushort>(BufferTarget.ArrayBuffer, (IntPtr) (array.Length * 2), array, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                iboHandles.Add(buffers);
                iboData.Add(array);
                ushortList.Clear();
              }
              newTexture = false;
              texturesGL.Add(-1);
              int glTextureName = 0;
              Core.LoadNewTexture(ref textureTable, textures, ref texture, ref glTextureName, sScale, tScale, exportModel, str1);
              texturesGL[texturesGL.Count - 1] = glTextureName;
            }
            short index7 = (short) ((int) command1[1] / 2);
            short index8 = (short) ((int) command1[2] / 2);
            short index9 = (short) ((int) command1[3] / 2);
            ushortList.Add(numArray[(int) index7]);
            ushortList.Add(numArray[(int) index8]);
            ushortList.Add(numArray[(int) index9]);
            data2[(int) numArray[(int) index7] * 2] = (float) file.f3d_verts[(int) numArray[(int) index7]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index7] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index7]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index8] * 2] = (float) file.f3d_verts[(int) numArray[(int) index8]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index8] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index8]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index9] * 2] = (float) file.f3d_verts[(int) numArray[(int) index9]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index9] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index9]].v * texture.textureHRatio;
          }
          if ((int) command1[0] == F3DEX2.F3DEX2_TRI2)
          {
            if (newTexture)
            {
              if (texturesGL.Count > 0)
              {
                ushort[] array = ushortList.ToArray();
                uint buffers = 0;
                GL.GenBuffers(1, out buffers);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffers);
                GL.BufferData<ushort>(BufferTarget.ArrayBuffer, (IntPtr) (array.Length * 2), array, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                iboHandles.Add(buffers);
                iboData.Add(array);
                ushortList.Clear();
              }
              newTexture = false;
              texturesGL.Add(-1);
              int glTextureName = 0;
              Core.LoadNewTexture(ref textureTable, textures, ref texture, ref glTextureName, sScale, tScale, exportModel, str1);
              texturesGL[texturesGL.Count - 1] = glTextureName;
            }
            short index10 = (short) ((int) command1[1] / 2);
            short index11 = (short) ((int) command1[2] / 2);
            short index12 = (short) ((int) command1[3] / 2);
            ushortList.Add(numArray[(int) index10]);
            ushortList.Add(numArray[(int) index11]);
            ushortList.Add(numArray[(int) index12]);
            data2[(int) numArray[(int) index10] * 2] = (float) file.f3d_verts[(int) numArray[(int) index10]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index10] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index10]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index11] * 2] = (float) file.f3d_verts[(int) numArray[(int) index11]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index11] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index11]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index12] * 2] = (float) file.f3d_verts[(int) numArray[(int) index12]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index12] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index12]].v * texture.textureHRatio;
            short index13 = (short) ((int) command1[5] / 2);
            short index14 = (short) ((int) command1[6] / 2);
            short index15 = (short) ((int) command1[7] / 2);
            ushortList.Add(numArray[(int) index13]);
            ushortList.Add(numArray[(int) index14]);
            ushortList.Add(numArray[(int) index15]);
            data2[(int) numArray[(int) index13] * 2] = (float) file.f3d_verts[(int) numArray[(int) index13]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index13] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index13]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index14] * 2] = (float) file.f3d_verts[(int) numArray[(int) index14]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index14] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index14]].v * texture.textureHRatio;
            data2[(int) numArray[(int) index15] * 2] = (float) file.f3d_verts[(int) numArray[(int) index15]].u * texture.textureWRatio;
            data2[(int) numArray[(int) index15] * 2 + 1] = (float) file.f3d_verts[(int) numArray[(int) index15]].v * texture.textureHRatio;
          }
        }
        ushort[] array1 = ushortList.ToArray();
        uint buffers1 = 0;
        GL.GenBuffers(1, out buffers1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffers1);
        GL.BufferData<ushort>(BufferTarget.ArrayBuffer, (IntPtr) (array1.Length * 2), array1, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        iboHandles.Add(buffers1);
        iboData.Add(array1);
        ushortList.Clear();
        GL.GenBuffers(1, out vboTexCoordHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboTexCoordHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (data2.Length * 4), data2, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        if (!exportModel)
          return;
        string str2 = "#Ripped by Skill" + Environment.NewLine + "mtllib dklevel.mtl" + Environment.NewLine;
        for (int index16 = 0; index16 < vertexData.Length; index16 += 3)
          str2 = str2 + "v " + (object) vertexData[index16] + " " + (object) vertexData[index16 + 1] + " " + (object) vertexData[index16 + 2] + Environment.NewLine;
        for (int index17 = 0; index17 < data2.Length; index17 += 2)
          str2 = str2 + "vt " + (object) data2[index17] + " " + (object) (float) ((double) data2[index17 + 1] * -1.0) + Environment.NewLine;
        for (int index18 = 0; index18 < iboHandles.Count; ++index18)
        {
          try
          {
            if (texturesGL.Count > index18)
            {
              if (texturesGL[index18] == -1)
                str2 = str2 + "usemtl none" + Environment.NewLine;
              else
                str2 = str2 + "usemtl " + (object) texturesGL[index18] + Environment.NewLine;
            }
            for (int index19 = 0; index19 < iboData[index18].Length; index19 += 3)
              str2 = str2 + string.Format("f {0}/{0} {1}/{1} {2}/{2}", (object) ((int) iboData[index18][index19] + 1), (object) ((int) iboData[index18][index19 + 1] + 1), (object) ((int) iboData[index18][index19 + 2] + 1)) + Environment.NewLine;
          }
          catch (Exception ex)
          {
          }
        }
        string str3 = "newmtl none" + Environment.NewLine;
        List<int> intList = new List<int>();
        for (int index20 = 0; index20 < texturesGL.Count; ++index20)
        {
          if (!intList.Contains(texturesGL[index20]))
          {
            str3 = str3 + "newmtl " + (object) texturesGL[index20] + Environment.NewLine + "map_Kd " + (object) texturesGL[index20] + ".png" + Environment.NewLine;
            intList.Add(texturesGL[index20]);
          }
        }
        StreamWriter streamWriter1 = new StreamWriter(str1 + "dklevel.obj");
        streamWriter1.WriteLine(str2);
        streamWriter1.Close();
        StreamWriter streamWriter2 = new StreamWriter(str1 + "dklevel.mtl");
        streamWriter2.WriteLine(str3);
        streamWriter2.Close();
        int num19 = (int) MessageBox.Show("Export Complete");
      }
      catch (Exception ex)
      {
      }
    }

    public static void GetBuffersFromDKModelFileCharacter(
      ref ModelFile file,
      ref byte[] bytesInFile,
      ref uint vboVertexHandle,
      ref float[] vertexData,
      ref uint vboColorHandle,
      ref uint vboTexCoordHandle,
      ref List<uint> iboHandles,
      ref List<ushort[]> iboData,
      ref List<int> texturesGL,
      ref List<Matrix4> matrices,
      ref List<int> mtxBuffer,
      bool exportModel)
    {
      string path = Program.EXPORT + file.fileAddress + "//";
      if (exportModel)
        Directory.CreateDirectory(path);
      mtxBuffer.Clear();
      matrices.Clear();
      List<ushort> ushortList = new List<ushort>();
      ushort[] numArray1 = new ushort[32];
      List<ModelSectionGroup> modelSectionGroupList = new List<ModelSectionGroup>();
      List<ModelSection> modelSectionList = new List<ModelSection>();
      F3DEX2 f3DeX2 = new F3DEX2();
      Texture texture = new Texture(0U, 0U, 0, 0);
      List<Texture> textureList = new List<Texture>();
      List<int> source = new List<int>();
      int num1 = 0;
      File.ReadAllBytes(".\\resources\\texturetable.bin");
      List<string> stringList = new List<string>();
      try
      {
        int f3Dstart = file.F3DStart;
        int f3Dend = file.F3DEnd;
        int f3Dcommands = file.F3DCommands;
        int vertStart = file.vertStart;
        int vtCount = file.VTCount;
        int textureCount = file.textureCount;
        F3DEX_VERT[] f3dVerts = file.f3d_verts;
        List<byte[]> commands = file.commands;
        Hashtable hashtable = new Hashtable();
        List<uint> uintList1 = new List<uint>();
        List<uint> uintList2 = new List<uint>();
        vertexData = new float[vtCount * 3];
        float[] numArray2 = new float[vtCount * 4];
        float[] numArray3 = new float[vtCount * 2];
        List<Vector3> vector3List = new List<Vector3>();
        List<float> floatList1 = new List<float>();
        List<float> floatList2 = new List<float>();
        List<Bone> boneList = new List<Bone>();
        Matrix4 i = Matrix4.I;
        for (int index1 = f3Dend + 4; index1 + 16 < bytesInFile.Length; index1 += 16)
        {
          int index2 = index1;
          short num2 = (short) bytesInFile[index2];
          byte id_ = bytesInFile[index2 + 1];
          int index3 = index2 + 4;
          float single1 = BitConverter.ToSingle(new byte[4]
          {
            bytesInFile[index3 + 3],
            bytesInFile[index3 + 2],
            bytesInFile[index3 + 1],
            bytesInFile[index3]
          }, 0);
          int index4 = index3 + 4;
          float single2 = BitConverter.ToSingle(new byte[4]
          {
            bytesInFile[index4 + 3],
            bytesInFile[index4 + 2],
            bytesInFile[index4 + 1],
            bytesInFile[index4]
          }, 0);
          int index5 = index4 + 4;
          float single3 = BitConverter.ToSingle(new byte[4]
          {
            bytesInFile[index5 + 3],
            bytesInFile[index5 + 2],
            bytesInFile[index5 + 1],
            bytesInFile[index5]
          }, 0);
          int num3 = index5 + 4;
          boneList.Add(new Bone((short) id_, (short) 0, num2, single1, single2, single3));
          float tx = single1;
          float ty = single2;
          float tz = single3;
          Matrix4 matrix4 = num2 == (short) byte.MaxValue || (int) num2 > matrices.Count ? Matrix4.I : matrices[(int) num2];
          matrices.Add(Matrix4.GetTranslationMatrix(tx, ty, tz) * matrix4);
        }
        for (int index6 = 0; index6 < f3Dcommands; ++index6)
        {
          byte[] numArray4 = commands[index6];
          int num4 = (int) numArray4[4];
          int num5 = (int) numArray4[5];
          int num6 = (int) numArray4[6];
          int num7 = (int) numArray4[7];
          int num8 = (int) numArray4[1];
          int num9 = (int) numArray4[2];
          int num10 = (int) numArray4[3];
          int num11 = (int) numArray4[0];
          if ((int) numArray4[0] == F3DEX2.F3DEX2_MTX)
          {
            if (source.Count<int>() != 0)
            {
              ushort[] array = ushortList.ToArray();
              uint buffers = 0;
              GL.GenBuffers(1, out buffers);
              GL.BindBuffer(BufferTarget.ArrayBuffer, buffers);
              GL.BufferData<ushort>(BufferTarget.ArrayBuffer, (IntPtr) (array.Length * 2), array, BufferUsageHint.StaticDraw);
              GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
              iboHandles.Add(buffers);
              iboData.Add(array);
              ushortList.Clear();
            }
            num1 = ((int) numArray4[4] << 24) + ((int) numArray4[5] << 16) + ((int) numArray4[6] << 8) + (int) numArray4[7];
            source.Add(num1);
          }
          if ((int) numArray4[0] == F3DEX2.F3DEX2_VTX)
          {
            byte[] numArray5 = commands[index6];
            int num12 = (int) numArray4[4] * 16777216 + (int) numArray4[5] * 65536 + (int) numArray4[6] * 256 + (int) numArray4[7];
            int num13 = (int) numArray4[1] * 65536 + (int) numArray4[2] * 256 + (int) numArray4[3];
            byte num14 = (byte) ((uint) num13 >> 12 & (uint) byte.MaxValue);
            byte num15 = (byte) (((uint) num13 >> 1 & (uint) sbyte.MaxValue) - (uint) num14);
            if (num15 > (byte) 63)
              num15 = (byte) 63;
            uint index7 = ((uint) (num12 << 8) >> 8) / 16U;
            try
            {
              for (int index8 = (int) num15; index8 < (int) num14 + (int) num15; ++index8)
              {
                if ((long) index7 < (long) f3dVerts.Length)
                {
                  vector3List.Add(matrices[(num1 - 67108864) / 64] * new Vector3((float) f3dVerts[(int) index7].x, (float) f3dVerts[(int) index7].y, (float) f3dVerts[(int) index7].z));
                  floatList1.Add((float) f3dVerts[(int) index7].u * texture.textureWRatio);
                  floatList1.Add((float) f3dVerts[(int) index7].v * texture.textureHRatio);
                  floatList2.Add(f3dVerts[(int) index7].r);
                  floatList2.Add(f3dVerts[(int) index7].g);
                  floatList2.Add(f3dVerts[(int) index7].b);
                  floatList2.Add(f3dVerts[(int) index7].a);
                  numArray1[index8] = (ushort) (vector3List.Count - 1);
                }
                ++index7;
              }
            }
            catch (Exception ex)
            {
            }
          }
          if ((int) numArray4[0] == F3DEX2.F3DEX2_TRI1)
          {
            short index9 = (short) ((int) numArray4[1] / 2);
            short index10 = (short) ((int) numArray4[2] / 2);
            short index11 = (short) ((int) numArray4[3] / 2);
            ushortList.Add(numArray1[(int) index9]);
            ushortList.Add(numArray1[(int) index10]);
            ushortList.Add(numArray1[(int) index11]);
          }
          if ((int) numArray4[0] == F3DEX2.F3DEX2_TRI2)
          {
            short index12 = (short) ((int) numArray4[1] / 2);
            short index13 = (short) ((int) numArray4[2] / 2);
            short index14 = (short) ((int) numArray4[3] / 2);
            ushortList.Add(numArray1[(int) index12]);
            ushortList.Add(numArray1[(int) index13]);
            ushortList.Add(numArray1[(int) index14]);
            short index15 = (short) ((int) numArray4[5] / 2);
            short index16 = (short) ((int) numArray4[6] / 2);
            short index17 = (short) ((int) numArray4[7] / 2);
            ushortList.Add(numArray1[(int) index15]);
            ushortList.Add(numArray1[(int) index16]);
            ushortList.Add(numArray1[(int) index17]);
          }
        }
        ushort[] array1 = ushortList.ToArray();
        uint buffers1 = 0;
        GL.GenBuffers(1, out buffers1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffers1);
        GL.BufferData<ushort>(BufferTarget.ArrayBuffer, (IntPtr) (array1.Length * 2), array1, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        iboHandles.Add(buffers1);
        iboData.Add(array1);
        source.Add(num1);
        ushortList.Clear();
        vertexData = new float[vector3List.Count * 3];
        int index18 = 0;
        foreach (Vector3 vector3 in vector3List)
        {
          vertexData[index18] = vector3.x;
          vertexData[index18 + 1] = vector3.y;
          vertexData[index18 + 2] = vector3.z;
          index18 += 3;
        }
        float[] array2 = floatList2.ToArray();
        float[] array3 = floatList1.ToArray();
        GL.GenBuffers(1, out vboVertexHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertexHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (vertexData.Length * 4), vertexData, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.GenBuffers(1, out vboColorHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboColorHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (array2.Length * 4), array2, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        foreach (int num16 in source)
        {
          int num17 = (num16 - 67108864) / 64;
          mtxBuffer.Add(num17);
        }
        GL.GenBuffers(1, out vboTexCoordHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboTexCoordHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (array3.Length * 4), array3, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        if (!exportModel)
          return;
        string str1 = "#Ripped by Skill" + Environment.NewLine + "mtllib dkCharacter.mtl" + Environment.NewLine;
        for (int index19 = 0; index19 < vertexData.Length; index19 += 3)
          str1 = str1 + "v " + (object) vertexData[index19] + " " + (object) vertexData[index19 + 1] + " " + (object) vertexData[index19 + 2] + Environment.NewLine;
        for (int index20 = 0; index20 < array3.Length; index20 += 2)
          str1 = str1 + "vt " + (object) array3[index20] + " " + (object) (float) ((double) array3[index20 + 1] * -1.0) + Environment.NewLine;
        for (int index21 = 0; index21 < iboHandles.Count; ++index21)
        {
          try
          {
            if (texturesGL.Count > index21)
            {
              if (texturesGL[index21] == -1)
                str1 = str1 + "usemtl none" + Environment.NewLine;
              else
                str1 = str1 + "usemtl " + (object) texturesGL[index21] + Environment.NewLine;
            }
            for (int index22 = 0; index22 < iboData[index21].Length; index22 += 3)
              str1 = str1 + string.Format("f {0}/{0} {1}/{1} {2}/{2}", (object) ((int) iboData[index21][index22] + 1), (object) ((int) iboData[index21][index22 + 1] + 1), (object) ((int) iboData[index21][index22 + 2] + 1)) + Environment.NewLine;
          }
          catch (Exception ex)
          {
          }
        }
        string str2 = "newmtl none" + Environment.NewLine;
        List<int> intList = new List<int>();
        for (int index23 = 0; index23 < texturesGL.Count; ++index23)
        {
          if (!intList.Contains(texturesGL[index23]))
          {
            str2 = str2 + "newmtl " + (object) texturesGL[index23] + Environment.NewLine + "map_Kd " + (object) texturesGL[index23] + ".png" + Environment.NewLine;
            intList.Add(texturesGL[index23]);
          }
        }
        StreamWriter streamWriter1 = new StreamWriter(path + "dkCharacter.obj");
        streamWriter1.WriteLine(str1);
        streamWriter1.Close();
        StreamWriter streamWriter2 = new StreamWriter(path + "dkCharacter.mtl");
        streamWriter2.WriteLine(str2);
        streamWriter2.Close();
        int num18 = (int) MessageBox.Show("Export Complete");
      }
      catch (Exception ex)
      {
      }
    }

    public static int CalcSectionVtxOffset(
      List<ModelSectionGroup> groups,
      List<ModelSection> sections,
      int meshIndex,
      int useNumber)
    {
      int num1 = 0;
      int num2 = 0;
      foreach (ModelSection section in sections)
      {
        if (section.meshID == meshIndex)
        {
          num1 = section.sectionID;
          if (useNumber == 0)
            num2 = section.vertStart;
          if (useNumber == 1 && section.unknownVertStart != 0)
            num2 = section.unknownVertStart;
          if (useNumber == 1 && section.unknownVertStart == 0)
            num2 = section.vertStart2;
          if (useNumber == 2)
          {
            num2 = section.vertStart3;
            break;
          }
          break;
        }
      }
      int num3 = 0;
      if (num1 > 0)
      {
        foreach (ModelSectionGroup group in groups)
        {
          if (num1 > group.sectionID)
            num3 += group.endOffset;
          else
            break;
        }
      }
      return num2 + num3;
    }

    public static void writeTexture(
      byte[] pixels,
      int width,
      int height,
      int file,
      string exportDir)
    {
      if (File.Exists(exportDir + file.ToString() + ".png"))
        return;
      byte[] source = new byte[width * height * 4];
      for (int index = 0; index < source.Length; index += 4)
      {
        source[index] = pixels[index + 2];
        source[index + 1] = pixels[index + 1];
        source[index + 2] = pixels[index];
        source[index + 3] = pixels[index + 3];
      }
      Rectangle rect = new Rectangle(0, 0, width, height);
      using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
      {
        try
        {
          BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
          IntPtr scan0 = bitmapdata.Scan0;
          Marshal.Copy(source, 0, scan0, source.Length);
          bitmap.UnlockBits(bitmapdata);
          bitmap.Save(exportDir + file.ToString() + ".png", ImageFormat.Png);
        }
        catch
        {
        }
        bitmap.Dispose();
      }
    }

    private static int GetSectionIndex(List<ModelSection> s, int n)
    {
      for (int index = 0; index < s.Count; ++index)
      {
        if (s[index].meshID == n)
          return index;
      }
      return 0;
    }
  }
}
