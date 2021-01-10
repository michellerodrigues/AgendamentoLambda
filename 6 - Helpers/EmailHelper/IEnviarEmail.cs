using System.Threading.Tasks;

namespace EmailHelper
{
    public interface IEmailService
    {
        Task<bool> Enviar(string remetente, string assunto, string mensagem);
    }
}