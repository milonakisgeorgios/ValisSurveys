<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="chargesEdit.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.chargesEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

        input[readonly=readonly]
        {
            border: 1px solid #d4d4d4;
            color: #808080;
        }
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
                <label>Selected Payment:</label>
            </div>
            <asp:TextBox ID="Payment" runat="server" ReadOnly="True" Width="300px"></asp:TextBox>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>UseOrder</label>
            </div>
            <asp:TextBox ID="UseOrder" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>Quantity limit</label>
            </div>
            <asp:TextBox ID="QuantityLimit" runat="server" Width="100px"></asp:TextBox>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>QuantityUsed</label>
            </div>
            <asp:TextBox ID="QuantityUsed" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>FirstChargeDt</label>
            </div>
            <asp:TextBox ID="FirstChargeDt" runat="server" ReadOnly="True" Width="130px"></asp:TextBox>
        </div>
    </div>
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <label>LastChargeDt</label>
            </div>
            <asp:TextBox ID="LastChargeDt" runat="server" ReadOnly="True" Width="130px"></asp:TextBox>
        </div>
    </div>

    <div>
        <asp:LinkButton ID="updateCollectorPayment" runat="server" CssClass="greenbutton" OnClick="updateCollectorPayment_Click" >Update Payment &gt;&gt;</asp:LinkButton>
    </div>


</asp:Content>
