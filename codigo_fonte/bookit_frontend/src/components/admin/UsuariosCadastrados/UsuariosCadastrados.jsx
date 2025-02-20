import React, { useState, useEffect } from 'react';
import './UsuariosCadastrados.css';

const UsuariosCadastrados = () => {
  const [servidores, setServidores] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [cpfParaExcluir, setCpfParaExcluir] = useState(null);
  const [usuarioVisualizado, setUsuarioVisualizado] = useState(null);

  useEffect(() => {
    const fetchServidores = async () => {
      try {
        const token = localStorage.getItem("token");
        const headers = {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        };

        const response = await fetch("http://localhost:5092/api/User/servidores", {
          method: "GET",
          headers: headers,
        });

        if (!response.ok) {
          throw new Error(`Erro ${response.status}: ${await response.text()}`);
        }

        const data = await response.json();
        setServidores(data);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchServidores();
  }, []);

  const visualizarUsuario = async (cpf) => {
    try {
      const token = localStorage.getItem("token");
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      };

      const response = await fetch(`http://localhost:5092/api/User/servidores/${cpf}`, {
        method: "GET",
        headers: headers,
      });

      if (!response.ok) {
        throw new Error(`Erro ${response.status}: ${await response.text()}`);
      }

      const usuario = await response.json();
      setUsuarioVisualizado(usuario);
    } catch (error) {
      alert(`Erro ao visualizar usuário: ${error.message}`);
    }
  };

  const confirmarExclusao = (cpf) => {
    setCpfParaExcluir(cpf);
  };

  const cancelarExclusao = () => {
    setCpfParaExcluir(null);
  };

  const fecharModal = () => {
    setUsuarioVisualizado(null);
  };

  if (loading) return <div>Carregando...</div>;
  if (error) return <div>Erro: {error}</div>;

  return (
      <div className="servidores-container">
      <h1>Servidores Cadastrados</h1>
      <fieldset>
        {servidores.length > 0 ? (
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>CPF</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {servidores.map((usuario) => (
                <tr key={usuario.id}>
                  <td>{usuario.nome}</td>
                  <td>{usuario.cpf}</td>
                  <td>
                    <button className="visualizar" onClick={() => visualizarUsuario(usuario.cpf)}>
                      Visualizar
                    </button>
                    <button className="excluir" onClick={() => confirmarExclusao(usuario.cpf)}>
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p>Não há servidores registrados.</p>
        )}
      </fieldset>

      {cpfParaExcluir && (
        <div className="modal">
          <div className="modal-content">
            <h2>Confirmar Exclusão</h2>
            <p>Tem certeza que deseja excluir o usuário com o CPF: {cpfParaExcluir}?</p>
            <button className="confirmar">Confirmar</button>
            <button className="cancelar" onClick={cancelarExclusao}>
              Cancelar
            </button>
          </div>
        </div>
      )}

      {usuarioVisualizado && (
        <div className="modal">
          <div className="modal-content">
            <h2>Informações do Usuário</h2>
            <p>Nome: {usuarioVisualizado.name}</p>
            <p>Siape: {usuarioVisualizado.siape}</p>
            <p>CPF: {usuarioVisualizado.cpf}</p>
            <p>Email: {usuarioVisualizado.email}</p>
            <p>Celular: {usuarioVisualizado.phoneNumber}</p>
            <button className="fechar" onClick={fecharModal}>Fechar</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default UsuariosCadastrados;
