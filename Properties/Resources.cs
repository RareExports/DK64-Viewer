// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Properties.Resources
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DK64Viewer.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (DK64Viewer.Properties.Resources.resourceMan == null)
          DK64Viewer.Properties.Resources.resourceMan = new ResourceManager("DK64Viewer.Properties.Resources", typeof (DK64Viewer.Properties.Resources).Assembly);
        return DK64Viewer.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => DK64Viewer.Properties.Resources.resourceCulture;
      set => DK64Viewer.Properties.Resources.resourceCulture = value;
    }
  }
}
