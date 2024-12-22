import { FC, useState, useContext } from "react";
import { Link } from "react-router-dom";
import { Context } from "../../index";

import { TiCloudStorage } from "react-icons/ti";
import classes from "./auth.module.css";

const LoginForm: FC = () => {
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const { store } = useContext(Context);

    return (
        <div className={classes.auth}>
            <TiCloudStorage fontSize={50} />
            <h2 className={classes.title}>Sign in to your account</h2>
            <input
                onChange={(e) => setEmail(e.target.value)}
                value={email}
                className={classes.input}
                type="text"
                placeholder="Почтовый адрес"
            />
            <input
                onChange={(e) => setPassword(e.target.value)}
                value={password}
                className={classes.input}
                type="text"
                placeholder="Пароль"
            />
            <button
                className={classes.button}
                onClick={() => store.login(email, password)}
            >
                Войти
            </button>
            <Link to="/registr" className={classes.link}>Регистрация</Link>
        </div>
    );
};

export default LoginForm;
