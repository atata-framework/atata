# Contributing Guidelines

## Issue Contributing

Feel free to report issues on GitHub.
Any issues or questions can also be targeted to any communication channel defined on [Atata Contact](https://atata.io/contact/) page.

## Code Contributing

### Development Prerequisites

- Visual Studio 2022 or JetBrains Rider.

### Setting Up For Development

In order to set up this project for further contributing do the following:

- Fork the repository.
- Clone the forked repository locally.
- Open `.sln` file located in the root of the repository.
- Run tests through IDE or with `dotnet test` command.

### Development

Please follow the rules during development:

- Fix (or suppress in rare cases) all code analysis warnings.
- Ensure that newly added public classes and members have XML documentation comments.
  This does not apply to classes in test projects.
- Run all tests in order to ensure the changes don't break anything.
- Try to add/update tests respectively.
- Follow [Semantic Versioning 2.0](https://semver.org/) during the source changes.
- [Create a pull request](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request-from-a-fork) when it is done.
