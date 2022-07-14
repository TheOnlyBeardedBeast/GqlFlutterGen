namespace GraphqlFlutterGen;
public static class Utils
{
    public static Dictionary<string, string> ScalarMap = new Dictionary<string, string>(){
            {"ID","String"},
            {"String","String"},
            {"Boolean","bool"},
            {"Int","int"},
            {"Float","double"},
            {"Byte","int"},
            {"Short","int"},
            {"Long","int"} ,
            {"Decimal","double"},
            {"Url","Uri"},
            {"DateTime","DateTime"},
            {"Date","DateTime"},
            {"Uuid","Guid"},
            {"Any","object"},
            {"UUID","String"}
        };

    public static void MapFields(this TypeDefinitionItem visited){
        foreach (var item in visited.Fields)
        {
            item.MapFieldTypes();
        }
    }

    public static void MapFieldTypes(this FieldDefinition field){
        string newType = ScalarMap.FirstOrDefault(e => e.Key == field.Type).Value ?? field.Type;
        if(field.OriginalType is not null){
            field.OriginalType = field.OriginalType.Replace(field.Type,newType);
        }
        field.Type = field.Type;
    }
}