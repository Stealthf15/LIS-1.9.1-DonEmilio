Public Class frmNoCopiesBC

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            frmPhlebotomy.NoCopies = txtCopies.Text
            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class