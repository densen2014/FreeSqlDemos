Imports CS_Lib

Public Class Form1

    Dim service = New Class1()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '初始化demo数据
        service.init()

        RefreshData()

    End Sub

    Private Sub RefreshData()
        service.RefreshData()
        Console.WriteLine("ItemList: " & service.ItemList.Count())
        DataGridView1.DataSource = service.ItemList
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        service.Add()
        RefreshData()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RefreshData()
    End Sub
End Class
