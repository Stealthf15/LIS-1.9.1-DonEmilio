<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAssyInfo
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAssyInfo))
        Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
        Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
        Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
        Me.txtLotNo = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.txtExpiry = New System.Windows.Forms.DateTimePicker()
        Me.txtMethodUsed = New DevExpress.XtraEditors.TextEdit()
        Me.txtReagent = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLotNo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMethodUsed.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtReagent.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SeparatorControl1
        '
        Me.SeparatorControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SeparatorControl1.Location = New System.Drawing.Point(1, 137)
        Me.SeparatorControl1.Name = "SeparatorControl1"
        Me.SeparatorControl1.Size = New System.Drawing.Size(352, 18)
        Me.SeparatorControl1.TabIndex = 107
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.btnSave.Appearance.Options.UseForeColor = True
        Me.btnSave.AppearanceHovered.BackColor = System.Drawing.Color.Gray
        Me.btnSave.AppearanceHovered.BackColor2 = System.Drawing.Color.Gray
        Me.btnSave.AppearanceHovered.BorderColor = System.Drawing.Color.Gray
        Me.btnSave.AppearanceHovered.Options.UseBackColor = True
        Me.btnSave.AppearanceHovered.Options.UseBorderColor = True
        Me.btnSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSave.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.btnSave.ImageOptions.Image = CType(resources.GetObject("btnSave.ImageOptions.Image"), System.Drawing.Image)
        Me.btnSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        Me.btnSave.Location = New System.Drawing.Point(120, 161)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(108, 27)
        Me.btnSave.TabIndex = 105
        Me.btnSave.Text = "&Add"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.btnClose.Appearance.Options.UseForeColor = True
        Me.btnClose.AppearanceHovered.BackColor = System.Drawing.Color.Gray
        Me.btnClose.AppearanceHovered.BackColor2 = System.Drawing.Color.Gray
        Me.btnClose.AppearanceHovered.BorderColor = System.Drawing.Color.Gray
        Me.btnClose.AppearanceHovered.Options.UseBackColor = True
        Me.btnClose.AppearanceHovered.Options.UseBorderColor = True
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClose.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.btnClose.ImageOptions.Image = CType(resources.GetObject("btnClose.ImageOptions.Image"), System.Drawing.Image)
        Me.btnClose.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        Me.btnClose.Location = New System.Drawing.Point(234, 161)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(108, 27)
        Me.btnClose.TabIndex = 106
        Me.btnClose.Text = "&Close"
        '
        'txtLotNo
        '
        Me.txtLotNo.Location = New System.Drawing.Point(102, 73)
        Me.txtLotNo.Name = "txtLotNo"
        Me.txtLotNo.Size = New System.Drawing.Size(217, 20)
        Me.txtLotNo.TabIndex = 108
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.LabelControl5.Appearance.Options.UseForeColor = True
        Me.LabelControl5.Location = New System.Drawing.Point(33, 76)
        Me.LabelControl5.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.LabelControl5.LookAndFeel.UseDefaultLookAndFeel = False
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(63, 13)
        Me.LabelControl5.TabIndex = 109
        Me.LabelControl5.Text = "Lot Number:"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.LabelControl1.Appearance.Options.UseForeColor = True
        Me.LabelControl1.Location = New System.Drawing.Point(36, 106)
        Me.LabelControl1.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.LabelControl1.LookAndFeel.UseDefaultLookAndFeel = False
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(60, 13)
        Me.LabelControl1.TabIndex = 111
        Me.LabelControl1.Text = "Expiry Date:"
        '
        'txtExpiry
        '
        Me.txtExpiry.CalendarFont = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiry.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtExpiry.Location = New System.Drawing.Point(102, 99)
        Me.txtExpiry.Name = "txtExpiry"
        Me.txtExpiry.Size = New System.Drawing.Size(217, 22)
        Me.txtExpiry.TabIndex = 112
        '
        'txtMethodUsed
        '
        Me.txtMethodUsed.Location = New System.Drawing.Point(102, 21)
        Me.txtMethodUsed.Name = "txtMethodUsed"
        Me.txtMethodUsed.Size = New System.Drawing.Size(217, 20)
        Me.txtMethodUsed.TabIndex = 113
        '
        'txtReagent
        '
        Me.txtReagent.Location = New System.Drawing.Point(102, 47)
        Me.txtReagent.Name = "txtReagent"
        Me.txtReagent.Size = New System.Drawing.Size(217, 20)
        Me.txtReagent.TabIndex = 114
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.LabelControl2.Appearance.Options.UseForeColor = True
        Me.LabelControl2.Location = New System.Drawing.Point(23, 24)
        Me.LabelControl2.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.LabelControl2.LookAndFeel.UseDefaultLookAndFeel = False
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(73, 13)
        Me.LabelControl2.TabIndex = 115
        Me.LabelControl2.Text = "Method Used:"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.LabelControl3.Appearance.Options.UseForeColor = True
        Me.LabelControl3.Location = New System.Drawing.Point(50, 50)
        Me.LabelControl3.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.LabelControl3.LookAndFeel.UseDefaultLookAndFeel = False
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(46, 13)
        Me.LabelControl3.TabIndex = 116
        Me.LabelControl3.Text = "Reagent:"
        '
        'frmAssyInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 200)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.txtReagent)
        Me.Controls.Add(Me.txtMethodUsed)
        Me.Controls.Add(Me.txtExpiry)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.txtLotNo)
        Me.Controls.Add(Me.SeparatorControl1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAssyInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Assay Information"
        CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLotNo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMethodUsed.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtReagent.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtLotNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtExpiry As DateTimePicker
    Friend WithEvents txtMethodUsed As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtReagent As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
End Class
