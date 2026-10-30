# Changelog
## 1.2.2
* Final version: this generator is retired together with Funcky 4.0.0, because C# has native `union` types and `closed`
  hierarchies since .NET 11. The readme carries the migration recipe. The package stays on NuGet for projects on the
  Funcky 3.x line and receives no further changes. See [polyadic/funcky#900](https://github.com/polyadic/funcky/issues/900).
  No functional changes.

## 1.2.1
* Fix `IEnumerable<YourType>` between the `<c>` tags

## 1.2.0
* Add the `GeneratePartitionExtension` option to enable generation of a specialized `Partition` method.

## 1.1.0
* Add support for generating `[JsonDerivedType]` attributes.

## 1.0.0
* Re-release of 0.1.0

## 0.1.0
* Initial release
