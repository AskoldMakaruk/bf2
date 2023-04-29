using FluentAssertions;

namespace ES.Test;

[TestFixture]
public class ViewParserTests
{
    // public class NameView : view
    // {
    //     public string Name { get; set; }
    // }
    //
    // public class PersonView
    // {
    //     public string Name { get; set; }
    //     public int Age { get; set; }
    // }
    //
    // [Test]
    // public void ParseFormat_ShouldFillModelWithValuesFromInputString()
    // {
    //     // Test case 1
    //     string format1 = "Hello, {Name}";
    //     string input1 = "Hello, pedik";
    //     var expectedModel1 = new NameView { Name = "pedik" };
    //     var actualModel1 = new NameView(format1, input1);
    //     actualModel1.Should().BeEquivalentTo(expectedModel1);
    //
    //     // Test case 2
    //     string format2 = "Age: {Age}, Name: {Name}";
    //     string input2 = "Age: 42, Name: John";
    //     var expectedModel2 = new PersonView { Age = 42, Name = "John" };
    //     var actualModel2 = ViewParser.ParseFormat<PersonView>(format2, input2);
    //     actualModel2.Should().BeEquivalentTo(expectedModel2);
    // }
}