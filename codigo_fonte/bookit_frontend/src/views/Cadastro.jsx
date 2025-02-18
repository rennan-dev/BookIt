import React, { useState } from "react";
import "./Cadastro.css";
import { Link, useNavigate } from "react-router-dom";

const Cadastro = () => {
  const [siape, setSiape] = useState("");
  const [cpf, setCpf] = useState("");
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [password, setPassword] = useState("");
  const [rePassword, setRePassword] = useState("");
  const navigate = useNavigate();

  const handleCpfChange = (e) => {
    let value = e.target.value.replace(/\D/g, "");
    value = value.replace(/^(\d{3})(\d)/, "$1.$2");
    value = value.replace(/^(\d{3})\.(\d{3})(\d)/, "$1.$2.$3");
    value = value.replace(/\.(\d{3})(\d)/, ".$1-$2");
    setCpf(value);
  };

  const handlePhoneChange = (e) => {
    let value = e.target.value.replace(/\D/g, "");
    value = value.replace(/^(\d{2})(\d)/g, "($1) $2");
    value = value.replace(/(\d{5})(\d{1,4})$/, "$1-$2");
    setPhoneNumber(value);
  };

  const [senhaErro, setSenhaErro] = useState('');

    const handleSenhaChange = (event) => {
        setPassword(event.target.value);
        setSenhaErro(''); 

        if (!validatePassword(event.target.value)) {
            setSenhaErro('mínimo 8 caracteres, 1 maiúscula, 1 minúscula, 1 número e 1 caractere especial');
        }
    };

  const validatePassword = (password) => {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    return regex.test(password);
  };

  const handleCadastro = async (e) => {
    e.preventDefault();

    if (!validatePassword(password)) {
      alert("A senha deve ter pelo menos 8 caracteres, incluindo 1 letra maiúscula, 1 minúscula, 1 número e 1 caractere especial.");
      return;
    }

    if (password !== rePassword) {
      alert("As senhas não coincidem!");
      return;
    }

    try {
      const response = await fetch("http://localhost:5092/api/User/cadastro", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ siape, cpf, name: fullName, email, phoneNumber, password , rePassword}),
      });

      if (response.ok) {
        alert("Solicitação de cadastro aceita, aguarde um email do admin com a sua confirmação.");
        navigate("/login");
      } else {
        const errorText = await response.text();
        alert(`Erro no cadastro: ${errorText}`);
      }
    } catch (error) {
      console.error("Erro ao conectar com o servidor:", error);
      alert("Erro ao conectar com o servidor. Verifique sua conexão.");
    }
  };

  return (
    <div className="cadastro-container">
      <h1 className="cadastro-title">Sistema de Reservas</h1>
      <fieldset className="cadastro-fieldset">
        <div className="cadastro-content">
          <img src="/assets/images/icet_logo.png" alt="ICET Logo" className="cadastro-logo" />

          <div className="cadastro-box">
            <h2>Cadastro de Usuário</h2>
            <fieldset className="cadastro-form-fieldset">
              <form onSubmit={handleCadastro}>
                <div>
                  <label htmlFor="siape">SIAPE:</label>
                  <input
                    type="text"
                    id="siape"
                    value={siape}
                    onChange={(e) => setSiape(e.target.value)}
                    required
                    pattern="\d{6}"
                    title="O SIAPE deve ter exatamente 6 dígitos numéricos."
                  />
                </div>
                <div>
                  <label htmlFor="cpf">CPF:</label>
                  <input
                    type="text"
                    id="cpf"
                    value={cpf}
                    onChange={handleCpfChange}
                    required
                    maxLength="14"
                    title="O CPF será formatado automaticamente."
                  />
                </div>
                <div>
                  <label htmlFor="name">Nome:</label>
                  <input
                    type="text"
                    id="name"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    required
                    maxLength="100"
                  />
                </div>
                <div>
                  <label htmlFor="email">E-mail:</label>
                  <input
                    type="email"
                    id="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    maxLength="120"
                  />
                </div>
                <div>
                  <label htmlFor="phoneNumber">Celular:</label>
                  <input
                    type="text"
                    id="phoneNumber"
                    value={phoneNumber}
                    onChange={handlePhoneChange}
                    required
                    maxLength="15"
                    title="O celular será formatado automaticamente."
                  />
                </div>
                {senhaErro && <div className="cadastro-error senha-error-active">{senhaErro}</div>}
                <div>
                    <label htmlFor="password">Senha:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => handleSenhaChange(e)} 
                        required
                        minLength="8"
                        maxLength="120"
                    />
                </div>

                <div>
                  <label htmlFor="rePassword">Confirmar Senha:</label>
                  <input
                    type="password"
                    id="rePassword"
                    value={rePassword}
                    onChange={(e) => setRePassword(e.target.value)}
                    required
                  />
                </div>
                <button type="submit" className="cadastro-button">Cadastrar</button>
              </form>
            </fieldset>
            <div className="cadastro-links">
              <span>Já possui uma conta?<Link to="/login">Clique aqui.</Link></span>
            </div>
          </div>
        </div>
      </fieldset>
    </div>
  );
};

export default Cadastro;