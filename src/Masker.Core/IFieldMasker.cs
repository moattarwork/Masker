namespace Masker.Core
{
    public interface IFieldMasker<T>
    {
        T Mask(T value);
    }
}