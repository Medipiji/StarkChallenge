using StarkChallenge.Utils;

namespace StarkChallenge.Models
{
    public class Client
    {
        private string nome, cpf;
        private long valor;

        public string Nome{  
            get => nome;
            set{
                if (string.IsNullOrEmpty(value)) return;
                if (value.ValidateSpecialChar()) return;
                nome = value;
            }
        }

        public string Cpf
        {
            get => cpf;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                cpf = value;
            }
        }

        public long Valor
        {
            get => valor;
            set
            {
                if (value.ValidateLongNegativeNumber()) return;
                valor = value;
            }
        }
    }
}
