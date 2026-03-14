# Zero Alloc Examples

Небольшие примеры и бенчмарки по zero-allocation подходам в C#/.NET.

## Запуск

```bash
dotnet run --project ZeroAllocExamples/ZeroAllocExamples.csproj
```

## Бенчмарки

```bash
dotnet run --project ZeroAllocExamples/ZeroAllocExamples.csproj -c Release
```

## Примеры

- `AsyncCacheDemo` — сравнение `Task` и `ValueTask` для кешированного (синхронного) сценария чтения из словаря.
- `CsvParsingDemo` — суммирование CSV-полей через `string.Split` и через `ReadOnlySpan<char>` без промежуточных массивов строк.
- `HashingDemo` — сравнение хеширования через `ComputeHash` (с выделением массива под хеш) и `TryComputeHash` в `stackalloc`-буфер `Span<byte>`.
- `LinqFilterDemo` — три подхода к фильтрации: классический LINQ, `class lease` c `ArrayPool`, и запись в буфер через `Span<int>`.
- `NetworkBufferDemo` — обработка сетевого буфера через `new byte[]` и через повторное использование памяти из `ArrayPool<byte>`.
- `StringBuildingDemo` — построение строки через `StringBuilder`, конкатенацию и `string.Create` с записью в целевой буфер.
