# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [8.0.1] - 2024-03-01
### Added
- Support for .NET 8.0 (LTS)

### Changed
- Upgraded AutoMapper to 13.0.1
- Upgraded Entity Framework Core packages to 8.0.3
- Upgraded Newtonsoft.Json to 13.0.3

### Removed
- Dropped support for .NET 7.0 (EOL)

## [7.0.0] - 2023-05-01
### Added
- Support for .NET 7.0

### Changed
- Upgraded AutoMapper to 12.x
- Upgraded Entity Framework Core packages to 7.x

## [6.0.0] - 2022-11-01
### Added
- Support for .NET 6.0 (LTS)
- Unit of work pattern for Dapper (`Crystal.Dapper`)
- Unit of work pattern for Entity Framework Core (`Crystal.EntityFrameworkCore`)
- AutoMapper integration for result projection
- Bulk insert/update/delete support
- Transaction support (`BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`)
