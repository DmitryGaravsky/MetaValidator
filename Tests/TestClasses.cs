namespace MetaValidator.Tests {
    using MetaValidator.Constraints;
    using NUnit.Framework;

    [System.ComponentModel.Category("FOO")]
    public class Foo { }
    class Bar { }
    //
    public static class FooBar {
        public static Foo Foo {
            get;
            set;
        }
        [System.ComponentModel.Category("BAR")]
        static Bar Bar {
            get { return new Bar(); }
        }
    }
}