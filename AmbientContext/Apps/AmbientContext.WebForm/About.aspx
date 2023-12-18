<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="AmbientContextWebForm.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.  <%= this.dr.ToString()%> <%: this.dr.Name + this.dr.Age %></h2>
        <h3>Your application description page.</h3>
        <p>Use this area to provide additional information.</p>
        
        
    </main>
</asp:Content>
