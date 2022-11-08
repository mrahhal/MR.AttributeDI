# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## Unreleased: 3.0.0

This version comes annotated for NRTs, and retargets to .net 6.

### Added

- Add support for NRTs (nullable reference types)

### Changed

- Change target framework to .net 6

### Removed

- Remove previously marked obsolete items

[**Full diff**](https://github.com/mrahhal/MR.AttributeDI/compare/2.1.0...HEAD)

## 2.1.0 - 2019-02-07

### Added

- Add support for forwarding service registrations ([#2](https://github.com/mrahhal/MR.AttributeDI/issues/2))

## 2.0.1 - 2018-10-30

### Changed

- Make [SourceLink](https://github.com/dotnet/sourcelink) compatible

## 2.0.0 - 2017-12-02

### Changed

- Update dependencies

[**Full diff**](https://github.com/mrahhal/MR.AttributeDI/compare/1.4.0...2.0.0)

## 1.4.0 - 2017-05-20

### Added

- Support `AsImplementedInterface` ([#3](https://github.com/mrahhal/MR.AttributeDI/issues/3))

### Changed

- Rename extension methods from `Configure` to `ConfigureFromAttributes` ([#4](https://github.com/mrahhal/MR.AttributeDI/issues/4))

[**Full diff**](https://github.com/mrahhal/MR.AttributeDI/compare/1.3.0...1.4.0)
