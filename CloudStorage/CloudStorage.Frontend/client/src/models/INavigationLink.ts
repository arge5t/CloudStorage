import { ReactElement } from "react";

export interface INavigationLink {
    key: string;
    label: string;
    path: string;
    icon: ReactElement
}
