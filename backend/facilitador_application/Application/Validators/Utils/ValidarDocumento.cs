namespace facilitador_application.Application.Validators.Utils
{
    internal class ValidarDocumento
    {
        // Valida CNPJ tradicional (somente dígitos)
        private static bool ValidarCNPJ(string cnpj)
        {
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());
            if (cnpj.Length != 14) return false;
            if (cnpj.All(c => c == cnpj[0])) return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * multiplicador1[i]).Sum();
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * multiplicador2[i]).Sum();
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith($"{digito1}{digito2}");
        }

        // Valida novo CNPJ alfanumérico (base 36, permite letras)
        private static bool ValidarCNPJAlfaNumerico(string cnpj)
        {
            // Remove caracteres especiais comuns, mas mantém letras e números
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14) return false;
            cnpj = cnpj.ToUpperInvariant();

            // Verifica se todos os caracteres são válidos (0-9, A-Z)
            if (!cnpj.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'Z')))
                return false;

            // Converte caractere para valor numérico base 36
            static int CharToValue(char c)
            {
                if (char.IsDigit(c)) return c - '0';
                return c - 'A' + 10;
            }

            // Converte valor numérico (0-35) para caractere
            static char ValueToChar(int valor)
            {
                if (valor >= 0 && valor <= 9) return (char)('0' + valor);
                return (char)('A' + valor - 10);
            }

            int[] pesos1 = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2, 7, 6 }; // 12 posições
            int[] pesos2 = { 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2, 7, 6 }; // 13 posições

            string baseCnpj = cnpj.Substring(0, 12);

            // Primeiro dígito verificador (13º caractere)
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += CharToValue(baseCnpj[i]) * pesos1[i];
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            if (digito1 != CharToValue(cnpj[12])) return false;

            // Segundo dígito verificador (14º caractere)
            string baseComPrimeiro = baseCnpj + ValueToChar(digito1);
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += CharToValue(baseComPrimeiro[i]) * pesos2[i];
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            return digito2 == CharToValue(cnpj[13]);
        }

        private static bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11) return false;
            if (cpf.All(c => c == cpf[0])) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicador1[i]).Sum();
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicador2[i]).Sum();
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{digito1}{digito2}");
        }

        public static bool ValidarCPFouCNPJ(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento)) return false;

            // Remove caracteres especiais comuns (pontos, traços, barras)
            string limpo = documento.Replace(".", "").Replace("-", "").Replace("/", "");

            // CPF: 11 dígitos numéricos
            if (limpo.Length == 11 && limpo.All(char.IsDigit))
                return ValidarCPF(limpo);

            // CNPJ antigo: 14 dígitos numéricos
            if (limpo.Length == 14 && limpo.All(char.IsDigit))
                return ValidarCNPJ(limpo);

            // Novo CNPJ alfanumérico: 14 caracteres com pelo menos uma letra
            if (limpo.Length == 14 && limpo.Any(char.IsLetter))
                return ValidarCNPJAlfaNumerico(limpo);

            return false;
        }
    }
}
