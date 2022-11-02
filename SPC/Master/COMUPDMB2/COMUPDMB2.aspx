<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeBehind="COMUPDMB2.aspx.vb" Inherits="SPC.COMUPDMB2" %>

<head id="Head1" runat="server">

<meta http-equiv="X-UA-Compatible" content="IE=8;IE=9;IE=10"/>
    <title></title>
    <link href="~/Images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <link href="~/Master/Site.css" rel="stylesheet" type="text/css" />

    <style>
        table th {
            background-color:#8DB4E2;
        }

        table tr:nth-child(odd) {
            background-color:#b0c4de;
        }
        table tr:nth-child(even) {
            background-color:white;
        }

        table td {
            padding:2px 2px 2px 2px ;
        }
        table td:nth-child(1) {
            text-align:center;
        }
    </style>

</head>


<body>

    <div style="width:1000px; height:800px; padding-top:10px; margin-left:auto;margin-right:auto;">

    <form id="form1" runat="server">
        <div style="float:left; height:800px;">
            <asp:GridView ID="grvList1" runat="server">
            </asp:GridView>
        </div>
        <div style="float:left; height:800px; overflow-y:scroll; padding-left:10px;">
            <asp:GridView ID="grvList2" runat="server">
            </asp:GridView>
        </div>
    </form>

    </div>


</body>