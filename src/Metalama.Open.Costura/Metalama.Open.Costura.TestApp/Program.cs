﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Metalama.Open.Costura.TestApp;

internal class Program
{
    private static void Main()
    {
        Delay();
    }

    private static void Delay()
    {
        ThenUse.Stuff();
    }
}