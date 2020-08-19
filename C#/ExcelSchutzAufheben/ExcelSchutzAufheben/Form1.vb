Imports System.IO
Imports OfficeOpenXml

Public Class Form1

    Private Sub btn_start_Click(sender As System.Object, e As System.EventArgs) Handles btn_start.Click
        Dim fInfo As FileInfo
        Dim dInfo As DirectoryInfo
        Dim lfInfo As New List(Of FileInfo)
        Dim s As String = ""



        If Trim(Trim(txt_Pfad.Text)) <> "" Then

            If InputBox("Bitte geben Sie das Kennwort zum löschen der Excel Sicherungen ein.", "Excel Kennwörter aufheben") = "Gott" Then
                If File.Exists(Trim(txt_Pfad.Text)) = True Then
                    fInfo = New FileInfo(Trim(txt_Pfad.Text))
                    If UCase(fInfo.Extension) = ".XLSX" Or UCase(fInfo.Extension) = ".XLSM" Then
                        lfInfo.Add(fInfo)
                    End If
                End If
                If Directory.Exists(Trim(txt_Pfad.Text)) = True Then
                    dInfo = New DirectoryInfo(Trim(txt_Pfad.Text))
                    For Each fInfo In dInfo.GetFiles
                        If UCase(fInfo.Extension) = ".XLSX" Or UCase(fInfo.Extension) = ".XLSM" Then
                            lfInfo.Add(fInfo)
                        End If
                    Next
                End If
                PW_Entfernen(lfInfo)
            Else
                MsgBox("Falsches Kennwort. " & vbNewLine & " Aktion wird abgebrochen.", MsgBoxStyle.OkOnly, "Excel Kennwörter aufheben")
            End If

        End If
    End Sub


    Private Sub PW_Entfernen(plfInfo As List(Of FileInfo))
        Dim fInfo As FileInfo
        Dim pck As ExcelPackage
        Dim wBook As ExcelWorkbook
        Dim wSheet As ExcelWorksheet

        For Each fInfo In plfInfo
            pck = New ExcelPackage(fInfo)
            wBook = pck.Workbook
            'Wenn Makros verwendet werden, dann auch dieses Kennwort ausschalten.
            If UCase(fInfo.Extension) = ".XLSM" Then
                wBook.VbaProject.Protection.SetPassword("")
            End If
            wBook.Protection.LockRevision = False
            wBook.Protection.LockStructure = False
            wBook.Protection.LockWindows = False
            For Each wSheet In wBook.Worksheets
                wSheet.Protection.IsProtected = False
            Next
            pck.Save()
        Next
    End Sub



End Class
