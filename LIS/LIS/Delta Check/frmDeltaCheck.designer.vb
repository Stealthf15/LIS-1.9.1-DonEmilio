<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDeltaCheck
    Inherits DevExpress.XtraEditors.XtraForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDeltaCheck))
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
        Me.btnSearch = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.cboLimit = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.BarManager = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar()
        Me.btnRefresh = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnClose = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.BarAndDockingController = New DevExpress.XtraBars.BarAndDockingController(Me.components)
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.GroupControl3 = New DevExpress.XtraEditors.GroupControl()
        Me.dtResult = New DevExpress.XtraGrid.GridControl()
        Me.GridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.rgSelect = New DevExpress.XtraEditors.RadioGroup()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.txtSearch = New DevExpress.XtraEditors.SearchControl()
        CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeader.SuspendLayout()
        CType(Me.cboLimit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarAndDockingController, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl3.SuspendLayout()
        CType(Me.dtResult, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgSelect.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList
        '
        Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList.Images.SetKeyName(0, "Folder.ico")
        '
        'pnlHeader
        '
        Me.pnlHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlHeader.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(173, Byte), Integer))
        Me.pnlHeader.Appearance.Options.UseBackColor = True
        Me.pnlHeader.Controls.Add(Me.LabelControl7)
        Me.pnlHeader.Controls.Add(Me.LabelControl4)
        Me.pnlHeader.Controls.Add(Me.txtSearch)
        Me.pnlHeader.Controls.Add(Me.btnSearch)
        Me.pnlHeader.Controls.Add(Me.LabelControl1)
        Me.pnlHeader.Controls.Add(Me.cboLimit)
        Me.pnlHeader.Controls.Add(Me.rgSelect)
        Me.pnlHeader.Location = New System.Drawing.Point(12, 42)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1226, 32)
        Me.pnlHeader.TabIndex = 10
        '
        'btnSearch
        '
        Me.btnSearch.ImageOptions.SvgImage = CType(resources.GetObject("btnSearch.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.btnSearch.ImageOptions.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.btnSearch.Location = New System.Drawing.Point(839, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(68, 23)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "&Apply"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Appearance.Options.UseForeColor = True
        Me.LabelControl1.Location = New System.Drawing.Point(10, 11)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(41, 13)
        Me.LabelControl1.TabIndex = 10
        Me.LabelControl1.Text = "Section:"
        '
        'cboLimit
        '
        Me.cboLimit.Location = New System.Drawing.Point(57, 7)
        Me.cboLimit.Name = "cboLimit"
        Me.cboLimit.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.cboLimit.Properties.Appearance.Options.UseForeColor = True
        Me.cboLimit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cboLimit.Properties.Items.AddRange(New Object() {"Chemistry", "Hematology"})
        Me.cboLimit.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.cboLimit.Properties.LookAndFeel.UseDefaultLookAndFeel = False
        Me.cboLimit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cboLimit.Properties.UseReadOnlyAppearance = False
        Me.cboLimit.Size = New System.Drawing.Size(196, 20)
        Me.cboLimit.TabIndex = 9
        '
        'BarManager
        '
        Me.BarManager.AllowCustomization = False
        Me.BarManager.AllowQuickCustomization = False
        Me.BarManager.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager.Controller = Me.BarAndDockingController
        Me.BarManager.DockControls.Add(Me.barDockControlTop)
        Me.BarManager.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager.DockControls.Add(Me.barDockControlRight)
        Me.BarManager.Form = Me
        Me.BarManager.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnRefresh, Me.btnClose})
        Me.BarManager.MainMenu = Me.Bar2
        Me.BarManager.MaxItemId = 3
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.btnRefresh, True), New DevExpress.XtraBars.LinkPersistInfo(Me.btnClose)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        '
        'btnRefresh
        '
        Me.btnRefresh.Caption = "&Refresh"
        Me.btnRefresh.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
        Me.btnRefresh.Id = 1
        Me.btnRefresh.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.btnRefresh.ImageOptions.SvgImage = CType(resources.GetObject("btnRefresh.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.btnRefresh.ImageOptions.SvgImageSize = New System.Drawing.Size(24, 24)
        Me.btnRefresh.Name = "btnRefresh"
        '
        'btnClose
        '
        Me.btnClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.btnClose.Caption = "&Close"
        Me.btnClose.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
        Me.btnClose.Id = 2
        Me.btnClose.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.btnClose.ImageOptions.SvgImage = CType(resources.GetObject("btnClose.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.btnClose.ImageOptions.SvgImageSize = New System.Drawing.Size(24, 24)
        Me.btnClose.Name = "btnClose"
        '
        'BarAndDockingController
        '
        Me.BarAndDockingController.AppearancesBar.MainMenuAppearance.Normal.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.BarAndDockingController.AppearancesBar.MainMenuAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BarAndDockingController.AppearancesBar.MainMenuAppearance.Normal.Options.UseFont = True
        Me.BarAndDockingController.AppearancesBar.MainMenuAppearance.Normal.Options.UseForeColor = True
        Me.BarAndDockingController.LookAndFeel.SkinName = "VS2010"
        Me.BarAndDockingController.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
        Me.BarAndDockingController.PropertiesBar.AllowLinkLighting = False
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager
        Me.barDockControlTop.Size = New System.Drawing.Size(1251, 36)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 582)
        Me.barDockControlBottom.Manager = Me.BarManager
        Me.barDockControlBottom.Size = New System.Drawing.Size(1251, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 36)
        Me.barDockControlLeft.Manager = Me.BarManager
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 546)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1251, 36)
        Me.barDockControlRight.Manager = Me.BarManager
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 546)
        '
        'GroupControl3
        '
        Me.GroupControl3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupControl3.AppearanceCaption.BackColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.GroupControl3.AppearanceCaption.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.GroupControl3.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.GroupControl3.AppearanceCaption.ForeColor = System.Drawing.Color.White
        Me.GroupControl3.AppearanceCaption.Options.UseBackColor = True
        Me.GroupControl3.AppearanceCaption.Options.UseBorderColor = True
        Me.GroupControl3.AppearanceCaption.Options.UseForeColor = True
        Me.GroupControl3.CaptionImageOptions.AllowGlyphSkinning = True
        Me.GroupControl3.CaptionImageOptions.SvgImage = CType(resources.GetObject("GroupControl3.CaptionImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.GroupControl3.CaptionImageOptions.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.GroupControl3.Controls.Add(Me.dtResult)
        Me.GroupControl3.Location = New System.Drawing.Point(12, 80)
        Me.GroupControl3.LookAndFeel.SkinName = "The Bezier"
        Me.GroupControl3.LookAndFeel.UseDefaultLookAndFeel = False
        Me.GroupControl3.Name = "GroupControl3"
        Me.GroupControl3.Size = New System.Drawing.Size(1226, 490)
        Me.GroupControl3.TabIndex = 185
        Me.GroupControl3.Text = "Previous Test Results"
        '
        'dtResult
        '
        Me.dtResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtResult.Location = New System.Drawing.Point(2, 27)
        Me.dtResult.MainView = Me.GridView
        Me.dtResult.MenuManager = Me.BarManager
        Me.dtResult.Name = "dtResult"
        Me.dtResult.Size = New System.Drawing.Size(1222, 461)
        Me.dtResult.TabIndex = 58
        Me.dtResult.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView})
        '
        'GridView
        '
        Me.GridView.Appearance.SelectedRow.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GridView.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.WhiteSmoke
        Me.GridView.Appearance.SelectedRow.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.GridView.Appearance.SelectedRow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.GridView.Appearance.SelectedRow.Options.UseBackColor = True
        Me.GridView.Appearance.SelectedRow.Options.UseBorderColor = True
        Me.GridView.Appearance.SelectedRow.Options.UseForeColor = True
        Me.GridView.GridControl = Me.dtResult
        Me.GridView.Name = "GridView"
        Me.GridView.OptionsCustomization.AllowColumnMoving = False
        Me.GridView.OptionsCustomization.AllowColumnResizing = False
        Me.GridView.OptionsCustomization.AllowFilter = False
        Me.GridView.OptionsCustomization.AllowGroup = False
        Me.GridView.OptionsCustomization.AllowQuickHideColumns = False
        Me.GridView.OptionsCustomization.AllowSort = False
        Me.GridView.OptionsSelection.CheckBoxSelectorColumnWidth = 30
        Me.GridView.OptionsView.ShowGroupPanel = False
        Me.GridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        '
        'LabelControl7
        '
        Me.LabelControl7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Appearance.Options.UseFont = True
        Me.LabelControl7.Location = New System.Drawing.Point(265, 11)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(75, 13)
        Me.LabelControl7.TabIndex = 158
        Me.LabelControl7.Text = "Search option:"
        '
        'rgSelect
        '
        Me.rgSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rgSelect.Location = New System.Drawing.Point(348, 5)
        Me.rgSelect.MenuManager = Me.BarManager
        Me.rgSelect.Name = "rgSelect"
        Me.rgSelect.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.rgSelect.Properties.Appearance.Options.UseBackColor = True
        Me.rgSelect.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.rgSelect.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Hospital No."), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Patient Name")})
        Me.rgSelect.Size = New System.Drawing.Size(221, 23)
        Me.rgSelect.TabIndex = 157
        '
        'LabelControl4
        '
        Me.LabelControl4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Appearance.Options.UseFont = True
        Me.LabelControl4.Location = New System.Drawing.Point(567, 10)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(63, 13)
        Me.LabelControl4.TabIndex = 156
        Me.LabelControl4.Text = "Search here:"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(634, 6)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(27, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(62, Byte), Integer))
        Me.txtSearch.Properties.Appearance.Options.UseForeColor = True
        Me.txtSearch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Repository.ClearButton(), New DevExpress.XtraEditors.Repository.SearchButton()})
        Me.txtSearch.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Blue"
        Me.txtSearch.Properties.LookAndFeel.UseDefaultLookAndFeel = False
        Me.txtSearch.Size = New System.Drawing.Size(199, 20)
        Me.txtSearch.TabIndex = 155
        '
        'frmDeltaCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1251, 582)
        Me.Controls.Add(Me.GroupControl3)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDeltaCheck"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delta Check"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        CType(Me.cboLimit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarAndDockingController, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl3.ResumeLayout(False)
        CType(Me.dtResult, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgSelect.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
    Friend WithEvents BarManager As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents btnRefresh As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnClose As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents BarAndDockingController As DevExpress.XtraBars.BarAndDockingController
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents ImageList As System.Windows.Forms.ImageList
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboLimit As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents btnSearch As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GroupControl3 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents dtResult As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSearch As DevExpress.XtraEditors.SearchControl
    Friend WithEvents rgSelect As DevExpress.XtraEditors.RadioGroup
End Class
