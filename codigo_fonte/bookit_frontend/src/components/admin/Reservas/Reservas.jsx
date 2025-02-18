import React, { useState } from 'react';
import './Reservas.css';  // Importando o arquivo CSS

const Reservas = () => {
  const [month, setMonth] = useState(new Date().getMonth());
  const [year, setYear] = useState(new Date().getFullYear());
  const [selectedAmbiente, setSelectedAmbiente] = useState("Auditório"); 

  const months = [
    'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
  ];

  const years = [2023, 2024, 2025];  
  const ambientes = ['Auditório', 'Sala de Reunião', 'Veículo']; 

  const daysOfWeek = ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'];

  const getDaysInMonth = (month, year) => {
    return new Date(year, month + 1, 0).getDate();
  };

  const getFirstDayOfMonth = (month, year) => {
    return new Date(year, month, 1).getDay();
  };

  const generateCalendar = (month, year) => {
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

  const handleMonthChange = (event) => {
    setMonth(parseInt(event.target.value));
  };

  const handleYearChange = (event) => {
    setYear(parseInt(event.target.value));
  };

  const handleAmbienteChange = (event) => {
    setSelectedAmbiente(event.target.value);
  };

  const calendar = generateCalendar(month, year);

  return (
    <div className="container">
      <div className="calendar-container">
        <h2 className="reservas-title">Reservas de Ambientes</h2>
        <hr />
        <div className="select-container">
          <label className="ambiente-label">
            Ambiente:
            <select value={selectedAmbiente} onChange={handleAmbienteChange}>
              <option value="">Selecione um ambiente</option>
              {ambientes.map((ambiente, index) => (
                <option key={index} value={ambiente}>{ambiente}</option>
              ))}
            </select>
          </label>
          <label style={{ marginLeft: '10px' }}>
            Mês:
            <select value={month} onChange={handleMonthChange}>
              {months.map((monthName, index) => (
                <option key={index} value={index}>{monthName}</option>
              ))}
            </select>
          </label>
          <label style={{ marginLeft: '10px' }}>
            Ano:
            <select value={year} onChange={handleYearChange}>
              {years.map((yearOption, index) => (
                <option key={index} value={yearOption}>{yearOption}</option>
              ))}
            </select>
          </label>
          <hr />
        </div>
        <div className="select-row">
          <div>Selecione a data correspondente à(s) reserva(s):</div>
        </div>
        <div className="table-container">
          <div className="calendar-header">
            <div className="ambiente">{selectedAmbiente || 'Ambiente'} </div>
            <div className="month-year">
              {months[month]} {year}
            </div>
          </div>
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
                    <td key={dayIndex}>{day}</td>
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