<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="add.aspx.cs" Inherits="ValisManager.clay.addressbook.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .rightButton
        {
            float: right;
        }

        .introPanel
        {
           margin: 0px 0px 32px 0px;
           font-size: .96em;
           background-color: #f8eeb2;
           color: #000c3c;
           line-height: 1.2em;
           padding: 4px;
           border-radius: 8px;
           width: 700px;
        }
        .introPanel ul
        {
            margin-left: 20px;
            font-size: .92em;
        }
        .introPanel li ul
        {
            font-size: .84em;
        }
        .introPanel .example
        {
            margin: 6px 0px 0px 0px;
        }

        .add-address-notify
        {
            font-size: .82em;
           width: 700px;
        }


        .optionsPanel
        {
            margin-bottom: 32px;
        }
        .optionsPanel .Header
        {
            color: #06553c;
            font-size: 1.1em;
            font-weight: bold;
        }
        .optionsPanel .help
        {
            font-size: .88em;
            width: 700px; 
            margin-top: 10px; 
            margin-bottom: 5px; 
            margin-left: 20px;
        }


        .succesfullResult
        {
            margin: 4px 0px 4px 20px;
            font-size: 1.1em;
            font-weight: bold;
            line-height: 1.3em;
            background-color: #87a61a;
            color: #fff;
            padding: 3px;
            border-radius: 4px;
            width: 400px;
        }
        .errorResult
        {
            margin: 4px 0px 4px 20px;
            font-size: 1.1em;
            font-weight: bold;
            line-height: 1.3em;
            background-color: #CC0000;
            color: #fff;
            padding: 3px;
            border-radius: 4px;
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1>Add Contacts to “<%: this.SelectedClientList.Name %>”</h1>
        <a class="greybutton rightButton" id="returnButton" href="~/clay/addressbook/addressbook.aspx" runat="server">&lt;&lt;&nbsp;Back to Address Book</a>
    </div>

            
    <div class="optionsPanel">
        <div class="Header">Enter Contacts to be Added</div>
        <div class="help">Copy and paste in your list of contacts</div>
        <div style="margin-left: 20px;"><asp:TextBox ID="manualText" runat="server" Rows="6" TextMode="MultiLine" Width="600px" EnableViewState="False"></asp:TextBox></div>
        <div style="margin-left: 20px; margin-top: 4px;">
            <asp:LinkButton ID="addContactsButton" runat="server" CssClass="greenbutton" OnClick="addContactsButton_Click">Add Contacts &gt;&gt;</asp:LinkButton>
        </div>
        <asp:PlaceHolder ID="stringImportResults" runat="server"></asp:PlaceHolder>
    </div>
    <div class="optionsPanel">
        <div class="Header">Choose a CSV file with Contacts to be Added</div>
        <div class="help">Choose an appropriate CSV file from your computer, and upload it to the server. Contacts will be automatically inserted into the list. The fields must follow the guidelines above.</div>
        <div style="margin-left: 20px;"><asp:FileUpload ID="csvFile" runat="server" Width="600px" EnableViewState="False" /></div>
        <div style="margin-left: 20px; margin-top: 4px;">
            <asp:LinkButton ID="importFileButton" runat="server" CssClass="greenbutton" OnClick="importFileButton_Click">Import File &gt;&gt;</asp:LinkButton>
        </div>
        <asp:PlaceHolder ID="cvsImportResults" runat="server"></asp:PlaceHolder>
    </div>

    <div class="add-address-notify">We do not disclose these email addresses to third parties or use them for any purpose other than to distribute your surveys. See our <a href="/mp/policy/privacy-policy">Privacy Policy</a> for more details. Email addresses are stored securely.</div>


    <div class="introPanel">
        <p>You can add contacts into the list either by hand or by means of a CSV file. Either way, your data must obey the following rules:</p>
            <ul>
                <li>One contact per line</li>
                <li>The fields must be separated by commas</li>
                <li><b>Field Order:</b> Email, First Name, Last Name, Title
                    <ul class="fieldList">
                        <li>Email - 255 characters or less</li>
                        <li>First Name - 127 characters or less</li>
                        <li>Last Name - 127 characters or less</li>
                        <li>Title - 255 characters or less</li>
                        </ul>
                </li>
                <li>Duplicate emails will automatically be filtered out</li>
            </ul>
                <div class="example"><span style="color: rgb(85, 85, 85);"><b>Example #1:</b></span> <span class="notranslate">mike@myemail.com, tom, jones, Microsoft</span></div>
                <div class="example"><span style="color: rgb(85, 85, 85);"><b>Example #2:</b></span> <span class="notranslate">jane@myemail.com,,,Microsoft</span></div>
    </div>


</asp:Content>
