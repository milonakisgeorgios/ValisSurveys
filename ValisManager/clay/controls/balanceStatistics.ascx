<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="balanceStatistics.ascx.cs" Inherits="ValisManager.clay.controls.balanceStatistics" %>
    <style>
    
        
            .statisticsPanel
            {
                border-top: medium none;
                margin: 0;
                padding: 0;
                background-color: #FFFFFF;
                width: 300px;
            }
            .statisticsHeader
            {
                color: #87A32E;
                font-size: 16px;
                font-weight: bold;
            }
            .totalsLine
            {    
                background-color: #EAEAE8;
            }
            .statisticsLine .label
            {    
                color: #797979;
                font-family: Arial,Verdana;
                font-size: 12px;
                font-weight: normal;
                display: inline-block;
                width: 230px;
            }
            .statisticsLine .value
            {    
                color: #424242;
                font-family: arial;
                font-size: 16px;
                font-weight: bold;
                padding-left: 10px;
            }
    </style>

    <table style="width: 100%" border="1">
        <tr>
            <td style="width: 33%;">
                <div class="statisticsPanel">
                    <div class="statisticsHeader">Email Credits</div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Balance</span><span class="value"><%=this.EmailsCreditsBalance %></span></div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Reserved</span><span class="value"><%=this.EmailsCreditsReserved %></span></div>
                    <div class="statisticsLine totalsLine"><span class="label">TOTAL</span><span class="value"><%=this.EmailsCreditsTotal %></span></div>
                </div>
            </td>
            <%if(ValisManager.Globals.UserToken.PrincipalType == Valis.Core.PrincipalType.SystemUser){ %>
            <td style="width: 33%;">
                <div class="statisticsPanel">
                    <div class="statisticsHeader">Response Credits</div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Balance</span><span class="value"><%=this.ResponsesCreditsBalance %></span></div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Reserved</span><span class="value"><%=this.ResponsesCreditsReserved %></span></div>
                    <div class="statisticsLine totalsLine"><span class="label">TOTAL</span><span class="value"><%=this.ResponsesCreditsTotal %></span></div>
                </div>
            </td>
            <%} %>
            <td style="width: 33%;">
                <div class="statisticsPanel">
                    <div class="statisticsHeader">Click Credits</div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Balance</span><span class="value"><%=this.ClicksCreditsBalance %></span></div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Reserved</span><span class="value"><%=this.ClicksCreditsReserved %></span></div>
                    <div class="statisticsLine totalsLine"><span class="label">TOTAL</span><span class="value"><%=this.ClicksCreditsTotal %></span></div>
                </div>
            </td>
        </tr>
    </table>