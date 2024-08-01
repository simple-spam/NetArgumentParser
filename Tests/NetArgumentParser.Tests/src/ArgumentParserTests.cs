using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NetArgumentParser.Converters;
using NetArgumentParser.Options;
using NetArgumentParser.Options.Context;
using NetArgumentParser.Tests.Models;

// Necessary for using dynamic
[assembly: InternalsVisibleTo("NetArgumentParser")]

namespace NetArgumentParser.Tests;

public class ArgumentParserTest
{
    [Fact]
    public void Parse_FlagOptions_OptionsHandledCorrectly()
    {
        var arguments = new string[]
        {
            "-w",
            "-xyz",
            "-lr",
            "--debug-mode"
        };

        bool savaLog = false;
        bool autoRotate = false;
        bool debugMode = false;
        bool autoIncreaseWidth = false;
        bool printX = false;
        bool printY = false;
        bool printZ = false;

        var options = new ICommonOption[]
        {
            new FlagOption("save-log", "l", afterHandlingAction: () => savaLog = true),
            new FlagOption("auto-rotate", "r", afterHandlingAction: () => autoRotate = true),
            new FlagOption("debug-mode", "d", afterHandlingAction: () => debugMode = true),
            new FlagOption("auto-increase-width", "w", afterHandlingAction: () => autoIncreaseWidth = true),
            new FlagOption("print-x-coord", "x", afterHandlingAction: () => printX = true),
            new FlagOption("print-y-coord", "y", afterHandlingAction: () => printY = true),
            new FlagOption("print-z-coord", "z", afterHandlingAction: () => printZ = true)
        };
        
        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeCompoundOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(savaLog);
        Assert.True(autoRotate);
        Assert.True(debugMode);
        Assert.True(autoIncreaseWidth);
        Assert.True(printX);
        Assert.True(printY);
        Assert.True(printZ);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_EnumValueOptions_OptionsHandledCorrectly()
    {
        var arguments = new string[]
        {
            "-s", StringSplitOptions.RemoveEmptyEntries.ToString(),
            "--bind-mode", "OneWayToSource"
        };

        StringSplitOptions splitOption = default;
        BindMode bindMode = default;

        var options = new ICommonOption[]
        {
            new EnumValueOption<StringSplitOptions>(
                "split-option", "s",
                afterValueParsingAction: t => splitOption = t),

            new EnumValueOption<BindMode>(
                "bind-mode", "b",
                afterValueParsingAction: t => bindMode = t)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(StringSplitOptions.RemoveEmptyEntries, splitOption);
        Assert.Equal(BindMode.OneWayToSource, bindMode);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_ValueOptions_OptionsHandledCorrectly()
    {
        const string boolValue = "true";
        const string byteValue = "10";
        const string charValue = "c";
        const string dateTimeValue = "1 January 2024";
        const string decimalValue = "1234567890";
        const string doubleValue = "-1e-5";
        const string shortValue = "-10000";
        const string intValue = "-10000000";
        const string longValue = "-10000000000";
        const string sbyteValue = "-10";
        const string floatValue = "100.15";
        const string stringValue = "Some text    with spaces\nAnd $symbols-1234";
        const string ushortValue = "10000";
        const string uintValue = "10000000";
        const string ulongValue = "10000000000";

        bool recievedBool = default;
        byte recievedByte = default;
        char recievedChar = default;
        DateTime recievedDateTime = default;
        decimal recievedDecimal = default;
        double recievedDouble = default;
        short recievedShort = default;
        int recievedInt = default;
        long recievedLong = default;
        sbyte recievedSByte = default;
        float recievedFloat = default;
        string recievedString = string.Empty;
        ushort recievedUShort = default;
        uint recievedUInt = default;
        ulong recievedULong = default;
        
        var arguments = new string[]
        {
            "-b", boolValue,
            "-B", byteValue,
            "-c", charValue,
            "--date", dateTimeValue,
            "-d", decimalValue,
            "-D", doubleValue,
            "-s", shortValue,
            "-i", intValue,
            "-l", longValue,
            "--sbyte", sbyteValue,
            "-f", floatValue,
            "-S", stringValue,
            "-U", ushortValue,
            "-I", uintValue,
            "-L", ulongValue,
        };

        var options = new ICommonOption[]
        {
            new ValueOption<bool>(string.Empty, "b", afterValueParsingAction: t => recievedBool = t),
            new ValueOption<byte>(string.Empty, "B", afterValueParsingAction: t => recievedByte = t),
            new ValueOption<char>(string.Empty, "c", afterValueParsingAction: t => recievedChar = t),
            new ValueOption<DateTime>("date", string.Empty, afterValueParsingAction: t => recievedDateTime = t),
            new ValueOption<decimal>(string.Empty, "d", afterValueParsingAction: t => recievedDecimal = t),
            new ValueOption<double>(string.Empty, "D", afterValueParsingAction: t => recievedDouble = t),
            new ValueOption<short>(string.Empty, "s", afterValueParsingAction: t => recievedShort = t),
            new ValueOption<int>(string.Empty, "i", afterValueParsingAction: t => recievedInt = t),
            new ValueOption<long>(string.Empty, "l", afterValueParsingAction: t => recievedLong = t),
            new ValueOption<sbyte>("sbyte", string.Empty, afterValueParsingAction: t => recievedSByte = t),
            new ValueOption<float>(string.Empty, "f", afterValueParsingAction: t => recievedFloat = t),
            new ValueOption<string>(string.Empty, "S", afterValueParsingAction: t => recievedString = t),
            new ValueOption<ushort>(string.Empty, "U", afterValueParsingAction: t => recievedUShort = t),
            new ValueOption<uint>(string.Empty, "I", afterValueParsingAction: t => recievedUInt = t),
            new ValueOption<ulong>(string.Empty, "L", afterValueParsingAction: t => recievedULong = t),
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(bool.Parse(boolValue), recievedBool);
        Assert.Equal(byte.Parse(byteValue), recievedByte);
        Assert.Equal(char.Parse(charValue), recievedChar);
        Assert.Equal(DateTime.Parse(dateTimeValue), recievedDateTime);
        Assert.Equal(decimal.Parse(decimalValue), recievedDecimal);
        Assert.Equal(double.Parse(doubleValue), recievedDouble);
        Assert.Equal(short.Parse(shortValue), recievedShort);
        Assert.Equal(int.Parse(intValue), recievedInt);
        Assert.Equal(long.Parse(longValue), recievedLong);
        Assert.Equal(sbyte.Parse(sbyteValue), recievedSByte);
        Assert.Equal(float.Parse(floatValue), recievedFloat);
        Assert.Equal(stringValue, recievedString);
        Assert.Equal(ushort.Parse(ushortValue), recievedUShort);
        Assert.Equal(uint.Parse(uintValue), recievedUInt);
        Assert.Equal(ulong.Parse(ulongValue), recievedULong);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_MultipleValueOptions_OptionsHandledCorrectly()
    {
        const string leftMargin = "10";
        const string topMargin = "20";
        const string rightMargin = "30";
        const string bottomMargin = "40";

        const string pointX = "-15.123";
        const string pointY = "100.987654321";

        const string file1 = "/home/user1/file1.txt";
        const string file2 = "file2.png";
        const string file3 = "./file3";

        Margin? margin = null;
        Point? point = null;
        List<string>? files = null;

        var arguments = new string[]
        {
            "-m", leftMargin, topMargin, rightMargin, bottomMargin,
            "-f", file1, file2, file3,
            "-p", pointX, pointY
        };

        var options = new ICommonOption[]
        {
            new MultipleValueOption<int>(
                string.Empty, "m",
                contextCapture: new FixedContextCapture(4),
                afterValueParsingAction: t => margin = new Margin(t[0], t[1], t[2], t[3])),
            
            new MultipleValueOption<string>(
                string.Empty, "f",
                contextCapture: new ZeroOrMoreContextCapture(),
                afterValueParsingAction: t => files = new List<string>(t)),
            
            new MultipleValueOption<double>(
                string.Empty, "p",
                contextCapture: new FixedContextCapture(2),
                afterValueParsingAction: t => point = new Point(t[0], t[1]))
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        var expectedMargin = new Margin(
            int.Parse(leftMargin),
            int.Parse(topMargin),
            int.Parse(rightMargin),
            int.Parse(bottomMargin));
        
        var expectedPoint = new Point(
            double.Parse(pointX),
            double.Parse(pointY));
        
        List<string> expectedFiles = [file1, file2, file3];

        Assert.Equal(expectedMargin, margin);
        Assert.Equal(expectedPoint, point);
        Assert.Equal(expectedFiles, files);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_ValueOptions_SpecialConverterApplied()
    {
        const int leftMargin = 10;
        const int topMargin = -20;
        const int rightMargin = 30;
        const int bottomMargin = -40;

        const double pointX = 15.14;
        const double pointY = -14.15;

        const int inputAngle = -45;
        
        int? angle = null;
        Margin? margin = null;
        Point? point = null;

        var arguments = new string[]
        {
            "-m", $"{leftMargin},{topMargin},{rightMargin},{bottomMargin}",
            "-a", inputAngle.ToString(),
            "-p", $"({pointX};{pointY})"
        };

        var options = new ICommonOption[]
        {
            new ValueOption<int>(string.Empty, "a", afterValueParsingAction: t => angle = t),
            new ValueOption<Margin>(string.Empty, "m", afterValueParsingAction: t => margin = t),    
            new ValueOption<Point>(string.Empty, "p", afterValueParsingAction: t => point = t)
        };

        var converters = new IValueConverter[]
        {
            new ValueConverter<Margin>(t =>
            {
                int[] data = t.Split(',').Select(int.Parse).ToArray();
                return new Margin(data[0], data[1], data[2], data[3]);
            }),

            new ValueConverter<Point>(t =>
            {
                double[] data = t[1..(t.Length - 1)]
                    .Split(';')
                    .Select(double.Parse)
                    .ToArray();

                return new Point(data[0], data[1]);
            }),

            new ValueConverter<int>(t => Math.Abs(int.Parse(t)))
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);
        parser.AddConverters(converters);
        
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        var expectedMargin = new Margin(
            leftMargin,
            topMargin,
            rightMargin,
            bottomMargin);
        
        var expectedPoint = new Point(pointX, pointY);

        Assert.Equal(Math.Abs(inputAngle), angle);
        Assert.Equal(expectedMargin, margin);
        Assert.Equal(expectedPoint, point);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_ValueOptions_DefaultValueApplied()
    {
        const int defaultAngle = 45;
        const double defaultWidth = 100.5;
        const double defaultHeight = 400.25;
        const string defaultName = "name";

        const int expectedAngle = defaultAngle;
        const double expectedWidth = 1920;
        const double expectedHeight = defaultHeight;
        const string expectedName = "some_name";
        
        int angle = default;
        double width = default;
        double height = default;
        string name = string.Empty;

        var arguments = new string[]
        {
            "-n", expectedName,
            "-w", expectedWidth.ToString()
        };
        
        var options = new ICommonOption[]
        {
            new ValueOption<int>(
                string.Empty, "a",
                defaultValue: new DefaultOptionValue<int>(defaultAngle),
                afterValueParsingAction: t => angle = t),

            new ValueOption<double>(
                string.Empty, "w",
                defaultValue: new DefaultOptionValue<double>(defaultWidth),
                afterValueParsingAction: t => width = t),

            new ValueOption<double>(
                string.Empty, "h",
                defaultValue: new DefaultOptionValue<double>(defaultHeight),
                afterValueParsingAction: t => height = t),

            new ValueOption<string>(
                string.Empty, "n",
                defaultValue: new DefaultOptionValue<string>(defaultName),
                afterValueParsingAction: t => name = t)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(expectedAngle, angle);
        Assert.Equal(expectedWidth, width);
        Assert.Equal(expectedHeight, height);
        Assert.Equal(expectedName, name);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_ValueOptions_ThrowsExceptionIfRequiredOptionNotHandled()
    {
        var arguments = new string[]
        {
            "-n", "name",
            "-w", "100.5",
        };

        var requiredOptions = new List<ICommonOption>()
        {
            new ValueOption<double>(
                string.Empty, "w",
                isRequired: true),

            new ValueOption<double>(
                string.Empty, "h",
                isRequired: true,
                defaultValue: new DefaultOptionValue<double>(1080)),

            new ValueOption<string>(
                string.Empty, "n",
                isRequired: true,
                defaultValue: new DefaultOptionValue<string>("name"))
        };

        var notSpecifiedRequiredOptionWithoutDefaultValue = new List<ICommonOption>()
        {
            new ValueOption<int>(
                string.Empty, "a",
                isRequired: true),
            
            new ValueOption<string>(
                string.Empty, "b",
                isRequired: true),

            new ValueOption<float>(
                string.Empty, "c",
                isRequired: true),
        };

        var notRequiredOptions = new List<ICommonOption>()
        {
            new ValueOption<int>(
                string.Empty, "A",
                isRequired: false),
            
            new ValueOption<double>(
                string.Empty, "o",
                isRequired: false)
        };
        
        IEnumerable<ICommonOption> allOptions = requiredOptions
            .Concat(notSpecifiedRequiredOptionWithoutDefaultValue)
            .Concat(notRequiredOptions);

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(allOptions.ToArray());

        foreach (ICommonOption option in notSpecifiedRequiredOptionWithoutDefaultValue)
        {
            Assert.Throws<RequiredOptionNotSpecifiedException>(() =>
            {
                parser.Parse(arguments);
            });

            parser.RemoveOption(option);
            parser.ResetOptionsHandledState();
        }

        Exception? ex = Record.Exception(() => parser.Parse(arguments));
        Assert.Null(ex);
    }

    [Fact]
    public void Parse_MinusBasedOptions_ExtraArgumentsExtracted()
    {
        var expectedExtraArguments = new string[]
        {
            "--height",
            "900",
            "24",
            "-h",
            "-125",
            "None",
            "--q"
        };

        var arguments = new string[]
        {
            "myapp",
            "-lr",
            expectedExtraArguments[0],
            expectedExtraArguments[1],
            "--margin", "15", "10", "5", "15",
            expectedExtraArguments[2],
            "-f", "/home/usr/file1.txt", "/home/usr/file2.png", "./file3",
            expectedExtraArguments[3],
            "-a", "-153.123",
            "--width", "500",
            expectedExtraArguments[4],
            "--split-option", StringSplitOptions.RemoveEmptyEntries.ToString(),
            expectedExtraArguments[5],
            expectedExtraArguments[6]
        };

        var options = new ICommonOption[]
        {
            new FlagOption("save_log", "l"),
            new FlagOption("auto-rotate", "r"),
            new EnumValueOption<StringSplitOptions>("split-option", "s"),
            new ValueOption<int>("width", "w"),
            new ValueOption<double>("angle", "a"),
            
            new MultipleValueOption<byte>("margin", "m",
                contextCapture: new FixedContextCapture(4)),
                
            new MultipleValueOption<string>("files", "f",
                contextCapture: new ZeroOrMoreContextCapture())
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            NumberOfArgumentsToSkip = 1,
            RecognizeCompoundOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_SlashBasedOptions_ExtraArgumentsExtracted()
    {
        var expectedExtraArguments = new string[]
        {
            "/Height",
            "900",
            "24",
            "/h",
            "-125",
            "None",
            "--q"
        };

        var arguments = new string[]
        {
            "myapp",
            "-lr",
            expectedExtraArguments[0],
            expectedExtraArguments[1],
            "/Margin", "15", "10", "5", "15",
            expectedExtraArguments[2],
            "-f", "C://path//file1.txt", @"D:\path\file2.png", "./file3", "file4",
            expectedExtraArguments[3],
            "/a", "-153.123",
            "--width", "500",
            expectedExtraArguments[4],
            "/split-option", StringSplitOptions.RemoveEmptyEntries.ToString(),
            expectedExtraArguments[5],
            expectedExtraArguments[6]
        };

        var options = new ICommonOption[]
        {
            new FlagOption("save_log", "l"),
            new FlagOption("auto-rotate", "r"),
            new EnumValueOption<StringSplitOptions>("split-option", "s"),
            new ValueOption<int>("width", "w"),
            new ValueOption<double>("angle", "a"),

            new MultipleValueOption<byte>("Margin", "m",
                contextCapture: new FixedContextCapture(4)),

            new MultipleValueOption<string>("files", "f",
                contextCapture: new ZeroOrMoreContextCapture())
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            NumberOfArgumentsToSkip = 1,
            RecognizeCompoundOptions = true,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_SlashBasedOptionsDisabled_SlashBasedOptionsNotRecognized()
    {
        const StringSplitOptions expectedSplitOption = StringSplitOptions.TrimEntries;
        
        const string file1 = "./file1";
        const string file2 = "/file2";
        const string file3 = "file3";
        const string file4 = "/file4";

        List<string> expectedFiles = [file1, file2, file3, file4];

        bool saveLog = default;
        int width = default;
        StringSplitOptions splitOption = default;
        List<string> files = [];
        
        var arguments = new string[]
        {
            "/l",
            "--Split", expectedSplitOption.ToString(),
            "/W", "100",
            "/Split", StringSplitOptions.RemoveEmptyEntries.ToString(),
            "--files", file1, file2, file3, file4
        };

        var expectedExtraArguments = new string[]
        {
            arguments[0],
            arguments[3],
            arguments[4],
            arguments[5],
            arguments[6]
        };

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "l",
                afterHandlingAction: () => saveLog = true),

            new ValueOption<int>(string.Empty, "W",
                afterValueParsingAction: t => width = t),

            new EnumValueOption<StringSplitOptions>("Split",
                afterValueParsingAction: t => splitOption = t),
            
            new MultipleValueOption<string>("files",
                afterValueParsingAction: t => files = [..t])
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeSlashOptions = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(default, saveLog);
        Assert.Equal(default, width);
        Assert.Equal(expectedSplitOption, splitOption);
        Assert.Equal(expectedFiles, files);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_SlashBasedOptionsEnabled_SlashBasedOptionsRecognized()
    {
        const int expectedWidth = 100;
        const StringSplitOptions expectedSplitOption = StringSplitOptions.RemoveEmptyEntries;
        
        const string file1 = "./file1";
        const string file2 = "/file2";
        const string file3 = "file3";
        const string file4 = "/file4";

        List<string> expectedFiles = [file1];

        bool saveLog = default;
        int width = default;
        StringSplitOptions splitOption = default;
        List<string> files = [];
        
        var arguments = new string[]
        {
            "/l",
            "/W", expectedWidth.ToString(),
            "/Split", expectedSplitOption.ToString(),
            "--files", file1, file2, file3, file4
        };

        var expectedExtraArguments = new string[]
        {
            arguments[7],
            arguments[8],
            arguments[9]
        };

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "l",
                afterHandlingAction: () => saveLog = true),

            new ValueOption<int>(string.Empty, "W",
                afterValueParsingAction: t => width = t),

            new EnumValueOption<StringSplitOptions>("Split",
                afterValueParsingAction: t => splitOption = t),
            
            new MultipleValueOption<string>("files",
                afterValueParsingAction: t => files = [..t])
        };
        
        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(saveLog);
        Assert.Equal(expectedWidth, width);
        Assert.Equal(expectedSplitOption, splitOption);
        Assert.Equal(expectedFiles, files);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_CompoundOptionsDisabled_OptionsNotExpanded()
    {
        var arguments = new string[]
        {
            "-a",
            "-abc",
            "-bc"
        };

        var expectedExtraArguments = new string[]
        {
            arguments[^1]
        };

        bool a = false;
        bool b = false;
        bool c = false;
        bool abc = false;

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "a", afterHandlingAction: () => a = true),
            new FlagOption(string.Empty, "b", afterHandlingAction: () => b = true),
            new FlagOption(string.Empty, "c", afterHandlingAction: () => c = true),
            new FlagOption(string.Empty, "abc", afterHandlingAction: () => abc = true)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeCompoundOptions = false
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(a);
        Assert.True(abc);
        Assert.False(b);
        Assert.False(c);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_CompoundOptionsEnabled_OptionsExpanded()
    {
        var arguments = new string[]
        {
            "-abc",
            "-d"
        };

        bool a = false;
        bool b = false;
        bool c = false;
        bool d = false;

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "a", afterHandlingAction: () => a = true),
            new FlagOption(string.Empty, "b", afterHandlingAction: () => b = true),
            new FlagOption(string.Empty, "c", afterHandlingAction: () => c = true),
            new FlagOption(string.Empty, "d", afterHandlingAction: () => d = true)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeCompoundOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(a);
        Assert.True(b);
        Assert.True(c);
        Assert.True(d);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_NotSkipFirstArguments_ArgumentsNotSkipped()
    {
        var arguments = new string[]
        {
            "-a",
            "-b",
            "-c",
            "-d"
        };

        bool a = false;
        bool b = false;
        bool c = false;
        bool d = false;

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "a", afterHandlingAction: () => a = true),
            new FlagOption(string.Empty, "b", afterHandlingAction: () => b = true),
            new FlagOption(string.Empty, "c", afterHandlingAction: () => c = true),
            new FlagOption(string.Empty, "d", afterHandlingAction: () => d = true)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            NumberOfArgumentsToSkip = 0
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(a);
        Assert.True(b);
        Assert.True(c);
        Assert.True(d);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_SkipFewFirstArguments_ArgumentsSkipped()
    {
        var arguments = new string[]
        {
            "-a",
            "-b",
            "-c",
            "-d"
        };

        bool a = false;
        bool b = false;
        bool c = false;
        bool d = false;

        var options = new ICommonOption[]
        {
            new FlagOption(string.Empty, "a", afterHandlingAction: () => a = true),
            new FlagOption(string.Empty, "b", afterHandlingAction: () => b = true),
            new FlagOption(string.Empty, "c", afterHandlingAction: () => c = true),
            new FlagOption(string.Empty, "d", afterHandlingAction: () => d = true)
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            NumberOfArgumentsToSkip = 3
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.False(a);
        Assert.False(b);
        Assert.False(c);
        Assert.True(d);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_OptionsWithAssignmentCharacter_HandledCorrectly()
    {
        const double expectedAngle = -5.5;
        const int expectedWidth = 1920;
        const int expectedHeight = 1080;
        const StringSplitOptions expectedSplitOption = StringSplitOptions.TrimEntries;

        double angle = default;
        int width = default;
        int height = default;
        StringSplitOptions splitOption = default;

        var expectedExtraArguments = new string[]
        {
            "10",
            "400",
            "100",
            StringSplitOptions.RemoveEmptyEntries.ToString()
        };

        var arguments = new string[]
        {
            $"-a={expectedAngle}",
            expectedExtraArguments[0],
            $"--width={expectedWidth}",
            expectedExtraArguments[1],
            $"/H={expectedHeight}",
            expectedExtraArguments[2],
            $"/split-option={expectedSplitOption}",
            expectedExtraArguments[3]
        };

        var options = new ICommonOption[]
        {
            new ValueOption<double>("angle", "a",
                afterValueParsingAction: t => angle = t),

            new ValueOption<int>("width", "w",
                afterValueParsingAction: t => width = t),

            new ValueOption<int>("height", "H",
                afterValueParsingAction: t => height = t),
            
            new EnumValueOption<StringSplitOptions>("split-option", "s",
                afterValueParsingAction: t => splitOption = t),
        };
        
        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(expectedAngle, angle);
        Assert.Equal(expectedWidth, width);
        Assert.Equal(expectedHeight, height);
        Assert.Equal(expectedSplitOption, splitOption);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_CounterOption_DuplicatesHandled()
    {
        int verbosityLevel = default;
        const int expectedVerbosityLevel = 5;

        var expectedExtraArguments = new string[]
        {
            "--angle",
            "45"
        };

        var arguments = new string[]
        {
            "-V",
            expectedExtraArguments[0],
            expectedExtraArguments[1],
            "-VVVV"
        };

        var options = new ICommonOption[]
        {
            new CounterOption(string.Empty, "V", increaseCounter: () => verbosityLevel++)
        };

        var parser = new ArgumentParser()
        {
            RecognizeCompoundOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.Equal(expectedVerbosityLevel, verbosityLevel);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    [Fact]
    public void Parse_HelpOption_OtherArgumentsSkipped()
    {
        bool help = default;
        bool verbose = default;
        double angle = default;
        StringSplitOptions splitOption = default;
        List<string>? files = default;

        var arguments = new string[]
        {
            "-v",
            "--angle", "100.5",
            "--help",
            "-s", StringSplitOptions.TrimEntries.ToString(),
            "-f", "file1", "file2", "file3"
        };
        
        var options = new ICommonOption[]
        {
            new FlagOption("verbose", "v",
                isRequired: true,
                afterHandlingAction: () => verbose = true),

            new HelpOption("help", afterHandlingAction: () => help = true),

            new ValueOption<double>("angle", "a",
                isRequired: true,
                afterValueParsingAction: t => angle = t),
            
            new EnumValueOption<StringSplitOptions>("split-option", "s",
                afterValueParsingAction: t => splitOption = t),

            new MultipleValueOption<string>("files", "f",
                isRequired: true,
                contextCapture: new ZeroOrMoreContextCapture(),
                afterValueParsingAction: t => files = [..t])
        };
        
        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(help);

        Assert.Equal(default, verbose);
        Assert.Equal(default, angle);
        Assert.Equal(default, splitOption);
        Assert.Equal(default, files);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_VersionOption_OtherArgumentsSkipped()
    {
        bool version = default;
        bool verbose = default;
        double angle = default;
        StringSplitOptions splitOption = default;
        List<string>? files = default;

        var arguments = new string[]
        {
            "-v",
            "--angle", "100.5",
            "--version",
            "-s", StringSplitOptions.TrimEntries.ToString(),
            "-f", "file1", "file2", "file3"
        };
        
        var options = new ICommonOption[]
        {
            new FlagOption("verbose", "v",
                isRequired: true,
                afterHandlingAction: () => verbose = true),

            new VersionOption("version", afterHandlingAction: () => version = true),

            new ValueOption<double>("angle", "a",
                isRequired: true,
                afterValueParsingAction: t => angle = t),
            
            new EnumValueOption<StringSplitOptions>("split-option", "s",
                afterValueParsingAction: t => splitOption = t),

            new MultipleValueOption<string>("files", "f",
                isRequired: true,
                contextCapture: new ZeroOrMoreContextCapture(),
                afterValueParsingAction: t => files = [..t])
        };
        
        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(version);

        Assert.Equal(default, verbose);
        Assert.Equal(default, angle);
        Assert.Equal(default, splitOption);
        Assert.Equal(default, files);

        Assert.Empty(extraArguments);
    }

    [Fact]
    public void Parse_DuplicateArguments_ThrowsException()
    {
        var arguments = new string[]
        {
            "-a", "5",
            "-w", "1920",
            "-a", "10"
        };

        var options = new ICommonOption[]
        {
            new ValueOption<int>(string.Empty, "a"),
            new ValueOption<double>(string.Empty, "w")
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false
        };

        parser.AddOptions(options);

        Assert.Throws<OptionAlreadyHandledException>(() =>
        {
            parser.Parse(arguments);
        });
    }

    [Fact]
    public void Parse_SeveralUseCases_ArgumentsParsedCorrectly()
    {
        const int marginLeft = 15;
        const int marginTop = 10;
        const int marginRight = 5;
        const int marginBottom = 15;

        const int pointX = 30;
        const int pointY = -40;

        const string file1 = "C://path//file1.txt";
        const string file2 = @"D:\path\file2.png";
        const string file3 = "./file3";
        const string file4 = "file4";

        const int expectedWidth = 500;
        const int expectedVerbosityLevel = 5;
        const double expectedOpacity = 0.5;
        const double expectedAbsAngle = 180;
        const double expectedAngle = -153.123;

        const BindMode expectedBindMode = BindMode.TwoWay;
        const StringSplitOptions expectedSplitOption = StringSplitOptions.RemoveEmptyEntries;

        var expectedMargin = new Margin(
            marginLeft,
            marginTop,
            marginRight,
            marginBottom);
        
        var expectedPoint = new Point(pointX, pointY);
        string[] expectedFiles = [file1, file2, file3, file4];

        bool saveLog = default;
        bool autoRotate = default;
        bool quickMode = default;
        BindMode bindMode = default;
        StringSplitOptions splitOption = default;
        int width = default;
        int verbosityLevel = default;
        double angle = default;
        double absAngle = default;
        double opacity = default;
        Point point = default;
        Margin? margin = null;
        List<string>? files = [];
        
        var expectedExtraArguments = new string[]
        {
            "height",
            "900",
            "24",
            "/h",
            "-125",
            "None",
            "--L",
            "0.9"
        };

        var arguments = new string[]
        {
            "myapp",
            "rebase",
            expectedExtraArguments[0],
            expectedExtraArguments[1],
            "-V",
            "/m", $"{marginLeft}", $"{marginTop}", $"{marginRight}", $"{marginBottom}",
            expectedExtraArguments[2],
            "--P", $"({pointX};{pointY})",
            "-f", file1, file2, file3, file4,
            expectedExtraArguments[3],
            "-a", $"{expectedAngle}",
            "-lr",
            "--width", $"{expectedWidth}",
            "-q",
            "-VVVV",
            expectedExtraArguments[4],
            "/split-option", $"{expectedSplitOption}",
            expectedExtraArguments[5],
            expectedExtraArguments[6],
            "--abs-angle", $"-{expectedAbsAngle}",
            $"--opacity={expectedOpacity}",
            $"-bind={expectedBindMode}",
            expectedExtraArguments[7]
        };

        var optionWithCustomConverter = new ValueOption<double>(
            longName: "abs-angle",
            afterValueParsingAction: t => absAngle = t)
        {
            Converter = new ValueConverter<double>(t => Math.Abs(double.Parse(t)))
        };

        var options = new ICommonOption[]
        {
            optionWithCustomConverter,

            new FlagOption("save_log", "l",
                afterHandlingAction: () => saveLog = true),

            new FlagOption("auto-rotate", "r",
                afterHandlingAction: () => autoRotate = true),

            new FlagOption("quick-mode", "q",
                afterHandlingAction: () => quickMode = true),

            new CounterOption(string.Empty, "V",
                increaseCounter: () => verbosityLevel++),

            new EnumValueOption<BindMode>("bind", "b",
                afterValueParsingAction: t => bindMode = t),

            new EnumValueOption<StringSplitOptions>("split-option", "s",
                afterValueParsingAction: t => splitOption = t),

            new ValueOption<int>("width", "w",
                afterValueParsingAction: t => width = t),

            new ValueOption<double>("angle", "a",
                afterValueParsingAction: t => angle = t),

            new ValueOption<double>("opacity", "o",
                afterValueParsingAction: t => opacity = t),

            new ValueOption<Point>("P",
                afterValueParsingAction: t => point = t),

            new MultipleValueOption<byte>("margin", "m",
                contextCapture: new FixedContextCapture(4),
                afterValueParsingAction: t => margin = new Margin(t[0], t[1], t[2], t[3])),

            new MultipleValueOption<string>("files", "f",
                contextCapture: new ZeroOrMoreContextCapture(),
                afterValueParsingAction: t => files = [..t])
        };

        var converters = new IValueConverter[]
        {
            new ValueConverter<Point>(t =>
            {
                double[] data = t[1..(t.Length - 1)]
                    .Split(';')
                    .Select(double.Parse)
                    .ToArray();

                return new Point(data[0], data[1]);
            })
        };

        var parser = new ArgumentParser()
        {
            UseDefaultHelpOption = false,
            NumberOfArgumentsToSkip = 2,
            RecognizeCompoundOptions = true,
            RecognizeSlashOptions = true
        };

        parser.AddOptions(options);
        parser.AddConverters(converters);

        parser.ParseKnownArguments(arguments, out List<string> extraArguments);

        Assert.True(saveLog);
        Assert.True(autoRotate);
        Assert.True(quickMode);
  
        Assert.Equal(expectedSplitOption, splitOption);
        Assert.Equal(expectedWidth, width);
        Assert.Equal(expectedVerbosityLevel, verbosityLevel);
        Assert.Equal(expectedAngle, angle);
        Assert.Equal(expectedAbsAngle, absAngle);
        Assert.Equal(expectedOpacity, opacity);
        Assert.Equal(expectedPoint, point);
        Assert.Equal(expectedMargin, margin);
        Assert.Equal(expectedFiles, files);

        VerifyExtraArguments(expectedExtraArguments, extraArguments);
    }

    private static void VerifyExtraArguments(
        IEnumerable<string> expected,
        IEnumerable<string> actual)
    {
        ArgumentNullException.ThrowIfNull(actual, nameof(expected));
        ArgumentNullException.ThrowIfNull(actual, nameof(actual));

        expected = expected.OrderBy(t => t);
        actual = actual.OrderBy(t => t);

        Assert.True(expected.SequenceEqual(actual));
    }
}
