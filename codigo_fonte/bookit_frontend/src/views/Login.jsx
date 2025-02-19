import React, { useState } from "react";
import "./Login.css";
import { Link, useNavigate } from "react-router-dom";

const Login = () => {
  const [cpf, setCpf] = useState("");
  const [senha, setSenha] = useState("");
  const navigate = useNavigate();

  const handleCpfChange = (e) => {
    let value = e.target.value.replace(/\D/g, "");
    value = value.replace(/^(\d{3})(\d)/, "$1.$2");
    value = value.replace(/^(\d{3})\.(\d{3})(\d)/, "$1.$2.$3");
    value = value.replace(/\.(\d{3})(\d)/, ".$1-$2");
    setCpf(value);
  };

  const decodeBase64Url = (input) => {
    let base64 = input.replace(/-/g, "+").replace(/_/g, "/");
    while (base64.length % 4 !== 0) {
      base64 += "=";
    }
    return window.atob(base64);
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    console.log("Tentando login com CPF:", cpf, "Senha:", senha);
  
    try {
      const response = await fetch("http://localhost:5092/api/User/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ cpf, password: senha }),
      });
  
      console.log("Status da resposta:", response.status);
      const responseText = await response.text();
      console.log("Resposta bruta da API:", responseText);
  
      if (response.ok) {
        localStorage.setItem("token", responseText);
  
        try {
          const payloadBase64 = responseText.split(".")[1];
          const payloadDecoded = JSON.parse(decodeBase64Url(payloadBase64));
  
          console.log("Payload decodificado do token:", payloadDecoded);
          const isAdmin = payloadDecoded.IsAdmin === "True";
          const isAprovado = payloadDecoded.IsAprovado === "True";
  
          console.log("Usuário é admin?", isAdmin);
          console.log("Usuário está aprovado?", isAprovado);
  
          if (!isAprovado) {
            alert("Você precisa aguardar a confirmação do administrador.");
          } else {
            window.location.href = isAdmin ? "/admin" : "/servidor";
          }
        } catch (decodeError) {
          console.error("Erro ao decodificar token:", decodeError);
          alert("Erro ao processar autenticação. Tente novamente.");
        }
      } else {
        console.error("Erro na autenticação:", response.status, responseText);
        alert("CPF ou senha inválidos!");
      }
    } catch (error) {
      console.error("Erro ao fazer login:", error);
      alert("Erro ao conectar com o servidor. Verifique sua conexão.");
    }
  };

  return (
    <div className="login-container">
      <h1 className="login-title">Sistema de Reservas</h1>
      <fieldset className="login-fieldset">
        <div className="login-content">
          <img src="/assets/images/icet_logo.png" alt="ICET Logo" className="login-logo" />

          <div className="login-box">
            <h2>Acesso ao Sistema BookIt</h2>
            <fieldset className="login-form-fieldset">
              <form onSubmit={handleLogin}>
                <div>
                  <label htmlFor="cpf">CPF:</label>
                  <input
                    type="text"
                    id="cpf"
                    name="cpf"
                    value={cpf}
                    onChange={handleCpfChange}
                    maxLength={14}
                  />
                </div>
                <div>
                  <label htmlFor="senha">Senha:</label>
                  <input
                    type="password"
                    id="senha"
                    name="senha"
                    value={senha}
                    onChange={(e) => setSenha(e.target.value)}
                  />
                </div>
                <button type="submit" className="login-button">Entrar</button>
              </form>
            </fieldset>
            <div className="login-links">
              <Link to="/esqueci-senha">Esqueci minha senha</Link>
              <Link to="/cadastro"> Meu primeiro acesso</Link>
            </div>
          </div>
        </div>
      </fieldset>
    </div>
  );
};

export default Login;