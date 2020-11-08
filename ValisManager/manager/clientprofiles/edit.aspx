<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.clientprofiles.edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            $('input.integer').onlyUnSignedNumbers();
        });
    </script>
    <style type="text/css">

        input#Name {
            width: 408px;
        }
        textarea#Comment {
            width: 408px;
        }
        input.integer
        {
            width: 100px;
        }

        div.column{
            float: left; margin-right: 88px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Edit Profile Details - <%: this.SelectedProfile != null ? this.SelectedProfile.Name : string.Empty %></span>
        <a class="form-header-link" href='<%=_UrlSuffix("list.aspx") %>'>(back to clients list)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>


    
    <div style="position: relative;">

        <div class="form-wrapper">

            <div class="form-line">
                <asp:Label ID="Label1" runat="server" Text="Name" AssociatedControlID="Name"></asp:Label><asp:TextBox ID="Name" runat="server" required="required"></asp:TextBox><%=GetRequiredIcon() %></div>        
            <div class="form-line">
                <asp:Label ID="Label2" runat="server" Text="Comment" AssociatedControlID="Comment"></asp:Label><asp:TextBox ID="Comment" runat="server" TextMode="MultiLine"></asp:TextBox></div>
            
        </div>

        
        <div class="form-wrapper column">
            <div class="form-line">
                <asp:Label ID="Label3" runat="server" Text="UseCredits" AssociatedControlID="UseCredits"></asp:Label><asp:CheckBox ID="UseCredits" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label14" runat="server" Text="CanTranslateSurveys" AssociatedControlID="CanTranslateSurveys"></asp:Label><asp:CheckBox ID="CanTranslateSurveys" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label15" runat="server" Text="CanUseSurveyTemplates" AssociatedControlID="CanUseSurveyTemplates"></asp:Label><asp:CheckBox ID="CanUseSurveyTemplates" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label16" runat="server" Text="CanUseQuestionTemplates" AssociatedControlID="CanUseQuestionTemplates"></asp:Label><asp:CheckBox ID="CanUseQuestionTemplates" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label17" runat="server" Text="CanCreateWebLinkCollectors" AssociatedControlID="CanCreateWebLinkCollectors"></asp:Label><asp:CheckBox ID="CanCreateWebLinkCollectors" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label18" runat="server" Text="CanCreateEmailCollectors" AssociatedControlID="CanCreateEmailCollectors"></asp:Label><asp:CheckBox ID="CanCreateEmailCollectors" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label19" runat="server" Text="CanCreateWebsiteCollectors" AssociatedControlID="CanCreateWebsiteCollectors"></asp:Label><asp:CheckBox ID="CanCreateWebsiteCollectors" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label20" runat="server" Text="CanUseSkipLogic" AssociatedControlID="CanUseSkipLogic"></asp:Label><asp:CheckBox ID="CanUseSkipLogic" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label21" runat="server" Text="CanExportData" AssociatedControlID="CanExportData"></asp:Label><asp:CheckBox ID="CanExportData" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label22" runat="server" Text="CanExportReport" AssociatedControlID="CanExportReport"></asp:Label><asp:CheckBox ID="CanExportReport" runat="server" />
            </div>
            <div class="form-line">
                <asp:Label ID="Label23" runat="server" Text="CanUseWebAPI" AssociatedControlID="CanUseWebAPI"></asp:Label><asp:CheckBox ID="CanUseWebAPI" runat="server" />
            </div>
        </div>


        <div class="form-wrapper column">
            <div class="form-line">
                <asp:Label ID="Label4" runat="server" Text="MaxNumberOfUsers" AssociatedControlID="MaxNumberOfUsers"></asp:Label><asp:TextBox ID="MaxNumberOfUsers" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label5" runat="server" Text="MaxNumberOfSurveys" AssociatedControlID="MaxNumberOfSurveys"></asp:Label><asp:TextBox ID="MaxNumberOfSurveys" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label6" runat="server" Text="MaxNumberOfLists" AssociatedControlID="MaxNumberOfLists"></asp:Label><asp:TextBox ID="MaxNumberOfLists" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label7" runat="server" Text="MaxNumberOfRecipientsPerList" AssociatedControlID="MaxNumberOfRecipientsPerList"></asp:Label><asp:TextBox ID="MaxNumberOfRecipientsPerList" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label8" runat="server" Text="MaxNumberOfRecipientsPerMessage" AssociatedControlID="MaxNumberOfRecipientsPerMessage"></asp:Label><asp:TextBox ID="MaxNumberOfRecipientsPerMessage" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label9" runat="server" Text="MaxNumberOfCollectorsPerSurvey" AssociatedControlID="MaxNumberOfCollectorsPerSurvey"></asp:Label><asp:TextBox ID="MaxNumberOfCollectorsPerSurvey" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label10" runat="server" Text="MaxNumberOfEmailsPerDay" AssociatedControlID="MaxNumberOfEmailsPerDay"></asp:Label><asp:TextBox ID="MaxNumberOfEmailsPerDay" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label11" runat="server" Text="MaxNumberOfEmailsPerWeek" AssociatedControlID="MaxNumberOfEmailsPerWeek"></asp:Label><asp:TextBox ID="MaxNumberOfEmailsPerWeek" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
            <div class="form-line">
                <asp:Label ID="Label12" runat="server" Text="MaxNumberOfEmailsPerMonth" AssociatedControlID="MaxNumberOfEmailsPerMonth"></asp:Label><asp:TextBox ID="MaxNumberOfEmailsPerMonth" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>

            <div class="form-line">
                <asp:Label ID="Label13" runat="server" Text="MaxNumberOfEmails" AssociatedControlID="MaxNumberOfEmails"></asp:Label><asp:TextBox ID="MaxNumberOfEmails" runat="server" CssClass="integer" MaxLength="9"></asp:TextBox>
            </div>
        </div>

    </div>

</asp:Content>
