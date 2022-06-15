// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Program
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System;
using System.Windows.Forms;

namespace DK64Viewer
{
  internal static class Program
  {
    public static string TMP = Application.StartupPath + "\\tmp\\";
    public static string EXPORT = Application.StartupPath + "\\export\\";

    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}
