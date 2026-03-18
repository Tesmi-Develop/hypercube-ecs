using System.Collections.Frozen;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Hypercube.Ecs.Analyzers.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class EntityPackGenerator : IIncrementalGenerator
{
    private const string AttributeFullName = "Hypercube.Ecs.Attributes.GenerateEntityPackAttribute";

    private static readonly FrozenSet<string> RequiredUsings = new HashSet<string>
    {
        "System",
        "Hypercube.Ecs.Components",
        "JetBrains.Annotations"
    }.ToFrozenSet();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var attributeSymbol = context.CompilationProvider
            .Select(static (compilation, _) =>
                compilation.GetTypeByMetadataName(AttributeFullName));

        var targetTypes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) =>
                    node is Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax s &&
                    s.AttributeLists.Count > 0,
                transform: static (ctx, _) =>
                {
                    var syntax = (Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax)ctx.Node;
                    return ctx.SemanticModel.GetDeclaredSymbol(syntax) as INamedTypeSymbol;
                })
            .Where(static s => s is not null);

        var pipeline = targetTypes.Combine(attributeSymbol);

        context.RegisterSourceOutput(pipeline, static (ctx, source) =>
        {
            var (symbol, attributeSymbol) = source;

            if (symbol is null || attributeSymbol is null)
                return;

            var attrData = symbol.GetAttributes()
                .FirstOrDefault(a =>
                    SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeSymbol));

            if (attrData is null)
                return;

            var maxComponents = 0;

            if (attrData.ConstructorArguments.Length > 0 &&
                attrData.ConstructorArguments[0].Value is int value)
            {
                maxComponents = value;
            }

            if (maxComponents <= 0)
                return;

            var namespaceName = symbol.ContainingNamespace.IsGlobalNamespace
                ? null
                : symbol.ContainingNamespace.ToDisplayString();

            var typeName = symbol.Name;

            var sourceText = GeneratePackedEntities(namespaceName, typeName, maxComponents);

            var hintName = $"{symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}.Pack.g.cs"
                .Replace("global::", "")
                .Replace('<', '_')
                .Replace('>', '_');

            ctx.AddSource(hintName, SourceText.From(sourceText, Encoding.UTF8));
        });
    }

    private static string GeneratePackedEntities(string? namespaceName, string typeName, int maxComponents)
    {
        var sb = new StringBuilder();

        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        foreach (var ns in RequiredUsings)
            sb.AppendLine($"using {ns};");

        sb.AppendLine();

        if (namespaceName != null)
        {
            sb.AppendLine($"namespace {namespaceName};");
            sb.AppendLine();
        }

        for (var count = 1; count <= maxComponents; count++)
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, count).Select(i => $"T{i}"));

            var componentFields = string.Join("\n",
                Enumerable.Range(1, count)
                    .Select(i => $"    public T{i} Component{i};"));

            var ctorParams = string.Join(", ",
                Enumerable.Range(1, count)
                    .Select(i => $"T{i} component{i}"));

            var assignments = string.Join("\n",
                Enumerable.Range(1, count)
                    .Select(i => $"        Component{i} = component{i};"));

            var implicitConversions = new StringBuilder();

            implicitConversions.AppendLine($"    public static implicit operator Entity({typeName}<{typeParams}> arg) => arg.Id;");

            for (var i = 1; i <= count; i++)
            {
                implicitConversions.AppendLine(
                    $"    public static implicit operator T{i}({typeName}<{typeParams}> arg) => arg.Component{i};");
            }

            var constraints = string.Join("\n",
                Enumerable.Range(1, count)
                    .Select(i => $"    where T{i} : struct, IComponent"));

            sb.AppendLine("[PublicAPI]");
            sb.AppendLine($"public struct {typeName}<{typeParams}>");
            sb.AppendLine(constraints);
            sb.AppendLine("{");

            sb.AppendLine("    public Entity Id;");
            sb.AppendLine(componentFields);
            sb.AppendLine();

            sb.AppendLine($"    public {typeName}(Entity id, {ctorParams})");
            sb.AppendLine("    {");
            sb.AppendLine("        Id = id;");
            sb.AppendLine(assignments);
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.Append(implicitConversions);

            sb.AppendLine("}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
