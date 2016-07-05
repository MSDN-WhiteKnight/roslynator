﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactoring
{
    internal static class CloseParenTokenRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, SyntaxToken closeParen)
        {
            if (!closeParen.IsKind(SyntaxKind.CloseParenToken))
                return;

            if (context.Settings.IsAnyRefactoringEnabled(
                    RefactoringIdentifiers.AddParameterNameToParameter,
                    RefactoringIdentifiers.RenameParameterAccordingToTypeName,
                    RefactoringIdentifiers.CheckParameterForNull)
                && closeParen.Parent?.IsKind(SyntaxKind.ParameterList) == true
                && context.Span.Start > 0
                && context.SupportsSemanticModel)
            {
                ParameterSyntax parameter = context.Root
                    .FindNode(new TextSpan(context.Span.Start - 1, 1))?
                    .FirstAncestorOrSelf<ParameterSyntax>();

                if (parameter != null)
                {
                    await AddOrRenameParameterRefactoring.ComputeRefactoringsAsync(context, parameter);

                    await CheckParameterForNullRefactoring.ComputeRefactoringAsync(context, parameter);
                }
            }

            if (closeParen.Parent?.IsKind(SyntaxKind.ArgumentList) == true)
            {
                ArgumentSyntax argument = ((ArgumentListSyntax)closeParen.Parent)
                    .Arguments
                    .FirstOrDefault(f => f.FullSpan.End == closeParen.FullSpan.Start);

                if (argument != null)
                    await ArgumentRefactoring.ComputeRefactoringsAsync(context, argument);
            }
        }
    }
}
