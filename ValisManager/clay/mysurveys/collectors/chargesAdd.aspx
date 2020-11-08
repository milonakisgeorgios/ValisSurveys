<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="chargesAdd.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.chargesAdd" ClientIDMode="Static" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

    </style>
    <script>
       $(document).ready(function () {

           $('#QuantityLimit').onlyUnSignedIntegers();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Add payment</span>
    </div>

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>Choose a payment</label>
            </div>
            <asp:DropDownList ID="Payment" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>Define a maximum quantity limit </label>
            </div>
            <asp:TextBox ID="QuantityLimit" runat="server"></asp:TextBox>
        </div>
    </div>
    <div>
        <asp:LinkButton ID="createCollectorPayment" runat="server" CssClass="greenbutton" OnClick="createCollectorPayment_Click">Add Payment &gt;&gt;</asp:LinkButton>
    </div>


</asp:Content>
