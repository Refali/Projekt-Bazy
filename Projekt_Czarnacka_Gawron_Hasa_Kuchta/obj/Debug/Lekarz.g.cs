﻿#pragma checksum "..\..\Lekarz.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CE60DFB810E5CAF9100D3CBA2775B24008958048"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using Projekt_Czarnacka_Gawron_Hasa_Kuchta;
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


namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta {
    
    
    /// <summary>
    /// Lekarz
    /// </summary>
    public partial class Lekarz : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label userLbl;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label statusLbl;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PanelEdycjaWizyty;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid WizytyView;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ZapiszEdycje;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtWyszukaj;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnWyloguj;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\Lekarz.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Label_WizytyZmiany;
        
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
            System.Uri resourceLocater = new System.Uri("/Projekt_Czarnacka_Gawron_Hasa_Kuchta;component/lekarz.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Lekarz.xaml"
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
            this.userLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.statusLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.PanelEdycjaWizyty = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.WizytyView = ((System.Windows.Controls.DataGrid)(target));
            
            #line 17 "..\..\Lekarz.xaml"
            this.WizytyView.RowEditEnding += new System.EventHandler<System.Windows.Controls.DataGridRowEditEndingEventArgs>(this.WizytyView_RowEditEnding);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ZapiszEdycje = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\Lekarz.xaml"
            this.ZapiszEdycje.Click += new System.Windows.RoutedEventHandler(this.ZapiszEdycje_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtWyszukaj = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.btnWyloguj = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\Lekarz.xaml"
            this.btnWyloguj.Click += new System.Windows.RoutedEventHandler(this.Wyloguj_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Label_WizytyZmiany = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

