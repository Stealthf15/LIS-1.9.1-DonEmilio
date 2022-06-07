Imports DevExpress.Utils.Layout
Imports DevExpress.XtraEditors

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTatMonitor
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
        Me.components = New System.ComponentModel.Container()
        Me.TablePanel1 = New DevExpress.Utils.Layout.TablePanel()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.dtWarding = New DevExpress.XtraGrid.GridControl()
        Me.GridWarding = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.dtCompleted = New DevExpress.XtraGrid.GridControl()
        Me.GridCompleted = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.dtProcessing = New DevExpress.XtraGrid.GridControl()
        Me.GridProcessing = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.dtCheckIn = New DevExpress.XtraGrid.GridControl()
        Me.GridCheckin = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.tmCheckin = New System.Windows.Forms.Timer(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.TablePanel2 = New DevExpress.Utils.Layout.TablePanel()
        Me.LabelControl13 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl12 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl11 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl10 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl9 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl()
        Me.lblHeader = New DevExpress.XtraEditors.LabelControl()
        Me.tmChangeSection = New System.Windows.Forms.Timer(Me.components)
        CType(Me.TablePanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TablePanel1.SuspendLayout()
        CType(Me.dtWarding, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridWarding, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtCompleted, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridCompleted, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtProcessing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridProcessing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtCheckIn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridCheckin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.TablePanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TablePanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TablePanel1
        '
        Me.TablePanel1.Columns.AddRange(New DevExpress.Utils.Layout.TablePanelColumn() {New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!)})
        Me.TablePanel1.Controls.Add(Me.LabelControl4)
        Me.TablePanel1.Controls.Add(Me.LabelControl3)
        Me.TablePanel1.Controls.Add(Me.LabelControl2)
        Me.TablePanel1.Controls.Add(Me.LabelControl1)
        Me.TablePanel1.Controls.Add(Me.dtWarding)
        Me.TablePanel1.Controls.Add(Me.dtCompleted)
        Me.TablePanel1.Controls.Add(Me.dtProcessing)
        Me.TablePanel1.Controls.Add(Me.dtCheckIn)
        Me.TablePanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TablePanel1.Location = New System.Drawing.Point(0, 130)
        Me.TablePanel1.Name = "TablePanel1"
        Me.TablePanel1.Rows.AddRange(New DevExpress.Utils.Layout.TablePanelRow() {New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 6.24!), New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 91.76!)})
        Me.TablePanel1.Size = New System.Drawing.Size(1303, 613)
        Me.TablePanel1.TabIndex = 2
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.BackColor = System.Drawing.Color.ForestGreen
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl4.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl4.Appearance.Options.UseBackColor = True
        Me.LabelControl4.Appearance.Options.UseFont = True
        Me.LabelControl4.Appearance.Options.UseForeColor = True
        Me.LabelControl4.Appearance.Options.UseTextOptions = True
        Me.LabelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel1.SetColumn(Me.LabelControl4, 3)
        Me.LabelControl4.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl4.Location = New System.Drawing.Point(980, 3)
        Me.LabelControl4.Name = "LabelControl4"
        Me.TablePanel1.SetRow(Me.LabelControl4, 0)
        Me.LabelControl4.Size = New System.Drawing.Size(320, 37)
        Me.LabelControl4.TabIndex = 7
        Me.LabelControl4.Text = "COMPLETED"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.BackColor = System.Drawing.Color.Gold
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl3.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl3.Appearance.Options.UseBackColor = True
        Me.LabelControl3.Appearance.Options.UseFont = True
        Me.LabelControl3.Appearance.Options.UseForeColor = True
        Me.LabelControl3.Appearance.Options.UseTextOptions = True
        Me.LabelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel1.SetColumn(Me.LabelControl3, 2)
        Me.LabelControl3.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl3.Location = New System.Drawing.Point(655, 3)
        Me.LabelControl3.Name = "LabelControl3"
        Me.TablePanel1.SetRow(Me.LabelControl3, 0)
        Me.LabelControl3.Size = New System.Drawing.Size(320, 37)
        Me.LabelControl3.TabIndex = 6
        Me.LabelControl3.Text = "PROCESSING"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.BackColor = System.Drawing.Color.CornflowerBlue
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl2.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl2.Appearance.Options.UseBackColor = True
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.Appearance.Options.UseForeColor = True
        Me.LabelControl2.Appearance.Options.UseTextOptions = True
        Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel1.SetColumn(Me.LabelControl2, 1)
        Me.LabelControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl2.Location = New System.Drawing.Point(329, 3)
        Me.LabelControl2.Name = "LabelControl2"
        Me.TablePanel1.SetRow(Me.LabelControl2, 0)
        Me.LabelControl2.Size = New System.Drawing.Size(320, 37)
        Me.LabelControl2.TabIndex = 5
        Me.LabelControl2.Text = "CHECKED-IN"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.BackColor = System.Drawing.Color.Orange
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl1.Appearance.Options.UseBackColor = True
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Appearance.Options.UseForeColor = True
        Me.LabelControl1.Appearance.Options.UseTextOptions = True
        Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel1.SetColumn(Me.LabelControl1, 0)
        Me.LabelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl1.Location = New System.Drawing.Point(3, 3)
        Me.LabelControl1.Name = "LabelControl1"
        Me.TablePanel1.SetRow(Me.LabelControl1, 0)
        Me.LabelControl1.Size = New System.Drawing.Size(320, 37)
        Me.LabelControl1.TabIndex = 4
        Me.LabelControl1.Text = "WARDING"
        '
        'dtWarding
        '
        Me.TablePanel1.SetColumn(Me.dtWarding, 0)
        Me.dtWarding.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtWarding.Location = New System.Drawing.Point(3, 42)
        Me.dtWarding.MainView = Me.GridWarding
        Me.dtWarding.Name = "dtWarding"
        Me.TablePanel1.SetRow(Me.dtWarding, 1)
        Me.dtWarding.Size = New System.Drawing.Size(320, 568)
        Me.dtWarding.TabIndex = 3
        Me.dtWarding.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridWarding})
        '
        'GridWarding
        '
        Me.GridWarding.GridControl = Me.dtWarding
        Me.GridWarding.Name = "GridWarding"
        Me.GridWarding.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridWarding.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridWarding.OptionsSelection.EnableAppearanceHideSelection = False
        Me.GridWarding.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect
        Me.GridWarding.OptionsView.ShowGroupPanel = False
        '
        'dtCompleted
        '
        Me.TablePanel1.SetColumn(Me.dtCompleted, 3)
        Me.dtCompleted.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtCompleted.Location = New System.Drawing.Point(980, 42)
        Me.dtCompleted.MainView = Me.GridCompleted
        Me.dtCompleted.Name = "dtCompleted"
        Me.TablePanel1.SetRow(Me.dtCompleted, 1)
        Me.dtCompleted.Size = New System.Drawing.Size(320, 568)
        Me.dtCompleted.TabIndex = 2
        Me.dtCompleted.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridCompleted})
        '
        'GridCompleted
        '
        Me.GridCompleted.GridControl = Me.dtCompleted
        Me.GridCompleted.Name = "GridCompleted"
        Me.GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridCompleted.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridCompleted.OptionsSelection.EnableAppearanceHideSelection = False
        Me.GridCompleted.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect
        Me.GridCompleted.OptionsView.ShowGroupPanel = False
        '
        'dtProcessing
        '
        Me.TablePanel1.SetColumn(Me.dtProcessing, 2)
        Me.dtProcessing.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtProcessing.Location = New System.Drawing.Point(655, 42)
        Me.dtProcessing.MainView = Me.GridProcessing
        Me.dtProcessing.Name = "dtProcessing"
        Me.TablePanel1.SetRow(Me.dtProcessing, 1)
        Me.dtProcessing.Size = New System.Drawing.Size(320, 568)
        Me.dtProcessing.TabIndex = 1
        Me.dtProcessing.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridProcessing})
        '
        'GridProcessing
        '
        Me.GridProcessing.GridControl = Me.dtProcessing
        Me.GridProcessing.Name = "GridProcessing"
        Me.GridProcessing.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridProcessing.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridProcessing.OptionsSelection.EnableAppearanceHideSelection = False
        Me.GridProcessing.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect
        Me.GridProcessing.OptionsView.ShowGroupPanel = False
        '
        'dtCheckIn
        '
        Me.TablePanel1.SetColumn(Me.dtCheckIn, 1)
        Me.dtCheckIn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtCheckIn.Location = New System.Drawing.Point(329, 42)
        Me.dtCheckIn.MainView = Me.GridCheckin
        Me.dtCheckIn.Name = "dtCheckIn"
        Me.TablePanel1.SetRow(Me.dtCheckIn, 1)
        Me.dtCheckIn.Size = New System.Drawing.Size(320, 568)
        Me.dtCheckIn.TabIndex = 0
        Me.dtCheckIn.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridCheckin})
        '
        'GridCheckin
        '
        Me.GridCheckin.GridControl = Me.dtCheckIn
        Me.GridCheckin.Name = "GridCheckin"
        Me.GridCheckin.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridCheckin.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridCheckin.OptionsSelection.EnableAppearanceHideSelection = False
        Me.GridCheckin.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect
        Me.GridCheckin.OptionsView.ShowGroupPanel = False
        '
        'tmCheckin
        '
        Me.tmCheckin.Enabled = True
        Me.tmCheckin.Interval = 1000
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.TablePanel2)
        Me.PanelControl1.Controls.Add(Me.lblHeader)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1303, 130)
        Me.PanelControl1.TabIndex = 3
        '
        'TablePanel2
        '
        Me.TablePanel2.Columns.AddRange(New DevExpress.Utils.Layout.TablePanelColumn() {New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.0!)})
        Me.TablePanel2.Controls.Add(Me.LabelControl13)
        Me.TablePanel2.Controls.Add(Me.LabelControl12)
        Me.TablePanel2.Controls.Add(Me.LabelControl11)
        Me.TablePanel2.Controls.Add(Me.LabelControl10)
        Me.TablePanel2.Controls.Add(Me.LabelControl9)
        Me.TablePanel2.Controls.Add(Me.LabelControl8)
        Me.TablePanel2.Controls.Add(Me.LabelControl7)
        Me.TablePanel2.Controls.Add(Me.LabelControl6)
        Me.TablePanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TablePanel2.Location = New System.Drawing.Point(2, 88)
        Me.TablePanel2.Name = "TablePanel2"
        Me.TablePanel2.Rows.AddRange(New DevExpress.Utils.Layout.TablePanelRow() {New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26.0!)})
        Me.TablePanel2.Size = New System.Drawing.Size(1299, 30)
        Me.TablePanel2.TabIndex = 6
        '
        'LabelControl13
        '
        Me.LabelControl13.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl13.Appearance.ForeColor = System.Drawing.Color.DarkRed
        Me.LabelControl13.Appearance.Options.UseFont = True
        Me.LabelControl13.Appearance.Options.UseForeColor = True
        Me.LabelControl13.Appearance.Options.UseTextOptions = True
        Me.LabelControl13.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl13.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl13, 7)
        Me.LabelControl13.Location = New System.Drawing.Point(1140, 3)
        Me.LabelControl13.Name = "LabelControl13"
        Me.TablePanel2.SetRow(Me.LabelControl13, 0)
        Me.LabelControl13.Size = New System.Drawing.Size(156, 42)
        Me.LabelControl13.TabIndex = 7
        Me.LabelControl13.Text = "STAT MODE TEXT COLOR"
        '
        'LabelControl12
        '
        Me.LabelControl12.Appearance.BackColor = System.Drawing.Color.OrangeRed
        Me.LabelControl12.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl12.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl12.Appearance.Options.UseBackColor = True
        Me.LabelControl12.Appearance.Options.UseFont = True
        Me.LabelControl12.Appearance.Options.UseForeColor = True
        Me.LabelControl12.Appearance.Options.UseTextOptions = True
        Me.LabelControl12.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl12.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl12, 6)
        Me.LabelControl12.Location = New System.Drawing.Point(977, 4)
        Me.LabelControl12.Name = "LabelControl12"
        Me.TablePanel2.SetRow(Me.LabelControl12, 0)
        Me.LabelControl12.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl12.TabIndex = 6
        Me.LabelControl12.Text = "EXCEED"
        '
        'LabelControl11
        '
        Me.LabelControl11.Appearance.BackColor = System.Drawing.Color.RoyalBlue
        Me.LabelControl11.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl11.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl11.Appearance.Options.UseBackColor = True
        Me.LabelControl11.Appearance.Options.UseFont = True
        Me.LabelControl11.Appearance.Options.UseForeColor = True
        Me.LabelControl11.Appearance.Options.UseTextOptions = True
        Me.LabelControl11.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl11.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl11, 5)
        Me.LabelControl11.Location = New System.Drawing.Point(815, 4)
        Me.LabelControl11.Name = "LabelControl11"
        Me.TablePanel2.SetRow(Me.LabelControl11, 0)
        Me.LabelControl11.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl11.TabIndex = 5
        Me.LabelControl11.Text = "WARNING"
        '
        'LabelControl10
        '
        Me.LabelControl10.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl10.Appearance.Options.UseFont = True
        Me.LabelControl10.Appearance.Options.UseTextOptions = True
        Me.LabelControl10.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl10.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl10, 4)
        Me.LabelControl10.Location = New System.Drawing.Point(653, 4)
        Me.LabelControl10.Name = "LabelControl10"
        Me.TablePanel2.SetRow(Me.LabelControl10, 0)
        Me.LabelControl10.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl10.TabIndex = 4
        Me.LabelControl10.Text = "TAT SIGN:"
        '
        'LabelControl9
        '
        Me.LabelControl9.Appearance.BackColor = System.Drawing.Color.Orange
        Me.LabelControl9.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl9.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl9.Appearance.Options.UseBackColor = True
        Me.LabelControl9.Appearance.Options.UseFont = True
        Me.LabelControl9.Appearance.Options.UseForeColor = True
        Me.LabelControl9.Appearance.Options.UseTextOptions = True
        Me.LabelControl9.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl9, 3)
        Me.LabelControl9.Location = New System.Drawing.Point(490, 4)
        Me.LabelControl9.Name = "LabelControl9"
        Me.TablePanel2.SetRow(Me.LabelControl9, 0)
        Me.LabelControl9.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl9.TabIndex = 3
        Me.LabelControl9.Text = "ER"
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.BackColor = System.Drawing.Color.SeaGreen
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl8.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl8.Appearance.Options.UseBackColor = True
        Me.LabelControl8.Appearance.Options.UseFont = True
        Me.LabelControl8.Appearance.Options.UseForeColor = True
        Me.LabelControl8.Appearance.Options.UseTextOptions = True
        Me.LabelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl8, 2)
        Me.LabelControl8.Location = New System.Drawing.Point(328, 4)
        Me.LabelControl8.Name = "LabelControl8"
        Me.TablePanel2.SetRow(Me.LabelControl8, 0)
        Me.LabelControl8.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl8.TabIndex = 2
        Me.LabelControl8.Text = "IPD"
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.BackColor = System.Drawing.Color.CornflowerBlue
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl7.Appearance.Options.UseBackColor = True
        Me.LabelControl7.Appearance.Options.UseFont = True
        Me.LabelControl7.Appearance.Options.UseForeColor = True
        Me.LabelControl7.Appearance.Options.UseTextOptions = True
        Me.LabelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl7, 1)
        Me.LabelControl7.Location = New System.Drawing.Point(165, 4)
        Me.LabelControl7.Name = "LabelControl7"
        Me.TablePanel2.SetRow(Me.LabelControl7, 0)
        Me.LabelControl7.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl7.TabIndex = 1
        Me.LabelControl7.Text = "OPD"
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Appearance.Options.UseFont = True
        Me.LabelControl6.Appearance.Options.UseTextOptions = True
        Me.LabelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.TablePanel2.SetColumn(Me.LabelControl6, 0)
        Me.LabelControl6.Location = New System.Drawing.Point(3, 4)
        Me.LabelControl6.Name = "LabelControl6"
        Me.TablePanel2.SetRow(Me.LabelControl6, 0)
        Me.LabelControl6.Size = New System.Drawing.Size(156, 21)
        Me.LabelControl6.TabIndex = 0
        Me.LabelControl6.Text = "PATIENT TYPE:"
        '
        'lblHeader
        '
        Me.lblHeader.Appearance.Font = New System.Drawing.Font("Segoe UI", 48.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Appearance.Options.UseFont = True
        Me.lblHeader.Appearance.Options.UseTextOptions = True
        Me.lblHeader.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblHeader.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.lblHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeader.Location = New System.Drawing.Point(2, 2)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(1299, 86)
        Me.lblHeader.TabIndex = 5
        Me.lblHeader.Text = "CHEMISTRY SECTION"
        '
        'tmChangeSection
        '
        Me.tmChangeSection.Enabled = True
        Me.tmChangeSection.Interval = 1000
        '
        'frmTatMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1303, 743)
        Me.Controls.Add(Me.TablePanel1)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frmTatMonitor"
        Me.Text = "Dashboard"
        CType(Me.TablePanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TablePanel1.ResumeLayout(False)
        CType(Me.dtWarding, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridWarding, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtCompleted, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridCompleted, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtProcessing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridProcessing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtCheckIn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridCheckin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.TablePanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TablePanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TablePanel1 As DevExpress.Utils.Layout.TablePanel
    Friend WithEvents dtCompleted As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridCompleted As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents dtProcessing As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridProcessing As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents dtCheckIn As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridCheckin As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents tmCheckin As Timer
    Friend WithEvents dtWarding As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridWarding As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PanelControl1 As PanelControl
    Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TablePanel2 As TablePanel
    Friend WithEvents LabelControl12 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl11 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl10 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl9 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl13 As LabelControl
    Friend WithEvents tmChangeSection As Timer
End Class
