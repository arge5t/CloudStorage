import { Guid } from "guid-typescript";
import { AxiosResponse } from "axios";
import $api from "../http";
import IFile from "../models/IFile";

export default class FileService {
    static async getFiles(id?: Guid): Promise<AxiosResponse<IFile[]>> {
        return await $api.get<IFile[]>("/File/files", {
            params: { id }
        });
    }
}
