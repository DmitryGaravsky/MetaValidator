namespace MetaValidator.Diagnostics.Win.Tests {
    using System;
    using System.Linq;
    using System.Reflection;
    using MetaValidator.Tests;
    using NUnit.Framework;

    #region TestClasses
    [System.ComponentModel.ToolboxItem(true), System.ComponentModel.Description("Foo Component")]
    public class FooComponent : System.ComponentModel.Component {
        [System.ComponentModel.Category("Data"), System.ComponentModel.DefaultValue(null)]
        [System.ComponentModel.Description("FooComponent.Value")]
        public int? Value { get; set; }
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.Description("FooComponent.HasValue")]
        public bool HasValue {
            get { return Value.HasValue; }
        }
    }
    public class BarComponent : System.ComponentModel.Component {
        public int? Value { get; set; }
        public bool HasValue {
            get { return Value.HasValue; }
        }
    }
    [DiagnosticScope(DiagnosticScopeSettings.Ignore)]
    public class FooBarComponent : System.ComponentModel.Component { }
    #endregion

    [TestFixture]
    public class WinComponentTests : ConfigurationDependentTest {
        public override void SetUp() {
            base.SetUp();
            Configurator.Configure(cfg);
        }
        [Test]
        public void Test_00_CorrectComponent() {
            var componentType = typeof(FooComponent);
            var scope = GetScope(componentType);
            ValidateComponentTypeAndToolboxItem(componentType, scope, true);
            ValidateComponentPropertiesAndAttributes(componentType, scope, true);
        }
        [Test]
        public void Test_01_IncorrectComponent() {
            var componentType = typeof(BarComponent);
            var scope = GetScope(componentType);
            ValidateComponentTypeAndToolboxItem(componentType, scope, false);
            ValidateComponentPropertiesAndAttributes(componentType, scope, false);
        }
        [Test]
        public void Test_02_IgnoreComponent() {
            var scope = GetScope(typeof(FooBarComponent));
            Assert.IsFalse(scope.Any());
        }
        IDiagnosticScope GetScope(Type componentType) {
            return cfg.Resolve<IDiagnosticScopeFactory>()
                    .Create(componentType);
        }
        void ValidateComponentTypeAndToolboxItem(Type componentType, IDiagnosticScope scope, bool expected) {
            var typeCtx = scope.ElementAt(0);
            Assert.AreEqual(componentType.Assembly, typeCtx.Assembly);
            Assert.AreEqual(componentType, typeCtx.Type);
            Assert.IsNull(typeCtx.Member);

            var typeDiagnostics = cfg.Resolve(typeCtx);
            Assert.AreEqual(2, typeDiagnostics.Count());
            Assert.IsTrue(typeDiagnostics.ElementAt(0) is ComponentShouldBeRegisteredAsToolboxItem);
            Assert.IsTrue(typeDiagnostics.ElementAt(1) is ComponentShouldHaveDescription);
            Assert.AreEqual(expected, typeDiagnostics.ElementAt(0).Validate(typeCtx));
            Assert.AreEqual(expected, typeDiagnostics.ElementAt(1).Validate(typeCtx));
        }
        void ValidateComponentPropertiesAndAttributes(Type componentType, IDiagnosticScope scope, bool expected) {
            var props = scope.Where(x => x.Member is PropertyInfo);
            Assert.AreEqual(4, props.Count());

            var propValueCtx = props.ElementAt(0);
            Assert.AreEqual(componentType.Assembly, propValueCtx.Assembly);
            Assert.AreEqual(componentType, propValueCtx.Type);
            Assert.AreEqual(componentType.GetProperty("Value"), propValueCtx.Member);
            var propHasValueCtx = props.ElementAt(1);
            Assert.AreEqual(componentType.Assembly, propHasValueCtx.Assembly);
            Assert.AreEqual(componentType, propHasValueCtx.Type);
            Assert.AreEqual(componentType.GetProperty("HasValue"), propHasValueCtx.Member);

            var propValueDiagnostics = cfg.Resolve(propValueCtx);
            Assert.AreEqual(5, propValueDiagnostics.Count());
            Assert.IsTrue(propValueDiagnostics.ElementAt(0) is ComponentShouldBeRegisteredAsToolboxItem);
            Assert.IsTrue(propValueDiagnostics.ElementAt(1) is ComponentShouldHaveDescription);
            Assert.IsTrue(propValueDiagnostics.ElementAt(2) is MemberShouldHaveDescription);
            Assert.IsTrue(propValueDiagnostics.ElementAt(3) is MemberShouldBeCategorized);
            Assert.IsTrue(propValueDiagnostics.ElementAt(4) is MemberShouldHaveDefaultValue);
            Assert.AreEqual(expected, propValueDiagnostics.ElementAt(2).Validate(propValueCtx));
            Assert.AreEqual(expected, propValueDiagnostics.ElementAt(3).Validate(propValueCtx));
            Assert.AreEqual(expected, propValueDiagnostics.ElementAt(4).Validate(propValueCtx));
            var propHasValueDiagnostics = cfg.Resolve(propHasValueCtx);
            Assert.AreEqual(4, propHasValueDiagnostics.Count());
            Assert.IsTrue(propHasValueDiagnostics.ElementAt(0) is ComponentShouldBeRegisteredAsToolboxItem);
            Assert.IsTrue(propHasValueDiagnostics.ElementAt(1) is ComponentShouldHaveDescription);
            Assert.IsTrue(propHasValueDiagnostics.ElementAt(2) is MemberShouldHaveDescription);
            Assert.IsTrue(propHasValueDiagnostics.ElementAt(3) is MemberShouldNotBeBrowsable);
            Assert.AreEqual(expected, propHasValueDiagnostics.ElementAt(2).Validate(propHasValueCtx));
            Assert.AreEqual(expected, propHasValueDiagnostics.ElementAt(3).Validate(propHasValueCtx));
        }
    }
}