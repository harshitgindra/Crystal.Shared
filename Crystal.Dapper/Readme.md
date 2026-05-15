# Crystal.Dapper

[![CI Build and Test](https://github.com/harshitgindra/Crystal.Shared/actions/workflows/ci.yml/badge.svg)](https://github.com/harshitgindra/Crystal.Shared/actions/workflows/ci.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Crystal.Dapper?label=NuGet)](https://www.nuget.org/packages/Crystal.Dapper/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Crystal.Dapper)](https://www.nuget.org/packages/Crystal.Dapper/)
[![Contributors](https://img.shields.io/github/contributors/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/graphs/contributors)
[![Issues](https://img.shields.io/github/issues/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/issues)
[![MIT License](https://img.shields.io/github/license/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/blob/main/LICENSE.txt)
[![Stars](https://img.shields.io/github/stars/harshitgindra/Crystal.Shared.svg)](https://github.com/harshitgindra/Crystal.Shared/stargazers)

A **Unit of Work** and **Repository pattern** wrapper around [Dapper](https://github.com/DapperLib/Dapper), standardizing data access across ASP.NET applications. Ensures database connections are always disposed, preventing memory leaks and open connections.

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

`Crystal.Dapper` provides a clean, testable abstraction over Dapper using the Unit of Work pattern. It enforces proper resource disposal so you never accidentally leave database connections open.

---

## Getting Started

### Install via NuGet

```bash
dotnet add package Crystal.Dapper
```

Or via the Package Manager Console:

```powershell
Install-Package Crystal.Dapper
```

### Requirements

- .NET 8.0 or .NET 10.0

---

## Usage

Refer to the [Wiki](https://github.com/harshitgindra/Crystal.Shared/wiki/Dapper-Examples) for full usage examples and guides.

---

## Built With

- [Dapper](https://www.nuget.org/packages/Dapper)
- [MicroOrm.Dapper.Repositories](https://www.nuget.org/packages/MicroOrm.Dapper.Repositories/)
- [AutoMapper](https://www.nuget.org/packages/AutoMapper/)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
- [System.Linq.Dynamic.Core](https://www.nuget.org/packages/System.Linq.Dynamic.Core/)

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
