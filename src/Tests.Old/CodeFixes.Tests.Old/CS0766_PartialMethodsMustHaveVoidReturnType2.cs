﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal static partial class CS0766_PartialMethodsMustHaveVoidReturnType
    {
        private partial class Foo2
        {
            partial object Bar();
        }
    }
}
