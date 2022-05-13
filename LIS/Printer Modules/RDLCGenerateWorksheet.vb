﻿Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Threading.Thread
Imports System.Text.RegularExpressions

Imports System.Text
Imports DevExpress

Imports Microsoft.Reporting
Imports MySql.Data.MySqlClient

Module RDLCGenerateWorksheet
    Public Function GenerateReport(ByVal dtName As String, ByVal dtFrom As DateTimePicker, ByVal dtTo As DateTimePicker, ByVal rdlcName As String, ByVal ReportViewerData As Microsoft.Reporting.WinForms.ReportViewer, ByVal dtType As String)
        'Clear all datasource
        ReportViewerData.LocalReport.DataSources.Clear()

        Connect()
        Dim sql As String
        Dim dt As DataTable = New DataTable
        Dim command As New MySqlCommand()

        command.Parameters.Clear()
        command.Parameters.AddWithValue("DateFrom", Format(dtFrom.Value, "yyyy-MM-dd"))
        command.Parameters.AddWithValue("DateTo", Format(dtTo.Value, "yyyy-MM-dd"))

        If dtType = "All" Then
            sql = "worksheet_" & dtName
        Else
            sql = "worksheet_" & dtName & "_type"

            If dtType = "Out Patient" Then
                command.Parameters.AddWithValue("@Type", "OPD")
            ElseIf dtType = "ER" Then
                command.Parameters.AddWithValue("@Type", "ER")
            Else
                command.Parameters.AddWithValue("@Type", "IPD")
            End If
        End If

        command.Connection = conn
        command.CommandType = CommandType.StoredProcedure
        command.CommandText = sql
        command.ExecuteNonQuery()
        Dim adapter As New MySqlDataAdapter(command)
        adapter.Fill(dt)
        Disconnect()

        'Set Report source data for Dataset1
        Dim ReportDataSource1 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        ReportDataSource1.Name = "DataSet1"
        ReportDataSource1.Value = dt
        ReportViewerData.LocalReport.DataSources.Add(ReportDataSource1)

        'Connect to Database for Dataset2
        Connect()
        Dim dt1 As DataTable = New DataTable
        Dim command1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM `company_profile`", conn)

        'Fill adapter set in Dataset2
        Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter(command1)
        adapter1.Fill(dt1)
        Disconnect()

        'Set Report source data for Dataset2
        Dim ReportDataSource2 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        ReportDataSource2.Name = "DataSet2"
        ReportDataSource2.Value = dt1
        ReportViewerData.LocalReport.DataSources.Add(ReportDataSource2)
        ReportViewerData.LocalReport.ReportPath = Application.StartupPath & "\Reports\Worksheet\" & rdlcName

        'Set Parameters for reporting
        Dim DateFrom As New Microsoft.Reporting.WinForms.ReportParameter("DateFrom1", Format(dtFrom.Value, "yyyy-MM-dd"))
        Dim DateTo As New Microsoft.Reporting.WinForms.ReportParameter("DateTo1", Format(dtTo.Value, "yyyy-MM-dd"))
        Dim CurUser As New Microsoft.Reporting.WinForms.ReportParameter("CurrUser", CurrUser)
        ReportViewerData.LocalReport.SetParameters(DateFrom)
        ReportViewerData.LocalReport.SetParameters(DateTo)
        ReportViewerData.LocalReport.SetParameters(CurUser)

        'Set display format
        ReportViewerData.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout)

        'Refresh Report
        ReportViewerData.RefreshReport()
        Return (rdlcName)
    End Function
End Module
