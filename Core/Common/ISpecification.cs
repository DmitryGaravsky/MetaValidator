namespace MetaValidator.Core {
    public interface ISpecification<T> {
        bool Match(T metadata);
    }
}