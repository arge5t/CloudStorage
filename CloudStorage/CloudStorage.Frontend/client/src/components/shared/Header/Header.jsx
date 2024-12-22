import classes from "./header.module.css";
import { IoMdAddCircle, IoIosNotifications } from "react-icons/io";

function Header({ user }) {
    return (
        <header className={classes.header}>
            <button className={classes.addButton}>
                <IoMdAddCircle />
                <span>Add File</span>
            </button>
            <IoIosNotifications fontSize={24} />
            <div className={classes.user}>
                <div className={classes.avatar}></div>
                <div className={classes.userInfo}>
                    <div>{user.name}</div>
                    <div>{user.email}</div>
                </div>
            </div>
        </header>
    );
}

export default Header;
