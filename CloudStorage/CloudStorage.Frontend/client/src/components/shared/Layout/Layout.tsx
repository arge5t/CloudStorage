import { Outlet } from "react-router-dom";

import { useContext } from "react";
import classes from "./layout.module.css";
import Sidebar from "../Sidebar/Sidebar";
import Header from "../Header/Header";
import { observer } from "mobx-react-lite";
import { Context } from "../../..";

function Layout() {
    const { store } = useContext(Context);

    store.getFiles();

    return (
        <div className={classes.layout}>
            <Sidebar />
            <div className={classes.content}>
                <Header user={store.user} />
                <div>{<Outlet />}</div>
            </div>
        </div>
    );
}

export default observer(Layout);
