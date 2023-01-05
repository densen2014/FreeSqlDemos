<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsNF48._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
        <asp:Button ID="buttonADD" runat="server" Text="ADD" OnClick="buttonADD_Click" />
        <asp:Button ID="buttonREFRESH" runat="server" Text="REFRESH" OnClick="buttonREFRESH_Click" />
            <asp:Label ID="LabelMsg" runat="server"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" class="table table-hover" Style="text-align: left; margin: 0px auto;" >
            </asp:GridView>
        <asp:FormView ID="FormView1" runat="server"></asp:FormView>

        </ContentTemplate>
    </asp:UpdatePanel>
     

</asp:Content>
