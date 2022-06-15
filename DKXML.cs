// Decompiled with JetBrains decompiler
// Type: DK64Viewer.DKXML
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using System;
using System.Collections.Generic;
using System.Xml;

namespace DK64Viewer
{
  public class DKXML
  {
    public static List<RomFile> ReadModelsXML()
    {
      List<RomFile> romFileList = new List<RomFile>();
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(".\\resources\\models.xml");
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.Name == "model")
          {
            string name = xmlTextReader.GetAttribute("name") == null ? "" : xmlTextReader.GetAttribute("name");
            string file = xmlTextReader.GetAttribute("file") == null ? "" : xmlTextReader.GetAttribute("file");
            romFileList.Add(new RomFile(name, file));
          }
        }
      }
      catch (Exception ex)
      {
      }
      return romFileList;
    }
  }
}
