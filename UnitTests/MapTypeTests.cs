using ITI.TypeScriptDtoGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namotion.Reflection;

namespace UnitTests;

[TestClass]
public class MapTypeTests
{
    [TestMethod]
    public void ItMapsPrimitiveTypes()
    {
        var unknownTypes = new List<Type>();

        Assert.AreEqual("string", DtoGenerator.MapType(typeof(string).ToContextualType(), unknownTypes));
        Assert.AreEqual("number", DtoGenerator.MapType(typeof(int).ToContextualType(), unknownTypes));
        Assert.AreEqual("number", DtoGenerator.MapType(typeof(double).ToContextualType(), unknownTypes));
        Assert.AreEqual("boolean", DtoGenerator.MapType(typeof(bool).ToContextualType(), unknownTypes));
        Assert.AreEqual("string", DtoGenerator.MapType(typeof(DateTime).ToContextualType(), unknownTypes));
        Assert.AreEqual("string", DtoGenerator.MapType(typeof(DateTimeOffset).ToContextualType(), unknownTypes));
        Assert.AreEqual("string", DtoGenerator.MapType(typeof(TimeSpan).ToContextualType(), unknownTypes));

        Assert.AreEqual(0, unknownTypes.Count);
    }

    [TestMethod]
    public void ItMapsNullableValueTypes()
    {
        var unknownTypes = new List<Type>();

        Assert.AreEqual("number | null | undefined", DtoGenerator.MapType(typeof(int?).ToContextualType(), unknownTypes));
        Assert.AreEqual("number | null | undefined", DtoGenerator.MapType(typeof(double?).ToContextualType(), unknownTypes));

        Assert.AreEqual(0, unknownTypes.Count);
    }

    [TestMethod]
    public void ItMapsLists()
    {
        var unknownTypes = new List<Type>();

        Assert.AreEqual(
            "number[]",
            DtoGenerator.MapType(typeof(List<int>).ToContextualType(), unknownTypes)
        );
        Assert.AreEqual(
            "(number | null | undefined)[]",
            DtoGenerator.MapType(typeof(List<int?>).ToContextualType(), unknownTypes)
        );

        Assert.AreEqual(0, unknownTypes.Count);
    }

    [TestMethod]
    public void ItMapsArrays()
    {
        var unknownTypes = new List<Type>();

        Assert.AreEqual(
            "number[]",
            DtoGenerator.MapType(typeof(int[]).ToContextualType(), unknownTypes)
        );
        Assert.AreEqual(
            "(number | null | undefined)[]",
            DtoGenerator.MapType(typeof(int?[]).ToContextualType(), unknownTypes)
        );
        Assert.AreEqual(
            "number[]",
            DtoGenerator.MapType(typeof(double[]).ToContextualType(), unknownTypes)
        );

        Assert.AreEqual(0, unknownTypes.Count);
    }

    private class TestDto
    {
        public string? NullableString { get; set; }
        public List<string?>? NullableStringList { get; set; }

        public char Character { get; set; }

        public char? NullableCharacter { get; set; }
    }

    [TestMethod]
    public void ItMapsNullableReferenceTypes()
    {
        var unknownTypes = new List<Type>();

        var properties = typeof(TestDto).GetContextualProperties();
        var nullableString = properties.Single(p => p.Name == "NullableString");
        var nullableStringList = properties.Single(p => p.Name == "NullableStringList");

        Assert.AreEqual(
            "string | null | undefined",
            DtoGenerator.MapType(nullableString.PropertyType, unknownTypes)
        );
        Assert.AreEqual(
            "(string | null | undefined)[] | null | undefined",
            DtoGenerator.MapType(nullableStringList.PropertyType, unknownTypes)
        );

        Assert.AreEqual(0, unknownTypes.Count);
    }

    [TestMethod]
    public void CharsMapToString()
    {
        var unknownTypes = new List<Type>();

        var properties = typeof(TestDto).GetContextualProperties();
        var character = properties.Single(p => p.Name == "Character");
        var nullableCharacter = properties.Single(p => p.Name == "NullableCharacter");

        Assert.AreEqual(
            "string",
            DtoGenerator.MapType(character.PropertyType, unknownTypes)
        );

        Assert.AreEqual(
            "string | null | undefined",
            DtoGenerator.MapType(nullableCharacter.PropertyType, unknownTypes)
        );

        Assert.AreEqual(0, unknownTypes.Count);
    }
}
