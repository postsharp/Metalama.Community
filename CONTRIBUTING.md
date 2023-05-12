# Contributing Guidelines

Welcome to `Metalama.Community`, a repo whose ownership is shared by contributors of the Metalama community and other upstream projects and curated by the Metalama core team.

We appreciate your interest in contributing to this project. Please take a moment to review these guidelines before making a contribution. 

## Issues

If you find any issues or bugs in the project, please report them by opening an issue in the project's issue tracker on GitHub. When reporting an issue, please provide as much detail as possible, including steps to reproduce the problem and any relevant error messages.

## Pull Requests

If you would like to contribute code to the project, please follow these steps:

1. Fork the repository and create a new branch for your feature or bug fix. We typically use the `topic/` prefix for all changes.
2. Make your changes.
3. Write tests that cover your changes and ensure that all tests pass using `Build.ps1 test`.
4. Ensure that there are _zero_ warnings.
    * The project coding style is strictly enforced using analyzers. To apply the code style, use `Build.ps1 codestyle format`.
5. If your changes add a new publishable package, update `eng\src\Program.cs`.
6. Add your name as a contributor to `LICENSE.md`. 
7. Submit a pull request to the main repository's `dev` branch.

When submitting a pull request, please provide a description of your changes and reference any related issues.

## Benevolent Dictatorship

Please note that this project is run using a benevolent dictatorship model, where the project's dictator makes decisions for the benefit of the project as a whole. In this case, the dictator is the PostSharp Technologies company. While we welcome contributions from the community, the ultimate decision-making authority rests with the project's dictator. This applies not only to technical decisions and product management decisions, but also to encouraging and enforcing constructive conduct among contributors. While we strive to be fair and transparent in our decision-making, we ask that you respect the authority of the project's dictator.

## License

By contributing to this project, you agree to license your contribution under the same license as the project.