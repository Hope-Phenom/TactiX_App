namespace TactiX_Exception
{
    public interface ITactiXExceptionFactory
    {
        public TactiXException Create();
        public TactiXException Create(TactiXErrorCodes errorCode, string errorDesc);
    }
}
