Imports System.ComponentModel
Imports FreeSql.DataAnnotations

Public Class Form1
    Dim fsql As IFreeSql = New FreeSql.FreeSqlBuilder().UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1").UseAutoSyncStructure(True).UseMonitorCommand(Sub(cmd) Console.Write(cmd.CommandText)).Build()

    Dim ItemList = New List(Of Item)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '初始化demo数据

        Dim ItemList = New List(Of Item)() From {
            New Item() With {
                .Text = "假装 First item",
                .Description = "This is an item description."
            },
            New Item() With {
                .Text = "的哥 Second item",
                .Description = "This is an item description."
            },
            New Item() With {
                .Text = "四风 Third item",
                .Description = "This is an item description."
            },
            New Item() With {
                .Text = "加州 Fourth item",
                .Description = "This is an item description."
            },
            New Item() With {
                .Text = "阳光 Fifth item",
                .Description = "This is an item description."
            },
            New Item() With {
                .Text = "孔雀 Sixth item",
                .Description = "This is an item description."
            }
        }

        If fsql.Select(Of Item)().Count() = 0 Then
            fsql.Insert(Of Item)().AppendData(ItemList).ExecuteAffrows()
        End If

        RefreshData()

    End Sub

    Private Sub RefreshData()
        ItemList = fsql.Select(Of Item)().ToList()

        Console.WriteLine("ItemList: " & ItemList.Count())

        DataGridView1.DataSource = ItemList

    End Sub


    Public Class Item
        <Column(IsIdentity:=True)>
        <DisplayName("序号")>
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set
                m_Id = Value
            End Set
        End Property
        Private m_Id As Integer

        <DisplayName("名称")>
        Public Property Text() As String
            Get
                Return m_Text
            End Get
            Set
                m_Text = Value
            End Set
        End Property
        Private m_Text As String

        <DisplayName("描述")>
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set
                m_Description = Value
            End Set
        End Property
        Private m_Description As String
    End Class

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim newone = New Item() With {
                .Text = "新的" & Now.Ticks.ToString(),
                .Description = "This is an new item description."
            }

        fsql.Insert(Of Item)().AppendData(newone).ExecuteAffrows()
        RefreshData()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RefreshData()
    End Sub
End Class
