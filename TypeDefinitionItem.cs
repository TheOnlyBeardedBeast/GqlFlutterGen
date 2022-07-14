namespace GraphqlFlutterGen;

public class TypeDefinitionItem
{
    public string Name { get; set; } = String.Empty;
    public TypeDefinitionType Type { get; set; }
    public List<FieldDefinition> Fields { get; } = new List<FieldDefinition>();
    public List<string> InterfaceKeys { get; } = new List<string>();
    public List<string> Directives { get; } = new List<string>();
}