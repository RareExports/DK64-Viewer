// Decompiled with JetBrains decompiler
// Type: DK64Viewer.Form1
// Assembly: DK64Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34C2999C-2061-412A-B2F3-E6D2C8F1D38B
// Assembly location: F:\DKViewer_0.04a\DK64Viewer.exe

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DK64Viewer
{
  public class Form1 : Form
  {
    private ModelFile modelFile;
    private List<string> mapHelper = new List<string>();
    private string dir = "";
    private string currentFile = "";
    private uint vboVertexHandle;
    private uint vboColorHandle;
    private uint vboTexCoordHandle;
    private List<uint> iboHandles = new List<uint>();
    private List<ushort[]> iboData = new List<ushort[]>();
    private List<int> textures = new List<int>();
    private float[] vertexData = new float[1];
    private List<Matrix4> matrices = new List<Matrix4>();
    private List<int> mtxBuffer = new List<int>();
    private float xrot;
    private float yrot;
    private float zrot;
    private float finalx;
    private float finaly;
    private float finalz = -5000f;
    private bool mouseDown;
    private bool zoomIn;
    private bool zoomOut;
    private bool left;
    private bool right;
    private GLCamera camera = new GLCamera();
    private int oldMouseX;
    private int oldMouseY;
    private int v2;
    private bool dkViewerTextUpdate;
    private IContainer components;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private FolderBrowserDialog folderBrowserDialog1;
    private DataGridView dgv_files;
    private Timer timer1;
    private Label label1;
    private Label lbl_fileName;
    private SaveFileDialog saveFileDialog1;
    private TrackBar tb_speedBar;
    private Label label3;
    private GLControl DKOpenGLC;
    private Label label4;
    private ToolStripMenuItem exportToGEOBJToolStripMenuItem;
    private FolderBrowserDialog folderBrowserDialog;
    private ToolStripMenuItem viewToolStripMenuItem;
    private ToolStripMenuItem lookAtModelToolStripMenuItem;
    private ToolStripMenuItem openRomToolStripMenuItem;
    private OpenFileDialog openFileDialog1;
    private Panel panel1;
    private Label label8;
    private TextBox tb_camZPos;
    private Label label7;
    private TextBox tb_camYPos;
    private Label label6;
    private TextBox tb_camXPos;
    private Label label5;
    private Label label10;
    private TextBox tb_camYRot;
    private Label label11;
    private TextBox tb_camXRot;

    public Form1() => this.InitializeComponent();

    private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.dir = this.folderBrowserDialog1.SelectedPath + "\\";
      this.dgv_files.Rows.Clear();
      this.dgv_files.Columns.Clear();
      DataGridViewColumnCollection columns = this.dgv_files.Columns;
      DataGridViewTextBoxColumn viewTextBoxColumn = new DataGridViewTextBoxColumn();
      viewTextBoxColumn.HeaderText = "File";
      viewTextBoxColumn.ReadOnly = true;
      viewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      viewTextBoxColumn.FillWeight = 25f;
      columns.Add((DataGridViewColumn) viewTextBoxColumn);
      foreach (string file in Directory.GetFiles(this.dir))
      {
        if (file.EndsWith(".bin"))
          this.dgv_files.Rows.Add(new object[1]
          {
            (object) Path.GetFileName(file)
          });
      }
    }

    private void ProcessFile()
    {
      try
      {
        Core.DeleteBuffer(this.vboVertexHandle, false);
        Core.DeleteBuffer(this.vboColorHandle, false);
        Core.DeleteBuffer(this.vboTexCoordHandle, false);
        Core.DeleteBuffers(this.iboHandles, false);
        Core.DeleteTextures(this.textures);
        this.iboData.Clear();
        this.iboHandles.Clear();
        this.textures.Clear();
        this.mtxBuffer.Clear();
        this.matrices.Clear();
        RomHandler.DecompressFileToHDD(Convert.ToInt32(this.currentFile, 16));
        byte[] bytesInFile = File.ReadAllBytes(Program.TMP + this.currentFile);
        this.modelFile = Core.ReadModel(ref bytesInFile);
        this.modelFile.fileAddress = this.currentFile;
        if (this.modelFile.FileType == ModelFileType.Character)
          Core.GetBuffersFromDKModelFileCharacter(ref this.modelFile, ref bytesInFile, ref this.vboVertexHandle, ref this.vertexData, ref this.vboColorHandle, ref this.vboTexCoordHandle, ref this.iboHandles, ref this.iboData, ref this.textures, ref this.matrices, ref this.mtxBuffer, false);
        else
          Core.GetBuffersFromDKModelFile(ref this.modelFile, ref bytesInFile, ref this.vboVertexHandle, ref this.vertexData, ref this.vboColorHandle, ref this.vboTexCoordHandle, ref this.iboHandles, ref this.iboData, ref this.textures, false);
        this.lbl_fileName.Text = this.modelFile.fileName;
        if (this.lookAtModelToolStripMenuItem.Checked)
          this.camera.LookAt(new Vector3(this.vertexData[0], this.vertexData[1], this.vertexData[2]));
        this.Draw();
      }
      catch
      {
      }
    }

    private void Draw()
    {
      try
      {
        Core.ClearScreen();
        GL.PushMatrix();
        GL.LoadMatrix(this.camera.GetWorldToViewMatrix());
        GL.EnableClientState(EnableCap.VertexArray);
        GL.EnableClientState(EnableCap.ColorArray);
        GL.EnableClientState(EnableCap.TextureCoordArray);
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vboVertexHandle);
        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr) (this.vertexData.Length * 4), this.vertexData, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vboVertexHandle);
        GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vboColorHandle);
        GL.ColorPointer(4, ColorPointerType.Float, 0, IntPtr.Zero);
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vboTexCoordHandle);
        GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
        for (int index = 0; index < this.iboHandles.Count; ++index)
        {
          GL.Disable(EnableCap.Texture2D);
          if (this.textures.Count > index)
          {
            if (this.textures[index] == -1)
            {
              GL.Disable(EnableCap.Texture2D);
            }
            else
            {
              GL.Enable(EnableCap.Texture2D);
              GL.BindTexture(TextureTarget.Texture2D, this.textures[index]);
            }
          }
          if (this.modelFile.FileType == ModelFileType.Character)
          {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.iboHandles[index]);
            GL.DrawElements(PrimitiveType.Triangles, ((IEnumerable<ushort>) this.iboData[index]).Count<ushort>(), DrawElementsType.UnsignedShort, IntPtr.Zero);
          }
          else
          {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.iboHandles[index]);
            GL.DrawElements(PrimitiveType.Triangles, ((IEnumerable<ushort>) this.iboData[index]).Count<ushort>(), DrawElementsType.UnsignedShort, IntPtr.Zero);
          }
        }
        foreach (Matrix4 matrix in this.matrices)
        {
          GL.PushMatrix();
          GL.Translate(matrix.matrix[0, 3], matrix.matrix[1, 3], matrix.matrix[2, 3]);
          Core.DrawCube(-2, -2, -2, 2, 2, 2);
          GL.PopMatrix();
        }
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.DisableClientState(EnableCap.VertexArray);
        GL.DisableClientState(EnableCap.ColorArray);
        GL.DisableClientState(EnableCap.TextureCoordArray);
        Core.PopMatrix();
        this.DKOpenGLC.SwapBuffers();
        this.UpdateCameraLocationTextBoxes();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.DisableClientState(EnableCap.VertexArray);
        GL.DisableClientState(EnableCap.ColorArray);
        GL.DisableClientState(EnableCap.TextureCoordArray);
        Core.PopMatrix();
        this.DKOpenGLC.SwapBuffers();
      }
    }

    private void UpdateCameraLocationTextBoxes()
    {
      this.dkViewerTextUpdate = true;
      this.tb_camXPos.Text = (-this.camera.Position.x).ToString();
      this.tb_camYPos.Text = (-this.camera.Position.y).ToString();
      this.tb_camZPos.Text = (-this.camera.Position.z).ToString();
      Vector3 rotationDegrees = this.camera.RotationDegrees;
      this.tb_camXRot.Text = (-rotationDegrees.x).ToString();
      this.tb_camYRot.Text = (-rotationDegrees.y).ToString();
      this.dkViewerTextUpdate = false;
    }

    private void UpdateCameraLocationFromTextBoxes()
    {
      try
      {
        if (this.dkViewerTextUpdate)
          return;
        this.camera.SetView(new Vector3(-Convert.ToSingle(this.tb_camXPos.Text), -Convert.ToSingle(this.tb_camYPos.Text), -Convert.ToSingle(this.tb_camZPos.Text)), new Vector3(-Convert.ToSingle(this.tb_camXRot.Text), -Convert.ToSingle(this.tb_camYRot.Text), 0.0f));
        this.Draw();
      }
      catch
      {
      }
    }

    private void CameraTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
        e.Handled = true;
      if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
        e.Handled = true;
      if (e.KeyChar != '-' || (sender as TextBox).Text.IndexOf('-') <= -1)
        return;
      e.Handled = true;
    }

    private void dgv_files_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgv_files.SelectedCells.Count <= 0)
        return;
      this.currentFile = this.dgv_files.SelectedCells[1].Value.ToString();
      this.ProcessFile();
    }

    private void DKOpenGLC_MouseDown(object sender, MouseEventArgs e) => this.mouseDown = true;

    private void DKOpenGLC_MouseUp(object sender, MouseEventArgs e) => this.mouseDown = false;

    private void DKOpenGLC_MouseMove(object sender, MouseEventArgs e)
    {
      int mouseDeltaX = e.X - this.oldMouseX;
      int mouseDeltaY = e.Y - this.oldMouseY;
      if (this.mouseDown)
      {
        this.camera.MouseUpdate(mouseDeltaX, mouseDeltaY);
        this.Draw();
      }
      this.oldMouseX = e.X;
      this.oldMouseY = e.Y;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (!(this.zoomIn | this.zoomOut | this.left | this.right))
        return;
      this.camera.PanUpdate(this.zoomIn, this.zoomOut, this.left, this.right);
      this.Draw();
    }

    private void DKOpenGLC_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.A:
          this.right = true;
          break;
        case Keys.D:
          this.left = true;
          break;
        case Keys.S:
          this.zoomOut = true;
          break;
        case Keys.W:
          this.zoomIn = true;
          break;
      }
    }

    private void DKOpenGLC_KeyUp(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.A:
          this.right = false;
          break;
        case Keys.D:
          this.left = false;
          break;
        case Keys.S:
          this.zoomOut = false;
          break;
        case Keys.W:
          this.zoomIn = false;
          break;
      }
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
    }

    private void openRomToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
      //   return;
      // RomHandler.Rom = File.ReadAllBytes(this.openFileDialog1.FileName);
      this.dgv_files.Rows.Clear();
      this.dgv_files.Columns.Clear();
      DataGridViewColumnCollection columns1 = this.dgv_files.Columns;
      DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn1.HeaderText = "Name";
      viewTextBoxColumn1.ReadOnly = true;
      viewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      viewTextBoxColumn1.FillWeight = 25f;
      columns1.Add((DataGridViewColumn) viewTextBoxColumn1);
      DataGridViewColumnCollection columns2 = this.dgv_files.Columns;
      DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn2.HeaderText = "File";
      viewTextBoxColumn2.ReadOnly = true;
      viewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      viewTextBoxColumn2.FillWeight = 25f;
      columns2.Add((DataGridViewColumn) viewTextBoxColumn2);
      foreach (RomFile romFile in DKXML.ReadModelsXML())
        this.dgv_files.Rows.Add((object) romFile.name, (object) romFile.file);
    }

    private void Form1_ResizeEnd(object sender, EventArgs e)
    {
      this.Draw();
      this.Refresh();
    }

    private void Camera_TextChanged(object sender, EventArgs e)
    {
      if ((sender as TextBox).Text.EndsWith("."))
        return;
      this.UpdateCameraLocationFromTextBoxes();
    }

    private void label5_Click(object sender, EventArgs e)
    {
    }

    private void writeMapNotesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      File.WriteAllLines(this.saveFileDialog1.FileName, this.mapHelper.ToArray());
    }

    private void tb_speedBar_ValueChanged(object sender, EventArgs e) => this.camera.movementSpeed = (float) (this.tb_speedBar.Value * 2);

    private void DKOpenGLC_Load(object sender, EventArgs e)
    {
      this.DKOpenGLC.MakeCurrent();
      Core.InitGl();
      Core.SetView(((Control) this.DKOpenGLC).Height, ((Control) this.DKOpenGLC).Width);
      this.timer1.Enabled = true;
    }

    private void exportToGEOBJToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.iboData.Count <= 0)
        return;
      Directory.CreateDirectory(Program.EXPORT + "\\" + this.currentFile + "\\");
      RomHandler.DecompressFileToHDD(Convert.ToInt32(this.currentFile, 16));
      byte[] bytesInFile = File.ReadAllBytes(Program.TMP + this.currentFile);
      this.modelFile = Core.ReadModel(ref bytesInFile);
      this.modelFile.fileAddress = this.currentFile;
      if (this.modelFile.FileType == ModelFileType.Character)
        Core.GetBuffersFromDKModelFileCharacter(ref this.modelFile, ref bytesInFile, ref this.vboVertexHandle, ref this.vertexData, ref this.vboColorHandle, ref this.vboTexCoordHandle, ref this.iboHandles, ref this.iboData, ref this.textures, ref this.matrices, ref this.mtxBuffer, true);
      else
        Core.GetBuffersFromDKModelFile(ref this.modelFile, ref bytesInFile, ref this.vboVertexHandle, ref this.vertexData, ref this.vboColorHandle, ref this.vboTexCoordHandle, ref this.iboHandles, ref this.iboData, ref this.textures, true);
    }

    private void lookAtModelToolStripMenuItem_Click(object sender, EventArgs e) => this.lookAtModelToolStripMenuItem.Checked = !this.lookAtModelToolStripMenuItem.Checked;

    private void DKOpenGLC_SizeChanged(object sender, EventArgs e)
    {
      if (!this.timer1.Enabled)
        return;
      this.Refresh();
      Core.SetView(((Control) this.DKOpenGLC).Height, ((Control) this.DKOpenGLC).Width);
      this.Draw();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      openRomToolStripMenuItem_Click(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.menuStrip1 = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.openRomToolStripMenuItem = new ToolStripMenuItem();
      this.exportToGEOBJToolStripMenuItem = new ToolStripMenuItem();
      this.viewToolStripMenuItem = new ToolStripMenuItem();
      this.lookAtModelToolStripMenuItem = new ToolStripMenuItem();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.dgv_files = new DataGridView();
      this.timer1 = new Timer(this.components);
      this.label1 = new Label();
      this.lbl_fileName = new Label();
      this.saveFileDialog1 = new SaveFileDialog();
      this.tb_speedBar = new TrackBar();
      this.label3 = new Label();
      this.DKOpenGLC = new GLControl();
      this.label4 = new Label();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.openFileDialog1 = new OpenFileDialog();
      this.panel1 = new Panel();
      this.label10 = new Label();
      this.tb_camYRot = new TextBox();
      this.label11 = new Label();
      this.tb_camXRot = new TextBox();
      this.label8 = new Label();
      this.tb_camZPos = new TextBox();
      this.label7 = new Label();
      this.tb_camYPos = new TextBox();
      this.label6 = new Label();
      this.tb_camXPos = new TextBox();
      this.label5 = new Label();
      this.menuStrip1.SuspendLayout();
      ((ISupportInitialize) this.dgv_files).BeginInit();
      this.tb_speedBar.BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip1.BackColor = Color.DarkGray;
      this.menuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.viewToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(1021, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.openRomToolStripMenuItem,
        (ToolStripItem) this.exportToGEOBJToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      this.openRomToolStripMenuItem.Name = "openRomToolStripMenuItem";
      this.openRomToolStripMenuItem.Size = new Size(158, 22);
      this.openRomToolStripMenuItem.Text = "Open Rom";
      this.openRomToolStripMenuItem.Click += new EventHandler(this.openRomToolStripMenuItem_Click);
      this.exportToGEOBJToolStripMenuItem.Name = "exportToGEOBJToolStripMenuItem";
      this.exportToGEOBJToolStripMenuItem.Size = new Size(158, 22);
      this.exportToGEOBJToolStripMenuItem.Text = "Export to GEOBJ";
      this.exportToGEOBJToolStripMenuItem.Click += new EventHandler(this.exportToGEOBJToolStripMenuItem_Click);
      this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.lookAtModelToolStripMenuItem
      });
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new Size(44, 20);
      this.viewToolStripMenuItem.Text = "View";
      this.lookAtModelToolStripMenuItem.Checked = true;
      this.lookAtModelToolStripMenuItem.CheckState = CheckState.Checked;
      this.lookAtModelToolStripMenuItem.Name = "lookAtModelToolStripMenuItem";
      this.lookAtModelToolStripMenuItem.Size = new Size(150, 22);
      this.lookAtModelToolStripMenuItem.Text = "Look at Model";
      this.lookAtModelToolStripMenuItem.Click += new EventHandler(this.lookAtModelToolStripMenuItem_Click);
      this.dgv_files.AllowUserToAddRows = false;
      this.dgv_files.AllowUserToDeleteRows = false;
      this.dgv_files.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.dgv_files.BackgroundColor = Color.White;
      this.dgv_files.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_files.Location = new Point(0, 27);
      this.dgv_files.Name = "dgv_files";
      this.dgv_files.ReadOnly = true;
      this.dgv_files.RowHeadersVisible = false;
      this.dgv_files.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgv_files.Size = new Size(233, 352);
      this.dgv_files.TabIndex = 2;
      this.dgv_files.SelectionChanged += new EventHandler(this.dgv_files_SelectionChanged);
      this.timer1.Interval = 16;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(240, 467);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Internal Name";
      this.lbl_fileName.AutoSize = true;
      this.lbl_fileName.Location = new Point(319, 467);
      this.lbl_fileName.Name = "lbl_fileName";
      this.lbl_fileName.Size = new Size(0, 13);
      this.lbl_fileName.TabIndex = 4;
      this.tb_speedBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.tb_speedBar.AutoSize = false;
      this.tb_speedBar.BackColor = Color.DarkGray;
      this.tb_speedBar.Location = new Point(863, 4);
      this.tb_speedBar.Margin = new Padding(0);
      this.tb_speedBar.Maximum = 50;
      this.tb_speedBar.Name = "tb_speedBar";
      this.tb_speedBar.Size = new Size(150, 20);
      this.tb_speedBar.TabIndex = 9;
      this.tb_speedBar.TickFrequency = 5;
      this.tb_speedBar.Value = 50;
      this.tb_speedBar.ValueChanged += new EventHandler(this.tb_speedBar_ValueChanged);
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.DarkGray;
      this.label3.Location = new Point(783, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(77, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Camera Speed";
      ((Control) this.DKOpenGLC).Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      ((Control) this.DKOpenGLC).BackColor = Color.Black;
      ((Control) this.DKOpenGLC).Location = new Point(236, 27);
      ((Control) this.DKOpenGLC).Name = "DKOpenGLC";
      ((Control) this.DKOpenGLC).Size = new Size(773, 429);
      ((Control) this.DKOpenGLC).TabIndex = 11;
      this.DKOpenGLC.VSync = false;
      ((UserControl) this.DKOpenGLC).Load += new EventHandler(this.DKOpenGLC_Load);
      ((Control) this.DKOpenGLC).SizeChanged += new EventHandler(this.DKOpenGLC_SizeChanged);
      ((Control) this.DKOpenGLC).KeyDown += new KeyEventHandler(this.DKOpenGLC_KeyDown);
      ((Control) this.DKOpenGLC).KeyUp += new KeyEventHandler(this.DKOpenGLC_KeyUp);
      ((Control) this.DKOpenGLC).MouseDown += new MouseEventHandler(this.DKOpenGLC_MouseDown);
      ((Control) this.DKOpenGLC).MouseMove += new MouseEventHandler(this.DKOpenGLC_MouseMove);
      ((Control) this.DKOpenGLC).MouseUp += new MouseEventHandler(this.DKOpenGLC_MouseUp);
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(799, 467);
      this.label4.Name = "label4";
      this.label4.Size = new Size(210, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "Created by Skill from BanjosBackpack.com";
      this.openFileDialog1.FileName = "dk64.z64";
      this.openFileDialog1.Filter = "Z64 files|*.z64";
      this.panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel1.BackColor = Color.DarkGray;
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.tb_camYRot);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.tb_camXRot);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.tb_camZPos);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.tb_camYPos);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.tb_camXPos);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Location = new Point(0, 385);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(233, 98);
      this.panel1.TabIndex = 13;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(105, 54);
      this.label10.Name = "label10";
      this.label10.Size = new Size(28, 13);
      this.label10.TabIndex = 11;
      this.label10.Text = "Yaw";
      this.tb_camYRot.Location = new Point(139, 51);
      this.tb_camYRot.Name = "tb_camYRot";
      this.tb_camYRot.Size = new Size(50, 20);
      this.tb_camYRot.TabIndex = 10;
      this.tb_camYRot.TextChanged += new EventHandler(this.Camera_TextChanged);
      this.tb_camYRot.KeyPress += new KeyPressEventHandler(this.CameraTextBox_KeyPress);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 54);
      this.label11.Name = "label11";
      this.label11.Size = new Size(31, 13);
      this.label11.TabIndex = 9;
      this.label11.Text = "Pitch";
      this.tb_camXRot.Location = new Point(49, 51);
      this.tb_camXRot.Name = "tb_camXRot";
      this.tb_camXRot.Size = new Size(50, 20);
      this.tb_camXRot.TabIndex = 8;
      this.tb_camXRot.TextChanged += new EventHandler(this.Camera_TextChanged);
      this.tb_camXRot.KeyPress += new KeyPressEventHandler(this.CameraTextBox_KeyPress);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(147, 29);
      this.label8.Name = "label8";
      this.label8.Size = new Size(14, 13);
      this.label8.TabIndex = 6;
      this.label8.Text = "Z";
      this.tb_camZPos.Location = new Point(165, 25);
      this.tb_camZPos.Name = "tb_camZPos";
      this.tb_camZPos.Size = new Size(50, 20);
      this.tb_camZPos.TabIndex = 5;
      this.tb_camZPos.TextChanged += new EventHandler(this.Camera_TextChanged);
      this.tb_camZPos.KeyPress += new KeyPressEventHandler(this.CameraTextBox_KeyPress);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(79, 28);
      this.label7.Name = "label7";
      this.label7.Size = new Size(14, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "Y";
      this.tb_camYPos.Location = new Point(95, 25);
      this.tb_camYPos.Name = "tb_camYPos";
      this.tb_camYPos.Size = new Size(50, 20);
      this.tb_camYPos.TabIndex = 3;
      this.tb_camYPos.TextChanged += new EventHandler(this.Camera_TextChanged);
      this.tb_camYPos.KeyPress += new KeyPressEventHandler(this.CameraTextBox_KeyPress);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 29);
      this.label6.Name = "label6";
      this.label6.Size = new Size(14, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "X";
      this.tb_camXPos.Location = new Point(27, 25);
      this.tb_camXPos.Name = "tb_camXPos";
      this.tb_camXPos.Size = new Size(50, 20);
      this.tb_camXPos.TabIndex = 1;
      this.tb_camXPos.TextChanged += new EventHandler(this.Camera_TextChanged);
      this.tb_camXPos.KeyPress += new KeyPressEventHandler(this.CameraTextBox_KeyPress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 9);
      this.label5.Name = "label5";
      this.label5.Size = new Size(93, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Camera Properties";
      this.label5.Click += new EventHandler(this.label5_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1021, 486);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.DKOpenGLC);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.tb_speedBar);
      this.Controls.Add((Control) this.lbl_fileName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dgv_files);
      this.Controls.Add((Control) this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form1);
      this.ShowIcon = false;
      this.Text = "DK64 Viewer";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.SizeChanged += new EventHandler(this.Form1_ResizeEnd);
      this.MouseMove += new MouseEventHandler(this.Form1_MouseMove);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((ISupportInitialize) this.dgv_files).EndInit();
      this.tb_speedBar.EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
