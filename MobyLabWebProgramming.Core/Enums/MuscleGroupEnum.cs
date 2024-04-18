using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;
using System.Text.Json.Serialization;

namespace MobyLabWebProgramming.Core.Enums;

/// <summary>
/// This is and example of a smart enum, you can modify it however you see fit.
/// Note that the class is decorated with a JsonConverter attribute so that it is properly serialized as a JSON.
/// </summary>
[JsonConverter(typeof(SmartEnumNameConverter<MuscleGroupEnum, string>))]
public sealed class MuscleGroupEnum : SmartEnum<MuscleGroupEnum, string>
{
    public static readonly MuscleGroupEnum Chest = new(nameof(Chest), "Chest");
    public static readonly MuscleGroupEnum Back = new(nameof(Back), "Back");
    public static readonly MuscleGroupEnum Legs = new(nameof(Legs), "Legs");
    public static readonly MuscleGroupEnum Biceps = new(nameof(Biceps), "Biceps");
    public static readonly MuscleGroupEnum Triceps = new(nameof(Triceps), "Triceps");
    public static readonly MuscleGroupEnum Shoulders = new(nameof(Shoulders), "Shoulders");

    private MuscleGroupEnum(string name, string value) : base(name, value)
    {
    }
}