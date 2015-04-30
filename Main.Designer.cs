namespace Raytracer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.renderedImage = new System.Windows.Forms.PictureBox();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mnuFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAvailableFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renderOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiThreadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRenderingMode = new System.Windows.Forms.ToolStripMenuItem();
            this.progressiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highQualityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jitteredSamplerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.componentEdgeDetectionSamplerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSuperSampling = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.renderAntialiasingSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShadows = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReflections = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRefractions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRenderDepth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveBmp = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSceneFile = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCancelRendering = new System.Windows.Forms.Button();
            this.btnRender = new System.Windows.Forms.Button();
            this.dlgSaveRay = new System.Windows.Forms.SaveFileDialog();
            this.pixelPosition = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.renderedImage)).BeginInit();
            this.mainMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderedImage
            // 
            this.renderedImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderedImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderedImage.Location = new System.Drawing.Point(0, 0);
            this.renderedImage.Name = "renderedImage";
            this.renderedImage.Size = new System.Drawing.Size(365, 407);
            this.renderedImage.TabIndex = 1;
            this.renderedImage.TabStop = false;
            this.renderedImage.Click += new System.EventHandler(this.renderedImage_Click);
            this.renderedImage.DoubleClick += new System.EventHandler(this.renderedImage_DoubleClick);
            this.renderedImage.MouseLeave += new System.EventHandler(this.renderedImage_MouseLeave);
            this.renderedImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderedImage_MouseMove);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFiles,
            this.renderOptionsToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(569, 24);
            this.mainMenu.TabIndex = 2;
            this.mainMenu.Text = "menuStrip1";
            // 
            // mnuFiles
            // 
            this.mnuFiles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.saveSceneToolStripMenuItem1,
            this.saveSceneToolStripMenuItem,
            this.mnuSave,
            this.mnuAvailableFiles,
            this.toolStripMenuItem1});
            this.mnuFiles.Name = "mnuFiles";
            this.mnuFiles.Size = new System.Drawing.Size(37, 20);
            this.mnuFiles.Text = "File";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(196, 22);
            this.mnuOpen.Text = "Open Scene...";
            this.mnuOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveSceneToolStripMenuItem1
            // 
            this.saveSceneToolStripMenuItem1.Name = "saveSceneToolStripMenuItem1";
            this.saveSceneToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.saveSceneToolStripMenuItem1.Text = "Save Scene";
            this.saveSceneToolStripMenuItem1.Click += new System.EventHandler(this.saveSceneToolStripMenuItem1_Click);
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.saveSceneToolStripMenuItem.Text = "Save Scene As...";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(196, 22);
            this.mnuSave.Text = "Save Rendered Image...";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuAvailableFiles
            // 
            this.mnuAvailableFiles.Name = "mnuAvailableFiles";
            this.mnuAvailableFiles.Size = new System.Drawing.Size(196, 22);
            this.mnuAvailableFiles.Text = "Available Files";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            // 
            // renderOptionsToolStripMenuItem
            // 
            this.renderOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiThreadedToolStripMenuItem,
            this.mnuRenderingMode,
            this.mnuSuperSampling,
            this.mnuShadows,
            this.mnuReflections,
            this.mnuRefractions,
            this.mnuRenderDepth,
            this.renderToolStripMenuItem});
            this.renderOptionsToolStripMenuItem.Name = "renderOptionsToolStripMenuItem";
            this.renderOptionsToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.renderOptionsToolStripMenuItem.Text = "Render Options";
            // 
            // multiThreadedToolStripMenuItem
            // 
            this.multiThreadedToolStripMenuItem.Checked = true;
            this.multiThreadedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.multiThreadedToolStripMenuItem.Name = "multiThreadedToolStripMenuItem";
            this.multiThreadedToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.multiThreadedToolStripMenuItem.Text = "Multi-Threaded?";
            this.multiThreadedToolStripMenuItem.Click += new System.EventHandler(this.multiThreadedToolStripMenuItem_Click);
            // 
            // mnuRenderingMode
            // 
            this.mnuRenderingMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressiveToolStripMenuItem,
            this.highQualityToolStripMenuItem});
            this.mnuRenderingMode.Name = "mnuRenderingMode";
            this.mnuRenderingMode.Size = new System.Drawing.Size(162, 22);
            this.mnuRenderingMode.Text = "Rendering Mode";
            // 
            // progressiveToolStripMenuItem
            // 
            this.progressiveToolStripMenuItem.Checked = true;
            this.progressiveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.progressiveToolStripMenuItem.Name = "progressiveToolStripMenuItem";
            this.progressiveToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.progressiveToolStripMenuItem.Text = "Progressive";
            this.progressiveToolStripMenuItem.Click += new System.EventHandler(this.progressiveToolStripMenuItem_Click);
            // 
            // highQualityToolStripMenuItem
            // 
            this.highQualityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jitteredSamplerToolStripMenuItem,
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem,
            this.componentEdgeDetectionSamplerToolStripMenuItem});
            this.highQualityToolStripMenuItem.Name = "highQualityToolStripMenuItem";
            this.highQualityToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.highQualityToolStripMenuItem.Text = "High Quality";
            this.highQualityToolStripMenuItem.Click += new System.EventHandler(this.progressiveToolStripMenuItem_Click);
            // 
            // jitteredSamplerToolStripMenuItem
            // 
            this.jitteredSamplerToolStripMenuItem.Name = "jitteredSamplerToolStripMenuItem";
            this.jitteredSamplerToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.jitteredSamplerToolStripMenuItem.Tag = "Jittered";
            this.jitteredSamplerToolStripMenuItem.Text = "Jittered Sampler";
            this.jitteredSamplerToolStripMenuItem.Click += new System.EventHandler(this.jitteredSamplerToolStripMenuItem_Click);
            // 
            // greyscaleEdgeDetectionSamplerToolStripMenuItem
            // 
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Checked = true;
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Name = "greyscaleEdgeDetectionSamplerToolStripMenuItem";
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Tag = "GreyscaleEdgeDetection";
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Text = "Greyscale Edge Detection Sampler";
            this.greyscaleEdgeDetectionSamplerToolStripMenuItem.Click += new System.EventHandler(this.jitteredSamplerToolStripMenuItem_Click);
            // 
            // componentEdgeDetectionSamplerToolStripMenuItem
            // 
            this.componentEdgeDetectionSamplerToolStripMenuItem.Name = "componentEdgeDetectionSamplerToolStripMenuItem";
            this.componentEdgeDetectionSamplerToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.componentEdgeDetectionSamplerToolStripMenuItem.Tag = "ComponentEdgeDetection";
            this.componentEdgeDetectionSamplerToolStripMenuItem.Text = "Component Edge Detection Sampler";
            this.componentEdgeDetectionSamplerToolStripMenuItem.Click += new System.EventHandler(this.jitteredSamplerToolStripMenuItem_Click);
            // 
            // mnuSuperSampling
            // 
            this.mnuSuperSampling.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.xToolStripMenuItem2,
            this.xToolStripMenuItem3,
            this.renderAntialiasingSamplesToolStripMenuItem});
            this.mnuSuperSampling.Name = "mnuSuperSampling";
            this.mnuSuperSampling.Size = new System.Drawing.Size(162, 22);
            this.mnuSuperSampling.Text = "Anti-Aliasing?";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Checked = true;
            this.xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.xToolStripMenuItem.Tag = "1";
            this.xToolStripMenuItem.Text = "1x";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.mnuSuperSampling_Click);
            // 
            // xToolStripMenuItem1
            // 
            this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            this.xToolStripMenuItem1.Size = new System.Drawing.Size(228, 22);
            this.xToolStripMenuItem1.Tag = "2";
            this.xToolStripMenuItem1.Text = "2x";
            this.xToolStripMenuItem1.Click += new System.EventHandler(this.mnuSuperSampling_Click);
            // 
            // xToolStripMenuItem2
            // 
            this.xToolStripMenuItem2.Name = "xToolStripMenuItem2";
            this.xToolStripMenuItem2.Size = new System.Drawing.Size(228, 22);
            this.xToolStripMenuItem2.Tag = "4";
            this.xToolStripMenuItem2.Text = "4x";
            this.xToolStripMenuItem2.Click += new System.EventHandler(this.mnuSuperSampling_Click);
            // 
            // xToolStripMenuItem3
            // 
            this.xToolStripMenuItem3.Name = "xToolStripMenuItem3";
            this.xToolStripMenuItem3.Size = new System.Drawing.Size(228, 22);
            this.xToolStripMenuItem3.Tag = "8";
            this.xToolStripMenuItem3.Text = "8x";
            this.xToolStripMenuItem3.Click += new System.EventHandler(this.mnuSuperSampling_Click);
            // 
            // renderAntialiasingSamplesToolStripMenuItem
            // 
            this.renderAntialiasingSamplesToolStripMenuItem.Name = "renderAntialiasingSamplesToolStripMenuItem";
            this.renderAntialiasingSamplesToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.renderAntialiasingSamplesToolStripMenuItem.Text = "Render Antialiasing Samples?";
            this.renderAntialiasingSamplesToolStripMenuItem.Click += new System.EventHandler(this.renderAntialiasingSamplesToolStripMenuItem_Click);
            // 
            // mnuShadows
            // 
            this.mnuShadows.Name = "mnuShadows";
            this.mnuShadows.Size = new System.Drawing.Size(162, 22);
            this.mnuShadows.Text = "Shadows?";
            this.mnuShadows.Click += new System.EventHandler(this.mnuShadows_Click);
            // 
            // mnuReflections
            // 
            this.mnuReflections.Name = "mnuReflections";
            this.mnuReflections.Size = new System.Drawing.Size(162, 22);
            this.mnuReflections.Text = "Reflections?";
            this.mnuReflections.Click += new System.EventHandler(this.mnuReflections_Click);
            // 
            // mnuRefractions
            // 
            this.mnuRefractions.Name = "mnuRefractions";
            this.mnuRefractions.Size = new System.Drawing.Size(162, 22);
            this.mnuRefractions.Text = "Refractions?";
            this.mnuRefractions.Click += new System.EventHandler(this.mnuRefractions_Click);
            // 
            // mnuRenderDepth
            // 
            this.mnuRenderDepth.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripMenuItem13,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem14,
            this.toolStripMenuItem15,
            this.toolStripMenuItem16});
            this.mnuRenderDepth.Name = "mnuRenderDepth";
            this.mnuRenderDepth.Size = new System.Drawing.Size(162, 22);
            this.mnuRenderDepth.Text = "Recursion Depth";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem6.Text = "1";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem7.Text = "2";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Checked = true;
            this.toolStripMenuItem8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem8.Text = "3";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem9.Text = "4";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem10.Text = "5";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem11.Text = "6";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem12.Text = "7";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem13.Text = "8";
            this.toolStripMenuItem13.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem2.Text = "9";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem3.Text = "10";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem4.Text = "11";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem5.Text = "12";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem14.Text = "13";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem15.Text = "14";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(86, 22);
            this.toolStripMenuItem16.Text = "15";
            // 
            // renderToolStripMenuItem
            // 
            this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            this.renderToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.renderToolStripMenuItem.Text = "Render";
            this.renderToolStripMenuItem.Click += new System.EventHandler(this.renderToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMessages);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 407);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rendering Messages";
            // 
            // txtMessages
            // 
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(3, 16);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(359, 59);
            this.txtMessages.TabIndex = 0;
            // 
            // dlgOpen
            // 
            this.dlgOpen.DefaultExt = "*.ray";
            // 
            // dlgSaveBmp
            // 
            this.dlgSaveBmp.Filter = "\"Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpeg)|*.jpeg|GIF files (*.gif)|*.gif|Al" +
    "l files (*.*)|*.*\"  ;";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.renderedImage);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtSceneFile);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(569, 485);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 4;
            // 
            // txtSceneFile
            // 
            this.txtSceneFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSceneFile.Location = new System.Drawing.Point(0, 0);
            this.txtSceneFile.Multiline = true;
            this.txtSceneFile.Name = "txtSceneFile";
            this.txtSceneFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSceneFile.Size = new System.Drawing.Size(200, 407);
            this.txtSceneFile.TabIndex = 1;
            this.txtSceneFile.TextChanged += new System.EventHandler(this.txtSceneFile_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblPercent);
            this.groupBox2.Controls.Add(this.btnCancelRendering);
            this.groupBox2.Controls.Add(this.btnRender);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 407);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 78);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // btnCancelRendering
            // 
            this.btnCancelRendering.Enabled = false;
            this.btnCancelRendering.Location = new System.Drawing.Point(7, 43);
            this.btnCancelRendering.Name = "btnCancelRendering";
            this.btnCancelRendering.Size = new System.Drawing.Size(75, 23);
            this.btnCancelRendering.TabIndex = 1;
            this.btnCancelRendering.Text = "Cancel";
            this.btnCancelRendering.UseVisualStyleBackColor = true;
            this.btnCancelRendering.Click += new System.EventHandler(this.btnCancelRendering_Click);
            // 
            // btnRender
            // 
            this.btnRender.Location = new System.Drawing.Point(7, 20);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(75, 23);
            this.btnRender.TabIndex = 0;
            this.btnRender.Text = "Render";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // dlgSaveRay
            // 
            this.dlgSaveRay.Filter = "\"Scene files (*.ray)|*.ray|All files (*.*)|*.*\"  ;";
            // 
            // pixelPosition
            // 
            this.pixelPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelPosition.Location = new System.Drawing.Point(466, 1);
            this.pixelPosition.Name = "pixelPosition";
            this.pixelPosition.Size = new System.Drawing.Size(100, 23);
            this.pixelPosition.TabIndex = 5;
            this.pixelPosition.Text = "X:Y";
            this.pixelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.Location = new System.Drawing.Point(88, 20);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(100, 23);
            this.lblPercent.TabIndex = 6;
            this.lblPercent.Text = "0%";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 512);
            this.Controls.Add(this.pixelPosition);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "Main";
            this.Text = "Petes Ray Tracer";
            ((System.ComponentModel.ISupportInitialize)(this.renderedImage)).EndInit();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox renderedImage;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuAvailableFiles;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.SaveFileDialog dlgSaveBmp;
        private System.Windows.Forms.ToolStripMenuItem renderOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiThreadedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuShadows;
        private System.Windows.Forms.ToolStripMenuItem mnuReflections;
        private System.Windows.Forms.ToolStripMenuItem mnuRefractions;
        private System.Windows.Forms.ToolStripMenuItem mnuRenderDepth;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtSceneFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSaveRay;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuSuperSampling;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem renderAntialiasingSamplesToolStripMenuItem;
        private System.Windows.Forms.Button btnCancelRendering;
        private System.Windows.Forms.ToolStripMenuItem mnuRenderingMode;
        private System.Windows.Forms.ToolStripMenuItem progressiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highQualityToolStripMenuItem;
        private System.Windows.Forms.Label pixelPosition;
        private System.Windows.Forms.ToolStripMenuItem jitteredSamplerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greyscaleEdgeDetectionSamplerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem componentEdgeDetectionSamplerToolStripMenuItem;
        private System.Windows.Forms.Label lblPercent;
    }
}

