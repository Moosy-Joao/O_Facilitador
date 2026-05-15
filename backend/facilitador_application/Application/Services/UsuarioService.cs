using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;

namespace facilitador_api.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public UsuarioService(IUsuarioRepository ususarioRepository, IEmpresaRepository empresaRepository)
        {
            _usuarioRepository = ususarioRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<bool> Ativar(Guid id)
        {
            var usuario = await _usuarioRepository.Existe(id);
            if (!usuario)
            {
                return false;
            }

            await _usuarioRepository.Ativar(id);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<bool> Atualizar(Guid id, UsuarioUpdateDTO dto)
        {
            // 1. Buscar o usuário existente
            var usuario = await _usuarioRepository.BuscarPorId(id);
            if (usuario == null)
            {
                return false;
            }

            // 2. Atualizar campos simples
            if (!string.IsNullOrEmpty(dto.Nome))
            {
                usuario.AtualizarNome(dto.Nome);
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                usuario.AtualizarEmail(dto.Email);
            }

            if (!string.IsNullOrEmpty(dto.Senha))
            {
                usuario.AtualizarSenha(dto.Senha);
            }

            if (!string.IsNullOrEmpty(dto.Cargo))
            {
                usuario.AtualizarCargo(dto.Cargo);
            }

            if (!string.IsNullOrEmpty(dto.Imagem))
            {
                usuario.AtualizarImagem(dto.Imagem);
            }

            // 5. Atualizar timestamp de modificação
            //usuario.AtualizarModificadoEm(DateTime.UtcNow);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<UsuarioResponseDTO?> BuscarPorId(Guid id)
        {
            var usuario = await _usuarioRepository.BuscarPorId(id);
            return usuario?.ToResponseDTO();
        }

        public async Task<List<UsuarioResponseDTO>> BuscarUsuarios()
        {
            var usuarios = await _usuarioRepository.BuscarTodos();
            return usuarios.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(UsuarioCreateDTO dto)
        {
            var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId);
            if (!empresaExiste)
            {
                return false;
            }

            var usuarioNovo = new Usuario(dto, dto.EmpresaId);

            await _usuarioRepository.Cadastrar(usuarioNovo);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var cliente = await _usuarioRepository.Existe(id);
            if (!cliente)
            {
                return false;
            }

            await _usuarioRepository.Desativar(id);
            await _usuarioRepository.Salvar();

            return true;
        }
    }
}
