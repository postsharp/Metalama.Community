# Contributing Guidelines

Welcome to `Metalama.Community`, a repo whose ownership is shared by contributors of the Metalama community and other upstream projects and curated by the Metalama core team.

We appreciate your interest in contributing to this project. Please take a moment to review these guidelines before making a contribution. 

## Issues

If you find any issues or bugs in the project, please report them by opening an issue in the project's [issue tracker](https://github.com/postsharp/Metalama.Community/issues) on GitHub. When reporting an issue, please provide as much detail as possible, including steps to reproduce the problem and any relevant error messages.

## C# Coding Style

The general rule we follow is: _use Visual Studio defaults_, with a few adaptations.

1. We use [Allman style](http://en.wikipedia.org/wiki/Indent_style#Allman_style) braces, where each brace begins on a new line. A single-line statement block can go without braces, but the block must be properly indented on its own line and must not be nested in other statement blocks that use braces (See rule 18 for more details). One exception is that a `using` statement is permitted to be nested within another `using` statement by starting on the following line at the same indentation level, even if the nested `using` contains a controlled block.
2. We use four spaces of indentation (no tabs).
3. We use `_camelCase` for internal and private fields and use `readonly` where possible. Prefix private fields with `_`. When used on static fields, `readonly` should come after `static` (e.g. `static readonly`, not `readonly static`).  Public fields should be used sparingly and should use PascalCasing with no prefix when used.
4. We always use `this.`.
5. We always specify the visibility, even if it's the default (e.g. `private string _foo`, not `string _foo`). Visibility should be the first modifier (e.g.`public abstract`, not `abstract public`).
6. Namespace imports should be specified at the top of the file, *outside* of
   `namespace` declarations, and should be sorted alphabetically, with the exception of `System.*` namespaces, which are to be placed on top of all others.
7. Avoid more than one empty line at any time. For example, do not have two blank lines between members of a type.
8. We always add whitespaces within parenthesis except with `typeof(Foo)`, `nameof(`Foo)`, and `()`` i.e. empty parameter lists.
9. When copy-pasting code from a different project, we add the code origin in a comment, and we reformat according to our standards.
10. We always use `var.
11. We use language keywords instead of BCL types (e.g. `int, string, float` instead of `Int32, String, Single`, etc) for both type references as well as method calls (e.g. `int.Parse` instead of `Int32.Parse`).
12. We use PascalCasing to name all our constant local variables and fields. 
13. We use PascalCasing for all method names, including local functions.
14. We use `nameof(...)` instead of `"..."` whenever possible and relevant.
15. Fields should be specified at the top within type declarations.
16. When including non-ASCII characters in the source code use Unicode escape sequences (\uXXXX) instead of literal characters. Literal non-ASCII characters occasionally get garbled by a tool or editor.
17. When using labels (for goto), indent the label one less than the current indentation.
18. When using a single-statement `if`, we follow these conventions:
    - Never use single-line form (for example: `if (source == null) throw new ArgumentNullException("source");`)
    - Using braces is always accepted, and required if any block of an `if`/`else if`/.../`else` compound statement uses braces or if a single statement body spans multiple lines.
19. Make all internal and private types `static` or `sealed` unless derivation from them is required.  As with any implementation detail, they can be changed if/when derivation is required in the future.
20. We give all members the minimal required visibility.

An [EditorConfig](https://editorconfig.org "EditorConfig homepage") file (`.editorconfig`) and a Resharper/Rider settings file (`eng\style\CommonStyle.DotSettings`) have been provided at the root of the runtime repository, enabling C# auto-formatting conforming to the above guidelines.

We [Resharper CleanupCode](https://www.jetbrains.com/help/resharper/CleanupCode.html) tool to ensure the code base maintains a consistent style over time, the tool automatically fixes the code base to conform to the guidelines outlined above. To run the tool, use the following command:

```
Build.ps1 codestyle format
```

## Pull Requests

:warning: By submitting a pull request, you are representing that you are the original author of the changes, and that any inclusion or third-party code are properly attributed to their author in comments and in `LICENSING.md`.

If you would like to contribute code to the project, please follow these steps:

1. Fork the repository and create a new branch for your feature or bug fix. We typically use the `topic/` prefix for all changes.
2. Make your changes.
3. Write tests that cover your changes and ensure that all tests pass using `Build.ps1 test`.
4. Ensure that there are _zero_ warnings.
    * The project coding style is strictly enforced using analyzers. 
    * To apply the code style, use `Build.ps1 codestyle format`. This step does not solve all issues.
    * We require zero warnings in `dotnet build` to merge a PR.
    * Run `Build.ps1 codestyle inspect` and verify that you are not adding warnings with your additions. Suppress false positives using Resharper annotations. Note that we do not require zero warnings from this tool at all time.
5. If your changes add a new publishable package, update `eng\src\Program.cs`.
6. Add your name as a contributor to `LICENSE.md`. 
7. Submit a pull request to the main repository's `dev` branch.

When submitting a pull request, please describe your changes and reference any related issues.

## Benevolent Dictatorship

Please note that this project is run using a benevolent dictatorship model, where the project's dictator makes decisions for the benefit of the project as a whole. In this case, the dictator is the PostSharp Technologies company. While we welcome contributions from the community, the ultimate decision-making authority rests with the project's dictator. This applies not only to technical decisions and product management decisions, but also to encouraging and enforcing constructive conduct among contributors. While we strive to be fair and transparent in our decision-making, we ask that you respect the authority of the project's dictator.

## License

By contributing to this project, you agree to license your contribution under the same license as the project.