import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [students, setStudents] = useState([]);
    const [newStudentName, setNewStudentName] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchStudents();
    }, []);

    const fetchStudents = async () => {
        try {
            const response = await fetch('/api/students'); // Your API endpoint
            if (!response.ok) {
                throw new Error('Failed to fetch students');
            }
            const result = await response.json();
            setStudents(result);
            setLoading(false);
        } catch (err) {
            setError(err.message);
            setLoading(false);
        }
    };

    const handleCreateStudent = async (e) => {
        e.preventDefault();
        setError('');

        const newStudent = { name: newStudentName };

        try {
            const response = await fetch('/api/students', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newStudent),
            });

            if (!response.ok) {
                throw new Error('Failed to create student');
            }

            setNewStudentName('');
            fetchStudents(); // Refresh the list of students
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div>
            <h1>Student Management</h1>
            <p>This component demonstrates fetching and creating students from the server.</p>

            {loading ? (
                <p><em>Loading... Please refresh once the backend has started.</em></p>
            ) : error ? (
                <p style={{ color: 'red' }}>{error}</p>
            ) : (
                <div>
                    <h2>Students List</h2>
                    <ul>
                        {students.map(student => (
                            <li key={student.id}>{student.name}</li>
                        ))}
                    </ul>
                </div>
            )}

            <form onSubmit={handleCreateStudent}>
                <h2>Create New Student</h2>
                <input
                    type="text"
                    placeholder="Student Name"
                    value={newStudentName}
                    onChange={(e) => setNewStudentName(e.target.value)}
                    required
                />
                <button type="submit">Add Student</button>
            </form>
        </div>
    );
}

export default App;
