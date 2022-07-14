using HotChocolate.Language;
using Stubble.Core;
using Stubble.Core.Builders;

namespace GraphqlFlutterGen;

class OperationResult {
    public string Name { get; set; }
    public string RootType { get; set; }
    public string Type { get; set; }
}

public class OperationRenderer
{
    private readonly List<QueryType> opertions;
    private readonly List<TypeDefinitionItem> items;
    private readonly string operationClassTemplate;
    private readonly StubbleVisitorRenderer stubble;

    public readonly List<TypeDefinitionItem> operationTypes = new List<TypeDefinitionItem>();
    public Dictionary<OperationType,string> OperationMap = new Dictionary<OperationType,string>
    {
        { OperationType.Query,"Query"},
        { OperationType.Mutation,"Mutation"},
        { OperationType.Subscription,"Subscription"}
    };
    public OperationRenderer(List<QueryType> opertions, List<TypeDefinitionItem> items, string operationClassTemplate)
    {
        this.opertions = opertions;
        this.items = items;
        this.operationClassTemplate = operationClassTemplate;
        this.stubble = new StubbleBuilder().Build();
    }

    

    public void RenderOperations()
    {
        foreach (QueryType item in this.opertions)
        {
                var operationType = new TypeDefinitionItem();
                operationType.Name = char.ToUpper(item.Name[0]) + item.Name.Substring(1);
                operationType.Type = TypeDefinitionType.Type;

                this.operationTypes.Add(operationType);

                this.MapSelections(item.Selection.SubSelection,OperationMap[item.OperationType], item.Name,operationType,null);
            
        }
    } 

    public void MapSelections(List<Selection> selections,string root, string rootName,TypeDefinitionItem fieldSource, string? name = null )
    {
        foreach (Selection selection in selections)
        {
            TypeDefinitionItem selectionRoot = items.First(e => e.Name == root);
            var selectionType = selectionRoot.Fields.First(e => e.Name == selection.Field);

            if(Utils.ScalarMap.Any(e => e.Key == selectionType.Type || e.Value == selectionType.Type)){
                fieldSource.Fields.Add(new FieldDefinition{
                    Type = selectionType.Type,
                    OriginalType = selectionType.OriginalType,
                    Name = selection.Alias ?? selection.Field
                });
                return;
            }

            var newName = rootName + selectionType.Type;
            newName = char.ToUpper(newName[0]) + newName.Substring(1);

            var operationType = new TypeDefinitionItem();
            operationType.Name = newName;
            operationType.Type = TypeDefinitionType.Type;

            this.operationTypes.Add(operationType);

            
            fieldSource.Fields.Add(new FieldDefinition{
                Type = newName,
                OriginalType = selectionType.OriginalType.Replace(selectionType.Type,newName),
                Name = selection.Alias ?? selection.Field
            });



            if(selection.SubSelection.Count > 0){
                this.MapSelections(selection.SubSelection,selectionType.Type,rootName ,operationType);
            }
        }
    }

}