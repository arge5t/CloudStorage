import { RiHomeLine } from "react-icons/ri";
import { BiGroup } from "react-icons/bi";
import { FaRegStar } from "react-icons/fa";
import { FiSettings } from "react-icons/fi";
import { LuBadgeHelp } from "react-icons/lu";
import { MdOutlineLogout } from "react-icons/md";
import { INavigationLink } from "../../models/INavigationLink";

export const SIDEBAR_MENU_LINKS:INavigationLink[] = [
    {
        key: "cloud",
        label: "My Cloud",
        path: "/cloud",
        icon: <RiHomeLine />
    },
    {
        key: "shared-files",
        label: "Shared with me",
        path: "/shared-files",
        icon: <BiGroup />
    },
    {
        key: "favorites",
        label: "Favorites",
        path: "/favorites",
        icon: <FaRegStar />
    },
    {
        key: "setting",
        label: "Setting",
        path: "/setting",
        icon: <FiSettings />
    }
];

export const SIDEBAR_BOTTOM_LINKS:INavigationLink[] = [
    {
        key: "help",
        label: "Help",
        path: "/help",
        icon: <LuBadgeHelp />
    },
    {
        key: "logout",
        label: "Logout",
        path: "/logout",
        icon: <MdOutlineLogout />
    }
];
