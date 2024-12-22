import {
    SIDEBAR_MENU_LINKS,
    SIDEBAR_BOTTOM_LINKS
} from "../../../lib/Navigation";

import { Link } from "react-router-dom";

import classes from "./sidebar.module.css";
import { TiCloudStorage } from "react-icons/ti";

function Sidebar() {
    return (
        <div className={classes.sidebar}>
            <a className={classes.logo} href="l">
                <TiCloudStorage fontSize={30} />
                <span>Clou.D</span>
            </a>
            <nav className={classes.menu}>
                <ul className={classes.list}>
                    {SIDEBAR_MENU_LINKS.map((item) => (
                        <li key={item.key} className={classes.item}>
                            {<SidebarLik item={item} />}
                        </li>
                    ))}
                </ul>
            </nav>
            <div className={classes.bottom}>
                {SIDEBAR_BOTTOM_LINKS.map((item) => (
                    <SidebarLik key={item.key} item={item} />
                ))}
            </div>
        </div>
    );
}

function SidebarLik({ item }) {
    return (
        <Link className={classes.link} to={item.path}>
            {item.icon}
            <span>{item.label}</span>
        </Link>
    );
}

export default Sidebar;
