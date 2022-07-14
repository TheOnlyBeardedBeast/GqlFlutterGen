namespace GraphqlFlutterGen;

public class TypeDefinitionField
    {
        public bool NotNullable { get; set; } = false;
        public bool Nullable
        {
            get { return !this.NotNullable; }
        }
        
        public bool IsList { get; set; } = false;
        public string Type { get; set; } = String.Empty;
        public TypeDefinitionField? InnerType { get; set; }
        public bool HasInnerType {get { return InnerType is not null;}}
        public string Name { get; set; } = String.Empty;
        public List<string> Directives { get; } = new List<string>();
    }