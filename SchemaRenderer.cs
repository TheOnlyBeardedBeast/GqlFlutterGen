using System.Text;
using Stubble.Core;
using Stubble.Core.Builders;

namespace GraphqlFlutterGen;

public class SchemaRenderer
{
    private readonly List<TypeDefinitionItem> items;
    private readonly string classTemplate;
    private readonly string classFieldTemplate;
    private readonly string enumTemplate;
    private readonly string classFieldTypeTemplate;
    private readonly string variableClassTemplate;
    private readonly StubbleVisitorRenderer stubble;


    public SchemaRenderer(List<TypeDefinitionItem> items, string classTemplate, string classFieldTemplate, string enumTemplate, string classFieldTypeTemplate, string variableClassTemplate)
    {
        this.items = items;
        this.classTemplate = classTemplate;
        this.classFieldTemplate = classFieldTemplate;
        this.enumTemplate = enumTemplate;
        this.classFieldTypeTemplate = classFieldTypeTemplate;
        this.variableClassTemplate = variableClassTemplate;
        this.stubble = new StubbleBuilder().Build();
    }
    public void RenderSchema()
    {

        Console.WriteLine("import 'package:simple_json_mapper/simple_json_mapper.dart';");

        foreach (var item in items)
        {
            item.MapFields();

            if (item.Type == TypeDefinitionType.Enum)
            {
                var output = stubble.Render(enumTemplate, item);

                Console.WriteLine(output);
            }

            if (item.Type == TypeDefinitionType.Type)
            {
                this.RenderClass(item);
            }

            if (item.Type == TypeDefinitionType.VariableType)
            {
                this.RenderClass(item, variableClassTemplate);
            }
        }
    }

    protected void RenderClass(TypeDefinitionItem item, string? template = null)
    {
        var output = stubble.Render(template ?? classTemplate, item);
        var sb = new StringBuilder();

        for (int i = 0; i < item.Fields.Count; i++)
        {
            if (item.Fields[i] is not null)
            {
                if (i == item.Fields.Count - 1)
                {
                    sb.Append(RenderField(item.Fields[i]));
                }
                else
                {
                    sb.AppendLine(RenderField(item.Fields[i]));
                }
            }
        }

        output = output.Replace("##Fields##", sb.ToString());

        Console.WriteLine(output);
    }

    protected string RenderField(FieldDefinition item)
    {
        var output = stubble.Render(classFieldTemplate, item).Replace("##Type##", this.RenderFieldType(item));
        return output;
    }

    protected string RenderFieldType(FieldDefinition item)
    {
        var typeNotNullable = item.OriginalType.IndexOf(item.Type + "!") > -1;

        var result = item.OriginalType.Replace("]!", ">").Replace("]", ">?").Replace("[", "List<").Replace("!", "");
        if (typeNotNullable)
        {
            return result;
        }
        
        return result + "?";
    }
}