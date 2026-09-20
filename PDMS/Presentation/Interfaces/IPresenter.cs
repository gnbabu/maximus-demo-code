namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPresenter<TView, TModel>
        where TView : IView<TModel>
        where TModel : class, new()
    {
        void Init();
    }
}
