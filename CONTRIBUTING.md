# Contributing to Crystal.Shared

Thank you for your interest in contributing! Here's everything you need to know to get started.

## Branching Strategy

| Branch | Purpose |
|---|---|
| `master` | Stable mainline — all PRs target this branch |
| `release/**` | Release branches (e.g. `release/8.1.0`) — triggers NuGet publish |
| `feature/**` | New features |
| `fix/**` | Bug fixes |

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) (for multi-targeting)

### Clone and Build

```bash
git clone https://github.com/harshitgindra/Crystal.Shared.git
cd Crystal.Shared
dotnet restore Crystal.Shared.Components.sln
dotnet build Crystal.Shared.Components.sln --configuration Debug
```

### Running Tests

```bash
dotnet test Crystal.Shared.Components.sln --verbosity normal
```

## Making Changes

1. Fork the repository
2. Create a branch from `master`:
   ```bash
   git checkout -b feature/my-feature
   ```
3. Make your changes with clear, focused commits
4. Ensure all tests pass locally
5. Open a Pull Request targeting `master`

## Pull Request Guidelines

- Keep PRs focused — one concern per PR
- Add or update tests for any code changes
- Do not bump version numbers in `.csproj` files — maintainers handle versioning
- Update `CHANGELOG.md` under `[Unreleased]` with a summary of your changes

## Releasing

Releases are triggered by pushing to a `release/**` branch (e.g. `release/8.1.0`). The CI pipeline will:
1. Extract the version from the branch name
2. Build and test
3. Pack NuGet packages
4. Publish to [nuget.org](https://www.nuget.org/)

## Questions?

Open an [issue](https://github.com/harshitgindra/Crystal.Shared/issues) or reach out via [@harshitgindra](https://twitter.com/harshitgindra).
