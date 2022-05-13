Public Class frmImmunoPatientList

    Public Type As String = ""

    Public Sub LoadRecords()
        Try
            LoadRecordsOnLV(lvList, "`patient_info`", 8, "patient_id")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmPatientList_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub frmNewOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadRecords()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.ItemClick

        If Me.lvList.SelectedItems.Count > 0 Then
            If Type = "New" Then
                frmImmunoNew.txtPatientID.Text = lvList.FocusedItem.SubItems(0).Text
                frmImmunoNew.txtName.Text = lvList.FocusedItem.SubItems(1).Text
                frmImmunoNew.cboSex.Text = lvList.FocusedItem.SubItems(2).Text
                frmImmunoNew.dtBDate.Text = lvList.FocusedItem.SubItems(3).Text
                frmImmunoNew.txtAge.Text = lvList.FocusedItem.SubItems(4).Text
                frmImmunoNew.txtAddress.Text = lvList.FocusedItem.SubItems(5).Text
                frmImmunoNew.txtContact.Text = lvList.FocusedItem.SubItems(6).Text
                Me.Close()
                Me.Dispose()
            ElseIf Type = "Old" Then
                frmImmunoOrdered.txtPatientID.Text = lvList.FocusedItem.SubItems(0).Text
                frmImmunoOrdered.txtName.Text = lvList.FocusedItem.SubItems(1).Text
                frmImmunoOrdered.cboSex.Text = lvList.FocusedItem.SubItems(2).Text
                frmImmunoOrdered.dtBDate.Text = lvList.FocusedItem.SubItems(3).Text
                frmImmunoOrdered.txtAge.Text = lvList.FocusedItem.SubItems(4).Text
                frmImmunoOrdered.txtAddress.Text = lvList.FocusedItem.SubItems(5).Text
                frmImmunoOrdered.txtContact.Text = lvList.FocusedItem.SubItems(6).Text
                Me.Close()
                Me.Dispose()
            End If
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.ItemClick
        Me.Close()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.ItemClick
        LoadRecords()
    End Sub

    Private Sub txtSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        SearchRecordsOnLV(lvList, "`patient_info`", 8, "`name`", Me.txtSearch.Text, "patient_id")
    End Sub
End Class