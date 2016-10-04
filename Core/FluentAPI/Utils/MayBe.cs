namespace MetaValidator.Core {
    using System;
    using System.Diagnostics;

    static class @MayBe {
        [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
        internal static TResult @Get<T, TResult>(this T @this, Func<T, TResult> @get, TResult defaultValue = default(TResult)) {
            return (@this != null && @get != null) ? @get(@this) : defaultValue;
        }
        [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
        internal static void @Do<T>(this T @this, Action<T> @do) {
            if(@this != null && @do != null) @do(@this);
        }
    }
}