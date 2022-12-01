﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Metalama.Open.Costura.WpfApp;

/// <summary>
///     Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Loaded += ( _, _ ) => this.Close();
    }
}