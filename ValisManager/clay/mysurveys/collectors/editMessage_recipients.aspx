<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="editMessage_recipients.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.editMessage_recipients" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
        |<style type="text/css">
             
            .form-paragraph-title
            {
                color: #333333;
                font-size: 12px;
                font-weight: normal;
                margin: 0 0 6px;
                padding: 0;
            }
            .form-paragraph-title label
            {
                font-size: 14px;
                font-weight: bold;
                display: inline-block;
                margin: 0px 6px 0px 6px;
            }
            .form-paragraph-line
            {
                font-size: 12px;
            }

             .form-paragraph-line select {
                margin-right: 6px;
                min-width: 120px;
             }
             .form-paragraph-line input[type=text] {
                margin-right: 6px;
             }
             .form-paragraph-line input[type=button] {
                margin-right: 6px;
             }
         </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Select Recipients</span>
    </div>
    <asp:Button ID="saveAndContinue" runat="server" Text="Save and Continue >>" CssClass="rightButton greenbutton" ClientIDMode="Static" OnClick="saveAndContinue_Click" />


    <div id="optionsWrapper">
        <div class="optionsTitle"></div>

        
        <%if(this.TotalNotEmailedRecipients > 0 ){ %>
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnNewAndUnsent" runat="server" GroupName="SendTo" ClientIDMode="Static" /><asp:Label ID="Label1" runat="server" Text="New/Unsent" AssociatedControlID="rdbtnNewAndUnsent"></asp:Label><span>(send to everyone who has not received an email message yet)</span>
                </div>
                <div class="form-paragraph-line indent1">The selection above matches <%=TotalNotEmailedRecipients %> potential recipients.</div>
            </div>
        </div>
        <%} %>
        
        <%if(this.TotalNotRespondedRecipients > 0){ %>
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnNotResponded" runat="server" GroupName="SendTo" ClientIDMode="Static" /><asp:Label ID="Label3" runat="server" Text="Not Responded" AssociatedControlID="rdbtnNotResponded"></asp:Label><span>(send to everyone who has received an email message, but has not responded)</span>
                </div>
                <div class="form-paragraph-line indent1">The selection above matches <%=TotalNotRespondedRecipients %> potential recipients.</div>
            </div>
        </div>
        <%} %>
        
        <%if(this.TotalRespondedRecipients > 0) { %>
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnAllResponded" runat="server" GroupName="SendTo" ClientIDMode="Static" /><asp:Label ID="Label4" runat="server" Text="Responded" AssociatedControlID="rdbtnAllResponded"></asp:Label><span>(send to everyone who has responded partially and/or completely)</span>
                </div>
                <div class="form-paragraph-line indent1">The selection above matches <%=TotalRespondedRecipients %> potential recipients.</div>
            </div>
        </div>
        <%} %>
        
        <%if(this.TotalNotEmailedRecipients != this.TotalRecipients){ %>
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnAll" runat="server" GroupName="SendTo" ClientIDMode="Static" /><asp:Label ID="Label5" runat="server" Text="All Emails" AssociatedControlID="rdbtnAll"></asp:Label><span>(send to everyone in your list, regardless of whether they have responded)</span>
                </div>
                <div class="form-paragraph-line indent1">The selection above matches <%=TotalRecipients %> potential recipients.</div>
            </div>
        </div>
        <%} %>

        <!--
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnCustom" runat="server" GroupName="SendTo" ClientIDMode="Static" /><asp:Label ID="Label2" runat="server" Text="Custom Criteria" AssociatedControlID="rdbtnCustom"></asp:Label><span>(send to everyone who matches the criteria you specify)</span>
                </div>
                <div class="form-paragraph-line indent1">
                    <div>Send to those people where</div>
                    <asp:DropDownList ID="ddlSearchField" runat="server"></asp:DropDownList><asp:DropDownList ID="ddlCriteria" runat="server"></asp:DropDownList><asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox><input id="btnPreview" type="button" value="button" />
                </div>
                <div class="form-paragraph-line indent1">The selection above has resulted in ?? potential recipients.</div>
            </div>
        </div>
        -->

    </div>

</asp:Content>
