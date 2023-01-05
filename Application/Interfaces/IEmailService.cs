using Application.Core;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        public void Send(Email email);
    }
}