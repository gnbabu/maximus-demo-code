
namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IView<TModel>
    where TModel : class,new()
    {
        TModel Model { get; set; }
    }
}
