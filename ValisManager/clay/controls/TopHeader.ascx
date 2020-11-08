<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TopHeader.ascx.cs" Inherits="ValisManager.clay.controls.TopHeader" %>

<div id="top-header">
        <div class="wrapper">
            <div style="position: absolute; top: 12px; left: 12px;">
                <img src="/content/images/logo.png" style="width:142px;"/>
            </div>    
            <div class="header-section first-child">
                <span class="brandName"><%: ValisManager.Globals.ClientName %></span>
            </div>
            <div class="header-section last-child">
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
    <div id="top-menu" class="responsive-nav" style="position: static; width: auto;">
        <div class="wrapper">
            <ul class="nav main-nav"  >
                <li class="nav-item"><a class="nav-link" href="/clay/home.aspx">Home</a></li>
                <li class="nav-item"><a class="nav-link" href="/clay/mysurveys/mysurveys.aspx">My Surveys</a></li>
                <li class="nav-item"><a class="nav-link" href="/clay/addressbook/addressbook.aspx">Address Book</a></li>
                <%if(ValisManager.Globals.UseCredits){ %>
                    <li class="nav-item"><a class="nav-link" href="/clay/payments/mypayments.aspx">Credits</a></li>
                <%} %>
                <li class="nav-item"><a class="nav-link" href="/clay/myaccount.aspx">My Account</a></li>
            </ul>
            <ul class="nav right-nav">
                <li class="nav-item"><a class="nav-link" href="/logoff.aspx">Logout</a></li>
            </ul>
        </div>
    </div>
