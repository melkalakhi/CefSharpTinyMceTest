Imports CefSharp.WinForms
Imports CefSharp
Imports System.Threading
Imports System.Threading.Tasks

Public Class Form1

    Private _ChromeBrowser As ChromiumWebBrowser

    Public Sub SetContent(html As String)
        html = EscapeHtml(html)
        Dim script = "tinyMCE.get('mytextarea').setContent(""" & html & """);"
        _ChromeBrowser.ExecuteScriptAsync(script)
    End Sub

    Public Function GetContent() As Task(Of CefSharp.JavascriptResponse)
        Dim task = _ChromeBrowser.EvaluateScriptAsync("function GetContent() {return tinyMCE.get('mytextarea').getContent();} GetContent();")
        Return task
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        CefSharp.Cef.Initialize()
        Dim url = New Uri("file:///" + IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tinymce.html").Replace("\\", "/"))
        _ChromeBrowser = New ChromiumWebBrowser(url.ToString())
        TableLayoutPanel1.Controls.Add(_ChromeBrowser, 0, 0)
        AddHandler _ChromeBrowser.FrameLoadEnd, AddressOf OnFrameLoadEnd
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        CefSharp.Cef.Shutdown()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SetContent("<p>Test<b>Lo'<ool""</b>OOOO</p>")
    End Sub

    Private Function EscapeHtml(unsafe As String) As String
        Return unsafe.Replace("""", "&quot;").Replace("'", "&#039;")
    End Function

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim response = Await GetContent()
        MessageBox.Show(response.Result.ToString())
    End Sub

    Private Sub OnFrameLoadEnd(sender As Object, e As FrameLoadEndEventArgs)
        SetContent("<p>START</p>")
    End Sub

End Class
