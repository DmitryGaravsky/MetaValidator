namespace MetaValidator.Diagnostics {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using MetaValidator.Core;

    sealed class DefaultDiagnosticScopeSettingsResolver : IDiagnosticScopeSettingsResolver {
        internal readonly static IDiagnosticScopeSettingsResolver Instance = new DefaultDiagnosticScopeSettingsResolver();
        DefaultDiagnosticScopeSettingsResolver() { }
        //
        DiagnosticScopeSettings IDiagnosticScopeSettingsResolver.Resolve(Assembly assembly) {
            return GetScopeSettings(assembly.GetCustomAttributes(false).OfType<Attribute>());
        }
        DiagnosticScopeSettings IDiagnosticScopeSettingsResolver.Resolve(Type type) {
            return GetScopeSettings(type.GetCustomAttributes(false).OfType<Attribute>());
        }
        DiagnosticScopeSettings IDiagnosticScopeSettingsResolver.Resolve(MemberInfo member) {
            return GetScopeSettings(member.GetCustomAttributes(false).OfType<Attribute>());
        }
        #region GetScopeSettings
        readonly static object syncObj = new object();
        readonly static IDictionary<Type, Func<Attribute, DiagnosticScopeSettings>> getSettingsCache =
            new Dictionary<Type, Func<Attribute, DiagnosticScopeSettings>>();
        static DiagnosticScopeSettings GetScopeSettings(IEnumerable<Attribute> attributes) {
            var scopeAttribute = attributes
                .OfType<Diagnostics.DiagnosticScopeAttribute>()
                .FirstOrDefault();
            if(scopeAttribute != null)
                return scopeAttribute.Settings;
            var customScopeAttribute = attributes
                .Where(a => a.GetType().FullName == "MetaValidator.Diagnostics.DiagnosticScopeAttribute")
                .Select(a => new { Type = a.GetType(), Attribute = a })
                .FirstOrDefault();
            return customScopeAttribute
                .@Get(a => GetAcessor(a.Type, "Settings")(a.Attribute));
        }
        readonly static Func<Attribute, DiagnosticScopeSettings> DefaultAccessor =
            a => DiagnosticScopeSettings.Default;
        static Func<Attribute, DiagnosticScopeSettings> GetAcessor(Type customAttributeType, string propertyName) {
            lock(syncObj) {
                Func<Attribute, DiagnosticScopeSettings> getSettings;
                if(!getSettingsCache.TryGetValue(customAttributeType, out getSettings)) {
                    getSettings = CreateAccesssor(customAttributeType, propertyName);
                    getSettingsCache.Add(customAttributeType, getSettings);
                }
                return getSettings;
            }
        }
        static Func<Attribute, DiagnosticScopeSettings> CreateAccesssor(Type customAttributeType, string propertyName) {
            try {
                var settingsProperty = customAttributeType.GetProperty(propertyName);
                if(settingsProperty != null && settingsProperty.PropertyType.IsEnum)
                    return CreateAccessorCore(customAttributeType, settingsProperty);
                else
                    return DefaultAccessor;
            }
            catch { return DefaultAccessor; }
        }
        static Func<Attribute, DiagnosticScopeSettings> CreateAccessorCore(Type customAttributeType, PropertyInfo prop) {
            var a = Expression.Parameter(typeof(Attribute), "a");
            return Expression.Lambda<Func<Attribute, DiagnosticScopeSettings>>(
                    Expression.Convert(
                        Expression.Convert(
                            Expression.Property(
                                Expression.Convert(a, customAttributeType)
                            , prop)
                        , typeof(int))
                    , typeof(DiagnosticScopeSettings))
                , a).Compile();
        }
        #endregion
    }
}