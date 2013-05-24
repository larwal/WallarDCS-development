<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewSynopticfrm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewSynopticfrm))
        Me.ToolStripNew = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnSelect = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCut = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCopy = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnPaste = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnUndo = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnRedo = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsDrawMode = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDrawElements = New System.Windows.Forms.ToolStripButton()
        Me.tsRepaint = New System.Windows.Forms.ToolStripButton()
        Me.tsClear = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnProperties = New System.Windows.Forms.ToolStripButton()
        Me.Context = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlCanvas = New System.Windows.Forms.Panel()
        Me.tsBtnExport = New System.Windows.Forms.ToolStripButton()
        Me.tsBtnImport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripNew.SuspendLayout()
        Me.Context.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripNew
        '
        Me.ToolStripNew.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel, Me.toolStripSeparator, Me.tsbtnSelect, Me.ToolStripSave, Me.ToolStripCancel, Me.tsbtnCut, Me.tsbtnCopy, Me.tsbtnPaste, Me.tsbtnUndo, Me.tsbtnRedo, Me.tsbtnDelete, Me.tsBtnExport, Me.tsBtnImport, Me.ToolStripSeparator2, Me.tsDrawMode, Me.tsbtnProperties, Me.tsbtnDrawElements, Me.tsRepaint, Me.tsClear})
        Me.ToolStripNew.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripNew.Name = "ToolStripNew"
        Me.ToolStripNew.Size = New System.Drawing.Size(870, 25)
        Me.ToolStripNew.TabIndex = 0
        Me.ToolStripNew.Text = "ToolStrip"
        '
        'ToolStripLabel
        '
        Me.ToolStripLabel.Name = "ToolStripLabel"
        Me.ToolStripLabel.Size = New System.Drawing.Size(111, 22)
        Me.ToolStripLabel.Text = "New Synoptic Form"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtnSelect
        '
        Me.tsbtnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnSelect.Image = CType(resources.GetObject("tsbtnSelect.Image"), System.Drawing.Image)
        Me.tsbtnSelect.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSelect.Name = "tsbtnSelect"
        Me.tsbtnSelect.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnSelect.Text = "&Select"
        Me.tsbtnSelect.ToolTipText = "Select"
        '
        'ToolStripSave
        '
        Me.ToolStripSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSave.AutoSize = False
        Me.ToolStripSave.BackColor = System.Drawing.Color.Lime
        Me.ToolStripSave.Image = CType(resources.GetObject("ToolStripSave.Image"), System.Drawing.Image)
        Me.ToolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSave.Name = "ToolStripSave"
        Me.ToolStripSave.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripSave.Text = "  &Save   "
        '
        'ToolStripCancel
        '
        Me.ToolStripCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripCancel.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.ToolStripCancel.Image = CType(resources.GetObject("ToolStripCancel.Image"), System.Drawing.Image)
        Me.ToolStripCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripCancel.Name = "ToolStripCancel"
        Me.ToolStripCancel.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripCancel.Text = "&Cancel"
        '
        'tsbtnCut
        '
        Me.tsbtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnCut.Enabled = False
        Me.tsbtnCut.Image = CType(resources.GetObject("tsbtnCut.Image"), System.Drawing.Image)
        Me.tsbtnCut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCut.Name = "tsbtnCut"
        Me.tsbtnCut.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnCut.Text = "C&ut"
        '
        'tsbtnCopy
        '
        Me.tsbtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnCopy.Enabled = False
        Me.tsbtnCopy.Image = CType(resources.GetObject("tsbtnCopy.Image"), System.Drawing.Image)
        Me.tsbtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCopy.Name = "tsbtnCopy"
        Me.tsbtnCopy.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnCopy.Text = "&Copy"
        '
        'tsbtnPaste
        '
        Me.tsbtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnPaste.Enabled = False
        Me.tsbtnPaste.Image = CType(resources.GetObject("tsbtnPaste.Image"), System.Drawing.Image)
        Me.tsbtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnPaste.Name = "tsbtnPaste"
        Me.tsbtnPaste.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnPaste.Text = "&Paste"
        '
        'tsbtnUndo
        '
        Me.tsbtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnUndo.Enabled = False
        Me.tsbtnUndo.Image = CType(resources.GetObject("tsbtnUndo.Image"), System.Drawing.Image)
        Me.tsbtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnUndo.Name = "tsbtnUndo"
        Me.tsbtnUndo.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnUndo.Text = "Undo"
        '
        'tsbtnRedo
        '
        Me.tsbtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnRedo.Enabled = False
        Me.tsbtnRedo.Image = CType(resources.GetObject("tsbtnRedo.Image"), System.Drawing.Image)
        Me.tsbtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnRedo.Name = "tsbtnRedo"
        Me.tsbtnRedo.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnRedo.Text = "Redo"
        '
        'tsbtnDelete
        '
        Me.tsbtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnDelete.Image = CType(resources.GetObject("tsbtnDelete.Image"), System.Drawing.Image)
        Me.tsbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelete.Name = "tsbtnDelete"
        Me.tsbtnDelete.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnDelete.Text = "Delete"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsDrawMode
        '
        Me.tsDrawMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsDrawMode.Image = CType(resources.GetObject("tsDrawMode.Image"), System.Drawing.Image)
        Me.tsDrawMode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsDrawMode.Name = "tsDrawMode"
        Me.tsDrawMode.Size = New System.Drawing.Size(23, 22)
        Me.tsDrawMode.Text = "Draw"
        '
        'tsbtnDrawElements
        '
        Me.tsbtnDrawElements.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.tsbtnDrawElements.Image = CType(resources.GetObject("tsbtnDrawElements.Image"), System.Drawing.Image)
        Me.tsbtnDrawElements.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDrawElements.Name = "tsbtnDrawElements"
        Me.tsbtnDrawElements.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnDrawElements.Text = "Pallet"
        '
        'tsRepaint
        '
        Me.tsRepaint.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.tsRepaint.Image = CType(resources.GetObject("tsRepaint.Image"), System.Drawing.Image)
        Me.tsRepaint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsRepaint.Name = "tsRepaint"
        Me.tsRepaint.Size = New System.Drawing.Size(67, 22)
        Me.tsRepaint.Text = "&Repaint"
        Me.tsRepaint.ToolTipText = "Repaint canvas"
        '
        'tsClear
        '
        Me.tsClear.BackColor = System.Drawing.Color.Red
        Me.tsClear.Image = CType(resources.GetObject("tsClear.Image"), System.Drawing.Image)
        Me.tsClear.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsClear.Name = "tsClear"
        Me.tsClear.Size = New System.Drawing.Size(54, 22)
        Me.tsClear.Text = "Clear"
        Me.tsClear.ToolTipText = "Clear Canvas"
        '
        'tsbtnProperties
        '
        Me.tsbtnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnProperties.Image = CType(resources.GetObject("tsbtnProperties.Image"), System.Drawing.Image)
        Me.tsbtnProperties.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnProperties.Name = "tsbtnProperties"
        Me.tsbtnProperties.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnProperties.Text = "Properties"
        '
        'Context
        '
        Me.Context.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmProperties})
        Me.Context.Name = "Context"
        Me.Context.Size = New System.Drawing.Size(155, 26)
        '
        'tsmProperties
        '
        Me.tsmProperties.Name = "tsmProperties"
        Me.tsmProperties.Size = New System.Drawing.Size(154, 22)
        Me.tsmProperties.Text = "&Eigenschappen"
        '
        'pnlCanvas
        '
        Me.pnlCanvas.BackColor = System.Drawing.SystemColors.Window
        Me.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCanvas.Location = New System.Drawing.Point(0, 25)
        Me.pnlCanvas.Name = "pnlCanvas"
        Me.pnlCanvas.Size = New System.Drawing.Size(870, 418)
        Me.pnlCanvas.TabIndex = 1
        '
        'tsBtnExport
        '
        Me.tsBtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtnExport.Enabled = False
        Me.tsBtnExport.Image = CType(resources.GetObject("tsBtnExport.Image"), System.Drawing.Image)
        Me.tsBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtnExport.Name = "tsBtnExport"
        Me.tsBtnExport.Size = New System.Drawing.Size(23, 22)
        Me.tsBtnExport.Text = "Export"
        '
        'tsBtnImport
        '
        Me.tsBtnImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtnImport.Enabled = False
        Me.tsBtnImport.Image = CType(resources.GetObject("tsBtnImport.Image"), System.Drawing.Image)
        Me.tsBtnImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtnImport.Name = "tsBtnImport"
        Me.tsBtnImport.Size = New System.Drawing.Size(23, 22)
        Me.tsBtnImport.Text = "Import"
        '
        'NewSynopticfrm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(870, 443)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlCanvas)
        Me.Controls.Add(Me.ToolStripNew)
        Me.DoubleBuffered = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewSynopticfrm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "NewSynoptic"
        Me.ToolStripNew.ResumeLayout(False)
        Me.ToolStripNew.PerformLayout()
        Me.Context.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripNew As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnDrawElements As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsRepaint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsClear As System.Windows.Forms.ToolStripButton
    Friend WithEvents Context As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlCanvas As System.Windows.Forms.Panel
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnSelect As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCut As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCopy As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsDrawMode As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnUndo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnRedo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnProperties As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsBtnExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsBtnImport As System.Windows.Forms.ToolStripButton
End Class
