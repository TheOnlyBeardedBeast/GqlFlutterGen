using HotChocolate.Language;
using System.Text.Json;

namespace GraphqlFlutterGen;

public class DummySchemaSyntaxWalker
    : SchemaSyntaxWalker<TypeDefinitionItem>
{
    public List<TypeDefinitionItem> VisitedItems { get; set; } = new List<TypeDefinitionItem>();

    public override void Visit(
        DocumentNode node,
        TypeDefinitionItem context)
    {
        if (node != null)
        {
            VisitDocument(node, context);
        }
    }

    protected override void VisitDocument(
        DocumentNode node,
        TypeDefinitionItem context)
    {
        VisitMany(node.Definitions, context, VisitDefinition);
    }

    protected override void VisitDefinition(
        IDefinitionNode node,
        TypeDefinitionItem context)
    {
        if (node is ITypeSystemExtensionNode)
        {
            VisitTypeExtensionDefinition(node, context);
        }
        else
        {
            VisitTypeDefinition(node, context);
        }
    }


    protected override void VisitTypeDefinition(
        IDefinitionNode node,
        TypeDefinitionItem context)
    {
        switch (node)
        {
            case SchemaDefinitionNode value:
                VisitSchemaDefinition(value, context);
                break;
            case DirectiveDefinitionNode value:
                VisitDirectiveDefinition(value, context);
                break;
            case ScalarTypeDefinitionNode value:
                VisitScalarTypeDefinition(value, context);
                break;
            case ObjectTypeDefinitionNode value:
                VisitObjectTypeDefinition(value, context);
                break;
            case InputObjectTypeDefinitionNode value:
                VisitInputObjectTypeDefinition(value, context);
                break;
            case InterfaceTypeDefinitionNode value:
                VisitInterfaceTypeDefinition(value, context);
                break;
            case UnionTypeDefinitionNode value:
                VisitUnionTypeDefinition(value, context);
                break;
            case EnumTypeDefinitionNode value:
                VisitEnumTypeDefinition(value, context);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    protected override void VisitTypeExtensionDefinition(
        IDefinitionNode node,
        TypeDefinitionItem context)
    {
        switch (node)
        {
            case SchemaExtensionNode value:
                VisitSchemaExtension(value, context);
                break;
            case ScalarTypeExtensionNode value:
                VisitScalarTypeExtension(value, context);
                break;
            case ObjectTypeExtensionNode value:
                VisitObjectTypeExtension(value, context);
                break;
            case InterfaceTypeExtensionNode value:
                VisitInterfaceTypeExtension(value, context);
                break;
            case UnionTypeExtensionNode value:
                VisitUnionTypeExtension(value, context);
                break;
            case EnumTypeExtensionNode value:
                VisitEnumTypeExtension(value, context);
                break;
            case InputObjectTypeExtensionNode value:
                VisitInputObjectTypeExtension(value, context);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    protected override void VisitSchemaDefinition(
        SchemaDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitMany(
            node.Directives,
            context,
            VisitDirective);

        VisitMany(
            node.OperationTypes,
            context,
            VisitOperationTypeDefinition);
    }

    protected override void VisitSchemaExtension(
        SchemaExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitMany(
            node.Directives,
            context,
            VisitDirective);

        VisitMany(
            node.OperationTypes,
            context,
            VisitOperationTypeDefinition);
    }

    protected override void VisitOperationTypeDefinition(
        OperationTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitNamedType(node.Type, context);
    }

    protected override void VisitDirectiveDefinition(
        DirectiveDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Arguments, context, VisitInputValueDefinition);
        VisitMany(node.Locations, context, VisitName);
    }

    protected override void VisitScalarTypeDefinition(
        ScalarTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
    }

    protected override void VisitScalarTypeExtension(
        ScalarTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
    }

    protected override void VisitObjectTypeDefinition(
        ObjectTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        var item = new TypeDefinitionItem { Name = node.Name.ToString(), Type = TypeDefinitionType.Type };

        using (var interfaces = node.Interfaces.GetEnumerator())
        {
            while (interfaces.MoveNext())
            {
                item.InterfaceKeys.Add(interfaces.Current.Name.Value);
            }
        }

        using (var directives = node.Directives.GetEnumerator())
        {
            while (directives.MoveNext())
            {
                item.Directives.Add(directives.Current.Name.Value);
            }
        }

        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Interfaces, context, VisitNamedType);
        VisitMany(node.Fields, item, VisitFieldDefinition);

        this.VisitedItems.Add(item);
    }

    protected override void VisitObjectTypeExtension(
        ObjectTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        var item = new TypeDefinitionItem { Name = node.Name.ToString(), Type = TypeDefinitionType.Extension };

        List<string> intfaces = new List<string>();
        using (var interfaces = node.Interfaces.GetEnumerator())
        {
            while (interfaces.MoveNext())
            {
                intfaces.Add(interfaces.Current.Name.Value);
            }
        }

        item.InterfaceKeys.AddRange(intfaces);

        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Interfaces, context, VisitNamedType);
        VisitMany(node.Fields, item, VisitFieldDefinition);

        this.VisitedItems.Add(item);
    }

    protected override void VisitFieldDefinition(
        FieldDefinitionNode node,
        TypeDefinitionItem context)
    {
        var field = this.TransformTypeField(node.Type, null);
        field.Name = node.Name.ToString();

        context.Fields.Add(field);
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Arguments, context, VisitInputValueDefinition);
        VisitType(node.Type, context);
        VisitMany(node.Directives, context, VisitDirective);
    }

    protected override void VisitInputObjectTypeDefinition(
        InputObjectTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Fields, context, VisitInputValueDefinition);
    }

    protected override void VisitInputObjectTypeExtension(
        InputObjectTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Fields, context, VisitInputValueDefinition);
    }

    protected override void VisitInterfaceTypeDefinition(
       InterfaceTypeDefinitionNode node,
       TypeDefinitionItem context)
    {
        var item = new TypeDefinitionItem { Name = node.Name.ToString(), Type = TypeDefinitionType.Interface };
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Fields, item, VisitFieldDefinition);
        this.VisitedItems.Add(item);
    }

    protected override void VisitInterfaceTypeExtension(
        InterfaceTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Fields, context, VisitFieldDefinition);
    }

    protected override void VisitUnionTypeDefinition(
        UnionTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Types, context, VisitNamedType);
    }

    protected override void VisitUnionTypeExtension(
        UnionTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Types, context, VisitNamedType);
    }

    protected override void VisitEnumTypeDefinition(
        EnumTypeDefinitionNode node,
        TypeDefinitionItem context)
    {
        var item = new TypeDefinitionItem();
        item.Name = node.Name.ToString();
        item.Type = TypeDefinitionType.Enum;

        VisitName(node.Name, context);
        VisitIfNotNull(node.Description, context, VisitStringValue);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Values, item, VisitEnumValueDefinition);

        VisitedItems.Add(item);
    }

    protected override void VisitEnumTypeExtension(
        EnumTypeExtensionNode node,
        TypeDefinitionItem context)
    {
        VisitName(node.Name, context);
        VisitMany(node.Directives, context, VisitDirective);
        VisitMany(node.Values, context, VisitEnumValueDefinition);
    }

    protected override void VisitEnumValueDefinition(
            EnumValueDefinitionNode node,
            TypeDefinitionItem context)
    {
        context.Fields.Add(new FieldDefinition { Name = node.Name.ToString() });
    }

    private static void VisitIfNotNull<T>(
        T? node,
        TypeDefinitionItem context,
        Action<T, TypeDefinitionItem> visitor)
        where T : class
    {
        if (node != null)
        {
            visitor(node, context);
        }
    }

    protected FieldDefinition TransformTypeField(ITypeNode type, FieldDefinition? context, bool nullable = false)
    {
        var ctx = context ?? new FieldDefinition();

        ctx.OriginalType = type.ToString();

    check:
        if (type.IsListType())
        {
            ctx.Wrappers.Add(new Wrapper
            {
                Type = WrapperType.LIST,
            });
            type = type.InnerType();
            goto check;
        }

        if (type.IsNonNullType())
        {
            ctx.Wrappers.Add(new Wrapper
            {
                Type = WrapperType.NONNULLABLE,
            });
            type = type.InnerType();
            goto check;
        }


        ctx.Type = type.ToString();
        return ctx;
    }
}