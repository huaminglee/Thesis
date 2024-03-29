﻿<%@ Page Language="C#" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <!-- Login Window -->
    <title>Thesis - Sample web application using Ext.NET, ExtJS and ASP.NET MVC 3</title>        
    <style type="text/css">
        h1 {
            font: normal 60px tahoma, arial, verdana;
            color: #E1E1E1;
        }
        
        h2 {
            font: normal 20px tahoma, arial, verdana;
            color: #E1E1E1;
        }
        
        h2 a {
            text-decoration: none;
            color: #E1E1E1;
        }        
        .x-window-mc {
            background-color : #F4F4F4 !important;
        }
        .x-window {
            background-color : #FFF !important;
        }
        
    </style>
    <script type="text/javascript">
        if (window.top.frames.length !== 0) {
            window.top.location = self.document.location;
        }
    </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" ></ext:ResourceManager>
    
    <h1>Thesis by Emre Turan</h1>
    <h2>@<a href="http://www.bilgi.edu.tr/">Istanbul Bilgi University</a> and Computer Science Departmant</h2>
    <ext:Window 
        ID="LoginWindow"
        runat="server" 
        Closable="false"
        Resizable="false"
        Height="174" 
        Icon="Lock" 
        Title="<%$ Resources:Login, Title_Login %>"
        Draggable="true"
        Width="300"
        Modal="true"
        Layout="fit"
        BodyBorder="false"
        Padding="5">        
        <Items>
                <ext:FormPanel ID="frmPnlLogin" 
                    runat="server" 
                    FormID="form1"
                    Border="false"
                    Layout="form"
                    BodyBorder="false" 
                    BodyStyle="background:transparent;"
                    Url='<%# Html.AttributeEncode(Url.Action("Login")) %>'>
                    <Items>
                        <ext:TextField 
                            ID="Username" 
                            runat="server" 
                            FieldLabel="<%$ Resources:Login, Username %>" 
                            AllowBlank="false"
                            Text="emre"
                            AnchorHorizontal="100%"
                            AutoFocus="true"
                            AutoFocusDelay="250"
                            />
                        <ext:TextField 
                            ID="Password" 
                            runat="server" 
                            InputType="Password" 
                            FieldLabel="<%$ Resources:Login, Password %>" 
                            AllowBlank="false"
                            Text="123456"
                            AnchorHorizontal="100%"
                            />
                        <ext:SelectBox
                            ID="Language"
                            runat="server"
                            FieldLabel="<%$ Resources:Login, Language %>"
                            AllowBlank="false"
                            AnchorHorizontal="100%"
                            >
                            <SelectedItem Text='<%# ViewData["LanguageName"] %>' Value='<%# ViewData["LanguageValue"] %>' AutoDataBind="true" />
                            <Items>
                                <ext:ListItem Text="English" Value="en-US" />
                                <ext:ListItem Text="España" Value="es-ES" />
                                <ext:ListItem Text="Nederlands" Value="nl-NL" />
                                <ext:ListItem Text="Deutsche" Value="de-DE" />
                            </Items>
                            <DirectEvents>
                                <Select CleanRequest="true" Method="POST" Url="/Account/ChangeLanguage" 
                                   Success="
                                        Ax.SetCookie('Language', #{Language}.getValue(), 365 * 10);
                                        #{LoginWindow}.setTitle(result.extraParamsResponse.Title_Login);
                                        #{Username}.setFieldLabel(result.extraParamsResponse.Username);
                                        #{Password}.setFieldLabel(result.extraParamsResponse.Password);
                                        #{Language}.setFieldLabel(result.extraParamsResponse.Language);
                                        #{Theme}.setFieldLabel(result.extraParamsResponse.Theme);
                                        #{btnLogin}.setText(result.extraParamsResponse.Button_Login);
                                        #{Authentication}.setValue(result.extraParamsResponse.Authentication);
                                        #{Verifying}.setValue(result.extraParamsResponse.Verifying);
                                        #{Title_Login_Error}.setValue(result.extraParamsResponse.Title_Login_Error);
                                   ">
                                    <ExtraParams><ext:Parameter Name="language" Value="#{Language}.getValue()" Mode="Raw" /></ExtraParams>
                                </Select>
                            </DirectEvents>
                         </ext:SelectBox>
                         <ext:SelectBox
                            ID="Theme"
                            runat="server"
                            FieldLabel="<%$ Resources:Login, Theme %>"
                            AllowBlank="false"
                            AnchorHorizontal="100%"
                            >
                            <SelectedItem Text='<%# ViewData["ThemeName"] %>' Value='<%# ViewData["ThemeValue"] %>' AutoDataBind="true" />
                            <Items>
                                <ext:ListItem Text="Default" Value="0" />
                                <ext:ListItem Text="Gray" Value="1" />
                                <ext:ListItem Text="Slate" Value="3" />
                            </Items>
                            <DirectEvents>
                                <Select CleanRequest="true" Method="POST" Url="/Account/ChangeTheme" Success="ChangeTheme(#{Theme}.getValue());">
                                    <ExtraParams><ext:Parameter Name="theme" Value="#{Theme}.getValue()" Mode="Raw" /></ExtraParams>
                                </Select>
                            </DirectEvents>
                         </ext:SelectBox>
                    </Items>
                    <KeyMap>
                        <ext:KeyBinding StopEvent="true">
                            <Keys><ext:Key Code="ENTER" /></Keys>
                            <Listeners>
                                <Event Handler="#{btnLogin}.fireEvent('click');" />
                            </Listeners>
                        </ext:KeyBinding>
                    </KeyMap>
                </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnLogin" runat="server" Text="<%$ Resources:Login, Button_Login %>" Icon="Accept">
                <DirectEvents>
                    <Click 
                        Url="/Account/Login" 
                        Timeout="60000"
                        FormID="form1"
                        CleanRequest="true" 
                        Method="POST"
                        Before="if(Ax.IsValidForm(#{frmPnlLogin})) { Wait(); } else { return false; }"
                        Failure="FailureMessage(result.errorMessage);">
                        <EventMask MinDelay="250" />
                        <ExtraParams>
                            <ext:Parameter Name="ReturnUrl" Value="Ext.urlDecode(String(document.location).split('?')[1]).r || '/'" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Hidden ID="Authentication" runat="server" Text="<%$ Resources:Login, Authentication %>" />
    <ext:Hidden ID="Verifying" runat="server" Text="<%$ Resources:Login, Verifying %>" />
    <ext:Hidden ID="Title_Login_Error" runat="server" Text="<%$ Resources:Login, Title_Login_Error %>" />
    <script type="text/javascript" src="<%: Url.Content("~/Scripts/Thesis.js") %>"></script>
    <script type="text/javascript">
        function Wait() {
            Ax.Wait(Authentication.getValue(), Verifying.getValue());
        }
        function FailureMessage(message) {
            Ax.ShowMessage(Title_Login_Error.getValue() , message, Ext.Msg.OK, Ext.MessageBox.ERROR);
        }
        function ChangeTheme(theme) {
            if (!theme) theme = 0;
            if(theme == '0')
                Ax.SetTheme('/extjs/resources/css/ext-all-embedded-css/ext.axd', 'blue', 0);
            else if(theme == '1')
                Ax.SetTheme('/extjs/resources/css/xtheme-gray-embedded-css/ext.axd', 'gray', 1);
            else if (theme == '3')
                Ax.SetTheme('/extjs/resources/css/xtheme-slate-embedded-css/ext.axd', 'slate', 3);
        }
    </script>
</body>
</html>
