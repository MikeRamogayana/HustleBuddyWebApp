<%@ Page Title="" Language="C#" MasterPageFile="~/HustleBuddyAuth.Master" AutoEventWireup="true" CodeBehind="Authentication.aspx.cs" Inherits="HustleBuddy.Pages.Authentication.Authentication" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <!--Login-->
    <div class="row mt-lg-5" id="login" runat="server">
        <div class="col-4 mt-5">
            <!--Leave Empty-->
        </div>

        <div class="col-8 mt-5">
            <div class="row col-6">
                <h2 class="mx-1">Log In</h2>
            </div>

            <div class="row col-6 mt-2">
                <p class="text-danger mx-1" id="errorMessage" runat="server" visible="false">Login failed! Check your credentials.</p>
            </div>

            <div class="row col-6 mt-2">
                <input id="username" runat="server" type="text" class="hb-input mx-3" placeholder="Username" required>
            </div>

            <div class="row col-6 mt-2">
                <input id="password" runat="server" type="password" class="hb-input mx-3" placeholder="Password" required>
            </div>

            <div class="row col-6 mt-2">
                <asp:Button CssClass="hb-btn mx-3" id="btnLogin" onclick="UserLogin" runat="server" Text="Login"/>
            </div>
                        
            <div class="mt-2"> 
                <p>Forgot password? Click <a id="forgot" class="underlineHover" href="/Pages/Authentication/Authentication.aspx?auth_code=reset">here</a> to reset it.</p>
                <p>Register for Account <a class="underlineHover" href="/Pages/Authentication/Authentication.aspx?auth_code=register">Sign up</a></p>
            </div>
        </div>
    </div>

    <!--Reset Password-->
    <div class="row mt-lg-4" id="reset" runat="server" visible="false">
        <div class="col-4">
            <!--Leae Empty-->
        </div>

        <div class="col-8 mt-5">
            <div class="row col-6 mb-2">
                <h2 class="heading-section col">Reset Password</h2>
            </div>

            <div class="row col-6">
                <hr class="mx-3">
            </div>

            <div class="row col-6 mb-2">
                <asp:TextBox CssClass="hb-input mx-3" ID="resetemail" runat="server" placeholder="Email"></asp:TextBox>
            </div>

            <div class="row col-6 mb-2">
                <asp:Button CssClass="hb-btn mx-3" Text="Send Reset Code" runat="server" OnClick="SendResetCode" />
            </div>

            <div class="row col-6">
                <hr class="mx-3">
            </div>

            <div class="row" id="status" runat="server" visible="false">
                        
            </div>

            <div class="row col-6 mb-2">
                <asp:TextBox CssClass="hb-input mx-3" id="code" runat="server" placeholder="Enter One Time Pin" Enabled="false"></asp:TextBox>
            </div>

            <div class="row col-6 mb-2">
                <asp:TextBox TextMode="Password" CssClass="hb-input mx-3" id="pass" runat="server" placeholder="New Password" Enabled="false"></asp:TextBox>
            </div>

            <div class="row col-6 mb-2">
                <asp:TextBox TextMode="Password" CssClass="hb-input mx-3" id="confirmpass" runat="server" placeholder="Re-Enter Password" Enabled="false"></asp:TextBox>
            </div>

            <div class="row col-6 mb-2">
                <asp:Button CssClass="hb-btn mx-3" Text="Reset Password" runat="server" OnClick="BtnResetPassword" />
            </div>
        </div>
    </div>

    <!--Register-->
    <div class="row mt-lg-4" id="register" runat="server" visible="false">
        <div class="col-4">
            <!--Leave Empty-->
        </div>

        <div class="col-8 mt-5">
            <div class="mb-2 row col-6">
                <h3 class="heading-section">Register Here</h3>
            </div>

            <div class="row col-6">
                <hr class="mx-3">
            </div>

            <div class="mb-2 row col-6">
                <p id="P1" runat="server" class="text-danger mx-1" visible="false">Email already exist, click <a href="/Pages/Authentication/Login" class="text-info">here</a> to login, or <a href="/Pages/Authentication/ForgotPassword" class="text-info">here</a> to reset password.</p>
            </div>
                    
            <div class="mb-2 row col-6">
                    <input type="text" class="hb-input mx-3" id="firstname" runat="server" placeholder="First Name">
            </div>
        
            <div class="mb-2 row col-6">
                    <input type="text" class="hb-input mx-3" id="lastname" runat="server" placeholder="Surname">
             </div>
    
            <div class="mb-2 row col-6">
                    <input type="number" class="hb-input mx-3" id="contact" runat="server" placeholder="079*******">
            </div>
    
            <div class="mb-2 row col-6">
                    <input type="email" class="hb-input mx-3" id="email" runat="server" placeholder="Email">
            </div>
    
            <div class="mb-2 row col-6">
                    <input type="text" class="hb-input mx-3" id="address" runat="server" placeholder="Address">
            </div>
    
            <div class="mb-2 row col-6">
                    <input type="text" class="hb-input mx-3" id="company" runat="server" placeholder="Company Name">
            </div>
    
            <div class="mb-2 row col-6">
                    <input type="password" class="hb-input mx-3" id="password1" runat="server" placeholder="Password">
            </div>
    
            <div class="mb-2 row col-6">
                    <input type="password" class="hb-input mx-3" id="confirmPass" placeholder="Confirm Password">
            </div>
    
            <div class="mb-2 row col-6">
                    <asp:Button Text="Register" CssClass="hb-btn mx-3" OnClick="UserRegistration" runat="server" />
            </div>
    
            <div class="mb-2 row col-6">
                    <p>Already have an account? <a class="underlineHover mx-3" href="/Pages/Authentication/Authentication.aspx">Sign in</a></p>
            </div>
        </div>
    </div>

</asp:Content>
