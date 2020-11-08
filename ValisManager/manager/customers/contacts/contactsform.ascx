<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="contactsform.ascx.cs" Inherits="ValisManager.manager.customers.contacts.contactsform" EnableViewState="true"%>


<div class="form-wrapper">
    <div class="form-line">
        <asp:Label ID="Label1" runat="server" Text="FirstName" AssociatedControlID="FirstName"></asp:Label><asp:TextBox ID="FirstName" runat="server" required="required" aria-required=”true”></asp:TextBox><%=GetRequiredIcon() %>
    </div>
    <div class="form-line">
        <asp:Label ID="Label2" runat="server" Text="LastName" AssociatedControlID="LastName"></asp:Label><asp:TextBox ID="LastName" runat="server" required="required" aria-required=”true”></asp:TextBox><%=GetRequiredIcon() %>
    </div>
    <div class="form-line">
        <asp:Label ID="Label7" runat="server" Text="UserName" AssociatedControlID="LogOnToken"></asp:Label><asp:TextBox ID="LogOnToken" runat="server" required="required" aria-required=”true”></asp:TextBox><%=GetRequiredIcon() %>
    </div>
    <%if (IsEditMode == false)
      { %>
    <div class="form-line">
        <asp:Label ID="Label9" runat="server" Text="Password" AssociatedControlID="PswdToken"></asp:Label><asp:TextBox ID="PswdToken" runat="server" required="required" aria-required=”true”></asp:TextBox><%=GetRequiredIcon() %>
    </div>
    <%} %>
    <div class="form-line">
        <asp:Label ID="Label3" runat="server" Text="Email" AssociatedControlID="Email"></asp:Label><asp:TextBox ID="Email" runat="server" TextMode="Email" required="required" aria-required=”true”></asp:TextBox><%=GetRequiredIcon() %>
    </div>
    <div class="form-line">
        <asp:Label ID="Label4" runat="server" Text="Title" AssociatedControlID="Title"></asp:Label><asp:TextBox ID="Title" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label5" runat="server" Text="Department" AssociatedControlID="Department"></asp:Label><asp:TextBox ID="Department" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label6" runat="server" Text="Country" AssociatedControlID="Country"></asp:Label><asp:DropDownList ID="Country" runat="server" EnableViewState="False"></asp:DropDownList>
    </div>
    <div class="form-line">
        <asp:Label ID="Label8" runat="server" Text="Prefecture" AssociatedControlID="Prefecture"></asp:Label><asp:TextBox ID="Prefecture" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label10" runat="server" Text="Town" AssociatedControlID="Town"></asp:Label><asp:TextBox ID="Town" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label11" runat="server" Text="Address" AssociatedControlID="Address"></asp:Label><asp:TextBox ID="Address" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label12" runat="server" Text="Zip" AssociatedControlID="Zip"></asp:Label><asp:TextBox ID="Zip" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label13" runat="server" Text="Telephone1" AssociatedControlID="Telephone1"></asp:Label><asp:TextBox ID="Telephone1" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label14" runat="server" Text="Telephone2" AssociatedControlID="Telephone2"></asp:Label><asp:TextBox ID="Telephone2" runat="server"></asp:TextBox>
    </div>
    <div class="form-line">
        <asp:Label ID="Label16" runat="server" Text="IsActive" AssociatedControlID="IsActive"></asp:Label><asp:CheckBox ID="IsActive" runat="server" />
    </div>
    <div class="form-line">
        <asp:Label ID="Label15" runat="server" Text="IsLockedOut" AssociatedControlID="IsLockedOut"></asp:Label><asp:CheckBox ID="IsLockedOut" runat="server" Enabled="False" />
    </div>
    <div class="form-line">
        <asp:Label ID="Label17" runat="server" Text="Role" AssociatedControlID="Role"></asp:Label><asp:DropDownList ID="Role" runat="server" EnableViewState="False"></asp:DropDownList>
    </div>
    <div class="form-line">
        <asp:Label ID="Label18" runat="server" Text="Comment" AssociatedControlID="Comment"></asp:Label><asp:TextBox ID="Comment" runat="server" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="form-line">
    </div>
</div>