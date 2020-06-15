using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class UIComponentMetadataTests
    {
        private UIComponentMetadata metadata;

        public List<Attribute> Declared => metadata.DeclaredAttributesList;

        public List<Attribute> ParentComponent => metadata.ParentComponentAttributesList;

        public List<Attribute> Assembly => metadata.AssemblyAttributesList;

        public List<Attribute> Global => metadata.GlobalAttributesList;

        public List<Attribute> Component => metadata.ComponentAttributesList;

        [SetUp]
        public void SetUp()
        {
            metadata = new UIComponentMetadata("Some Item", typeof(TextInput<OrdinaryPage>), typeof(OrdinaryPage))
            {
                DeclaredAttributesList = new List<Attribute>(),
                ParentComponentAttributesList = new List<Attribute>(),
                AssemblyAttributesList = new List<Attribute>(),
                GlobalAttributesList = new List<Attribute>(),
                ComponentAttributesList = new List<Attribute>()
            };
        }

        [Test]
        public void UIComponentMetadata_Get_ForAttribute_AtAssemblyLevel()
        {
            Assembly.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Global.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Component.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });

            metadata.Get<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameAs(Assembly.Single());
        }

        [Test]
        public void UIComponentMetadata_Get_ForAttribute_AtDeclaredLevel()
        {
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            ParentComponent.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Assembly.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Global.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Component.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });

            metadata.Get<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameAs(Declared.Single());
        }

        [Test]
        public void UIComponentMetadata_Get_ForAttribute_None()
        {
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByNameAttribute) });
            ParentComponent.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByContentAttribute) });
            Assembly.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByLabelAttribute) });
            Global.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByClassAttribute) });
            Component.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByCssAttribute) });

            metadata.Get<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeNull();
        }

        [Test]
        public void UIComponentMetadata_GetAll_ForAttribute_AllLevelsHaveTargetAttributeType()
        {
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            ParentComponent.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Assembly.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Global.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });
            Component.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByIdAttribute) });

            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameSequenceAs(All(Declared, Assembly, Global, Component));
        }

        [Test]
        public void UIComponentMetadata_GetAll_ForAttribute_AllLevelsWithoutTarget()
        {
            Declared.Add(new FindSettingsAttribute { });
            ParentComponent.Add(new FindSettingsAttribute { });
            Assembly.Add(new FindSettingsAttribute { });
            Global.Add(new FindSettingsAttribute { });
            Component.Add(new FindSettingsAttribute { });

            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameSequenceAs(All(Declared, Assembly, Global, Component));
        }

        [Test]
        public void UIComponentMetadata_GetAll_ForAttribute_AllLevelsHaveTargetAnyType()
        {
            Declared.Add(new FindSettingsAttribute { TargetAnyType = true });
            ParentComponent.Add(new FindSettingsAttribute { TargetAnyType = true });
            Assembly.Add(new FindSettingsAttribute { TargetAnyType = true });
            Global.Add(new FindSettingsAttribute { TargetAnyType = true });
            Component.Add(new FindSettingsAttribute { TargetAnyType = true });

            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameSequenceAs(All(ParentComponent, Assembly, Global));
        }

        [Test]
        public void UIComponentMetadata_GetAll_ForAttribute_AllLevelsHaveAppropriateTarget()
        {
            Declared.Add(new FindSettingsAttribute { });
            ParentComponent.Add(new FindSettingsAttribute { TargetAnyType = true });
            Assembly.Add(new FindSettingsAttribute { });
            Global.Add(new FindSettingsAttribute { });
            Component.Add(new FindSettingsAttribute { });

            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameSequenceAs(All(Declared, ParentComponent, Assembly, Global, Component));
        }

        [Test]
        public void UIComponentMetadata_GetAll_ForAttribute_MoreSpecificTargetAttributeType()
        {
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(TermFindAttribute) });
            Declared.Add(new FindSettingsAttribute { TargetAttributeTypes = new[] { typeof(FindByXPathAttribute), typeof(FindByIdAttribute) } });
            Declared.Add(new FindSettingsAttribute { });
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindByLabelAttribute) });
            Declared.Add(new FindSettingsAttribute { TargetAttributeType = typeof(FindAttribute) });

            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute<FindByIdAttribute>()).
                Should().BeSameSequenceAs(Declared[1], Declared[0], Declared[4], Declared[2]);
        }

        [Test]
        public void UIComponentMetadata_Push_Single()
        {
            Assembly.Add(new FindSettingsAttribute { });

            var pushed = new FindSettingsAttribute { };

            metadata.Push(pushed);

            metadata.DeclaredAttributes.
                Should().BeSameSequenceAs(pushed);

            metadata.GetAll<FindSettingsAttribute>().
                Should().BeSameSequenceAs(pushed, Assembly[0]);

            metadata.Get<FindSettingsAttribute>().
                Should().BeSameAs(pushed);
        }

        [Test]
        public void UIComponentMetadata_Push_Multiple()
        {
            Assembly.Add(new FindSettingsAttribute { });

            var pushed = new List<FindSettingsAttribute> { new FindSettingsAttribute { }, new FindSettingsAttribute { } };

            metadata.Push(pushed);

            metadata.DeclaredAttributes.
                Should().BeSameSequenceAs(pushed);

            metadata.GetAll<FindSettingsAttribute>().
                Should().BeSameSequenceAs(pushed[0], pushed[1], Assembly[0]);

            metadata.Get<FindSettingsAttribute>().
                Should().BeSameAs(pushed[0]);
        }

        [Test]
        public void UIComponentMetadata_ComponentDefinitionAttribute_ForControl()
        {
            var defaultComponentDefinition = metadata.ComponentDefinitionAttribute;
            defaultComponentDefinition.ScopeXPath.Should().Be(ScopeDefinitionAttribute.DefaultScopeXPath);
            defaultComponentDefinition.ComponentTypeName.Should().Be("control");

            var componentDefinition = new ControlDefinitionAttribute("component");
            Component.Add(componentDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(componentDefinition);

            var globalDefinition = new ControlDefinitionAttribute("global");
            Global.Add(globalDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(globalDefinition);

            var assemblyDefinition = new ControlDefinitionAttribute("assembly");
            Assembly.Add(assemblyDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(assemblyDefinition);

            var parentComponentDefinition = new ControlDefinitionAttribute("parent-component");
            ParentComponent.Add(parentComponentDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(assemblyDefinition);

            var targetedParentComponentDefinition = new ControlDefinitionAttribute("parent-component-targeted") { TargetAnyType = true };
            ParentComponent.Add(targetedParentComponentDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(targetedParentComponentDefinition);

            var declaredDefinition = new ControlDefinitionAttribute("declared");
            Declared.Add(declaredDefinition);

            metadata.ComponentDefinitionAttribute.Should().BeSameAs(declaredDefinition);
        }

        private static IEnumerable<Attribute> All(params IEnumerable<Attribute>[] attributeCollections)
        {
            if (attributeCollections == null || attributeCollections.Length == 0)
                return Enumerable.Empty<Attribute>();

            return attributeCollections.SelectMany(x => x).ToArray();
        }
    }
}
