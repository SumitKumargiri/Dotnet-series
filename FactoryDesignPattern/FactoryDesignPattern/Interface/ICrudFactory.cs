namespace FactoryDesignPattern.Interface
{
    public interface ICrudFactory
    {
        ICrud CreateCrudService(string type);
    }
}
