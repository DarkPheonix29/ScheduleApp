import React, { useState } from 'react';
import { startOfWeek, addDays, format } from 'date-fns';
import './Calendar.css';

const Calendar = () => {
    const [currentDate, setCurrentDate] = useState(new Date());
    const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

    const startOfWeekDate = startOfWeek(currentDate, { weekStartsOn: 0 }); // Sunday as the start of the week

    // Function to render each day of the current week
    const renderDays = () => {
        return daysOfWeek.map((day, index) => {
            const dayDate = addDays(startOfWeekDate, index);
            return (
                <div key={index} className="calendar-day">
                    <div className="day-name">{day}</div>
                    <div className="day-date">{format(dayDate, 'dd MMM')}</div>
                </div>
            );
        });
    };

    return (
        <div className="calendar">
            <h2>Weekly Calendar</h2>
            <div className="calendar-grid">{renderDays()}</div>
        </div>
    );
};

export default Calendar;
