import { useContext } from "react";
import classes from "./cloud.module.css";
import { Context } from "../..";
import { observer } from "mobx-react-lite";

function Cloud() {
    const { store } = useContext(Context);

    return (
        <div className={classes}>
            {store.files.map((file) => (
                <div key={file.id}>{file.name}</div>
            ))}
        </div>
    );
}

export default observer(Cloud);
