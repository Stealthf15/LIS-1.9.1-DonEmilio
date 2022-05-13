Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports System.Linq
Imports System.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.BarCode

Public Class frmAssyInfo

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@LotNo", txtLotNo.Text)
        rs.Parameters.AddWithValue("@Expiry", txtExpiry.Text)
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "INSERT INTO `rat_lot_no` (`lot_number`, `expiry_date`) VALUES (@LotNo, @Expiry)"
        rs.ExecuteNonQuery()
        Disconnect()

        Me.Close()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class