<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="Edit_Survey.aspx.cs" Inherits="ValisManager.clay.mysurveys.Edit_Survey" ValidateRequest="true"%>
<%@ Register src="../controls/EditSurveyTabs.ascx" tagname="EditSurveyTabs" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/scripts/jquery.form.min.js"></script>
    <script src="/clay/textedit/ckeditor.js"></script>

    <style type="text/css">
        
        div.pageTitle {
            background-image: url(/content/images/survey.png);
            background-position: 0px 4px;
            background-repeat: no-repeat;
            padding-left: 36px;
        }
        a.saveButton
        {
            font-size: 1.2em;
            font-weight: normal;

            padding: 0px 8px 0px 8px;
            cursor: pointer;
            float: right;
        }

        div.pageTools
        {
            margin: 0px;
        }


        div.optionsForm
        {
            margin: 0px 0px 0px 0px;
            font-size: .9em;
            padding: 24px 0px 32px 12px;
            /*border: 0px solid red;*/
            background: none;
            background-color: #f1f1f1;
        }
        div.formSection
        {
            margin: 0px 0px 24px 12px;
        }
        div.formSectionWrapper
        {

        }
        div.sectionTitle
        {    
            color: #0077b5;
            font-size: 1em;
            font-weight: bold;
            margin: 0 0 6px;
            padding: 0;
        }
        div.sectionTitle2
        {    
            color: #3f3f3f;
            font-size: 1em;
            font-weight: bold;
            margin: 0 0 6px;
            padding: 0;
        }
        div.intend1
        {
            margin: 8px 0px 4px 8px;
        }
        div.intend2
        {
            margin: 4px 0px 8px 24px;
        }
        div.lineWrapper
        {
            padding: 4px 0px 0px 0px;
        }

        label.frontlabel
        {
            display: inline-block;
            width: 100px;
            text-align: right;
            margin-right: 8px;
        }

        /*we don't want the border lines arround tabs inside our form:*/
        div.ui-tabs
        {
            border-width: 0px;
        }
        
        img.required-image
        {
            margin-left: 0;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1><a class="back-link" href='<%=_UrlSuffix("mysurveys.aspx") %>'><%=this.Server.HtmlEncode(this.SelectedSurvey.Title) %></a></h1>
        <%if(this.SelectedSurvey.TextsLanguage != 0)
          {
              Response.Write(this.GetTextsLanguageThumbnail());
          }%>
        <a class="greenbutton saveButton" id="SaveButton1" runat="server">Save Changes</a>
    </div>
    <div class="pageTools">
        <uc1:EditSurveyTabs ID="EditSurveyTabs2" runat="server" />
    </div>


    <div id="optionsForm" class="optionsForm">
        <ul>
            <li><a href="#tab1"><span>Title</span></a></li>
            <li><a href="#tab2"><span>Details</span></a></li>
            <li><a href="#tab3"><span>Welcome Page</span></a></li>
            <li><a href="#tab4"><span>"Thank You" Page</span></a></li>
            <li><a href="#tab5"><span>Disqualification Page</span></a></li>
            <li><a href="#tab6"><span>Footer</span></a></li>
            <li><a href="#tab7"><span>EndSurvey</span></a></li>
        </ul>
        <div id="tab1">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Title</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:TextBox ID="SurveyTitle" runat="server" Width="482px"></asp:TextBox><%=GetRequiredIcon() %></div>
                    </div>
                    </div>
            </div>

            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Header Html<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:TextBox ID="HeaderHtml" runat="server" Height="137px" TextMode="MultiLine" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>
                </div>
            </div>

            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Survey/Page Titles</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="ShowSurveyTitle" runat="server" /><asp:Label ID="Label6" runat="server">Show Survey Title in Actual Survey</asp:Label></div>
                        <div class="lineWrapper"><asp:CheckBox ID="ShowPageTitles" runat="server" /><asp:Label ID="Label7" runat="server">Show Page Titles in Actual Survey</asp:Label></div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab2">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Survey Language</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="ShowLanguageSelector" runat="server" /><asp:Label ID="Label15" runat="server" AssociatedControlID="ShowLanguageSelector">Show language selector</asp:Label></div>
                    </div>
                </div>
            </div>
        

            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Page and Question Numbering</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="UsePageNumbering" runat="server" />
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="UsePageNumbering">Use Page Numbering</asp:Label></div>
                
                        <div class="lineWrapper">
                            <asp:CheckBox ID="UseQuestionNumbering" runat="server" />
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="UseQuestionNumbering">Number questions over entire survey</asp:Label>
                            <div class="intend2">
                                <div class="lineWrapper"><asp:RadioButton ID="NumberingPerPage" runat="server" GroupName="QuestionNumberingType" /><asp:Label ID="Label3" runat="server" AssociatedControlID="NumberingPerPage">Number each page of questions separately</asp:Label></div>
                                <div class="lineWrapper"><asp:RadioButton ID="NumberingEntireSurvey" runat="server" GroupName="QuestionNumberingType" /><asp:Label ID="Label4" runat="server" AssociatedControlID="NumberingEntireSurvey">Number questions over entire survey</asp:Label></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Progress Bar Settings</div>
                    <div class="intend1">
                        <div class="lineWrapper">
                            <asp:CheckBox ID="UseProgressBar" runat="server" /><asp:Label ID="Label5" runat="server" AssociatedControlID="UseProgressBar">Show Progress Bar</asp:Label>
                            <asp:DropDownList ID="ProgressBarPosition" runat="server"><asp:ListItem Text="at top of page" Value="0"></asp:ListItem><asp:ListItem Text="at bottom of page" Value="1"></asp:ListItem></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>


            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Navigation Buttons</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:Label ID="Label16" CssClass="frontlabel" runat="server" AssociatedControlID="StartButton">Start Button:</asp:Label><asp:TextBox ID="StartButton" runat="server" Width="180px"></asp:TextBox><%=CheckAndGetTranslatableIcon() %><%=GetRequiredIcon() %></div>
                        <div class="lineWrapper"><asp:Label ID="Label8" CssClass="frontlabel" runat="server" AssociatedControlID="PreviousButton">Previous Button:</asp:Label><asp:TextBox ID="PreviousButton" runat="server" Width="180px"></asp:TextBox><%=CheckAndGetTranslatableIcon() %><%=GetRequiredIcon() %></div>
                        <div class="lineWrapper"><asp:Label ID="Label9" CssClass="frontlabel" runat="server" AssociatedControlID="NextButton" >Next Button:</asp:Label><asp:TextBox ID="NextButton" runat="server" Width="180px"></asp:TextBox><%=CheckAndGetTranslatableIcon() %><%=GetRequiredIcon() %></div>
                        <div class="lineWrapper"><asp:Label ID="Label10" CssClass="frontlabel" runat="server" AssociatedControlID="DoneButton">Done Button:</asp:Label><asp:TextBox ID="DoneButton" runat="server" Width="180px"></asp:TextBox><%=CheckAndGetTranslatableIcon() %><%=GetRequiredIcon() %></div>
                    </div>
                </div>
            </div>

            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Required Question Highlight</div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:RadioButton ID="RequiredHighlightType_0" runat="server" GroupName="RequiredHighlightType" /><asp:Label ID="Label13" runat="server">Do not highlight required questions</asp:Label></div>
                        <div class="lineWrapper"><asp:RadioButton ID="RequiredHighlightType_1" runat="server" GroupName="RequiredHighlightType" /><asp:Label ID="Label12" runat="server" >Use asterisk ( <span style="line-height: 20px; font-size: 20px;">*</span> )&nbsp;&nbsp;to highlight required questions</asp:Label></div>
                    </div>
                </div>
            </div>
            
        </div>
        <div id="tab3">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Welcome Page<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="ShowWelcomePage" runat="server" /><asp:Label ID="Label11" runat="server" Text="Label" AssociatedControlID="ShowWelcomePage">Show welcome page</asp:Label></div>
                        <div class="lineWrapper"><asp:TextBox ID="WelcomeHtml" runat="server" Height="137px" TextMode="MultiLine" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>

                </div>
            </div>
        </div>
        <div id="tab4">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">"Thank You" Page<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="ShowGoodbyePage" runat="server" /><asp:Label ID="Label17" runat="server" Text="Label" AssociatedControlID="ShowGoodbyePage">Show "Thank You" Page</asp:Label></div>
                        <div class="lineWrapper"><asp:TextBox ID="GoodbyeHtml" runat="server" Height="137px" TextMode="MultiLine" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab5">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Disqualification Mode</div>
                    <div class="intend1">
                        <asp:DropDownList ID="OnDisqualificationMode" runat="server">
                            <asp:ListItem Value="0">Show Page</asp:ListItem>
                            <asp:ListItem Value="1">GoTo Url</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="sectionTitle">Disqualification Url<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper">
                            <asp:TextBox ID="DisqualificationUrl" runat="server" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>
                    <div class="sectionTitle">Disqualification Page<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:TextBox ID="DisqualificationHtml" runat="server" Height="137px" TextMode="MultiLine" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab6">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">Survey Footer<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper"><asp:CheckBox ID="ShowCustomFooter" runat="server" /><asp:Label ID="Label14" runat="server" Text="Label" AssociatedControlID="ShowCustomFooter">Show custom footer</asp:Label></div>
                        <div class="lineWrapper"><asp:TextBox ID="FooterHtml" runat="server" Height="137px" TextMode="MultiLine" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab7">
            <div class="formSection">
                <div class="formSectionWrapper">
                    <div class="sectionTitle">OnCompletionMode</div>
                    <div class="intend1">
                        <asp:DropDownList ID="OnCompletionMode" runat="server">
                            <asp:ListItem Value="0">CloseWindow</asp:ListItem>
                            <asp:ListItem Value="1">GoTo Url</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="sectionTitle">OnCompletion Url<%=CheckAndGetTranslatableIcon() %></div>
                    <div class="intend1">
                        <div class="lineWrapper">
                            <asp:TextBox ID="OnCompletionUrl" runat="server" Width="482px" ClientIDMode="Static"></asp:TextBox></div>
                    </div>


                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function ()
        {
            $('#optionsForm').tabs({
                activate: function (event, ui)
                {
                    $.cookie("clay.mysurveys.Edit_Survey", $("#optionsForm").tabs("option", "active"));
                },
                active: $.cookie("clay.mysurveys.Edit_Survey")
             });

            textEdit('HeaderHtml', 2, false, theAccessToken, <%=this.SelectedSurvey.Client%>,<%=this.SelectedSurvey.SurveyId %>);
            textEdit('WelcomeHtml', 2, false, theAccessToken, <%=this.SelectedSurvey.Client%>,<%=this.SelectedSurvey.SurveyId %>);
            textEdit('GoodbyeHtml', 2, false, theAccessToken, <%=this.SelectedSurvey.Client%>,<%=this.SelectedSurvey.SurveyId %>);
            textEdit('FooterHtml', 1, false, theAccessToken, <%=this.SelectedSurvey.Client%>,<%=this.SelectedSurvey.SurveyId %>);
            textEdit('DisqualificationHtml', 2, false, theAccessToken, <%=this.SelectedSurvey.Client%>,<%=this.SelectedSurvey.SurveyId %>);
        });
    </script>
</asp:Content>
