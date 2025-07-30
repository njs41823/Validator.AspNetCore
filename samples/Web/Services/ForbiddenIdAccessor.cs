namespace Web.Services
{
    public interface IForbiddenIdAccessor
    {
        int GetForbiddenId();
    }

    public class ForbiddenIdAccessor : IForbiddenIdAccessor
    {
        public int GetForbiddenId()
        {
            return 69;
        }
    }
}
