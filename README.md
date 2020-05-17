# Motivation

Чтобы использовать [сериализацию](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/#binary-and-xml-serialization) для хранения кеша в приложение. Необходимо поддерживать совместимость между вресиями.

Как это обеспечить:
- использовать [Version tolerant serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/version-tolerant-serialization).
- перезаписывать файл в случае если известно что модель поменялась.

# implementation options

## SharpObjectGenerator

- Create object by model.
- Get hashcode of object.

### restrictions
- Each time object creation must be the same.
- Every implementation of abstraction must be set.

### used
- [AutoBogus](https://github.com/nickdodd79/AutoBogus) - object generator by adding auto creation 
- [DeepEqual](https://github.com/jamesfoster/DeepEqual) - for test equals of objects 