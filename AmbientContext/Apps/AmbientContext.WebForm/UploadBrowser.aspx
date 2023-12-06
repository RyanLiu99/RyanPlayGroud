<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadBrowser.aspx.cs" Inherits="AmbientContextWebForm.Upload" %>

<%@ Import Namespace="System.Threading" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload B</title>
</head>
<body>
<%=(((AmbientContext.Shared.DotNetStandardLib.Models.MedrioPrincipal)Thread.CurrentPrincipal).Study!.ID) %>


<%=this.FileContent%>


    <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        
        <asp:Button ID="btnUpload" runat="server" Text="Upload"  />
      
    </form>
</body>
</html>
