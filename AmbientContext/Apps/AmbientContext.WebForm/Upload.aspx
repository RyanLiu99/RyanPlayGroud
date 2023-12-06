<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="AmbientContextWebForm.Upload" %>

<%@ Import Namespace="System.Threading" %>

<%:(((AmbientContext.Shared.DotNetStandardLib.Models.MedrioPrincipal)Thread.CurrentPrincipal).Study!.ID) %>

<%--
<%:this.FileContent%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        
        <asp:Button ID="btnUpload" runat="server" Text="Upload"  />
      
    </form>
</body>
</html>--%>
