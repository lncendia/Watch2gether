import 'reflect-metadata'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import {Provider} from 'inversify-react';
import container from './container/inversify.config.ts';
import {UserContextProvider} from "./contexts/UserContext/UserContext.tsx";

// Привязываем приложение к корню документа
ReactDOM.createRoot(document.getElementById('root')!).render(
    <Provider container={container}>
        <UserContextProvider>
            <App/>
        </UserContextProvider>
    </Provider>
)
