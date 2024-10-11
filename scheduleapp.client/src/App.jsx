import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [data, setData] = useState();

    useEffect(() => {
        fetchData();
    }, []);

    const contents = data === undefined
        ? <p><em>Loading... Please refresh once the backend has started.</em></p>
        : <div>
            {/* Render your data here */}
            <p>Data loaded successfully!</p>
        </div>;

    return (
        <div>
            <h1>Data Fetch Example</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function fetchData() {
        const response = await fetch('your-endpoint'); // Update with your API endpoint
        const result = await response.json();
        setData(result);
    }
}

export default App;
