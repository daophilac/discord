﻿#pragma checksum "..\..\..\..\Pages\Channel\PermissionsPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F0142082DD014FE941B5F3E91B564CC647291841"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Discord.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Discord.Pages {
    
    
    /// <summary>
    /// PermissionPage
    /// </summary>
    public partial class PermissionPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.DockPanel CDPRoleList;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Label CLRoleName;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.CheckBox CCBViewMessage;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.CheckBox CCBReact;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.CheckBox CCBSendMessage;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.CheckBox CCBSendImage;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Button CBSave;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Discord;component/pages/channel/permissionspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CDPRoleList = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.CLRoleName = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.CCBViewMessage = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.CCBReact = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.CCBSendMessage = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.CCBSendImage = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.CBSave = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\Pages\Channel\PermissionsPage.xaml"
            this.CBSave.Click += new System.Windows.RoutedEventHandler(this.CBSave_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

