namespace blqw.Kanai
{
    public interface IConvertorFactory
    {
        IConvertor<T> Build<T>();

        bool CanBuild<T>();
    }
}
