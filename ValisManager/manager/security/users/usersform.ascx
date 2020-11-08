<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="usersform.ascx.cs" Inherits="ValisManager.manager.security.users.usersform" EnableViewState="true"%>
<script>

</script>
<style type="text/css">


</style>

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
        <asp:Label ID="Label4" runat="server" Text="IsActive" AssociatedControlID="IsActive"></asp:Label><asp:CheckBox ID="IsActive" runat="server" />
    </div>
    <div class="form-line">
        <asp:Label ID="Label8" runat="server" Text="IsLockedOut" AssociatedControlID="IsLockedOut"></asp:Label><asp:CheckBox ID="IsLockedOut" runat="server" Enabled="False" />
    </div>
    <div class="form-line">
        <asp:Label ID="Label5" runat="server" Text="Role" AssociatedControlID="Role"></asp:Label><asp:DropDownList ID="Role" runat="server"></asp:DropDownList>
    </div>
    <div class="form-line">
        <asp:Label ID="Label6" runat="server" Text="Notes" AssociatedControlID="Notes"></asp:Label><asp:TextBox ID="Notes" runat="server" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="form-line">

    </div>
</div>