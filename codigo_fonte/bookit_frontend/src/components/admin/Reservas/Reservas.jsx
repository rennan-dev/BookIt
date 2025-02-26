import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Reservas.css';  

const Reservas = () => {
  const [month, setMonth] = useState(new Date().getMonth());
  const [year, setYear] = useState(new Date().getFullYear());
  const [selectedAmbiente, setSelectedAmbiente] = useState("Auditório");
  const [reservas, setReservas] = useState({});
  const navigate = useNavigate();

  const months = [
    'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
  ];

  const years = [2023, 2024, 2025];
  const ambientes = ['Auditório', 'Mini Auditório', 'Sala de Reunião', 'Ford Ranger', 'Mitsubishi L200'];
  const daysOfWeek = ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'];

  const getDaysInMonth = (month, year) => new Date(year, month + 1, 0).getDate();
  const getFirstDayOfMonth = (month, year) => new Date(year, month, 1).getDay();

  const fetchReservas = async () => {
    const reservasPorDia = {};
    const daysInMonth = getDaysInMonth(month, year);

    for (let day = 1; day <= daysInMonth; day++) {
      const formattedDay = day.toString().padStart(2, '0'); 
      const formattedMonth = (month + 1).toString().padStart(2, '0'); 
      const formattedDate = `${year}-${formattedMonth}-${formattedDay}`;

      try {
        const response = await fetch(`http://localhost:5092/api/User/reservas/${formattedDate}/${selectedAmbiente}`);

        if (response.ok) {
          const dataReservas = await response.json();
          const qtdHorarios = dataReservas.reduce((total, reserva) => total + reserva.horarios.split(",").length, 0);
          reservasPorDia[day] = qtdHorarios;
        }
      } catch (error) {
        console.error(`Erro ao buscar reservas para ${formattedDate}:`, error);
      }
    }

    setReservas(reservasPorDia);
  };

  useEffect(() => {
    fetchReservas();
  }, [month, year, selectedAmbiente]);

  const handleDayClick = (day) => {
    if (day) {
      const formattedMonth = (month + 1).toString().padStart(2, '0');
      const formattedDay = day.toString().padStart(2, '0');
      const selectedDate = `${year}-${formattedMonth}-${formattedDay}`;
      navigate(`/reservas-cadastradas/${selectedDate}/${selectedAmbiente}`);
    }
  };

  const generateCalendar = () => {
    const daysInMonth = getDaysInMonth(month, year);
    const firstDay = getFirstDayOfMonth(month, year);
    const calendar = [];

    let day = 1;
    for (let i = 0; i < 6; i++) {
      const week = [];
      for (let j = 0; j < 7; j++) {
        if (i === 0 && j < firstDay) {
          week.push('');
        } else if (day > daysInMonth) {
          week.push('');
        } else {
          week.push(day);
          day++;
        }
      }
      calendar.push(week);
      if (day > daysInMonth) break;
    }

    return calendar;
  };

  const calendar = generateCalendar();

  return (
    <div className="container">
      <div className="calendar-container">
        <h2 className="reservas-title">Reservas de Ambientes</h2>
        <hr />
        <div className="select-container">
          <label className="ambiente-label">
            Ambiente:
            <select value={selectedAmbiente} onChange={(e) => setSelectedAmbiente(e.target.value)}>
              {ambientes.map((ambiente, index) => (
                <option key={index} value={ambiente}>
                  {ambiente}
                </option>
              ))}
            </select>
          </label>
          <label style={{ marginLeft: '10px' }}>
            Mês:
            <select value={month} onChange={(e) => setMonth(parseInt(e.target.value))}>
              {months.map((monthName, index) => (
                <option key={index} value={index}>
                  {monthName}
                </option>
              ))}
            </select>
          </label>
          <label style={{ marginLeft: '10px' }}>
            Ano:
            <select value={year} onChange={(e) => setYear(parseInt(e.target.value))}>
              {years.map((yearOption, index) => (
                <option key={index} value={yearOption}>
                  {yearOption}
                </option>
              ))}
            </select>
          </label>
          <hr />
        </div>
        <div className="select-row">
          <div>Selecione a data correspondente à(s) reserva(s):</div>
        </div>
        <div className="table-container">
          <table>
            <thead>
              <tr>
                {daysOfWeek.map((day, index) => (
                  <th key={index}>{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {calendar.map((week, weekIndex) => (
                <tr key={weekIndex}>
                  {week.map((day, dayIndex) => (
                    <td
                      key={dayIndex}
                      onClick={() => handleDayClick(day)}
                      style={{ cursor: 'pointer', position: 'relative' }}
                    >
                      {day}
                      {day && reservas[day] ? (
                        <span
                          style={{
                            fontSize: '0.8em',
                            color: 'red',
                            position: 'absolute',
                            bottom: '2px',
                            right: '2px',
                          }}
                        >
                          ({reservas[day]})
                        </span>
                      ) : null}
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Reservas;