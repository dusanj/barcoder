<!--Free / Open Source Software License
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY 1T3XT, 1T3XT DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL: http://itextpdf.com/terms-of-use/
The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, as required under Section 5 of the GNU Affero General Public License.
In accordance with Section 7(b) of the GNU Affero General Public License, you must retain the producer line in every PDF that is created or manipulated using iText.
You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory as soon as you develop commercial activities involving the iText software without disclosing the source code of your own applications. These activities include: offering paid services to customers as an ASP, serving PDFs on the fly in a web application, shipping iText with a closed source product. -->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CreatePDF._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>McBarCoder</title>
    <style type="text/css">
     body 
     {
         font-family: Courier, Courier New, Consolas;
         text-align:center;
         color: #333;         
     }
     a, a:active
     {
         color: #239;
         text-decoration: none;
     }
     a:hover
     {
         text-decoration: underline;
     }
     #page 
     {
         margin: 0px auto;
         width: 750px;
         height:750px;
         background-image: url('http://w8appsrv:8088/bg.png');
         background-repeat: no-repeat;
     }
     #TextBox1
     {
         font-size: 32px;
         border: 1px solid #666;
         font-family: Courier New;
                 
     }
     #Button1
     {
         color: #239;
         font-size: 12px;
                  
     }
     #Label1
     {
         letter-spacing: 3px;

     }
         
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
		<br />
        <br />
        <br />
		<br />
        <br />
        <asp:RadioButton ID="rbt_ean13" runat="server" Text="EAN-13" 
            AutoPostBack="True" GroupName="ean" Checked="True"
            oncheckedchanged="rbt_ean8_CheckedChanged" />
        <asp:RadioButton ID="rbt_ean8" runat="server" Text="EAN-8" 
            AutoPostBack="True" GroupName="ean" 
            oncheckedchanged="rbt_ean8_CheckedChanged" />
        <asp:RadioButton ID="rbt_qr" runat="server" Text="QR Code" 
            AutoPostBack="True" GroupName="ean" 
            oncheckedchanged="rbt_ean8_CheckedChanged" />
        <br />
        <br />
		<asp:TextBox ID="TextBox1" runat="server" MaxLength="13" Width="255px"  Height="55px" 
            ToolTip="Ovde dodju brojke za bar kod"></asp:TextBox>
		&nbsp;
        <asp:Button ID="Button1" runat="server" Text="EAN-13 -> pdf" 
            onclick="Button1_Click" Height="55px" Width="110px" 
            ToolTip="Ovde kliknes da dobijes PDF sa bar kodom." />
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="TextBox1" ErrorMessage="^ Upisi brojeve ^"></asp:RequiredFieldValidator> 
                   
        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
            ControlToValidate="TextBox1" ErrorMessage="Upisati tacno 13 brojeva." ValidationExpression="^[0-9]{13}" Display="Dynamic"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
            ControlToValidate="TextBox1" ErrorMessage="Upisati tacno 8 brojeva." ValidationExpression="^[0-9]{8}" Display="None"></asp:RegularExpressionValidator>
        <br />
        <asp:CheckBox ID="cbxGuardBars" runat="server" AutoPostBack="True" 
            Checked="True" oncheckedchanged="cbxGuardBars_CheckedChanged" Text="Guard Bars" 
            ToolTip="Ako je oznaceno bar kod se kreira sa duzim linijama koje odvajaju grupe cifara " />
&nbsp;<asp:CheckBox ID="cbxTextAbove" runat="server" Enabled="False" 
            Text="Text Above" 
            ToolTip="Brojevi iznad bar koda, ova opcija je moguca samo kada su Guard Bars deaktivirane" />
        <br />
        <br />
        <asp:Image ID="imgBarCode" runat="server" Visible="False" 
            onclick="imgBarCode_Click" Height="88px" Width="162px"/>
        <br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Preview</asp:LinkButton>
    </div>
    </form>
</body>
</html>
