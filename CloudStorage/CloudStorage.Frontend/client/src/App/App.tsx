import { useContext } from "react";
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Navigate
} from "react-router-dom";
import Layout from "../components/shared/Layout/Layout";
import Cloud from "../components/Cloud/Cloud";
import Login from "../components/Auth/Login";
import Registration from "../components/Auth/Registration";
import { Context } from "..";
import { observer } from "mobx-react-lite";

import "./app.css";

const AuthenticatedRoutes = () => {
    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route index element={<Navigate to="/cloud" replace />} />
                <Route path="/cloud" element={<Cloud />} />
                <Route path="/shared-files" element={<div>Shared Files</div>} />
                <Route path="/favorites" element={<div>Favorites</div>} />
                <Route path="/setting" element={<div>Settigs</div>} />
                <Route path="/help" element={<div>Help</div>} />
                <Route path="/logout" element={<div>Logout</div>} />
            </Route>
            <Route path="*" element={<Navigate to="/" />} />
        </Routes>
    );
};

const UnauthenticatedRoutes = () => {
    return (
        <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/registr" element={<Registration />} />
            <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
    );
};
function App() {
    const { store } = useContext(Context);

    return (
        <>
            <Router>
                {store.isAuth ? (
                    <AuthenticatedRoutes />
                ) : (
                    <UnauthenticatedRoutes />
                )}
            </Router>
        </>
    );
}

export default observer(App);
