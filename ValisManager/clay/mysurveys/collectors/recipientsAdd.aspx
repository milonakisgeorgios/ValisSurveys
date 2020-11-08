<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="recipientsAdd.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.recipientsAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

    </style>
    <script>
        function OnClick_rdButton()
        {
            if ($('#rdbtnManual').is(':checked'))
            {
                $('#panFromAddressBook').hide();
                $('#panFromCollector').hide();
                $('#panAddManually').show();
            }
            else if ($('#rdbtnFromList').is(':checked'))
            {
                $('#panAddManually').hide();
                $('#panFromCollector').hide();
                $('#panFromAddressBook').show();
            }
            else if ($('#rdbtnFromSurvey').is(':checked'))
            {
                $('#panAddManually').hide();
                $('#panFromAddressBook').hide();
                $('#panFromCollector').show();
            }
        }


        $(document).ready(function () {
            $('#rdbtnManual').click(OnClick_rdButton);
            $('#rdbtnFromList').click(OnClick_rdButton);
            $('#rdbtnFromSurvey').click(OnClick_rdButton);

            OnClick_rdButton();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Add recipients</span>
    </div>
    <a class="backToList rightButton" id="btnBackToList" href="recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>"><< Back to Recipients</a>


    <div id="accordionWrapper">

        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnManual" runat="server" GroupName="AddRecipients" ClientIDMode="Static" /><asp:Label ID="Label1" runat="server" Text="Add Manually" AssociatedControlID="rdbtnManual"></asp:Label>&nbsp;<span style="color: #797979;font-size: 11px;font-weight: normal;font-family: Arial,Verdana;">(One recipient per line)</span>
                </div>
                <div id="panAddManually" style="display: none" >
                    <div style="float: left;">
                        <asp:TextBox ID="manualText" runat="server" Rows="6" TextMode="MultiLine" Columns="60" EnableViewState="False"></asp:TextBox>
                    </div>
                    <div class="hint" style="float:left;margin-left:15px;margin-top:10px;">
                        <div style="margin-bottom:5px;font-weight:bold;">Character limits</div>
                        Email - 255 characters<br>
                        First Name  - 50 characters<br>
                        Last Name - 50 characters<br>
                        Custom Data - 1000 characters<br>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="hint" style="margin-bottom: 20px;">Email Address, First Name, Last Name, Custom Data</div>
                    <div>
                        <asp:LinkButton ID="addManualRecipientsButton" runat="server" CssClass="greenbutton" OnClick="OnAddManualrecipients">Add Contacts &gt;&gt;</asp:LinkButton>
                    </div>

                </div>
            </div>
        </div>
        
        <div class="horizontal_separator"></div>

        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnFromList" runat="server" GroupName="AddRecipients" ClientIDMode="Static" /><asp:Label ID="Label2" runat="server" Text="From Address Book" AssociatedControlID="rdbtnFromList"></asp:Label>
                </div>
                <%if(ShowAddressBookList){ %>
                <div id="panFromAddressBook" style="display: none" >
                    <div class="hint">Select a list in your address book:</div>
                    <div style="margin-bottom: 20px;">
                        <asp:DropDownList ID="AddressBookList" runat="server"></asp:DropDownList>
                    </div>
                    <div>
                        <asp:LinkButton ID="addAddressListButton" runat="server" CssClass="greenbutton" OnClick="OnAddAddressListRecipients">Add Contacts &gt;&gt;</asp:LinkButton>
                    </div>
                </div>
                <%} else { %>
                <div id="panFromAddressBook" style="display: none" >
                    There are no lists in your Address Book. Would you like to <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/clay/addressbook/addressbook.aspx">create a new list</asp:HyperLink>?
                </div>
                <%} %>
            </div>
        </div>
        
        <div class="horizontal_separator"></div>

        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnFromSurvey" runat="server" GroupName="AddRecipients" ClientIDMode="Static" /><asp:Label ID="Label3" runat="server" Text="From Collector" AssociatedControlID="rdbtnFromSurvey"></asp:Label>
                </div>
                <div id="panFromCollector" style="display: none" >
                    <div class="hint">Select a survey below that has a list-based collector to import:</div>
                    <div style="margin-bottom: 20px;">
                        <asp:DropDownList ID="CollectorsList" runat="server"></asp:DropDownList>
                    </div>
                    <div>
                        <asp:LinkButton ID="addCollListButton" runat="server" CssClass="greenbutton" OnClick="OnAddCollListRecipients">Add Contacts &gt;&gt;</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="horizontal_separator"></div>

    </div>

</asp:Content>
