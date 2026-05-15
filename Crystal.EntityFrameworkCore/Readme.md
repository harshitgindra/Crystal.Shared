# Crystal.EntityFrameworkCore

[![CI Build and Test](https://github.com/harshitgindra/Crystal.Shared/actions/workflows/ci.yml/badge.svg)](https://github.com/harshitgindra/Crystal.Shared/actions/workflows/ci.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Crystal.EntityFrameworkCore?label=NuGet)](https://www.nuget.org/packages/Crystal.EntityFrameworkCore/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Crystal.EntityFrameworkCore)](https://www.nuget.org/packages/Crystal.EntityFrameworkCore/)
[![Contributors](https://img.shields.io/github/contributors/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/graphs/contributors)
[![Issues](https://img.shields.io/github/issues/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/issues)
[![MIT License](https://img.shields.io/github/license/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/blob/main/LICENSE.txt)
[![Stars](https://img.shields.io/github/stars/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/stargazers)

A **Unit of Work** and **Repository pattern** wrapper around [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/), standardizing data access across ASP.NET applications. Ensures `DbContext` instances are properly disposed, preventing memory leaks and open contexts.

---

## Table of Contents

- [About](#about)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Built With](#built-with)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## About

`Crystal.EntityFrameworkCore` provides a clean, testable abstraction over EF Core using the Unit of Work pattern. It enforces proper `DbContext` lifecycle management and includes support for AutoMapper projections, bulk operations, and dynamic LINQ queries.

---

## Getting Started

### Install via NuGet

```bash
dotnet add package Crystal.EntityFrameworkCore
```

Or via the Package Manager Console:

```powershell
Install-Package Crystal.EntityFrameworkCore
```

### Requirements

- .NET 8.0 or .NET 10.0

---

## Usage

Refer to the [Wiki](https://github.com/harshitgindra/Crystal.Shared/wiki/Entity-Framework-Core-Example-1) for full usage examples and guides.

---

## Built With

- [Entity Framework Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [AutoMapper](https://www.nuget.org/packages/AutoMapper/)
- [System.Linq.Dynamic.Core](https://www.nuget.org/packages/System.Linq.Dynamic.Core/)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
- [Z.EntityFramework.Extensions.EFCore](https://www.nuget.org/packages/Z.EntityFramework.Extensions.EFCore/)

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](https://github.com/harshitgindra/Crystal.Shared/blob/main/CONTRIBUTING.md) to get started.

1. Fork the repository
2. Create a branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m 'Add my feature'`
4. Push the branch: `git push origin feature/my-feature`
5. Open a Pull Request targeting `master`

---

## License

Distributed under the MIT License. See [LICENSE.txt](https://github.com/harshitgindra/Crystal.Shared/blob/main/LICENSE.txt) for more information.

---

## Contact

Harshit Gindra — [@harshitgindra](https://twitter.com/harshitgindra) · [LinkedIn](https://linkedin.com/in/harshit-gindra)

Project Link: [https://github.com/harshitgindra/Crystal.Shared](https://github.com/harshitgindra/Crystal.Shared)
