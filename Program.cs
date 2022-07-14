using HotChocolate.Language;
using GraphqlFlutterGen;
using Stubble.Core.Builders;
using System.Text;
using System.Text.Json;

var schema = File.ReadAllText("./schema.gql");
var operations = File.ReadAllText("./operations.gql");

DocumentNode gqlDocument = Utf8GraphQLParser.Parse(schema);
DocumentNode oDocument = Utf8GraphQLParser.Parse(operations);

var walker = new DummySchemaSyntaxWalker();
walker.Visit(gqlDocument, new TypeDefinitionItem());


var enumTemplate = File.ReadAllText("./templates/enum.Mustache");
var classTemplate = File.ReadAllText("./templates/class.Mustache");
var classField = File.ReadAllText("./templates/classField.Mustache");
var classFieldType = File.ReadAllText("./templates/classFieldType.Mustache");
var operationClassTemplate = File.ReadAllText("./templates/operationClass.Mustache");

new SchemaRenderer(walker.VisitedItems, classTemplate, classField, enumTemplate, classFieldType).RenderSchema();

var sresult = JsonSerializer.Serialize(walker.VisitedItems);
File.WriteAllText("./sresult.json", sresult);

/// <summary>
/// lol
/// </summary>
/// <returns></returns>
var oWalker = new OperationWalker(walker.VisitedItems);
oWalker.Visit(oDocument, new GraphqlFlutterGen.QueryType());


var orenderer = new OperationRenderer(oWalker.Visited, walker.VisitedItems, operationClassTemplate);
orenderer.RenderOperations();

new SchemaRenderer(orenderer.operationTypes, classTemplate, classField, enumTemplate, classFieldType).RenderSchema();

var result = JsonSerializer.Serialize(oWalker.Visited);
File.WriteAllText("./result.json", result);