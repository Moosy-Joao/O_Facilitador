namespace facilitador_application.Application.Validators.Utils
{
    using System.Text.RegularExpressions;

    namespace facilitador_application.Application.Validators.Utils
    {
        public class ValidarEmail
        {
            private static readonly Regex EmailRegex = new Regex(
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Valida um endereço de e-mail de forma mais abrangente.
            public static bool Validar(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                // Remove espaços extras
                email = email.Trim();

                // Validação básica de comprimento (evita abusos)
                if (email.Length < 5 || email.Length > 254)
                    return false;

                // Verifica se há apenas um '@'
                int arrobaCount = email.Count(c => c == '@');
                if (arrobaCount != 1)
                    return false;

                // Divide local e domínio
                string[] partes = email.Split('@');
                string local = partes[0];
                string dominio = partes[1];

                // Valida parte local (antes do @)
                if (string.IsNullOrEmpty(local) || local.Length > 64)
                    return false;

                // Valida parte do domínio
                if (string.IsNullOrEmpty(dominio) || dominio.Length > 255)
                    return false;

                // Verifica se o domínio possui pelo menos um ponto e não começa/termina com ponto ou hífen
                if (!dominio.Contains('.') || dominio.StartsWith('.') || dominio.EndsWith('.') ||
                    dominio.StartsWith('-') || dominio.EndsWith('-'))
                    return false;

                // Verifica se há caracteres inválidos no domínio (apenas letras, números, pontos e hífens)
                if (!dominio.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '-'))
                    return false;

                // Verifica se a extensão do domínio (TLD) tem pelo menos 2 caracteres e apenas letras
                string[] domainParts = dominio.Split('.');
                string tld = domainParts.Last();
                if (tld.Length < 2 || !tld.All(char.IsLetter))
                    return false;

                // Aplica a regex como validação final de formato (opcional, mas mantém compatibilidade)
                return EmailRegex.IsMatch(email);
            }
        }
    }
}
