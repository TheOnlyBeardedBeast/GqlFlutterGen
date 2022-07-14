public enum WrapperType {
    NONNULLABLE,
    LIST,
}

public class Wrapper{
    public WrapperType Type { get; set; }
}

public class FieldDefinition{
    public string Name { get; set; } = default!;
    public bool Nullable { get; set; }
    public string Type { get; set; } = default!;
    public string OriginalType { get; set; } = default!;
    public List<string> Directives { get; } = new List<string>();
    public List<Wrapper> Wrappers { get; } = new List<Wrapper>();
}