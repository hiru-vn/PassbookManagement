﻿#pragma checksum "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1D63BB2422C1CAD1B6080ADA797B6F86676B1D70"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MainProgram.Converter;
using MainProgram.Pages.ManagePassbookSubPages;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace MainProgram.Pages.ManagePassbookSubPages {
    
    
    /// <summary>
    /// PassbookViewPage
    /// </summary>
    public partial class PassbookViewPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid_Main;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxTask;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Task_ButtonClearText;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RadioButton_searchID;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RadioButton_searchName;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listviewtask;
        
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
            System.Uri resourceLocater = new System.Uri("/MainProgram;component/pages/managepassbooksubpages/passbookviewpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
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
            this.Grid_Main = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.TextBoxTask = ((System.Windows.Controls.TextBox)(target));
            
            #line 70 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
            this.TextBoxTask.KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_Search_KeyDown);
            
            #line default
            #line hidden
            
            #line 70 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
            this.TextBoxTask.KeyUp += new System.Windows.Input.KeyEventHandler(this.TextBox_Search_KeyUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Task_ButtonClearText = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\..\Pages\ManagePassbookSubPages\PassbookViewPage.xaml"
            this.Task_ButtonClearText.Click += new System.Windows.RoutedEventHandler(this.Button_ClearText_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RadioButton_searchID = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.RadioButton_searchName = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.listviewtask = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
