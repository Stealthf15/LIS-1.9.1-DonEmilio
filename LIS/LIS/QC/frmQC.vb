Imports System.IO
Public Class frmQC
    Private _No_of_Run As String
    Private _run As String
    Private Sub LoadData()
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@TestName", cboLimit.Text)
        rs.Parameters.Add("@Date_From", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtFrom.Value, "yyyy-MM-dd")
        rs.Parameters.Add("@Date_To", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtTo.Value, "yyyy-MM-dd")
        rs.Parameters.AddWithValue("@ControlID", cboControl.Text)
        rs.Parameters.AddWithValue("@LotNo", cboLot.Text)
        rs.Parameters.AddWithValue("@Instrument", cboMachines.Text)

        LoadRecordsOnLVSQL(lvList, "SELECT DISTINCT `universal_id`, `test_code` FROM `control_result` WHERE `test_type` = @TestName AND `sample_id` = @ControlID AND `instrument` = @Instrument AND `lot_no` = @LotNo AND (`date` BETWEEN @Date_From AND @Date_To)", 1)
    End Sub

    Private Sub frmQC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AutoLoadTestName()
    End Sub

    Private Sub lvList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvList.SelectedIndexChanged
        Dim Ave, stdDevP1, stdDevP3, stdDevP2, stdDevN1, stdDevN2, stdDevN3, sd As Double
        Try
            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@TestCode", lvList.FocusedItem.SubItems(1).Text)
            rs.Parameters.Add("@Date_From", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtFrom.Value, "yyyy-MM-dd")
            rs.Parameters.Add("@Date_To", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtTo.Value, "yyyy-MM-dd")
            rs.Parameters.AddWithValue("@ControlID", cboControl.Text)
            rs.Parameters.AddWithValue("@LotNo", cboLot.Text)
            rs.Parameters.AddWithValue("@Instrument", cboMachines.Text)
            Try
                Connect()
                rs.Connection = conn
                rs.CommandText = "SELECT `target`, `sd`, `ul`, `ll`, `plus_one`, `minus_one`, `plus_three`, `minus_three` FROM `control_target` WHERE `test_code` = @TestCode AND `control_id` = @ControlID  AND `instrument` = @Instrument ORDER BY `id`"
                reader = rs.ExecuteReader
                reader.Read()
                Ave = Val(reader(0))
                sd = Val(reader(1))

                stdDevP1 = Val(reader(4))
                stdDevN1 = Val(reader(5))

                stdDevP2 = Val(reader(2))
                stdDevN2 = Val(reader(3))

                stdDevP3 = Val(reader(6))
                stdDevN3 = Val(reader(7))
                Disconnect()
            Catch ex As Exception
                Disconnect()
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Chart1.Titles.Clear()
                Chart1.Series.Clear()
                Exit Sub
            End Try

            Chart1.Titles.Clear()
            Chart1.Series.Clear()

            Chart1.Titles.Add(cboLimit.Text & " Quality Control")
            Chart1.Titles.Add(lvList.FocusedItem.Text & "(" & lvList.FocusedItem.SubItems(1).Text & ")")
            Chart1.Titles.Add("(" & cboControl.Text & ") " & cboMachines.Text)
            Chart1.Titles.Add("Lot No.: " & cboLot.Text)
            Chart1.Titles.Add(Format(dtFrom.Value, "yyyy-MM-dd") & " - " & Format(dtTo.Value, "yyyy-MM-dd"))
            Chart1.Titles(0).Font = New Font("Tahoma", 12, FontStyle.Bold)
            Chart1.Titles(1).Font = New Font("Tahoma", 10, FontStyle.Bold)
            Chart1.Titles(2).Font = New Font("Tahoma", 10, FontStyle.Regular)
            Chart1.Titles(3).Font = New Font("Tahoma", 10, FontStyle.Regular)
            Chart1.Titles(4).Font = New Font("Tahoma", 10, FontStyle.Regular)

            Chart1.Series.Add("+3 SD " & stdDevP3)
            Chart1.Series("+3 SD " & stdDevP3).BorderWidth = 3
            Chart1.Series("+3 SD " & stdDevP3).Color = Color.Pink
            Chart1.Series("+3 SD " & stdDevP3).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("+3 SD " & stdDevP3).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("+2 SD " & stdDevP2)
            Chart1.Series("+2 SD " & stdDevP2).BorderWidth = 3
            Chart1.Series("+2 SD " & stdDevP2).Color = Color.LightGreen
            Chart1.Series("+2 SD " & stdDevP2).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("+2 SD " & stdDevP2).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("+1 SD " & stdDevP1)
            Chart1.Series("+1 SD " & stdDevP1).BorderWidth = 3
            Chart1.Series("+1 SD " & stdDevP1).Color = Color.LightBlue
            Chart1.Series("+1 SD " & stdDevP1).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("+1 SD " & stdDevP1).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("Mean " & Ave)
            Chart1.Series("Mean " & Ave).BorderWidth = 3
            Chart1.Series("Mean " & Ave).Color = Color.Orange
            Chart1.Series("Mean " & Ave).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("Mean " & Ave).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("-1 SD " & stdDevN1)
            Chart1.Series("-1 SD " & stdDevN1).BorderWidth = 3
            Chart1.Series("-1 SD " & stdDevN1).Color = Color.LightBlue
            Chart1.Series("-1 SD " & stdDevN1).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("-1 SD " & stdDevN1).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("-2 SD " & stdDevN2)
            Chart1.Series("-2 SD " & stdDevN2).BorderWidth = 3
            Chart1.Series("-2 SD " & stdDevN2).Color = Color.LightGreen
            Chart1.Series("-2 SD " & stdDevN2).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("-2 SD " & stdDevN2).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("-3 SD " & stdDevN3)
            Chart1.Series("-3 SD " & stdDevN3).BorderWidth = 3
            Chart1.Series("-3 SD " & stdDevN3).Color = Color.Pink
            Chart1.Series("-3 SD " & stdDevN3).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("-3 SD " & stdDevN3).Font = New Font("Tahoma", 10, FontStyle.Bold)

            Chart1.Series.Add("Control Value")
            Chart1.Series("Control Value").MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
            Chart1.Series("Control Value").MarkerSize = 1
            Chart1.Series("Control Value").MarkerColor = Color.Black
            Chart1.Series("Control Value").IsValueShownAsLabel = True
            Chart1.Series("Control Value").BorderWidth = 3
            Chart1.Series("Control Value").Color = Color.Black
            Chart1.Series("Control Value").IsXValueIndexed = True
            Chart1.Series("Control Value").ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Chart1.Series("Control Value").Font = New Font("Tahoma", 8, FontStyle.Bold)

            'Disconnect2()
            'Connect2()
            'rs2.Parameters.Clear()
            'rs2.Connection = conn2
            'rs2.Parameters.AddWithValue("@ControlID", cboControl.Text)
            'rs2.CommandText = "SELECT `run_count`FROM `control_result` WHERE `sample_id` = @controlID"
            'reader2 = rs2.ExecuteReader
            'While reader2.Read
            '    _No_of_Run = reader2(0).ToString
            'End While

            'For i As Integer = 1 To _No_of_Run Step 1
            '    _run = i
            'Next
            'Disconnect2()

            Connect()
            rs.Connection = conn
            rs.CommandText = "SELECT `measurement`, DATE_FORMAT(`date`, '%m/%d') FROM `control_result` WHERE `test_code` = @TestCode AND `sample_id` = @ControlID AND `instrument` = @Instrument AND `lot_no` = @LotNo AND (`date` BETWEEN @Date_From AND @Date_To) ORDER BY `date`"
            reader = rs.ExecuteReader
            For x As Integer = 1 To 31 Step 1
                While reader.Read
                    Chart1.Series("Mean " & Ave).Points.AddXY(reader(1).ToString, Ave)
                    Chart1.Series("+1 SD " & stdDevP1).Points.AddXY(reader(1).ToString, stdDevP1)
                    Chart1.Series("-1 SD " & stdDevN1).Points.AddXY(reader(1).ToString, stdDevN1)

                    Chart1.Series("+2 SD " & stdDevP2).Points.AddXY(reader(1).ToString, stdDevP2)
                    Chart1.Series("-2 SD " & stdDevN2).Points.AddXY(reader(1).ToString, stdDevN2)

                    Chart1.Series("+3 SD " & stdDevP3).Points.AddXY(reader(1).ToString, stdDevP3)
                    Chart1.Series("-3 SD " & stdDevN3).Points.AddXY(reader(1).ToString, stdDevN3)
                    Chart1.Series("Control Value").Points.AddXY(reader(1).ToString, reader(0).ToString)
                End While
            Next
            Disconnect()

            Chart1.Series("Control Value").ChartArea = "ChartArea1"
            Chart1.Series("Mean " & Ave).ChartArea = "ChartArea1"
            Chart1.Series("+1 SD " & stdDevP1).ChartArea = "ChartArea1"
            Chart1.Series("-1 SD " & stdDevN1).ChartArea = "ChartArea1"

            Chart1.Series("+2 SD " & stdDevP2).ChartArea = "ChartArea1"
            Chart1.Series("-2 SD " & stdDevN2).ChartArea = "ChartArea1"

            Chart1.Series("+3 SD " & stdDevP3).ChartArea = "ChartArea1"
            Chart1.Series("-3 SD " & stdDevN3).ChartArea = "ChartArea1"

            Chart1.ChartAreas(0).AxisX.Minimum = 1
            Chart1.ChartAreas(0).AxisX.Interval = 1

            Chart1.ChartAreas(0).AxisY.Minimum = Ave - (sd * 6)
            Chart1.ChartAreas(0).AxisY.Maximum = Ave + (sd * 6)
            Chart1.ChartAreas(0).AxisY.Interval = sd * 2
            'Chart1.ChartAreas(0).AxisY.IntervalOffset = 1

            Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = -45
        Catch ex As Exception
            Exit Sub
            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub cboLimit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLimit.SelectedIndexChanged
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@Section", cboLimit.Text)

        Me.cboControl.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT DISTINCT `sample_id` FROM `control_result` WHERE `test_type` = @Section"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboControl.Properties.Items.Add(reader(0).ToString)
        End While
        Disconnect()
    End Sub

    Public Sub AutoLoadTestName()
        Me.cboLimit.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT * FROM `testtype` WHERE `test_name` NOT LIKE 'All' ORDER BY `test_name`"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboLimit.Properties.Items.Add(reader(1).ToString)
        End While
        Disconnect()

        Me.cboMachines.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT DISTINCT `instrument` FROM `control_setting` ORDER BY `instrument` DESC"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboMachines.Properties.Items.Add(reader(0).ToString)
        End While
        Disconnect()
    End Sub

    Private Sub btnClose_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnClose.ItemClick
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub btnPreview_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPreview.ItemClick
        PrintDocument1.DefaultPageSettings.Landscape = True
        PrintDocument1.DefaultPageSettings.PaperSize = New Printing.PaperSize("First custom size", 850, 1300)
        Chart1.Printing.PrintDocument = PrintDocument1 ' this enables the adding of other material to the page on which the chart is printed
        Chart1.Printing.PrintPreview()
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Chart1.Printing.PrintPaint(e.Graphics, New Rectangle(0, 0, Chart1.Width - 50, Chart1.Height)) ' draw the chart
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        LoadData()
        Chart1.Titles.Clear()
        Chart1.Series.Clear()
    End Sub

    Private Sub btnView_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnView.ItemClick
        frmQCResults.MainSampleID = cboControl.Text
        frmQCResults.Section = cboLimit.Text
        frmQCResults.ShowDialog()
    End Sub

    Private Sub btnPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrint.ItemClick
        Try
            Dim dt As String = Date.Now.ToString("MM-dd-yyyy")
            Dim Path As String = Application.StartupPath & "\LJGraph\" & lvList.FocusedItem.SubItems(1).Text & " - " & dt
            Dim start As String = Application.StartupPath & "\LJGraph\" & Format(dtFrom.Value, "yyyy-MM-dd") & " to " & Format(dtTo.Value, "yyyy-MM-dd")

            If Not Directory.Exists(start) Then
                Directory.CreateDirectory(start)
                If Directory.Exists(start) Then
                    Me.Chart1.SaveImage(start & "\" & lvList.FocusedItem.SubItems(0).Text & ".jpg", Drawing.Imaging.ImageFormat.Jpeg)
                    MessageBox.Show("LJ Graph successfully saved as Image.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Else
                Me.Chart1.SaveImage(start & "\" & lvList.FocusedItem.SubItems(0).Text & ".jpg", Drawing.Imaging.ImageFormat.Jpeg)
                MessageBox.Show("LJ Graph successfully saved as Image.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
                'Me.Chart1.SaveImage(Application.StartupPath & "\LJGraph\" & lvList.FocusedItem.SubItems(0).Text & ".jpg", Drawing.Imaging.ImageFormat.Jpeg)
                'MessageBox.Show("LJ Graph successfully saved as Image.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Exit Sub
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While Saving", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub

    Private Sub btnRefresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnRefresh.ItemClick
        LoadData()
        Chart1.Titles.Clear()
        Chart1.Series.Clear()
    End Sub

    Private Sub frm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'MainFOrm.aceFecal.Appearance.Normal.BackColor = Color.FromArgb(6, 31, 71)
        MainFOrm.btnQC.Appearance.Normal.BackColor = Color.FromArgb(16, 110, 190)
        MainFOrm.btnQC.Appearance.Normal.ForeColor = Color.FromArgb(255, 255, 255)
    End Sub

    Private Sub frm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MainFOrm.lblTitle.Text = ""
        MainFOrm.btnQC.Appearance.Normal.BackColor = Color.FromArgb(240, 240, 240)
        MainFOrm.btnQC.Appearance.Normal.ForeColor = Color.FromArgb(27, 41, 62)
        Me.Dispose()
    End Sub

    Private Sub cboControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboControl.SelectedIndexChanged
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@Control", cboControl.Text)

        Me.cboLot.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT DISTINCT `lot_no` FROM `control_result` WHERE `sample_id` = @Control"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboLot.Properties.Items.Add(reader(0).ToString)
        End While
        Disconnect()
    End Sub

    Private Sub cboMachines_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMachines.SelectedIndexChanged
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@Machine", cboMachines.Text)

        Me.cboControl.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT DISTINCT `sample_id` FROM `control_result` WHERE `instrument` = @Machine"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboControl.Properties.Items.Add(reader(0).ToString)
        End While
        Disconnect()
    End Sub
End Class