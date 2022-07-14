using HotChocolate.Language;

namespace GraphqlFlutterGen;

public enum OType {
    OPERATION,
    FRAGMENT
}

public class Selection {
    public string Field {get; set;}
    public string Alias { get; set; }
    public List<string> Arguments {get; set;} = new List<string>();
    public List<Selection> SubSelection {get; set;} = new List<Selection>();

    public string Type {get; set;}
}

public class Variable {
    public string Name { get; set; }
    public string Type { get; set; }
    public object? DefaultValue { get; set; }
}

public class QueryType{
    public OType Type {get; set;}

    public List<Variable> Variables {get; set;} = new List<Variable>();
    public Selection Selection {get; set;}
    public string Name {get; set;}

    public OperationType OperationType {get; set;}
};

public class OperationWalker
    : QuerySyntaxWalker<QueryType>
{

    protected readonly List<TypeDefinitionItem> schema;
    public OperationWalker(List<TypeDefinitionItem> schema)
    {
        this.schema = schema;
    }
    public List<QueryType> Visited = new List<QueryType>();

    public override void Visit(
        DocumentNode node,
        QueryType context)
    {
        if (node != null)
        {
            VisitDocument(node, context);
        }
    }

    protected override void VisitDocument(
        DocumentNode node,
        QueryType context)
    {
        VisitMany(node.Definitions, context, VisitDefinition);
    }

    protected override void VisitDefinition(
        IDefinitionNode node,
        QueryType context)
    {
        switch (node)
        {
            case OperationDefinitionNode value:
                VisitOperationDefinition(value, context);
                break;
            case FragmentDefinitionNode value:
                if (VisitFragmentDefinitions)
                {
                    VisitFragmentDefinition(value, context);
                }
                break;
            default:
                VisitUnsupportedDefinitions(node, context);
                break;
        }
    }

    protected override void VisitOperationDefinition(
        OperationDefinitionNode node,
        QueryType context)
    {
        var item = new QueryType();
        item.OperationType = node.Operation;

        if (node.Name != null)
        {

            item.Name = node.Name.Value;

            using (var variables = node.VariableDefinitions.GetEnumerator())
        {
            while (variables.MoveNext())
            {
                item.Variables.Add(new Variable{
                    Name = variables.Current.Variable.Name.Value,
                    Type = variables.Current.Type.ToString(),
                });
            }
        }

            

            // VisitName(node.Name, context);

            VisitMany(
                node.VariableDefinitions,
                context,
                VisitVariableDefinition);

            VisitMany(
                node.Directives,
                context,
                VisitDirective);
        }
        if (node.SelectionSet != null)
        {
            item.Selection = new Selection();
            OVisitSelectionSet(node.SelectionSet, item.Selection, "Query");
        }

        Visited.Add(item);
    }

    protected override void VisitName(NameNode node, QueryType context)
    {
        // context.Name = node.Value;
    }

    protected override void VisitVariableDefinition(
       VariableDefinitionNode node,
       QueryType context)
    {
        VisitVariable(node.Variable, context);
        VisitType(node.Type, context);


        if (node.DefaultValue != null)
        {
            VisitValue(node.DefaultValue, context);
        }
    }

    protected override void VisitFragmentDefinition(
        FragmentDefinitionNode node,
        QueryType context)
    {
        VisitName(node.Name, context);

        VisitMany(
            node.VariableDefinitions,
            context,
            VisitVariableDefinition);

        VisitNamedType(node.TypeCondition, context);

        VisitMany(
            node.Directives,
            context,
            VisitDirective);

        if (node.SelectionSet != null)
        {
            VisitSelectionSet(node.SelectionSet, context);
        }
    }

    protected void OVisitSelectionSet(
        SelectionSetNode node,
        Selection context, string typeRoot)
    {

        foreach (var item in node.Selections)
            {
                var selection = new Selection();
                OVisitSelection(item, selection, typeRoot);
                context.SubSelection.Add(selection);
            }
    }

    protected void OVisitSelection(ISelectionNode node, Selection context,string typeRoot)
    {
        switch (node.Kind)
        {
            case SyntaxKind.Field:
                OVisitSelectionField((FieldNode)node, context, typeRoot);
                break;
            // case SyntaxKind.FragmentSpread:
            //     VisitFragmentSpread((FragmentSpreadNode)node, context);
            //     break;
            // case SyntaxKind.InlineFragment:
            //     VisitInlineFragment((InlineFragmentNode)node, context);
            //     break;
            default:
                throw new NotSupportedException();
        }
    }

    protected void OVisitSelectionField(
        FieldNode node,
        Selection context,
        string typeRoot)
    {
        if (node.Alias != null)
        {
            // VisitName(node.Alias, context);
            context.Alias = node.Alias.Value;
        }

        context.Field = node.Name.Value;

        // VisitName(node.Name, context);

        // VisitMany(
        //     node.Arguments,
        //     context,
        //     VisitArgument);

        // VisitMany(
        //     node.Directives,
        //     context,
        //     VisitDirective);

        if (node.SelectionSet != null)
        {
            OVisitSelectionSet(node.SelectionSet, context, "");
        }
    }

    protected override void VisitField(
        FieldNode node,
        QueryType context)
    {
        if (node.Alias != null)
        {
            VisitName(node.Alias, context);
        }

        VisitName(node.Name, context);

        VisitMany(
            node.Arguments,
            context,
            VisitArgument);

        VisitMany(
            node.Directives,
            context,
            VisitDirective);

        if (node.SelectionSet != null)
        {
            VisitSelectionSet(node.SelectionSet, context);
        }
    }

    protected override void VisitFragmentSpread(
        FragmentSpreadNode node,
        QueryType context)
    {
        VisitName(node.Name, context);

        VisitMany(
            node.Directives,
            context,
            VisitDirective);
    }

    protected override void VisitInlineFragment(
        InlineFragmentNode node,
        QueryType context)
    {
        if (node.TypeCondition != null)
        {
            VisitNamedType(node.TypeCondition, context);
        }

        VisitMany(
            node.Directives,
            context,
            VisitDirective);

        if (node.SelectionSet != null)
        {
            VisitSelectionSet(node.SelectionSet, context);
        }
    }
}