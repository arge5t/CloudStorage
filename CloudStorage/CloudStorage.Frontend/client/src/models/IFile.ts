import { Guid } from "guid-typescript";

export default interface IFile {
    id: Guid;
    name: string;
    type: string;
    size: number;
    path: string;
    createTime: string;
    editTime?: string;
    userId: string;
    parentId: string;
}
