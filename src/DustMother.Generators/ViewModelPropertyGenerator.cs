//using DustMother.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnSave.Extensions;

namespace DustMother.Generators
{
    [Generator]
    public class ViewModelPropertyGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (AttributeReceiver<ViewModelPropertyAttribute, ClassDeclarationSyntax>)context.SyntaxReceiver;

            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}

            foreach (var classDeclarationSyntax in receiver.Candidates.Select(c => c.Value))
            {
                var model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree, true);
                var declaredSymbol = ModelExtensions.GetDeclaredSymbol(model, classDeclarationSyntax);

                var field = declaredSymbol as ITypeSymbol;

                if (field is null)
                    continue;

                var code = GenerateClassPropertyCode(field, model);
                if (code is not null)
                {
                    context.AddSource($"{field.Name}_Generated.cs", code);
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AttributeReceiver<ViewModelPropertyAttribute, ClassDeclarationSyntax>(c => c.GetFullName()));
        }

        private static string? GenerateClassPropertyCode(ITypeSymbol classSymbol, SemanticModel model)
        {
            var ns = classSymbol.ContainingNamespace.ToString();
            var className = classSymbol.Name;
            var builder = new ExperimentalClassBuilder(ns, className) { UseTypeName = false };
            var saveType = classSymbol.BaseType.TypeArguments.FirstOrDefault(ta => ta.BaseType != null && ta.BaseType.Name.Contains("WingmanSave"));
            var classProps = classSymbol.GetProperties(saveType.Name);
            var saveDataProp = classProps.FirstOrDefault();
            if (saveDataProp != null)
            {
                var attrs = classSymbol.GetAttributes();
                foreach (var attr in attrs)
                {
                    var savePropName = attr.ConstructorArguments.First(a => a.Type.Name == "String").Value as string;
                    //var savePropType = attr.ConstructorArguments.First(a => a.Kind == TypedConstantKind.Type).Value as ITypeSymbol;
                    //var baseType = savePropType.BaseType; //this is UnrealPropertyBase<something>
                    //builder.AddImport(baseType.ContainingNamespace);
                    //var targetPropertyType = baseType.TypeArguments.First(); //this is int or whatever
                    var savePropSymbol = saveType.GetProperties().FirstOrDefault(p => p.Name == savePropName);
                    var targetPropertyType = savePropSymbol.Type;
                    builder.AddImport(targetPropertyType.ContainingNamespace);
                    var viewPropertyName = attr.GetValue(nameof(ViewModelPropertyAttribute.SavePropertyName), savePropName);
                    var pendingProperty = attr.GetValue(nameof(ViewModelPropertyAttribute.PendingChanges), null);
                    var noDefault = attr.GetFlag(nameof(ViewModelPropertyAttribute.NoDefault));

                    //
                    


                    //

                    var defaultCondition = noDefault ? $"&& value != default({targetPropertyType})" : string.Empty;
                    var pendingSetter = string.IsNullOrWhiteSpace(pendingProperty) ? string.Empty : $"{pendingProperty} = true;";
                    var getter = $"get => {saveDataProp.Name}?.{savePropName} ?? null;";
                    var setter = $@"set {{
if ({saveDataProp.Name}?.{savePropName} != null && value != null {defaultCondition} && value != {saveDataProp.Name}.{savePropName}) {{
    {saveDataProp.Name}.{savePropName} = value;
    {pendingSetter}
    OnPropertyChanged();
}}
}}";
                    builder.AddProperty(targetPropertyType, viewPropertyName, getter, setter);
                }

                var gen = builder.Build();
                return gen;
            }

            return null;
        }
    }
}
