namespace MetaValidator.Diagnostics.Win {
    using System;
    using MetaValidator.Core;
    using MetaValidator.Diagnostics.FluentAPI;
    using MetaValidator.Constraints.FluentAPI;
    using MetaValidator.Constraints.Win.FluentAPI;
    using System.Reflection;

    public static class Configurator {
        public static void Configure(IConfiguration configuration) {
            configuration.For<Type>(x => x.IsComponent())
                .Register<ComponentShouldBeRegisteredAsToolboxItem>()
                .Register<ComponentShouldHaveDescription>();
            //
            configuration.For<PropertyInfo>()
                .Register(new MemberShouldHaveDescription())
                .If(x => x.IsReadOnly())
                    .Register(new MemberShouldNotBeBrowsable())
                .Else()
                    .Register(new MemberShouldBeCategorized())
                    .Register(new MemberShouldHaveDefaultValue());
        }
    }
}