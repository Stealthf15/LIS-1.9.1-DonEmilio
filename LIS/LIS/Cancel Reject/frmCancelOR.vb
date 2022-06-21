Public Class frmCancelOR

    Public MainID As String
    Public ID As String
    Public sID As String
    Public pID As String
    Public pName As String
    Public pTest As String
    Public pSection As String
    Public pSubSection As String

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@ID", ID)
        rs.Parameters.AddWithValue("@SampleID", sID)
        rs.Parameters.AddWithValue("@Section", pSection)
        rs.Parameters.AddWithValue("@SubSection", pSubSection)
        rs.Parameters.AddWithValue("@Comment", txtComment.Text)

        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "UPDATE `tmpworklist` SET `status` = 'Cancelled' WHERE `id` = @ID"
        rs.ExecuteNonQuery()
        Disconnect()

        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @SampleID AND `section` = @Section AND `sub_section` = @SubSection"
        reader = rs.ExecuteReader
        reader.Read()
        If reader.HasRows Then
            Disconnect()
            UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @SampleID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
        Else
            Disconnect()
            SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
        End If
        Disconnect()

        'Log activity
        SpecimenActivity("z_logs_specimen", sID, pID, pName, CurrUser, "Specimen Cancelled", txtComment.Text, pTest, pSection, pSubSection)

        Me.Close()

        MessageBox.Show("Patient order successfully cancelled.", "Cancel Sample", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub frmRejectOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtComment.Focus()
    End Sub

    Private Sub frmRejectOrder_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub
End Class