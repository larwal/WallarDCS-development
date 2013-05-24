<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainSynopFrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainSynopFrm))
        Me.pnlToolStrip = New System.Windows.Forms.Panel()
        Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsCanvasInfo = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripRepaint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.canvas = New System.Windows.Forms.Panel()
        Me.pnlToolStrip.SuspendLayout()
        Me.ToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlToolStrip
        '
        Me.pnlToolStrip.Controls.Add(Me.ToolStripContainer)
        Me.pnlToolStrip.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.pnlToolStrip.Name = "pnlToolStrip"
        Me.pnlToolStrip.Size = New System.Drawing.Size(762, 26)
        Me.pnlToolStrip.TabIndex = 0
        '
        'ToolStripContainer
        '
        '
        'ToolStripContainer.BottomToolStripPanel
        '
        Me.ToolStripContainer.BottomToolStripPanel.Enabled = False
        '
        'ToolStripContainer.ContentPanel
        '
        Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(762, 1)
        Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        '
        'ToolStripContainer.LeftToolStripPanel
        '
        Me.ToolStripContainer.LeftToolStripPanel.Enabled = False
        Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer.Name = "ToolStripContainer"
        '
        'ToolStripContainer.RightToolStripPanel
        '
        Me.ToolStripContainer.RightToolStripPanel.Enabled = False
        Me.ToolStripContainer.Size = New System.Drawing.Size(762, 26)
        Me.ToolStripContainer.TabIndex = 0
        Me.ToolStripContainer.Text = "ToolStripContainer1"
        '
        'ToolStripContainer.TopToolStripPanel
        '
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsCanvasInfo, Me.ToolStripRepaint, Me.ToolStripProgressBar1})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(295, 25)
        Me.ToolStrip1.TabIndex = 0
        '
        'tsCanvasInfo
        '
        Me.tsCanvasInfo.Name = "tsCanvasInfo"
        Me.tsCanvasInfo.Size = New System.Drawing.Size(111, 22)
        Me.tsCanvasInfo.Text = "Canvas Information"
        '
        'ToolStripRepaint
        '
        Me.ToolStripRepaint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripRepaint.Image = CType(resources.GetObject("ToolStripRepaint.Image"), System.Drawing.Image)
        Me.ToolStripRepaint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripRepaint.Name = "ToolStripRepaint"
        Me.ToolStripRepaint.Size = New System.Drawing.Size(129, 22)
        Me.ToolStripRepaint.Text = "Herteken synoptiek"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.AutoSize = False
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(10, 22)
        '
        'canvas
        '
        Me.canvas.BackColor = System.Drawing.SystemColors.Window
        Me.canvas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.canvas.Location = New System.Drawing.Point(0, 26)
        Me.canvas.Name = "canvas"
        Me.canvas.Size = New System.Drawing.Size(762, 430)
        Me.canvas.TabIndex = 1
        '
        'MainSynopFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 456)
        Me.Controls.Add(Me.canvas)
        Me.Controls.Add(Me.pnlToolStrip)
        Me.Name = "MainSynopFrm"
        Me.Text = "MainSynopFrm"
        Me.pnlToolStrip.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ResumeLayout(False)
        Me.ToolStripContainer.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlToolStrip As System.Windows.Forms.Panel
    Friend WithEvents canvas As System.Windows.Forms.Panel
    Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsCanvasInfo As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripRepaint As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
End Class
