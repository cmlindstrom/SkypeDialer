﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.261
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("SkypeDialer2.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        Friend ReadOnly Property Globe() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Globe", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        '''&lt;customUI xmlns=&quot;http://schemas.microsoft.com/office/2006/01/customui&quot; onLoad=&quot;Ribbon_Load&quot;&gt;
        '''  &lt;ribbon&gt;
        '''    &lt;tabs&gt;
        '''      &lt;tab idMso=&quot;TabContact&quot;&gt;
        '''        &lt;group id=&quot;GroupSkypeDialer&quot; label=&quot;SkypeDialer&quot; insertAfterMso=&quot;GroupCommunicate&quot;&gt;
        '''          &lt;button id=&quot;btn_LaunchCallManager&quot; 
        '''                  label=&quot;Call via Skype&quot; 
        '''                  imageMso=&quot;AutoDial&quot;
        '''                  size=&quot;large&quot; 
        '''                  screentip=&quot;Launch SkypeDialer&quot; 
        '''                 [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property mnuContactRibbonNewButton() As String
            Get
                Return ResourceManager.GetString("mnuContactRibbonNewButton", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        '''&lt;customUI xmlns=&quot;http://schemas.microsoft.com/office/2006/01/customui&quot; onLoad=&quot;Ribbon_Load&quot;&gt;
        '''  &lt;ribbon&gt;
        '''    &lt;tabs&gt;
        '''      &lt;tab idMso=&quot;TabContact&quot;&gt;
        '''        &lt;group idMso=&quot;GroupCommunicate&quot; visible=&quot;false&quot; /&gt;
        '''        &lt;group id=&quot;GroupSkypeDialer&quot; label=&quot;Communicate&quot; insertAfterMso=&quot;GroupShow&quot;&gt;
        '''          &lt;button id=&quot;btn_SendEmail&quot;
        '''                  label=&quot;E-Mail&quot;
        '''                  imageMso=&quot;NewMessageToContact&quot;
        '''                  size=&quot;large&quot;
        '''                  scre [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property mnuContactRibbonRewrite() As String
            Get
                Return ResourceManager.GetString("mnuContactRibbonRewrite", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property MutedCall() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("MutedCall", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Options24bit() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Options24bit", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Skype_Logo_Small() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Skype_Logo_Small", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Skype24bit() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Skype24bit", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property UnmutedCall() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("UnmutedCall", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
